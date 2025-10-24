namespace BigGustave
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    internal static class PngOpener
    {
        class MyIdatStream
        {
            byte[] buffer = new byte[1 << 14];
            int position;

            public void Write(byte[] buffer, int offset, int count)
            {
                if (this.buffer.Length < position + count)
                    Array.Resize(ref this.buffer, Math.Max(2 * this.buffer.Length, position + count));
                Array.Copy(buffer, offset, this.buffer, (position += count) - count, count);
            }

            public byte[] Inflate()
            {
#if NOVRAGE
                byte[] bytesOut;
                using (var output = new MemoryStream())
                {
                    using (var memoryStream = new MemoryStream(buffer, 2, position - 6))
                    {
                        using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                        {
                            deflateStream.CopyTo(output);
                            deflateStream.Close();
                        }
                    }
                    bytesOut = output.ToArray();
                }
                return bytesOut;
#else
                var gzBuffer = new byte[14 + position - 6];
                gzBuffer[4] = 0x1f;
                gzBuffer[5] = 0x8b;
                gzBuffer[6] = 0x08;
                Array.Copy(buffer, 2, gzBuffer, 14, position - 6);
                var bytesOut = new byte[100 << 20];
                var l = new VRage.MyCompressionStreamLoad(gzBuffer).GetBytes(bytesOut.Length, bytesOut);
                Array.Resize(ref bytesOut, l);
                return bytesOut;
#endif
            }
        }

        public static Png Open(Stream stream, IChunkVisitor chunkVisitor = null) => Open(stream, new PngOpenerSettings
        {
            ChunkVisitor = chunkVisitor
        });

        public static Png Open(Stream stream, PngOpenerSettings settings)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException($"The provided stream of type {stream.GetType().FullName} was not readable.");
            }

            var validHeader = HasValidHeader(stream);

            if (!validHeader.IsValid)
            {
                throw new ArgumentException($"The provided stream did not start with the PNG header. Got {validHeader}.");
            }

            var crc = new byte[4];
            var imageHeader = ReadImageHeader(stream, crc);

            var hasEncounteredImageEnd = false;

            Palette palette = null;

            {
                var memoryStream = new MyIdatStream();
                {
                    ChunkHeader header;
                    while (TryReadChunkHeader(stream, out header))
                    {
                        if (hasEncounteredImageEnd)
                        {
                            if (settings?.DisallowTrailingData == true)
                            {
                                throw new InvalidOperationException($"Found another chunk {header} after already reading the IEND chunk.");
                            }

                            break;
                        }

                        var bytes = new byte[header.Length];
                        var read = stream.Read(bytes, 0, bytes.Length);
                        if (read != bytes.Length)
                        {
                            throw new InvalidOperationException($"Did not read {header.Length} bytes for the {header} header, only found: {read}.");
                        }

                        if (header.IsCritical)
                        {
                            switch (header.Name)
                            {
                                case "PLTE":
                                    if (header.Length % 3 != 0)
                                    {
                                        throw new InvalidOperationException($"Palette data must be multiple of 3, got {header.Length}.");
                                    }

                                    // Ignore palette data unless the header.ColorType indicates that the image is paletted.
                                    if (imageHeader.ColorType.HasFlag(ColorType.PaletteUsed)) {
                                        palette = new Palette(bytes);
                                    }

                                    break;
                                case "IDAT":
                                    memoryStream.Write(bytes, 0, bytes.Length);
                                    break;
                                case "IEND":
                                    hasEncounteredImageEnd = true;
                                    break;
                                default:
                                    throw new NotSupportedException($"Encountered critical header {header} which was not recognised.");
                            }
                        }
                        else
                        {
                            switch (header.Name)
                            {
                                case "tRNS":
                                    // Add transparency to palette, if the PLTE chunk has been read.
                                    if (palette != null)
                                    {
                                        palette.SetAlphaValues(bytes);
                                    }
                                    break;
                            }
                        }

                        read = stream.Read(crc, 0, crc.Length);
                        if (read != 4)
                        {
                            throw new InvalidOperationException($"Did not read 4 bytes for the CRC, only found: {read}.");
                        }

                        //var result = (int)Crc32.Calculate(Encoding.ASCII.GetBytes(header.Name), bytes);
                        //var crcActual = (crc[0] << 24) + (crc[1] << 16) + (crc[2] << 8) + crc[3];
                        //
                        //if (result != crcActual)
                        //{
                        //    throw new InvalidOperationException($"CRC calculated {result} did not match file {crcActual} for chunk: {header.Name}.");
                        //}

                        settings?.ChunkVisitor?.Visit(stream, imageHeader, header, bytes, crc);
                    }
                }

                var bytesOut = memoryStream.Inflate();

                var bspp = Decoder.GetBytesAndSamplesPerPixel(imageHeader);

                bytesOut = Decoder.Decode(bytesOut, imageHeader, bspp.bytesPerPixel, bspp.samplesPerPixel);

                return new Png(imageHeader, new RawPngData(bytesOut, bspp.bytesPerPixel, palette, imageHeader), palette?.HasAlphaValues ?? false);
            }
        }

        private static HeaderValidationResult HasValidHeader(Stream stream)
        {
            return new HeaderValidationResult(stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte(),
                stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte());
        }

        private static bool TryReadChunkHeader(Stream stream, out ChunkHeader chunkHeader)
        {
            chunkHeader = default(ChunkHeader);

            var position = stream.Position;
            byte[] headerBytes;
            if (!StreamHelper.TryReadHeaderBytes(stream, out headerBytes))
            {
                return false;
            }

            var length = StreamHelper.ReadBigEndianInt32(headerBytes, 0);

            var name = Encoding.ASCII.GetString(headerBytes, 4, 4);

            chunkHeader = new ChunkHeader(position, length, name);

            return true;
        }

        private static ImageHeader ReadImageHeader(Stream stream, byte[] crc)
        {
            ChunkHeader header;
            if (!TryReadChunkHeader(stream, out header))
            {
                throw new ArgumentException("The provided stream did not contain a single chunk.");
            }

            if (header.Name != "IHDR")
            {
                throw new ArgumentException($"The first chunk was not the IHDR chunk: {header}.");
            }

            if (header.Length != 13)
            {
                throw new ArgumentException($"The first chunk did not have a length of 13 bytes: {header}.");
            }

            var ihdrBytes = new byte[13];
            var read = stream.Read(ihdrBytes, 0, ihdrBytes.Length);

            if (read != 13)
            {
                throw new InvalidOperationException($"Did not read 13 bytes for the IHDR, only found: {read}.");
            }

            read = stream.Read(crc, 0, crc.Length);
            if (read != 4)
            {
                throw new InvalidOperationException($"Did not read 4 bytes for the CRC, only found: {read}.");
            }

            var width = StreamHelper.ReadBigEndianInt32(ihdrBytes, 0);
            var height = StreamHelper.ReadBigEndianInt32(ihdrBytes, 4);
            var bitDepth = ihdrBytes[8];
            var colorType = ihdrBytes[9];
            var compressionMethod = ihdrBytes[10];
            var filterMethod = ihdrBytes[11];
            var interlaceMethod = ihdrBytes[12];

            return new ImageHeader(width, height, bitDepth, (ColorType)colorType, (CompressionMethod)compressionMethod, (FilterMethod)filterMethod,
                (InterlaceMethod)interlaceMethod);
        }
    }
}
