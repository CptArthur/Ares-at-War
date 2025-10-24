namespace BigGustave
{
    using System;

    internal static class Decoder
    {
        public static VtupBSPP GetBytesAndSamplesPerPixel(ImageHeader header)
        {
            var bitDepthCorrected = (header.BitDepth + 7) / 8;

            var samplesPerPixel = SamplesPerPixel(header);

            return new VtupBSPP((byte)(samplesPerPixel * bitDepthCorrected), samplesPerPixel);
        }

        public static byte[] Decode(byte[] decompressedData, ImageHeader header, byte bytesPerPixel, byte samplesPerPixel)
        {
            switch (header.InterlaceMethod)
            {
                case InterlaceMethod.None:
                    {
                        var bytesPerScanline = BytesPerScanline(header, samplesPerPixel);

                        var currentRowStartByteAbsolute = 1;
                        for (var rowIndex = 0; rowIndex < header.Height; rowIndex++)
                        {
                            var filterType = (FilterType)decompressedData[currentRowStartByteAbsolute - 1];

                            var previousRowStartByteAbsolute = (rowIndex) + (bytesPerScanline * (rowIndex - 1));

                            var end = currentRowStartByteAbsolute + bytesPerScanline;
                            for (var currentByteAbsolute = currentRowStartByteAbsolute; currentByteAbsolute < end; currentByteAbsolute++)
                            {
                                ReverseFilter(decompressedData, filterType, previousRowStartByteAbsolute, currentRowStartByteAbsolute, currentByteAbsolute, currentByteAbsolute - currentRowStartByteAbsolute, bytesPerPixel);
                            }

                            currentRowStartByteAbsolute += bytesPerScanline + 1;
                        }

                        return decompressedData;
                    }
                case InterlaceMethod.Adam7:
                    {
                        var pixelsPerRow = header.Width * bytesPerPixel;
                        var newBytes = new byte[header.Height * pixelsPerRow];
                        var i = 0;
                        var previousStartRowByteAbsolute = -1;
                        // 7 passes
                        for (var pass = 0; pass < 7; pass++)
                        {
                            var numberOfScanlines = Adam7.GetNumberOfScanlinesInPass(header, pass);
                            var numberOfPixelsPerScanline = Adam7.GetPixelsPerScanlineInPass(header, pass);

                            if (numberOfScanlines <= 0 || numberOfPixelsPerScanline <= 0)
                            {
                                continue;
                            }

                            for (var scanlineIndex = 0; scanlineIndex < numberOfScanlines; scanlineIndex++)
                            {
                                var filterType = (FilterType)decompressedData[i++];
                                var rowStartByte = i;

                                for (var j = 0; j < numberOfPixelsPerScanline; j++)
                                {
                                    var pixelIndex = Adam7.GetPixelIndexForScanlineInPass(header, pass, scanlineIndex, j);
                                    for (var k = 0; k < bytesPerPixel; k++)
                                    {
                                        var byteLineNumber = (j * bytesPerPixel) + k;
                                        ReverseFilter(decompressedData, filterType, previousStartRowByteAbsolute, rowStartByte, i, byteLineNumber, bytesPerPixel);
                                        i++;
                                    }

                                    var start = pixelsPerRow * pixelIndex.y + pixelIndex.x * bytesPerPixel;
                                    Array.ConstrainedCopy(decompressedData, rowStartByte + j * bytesPerPixel, newBytes, start, bytesPerPixel);
                                }

                                previousStartRowByteAbsolute = rowStartByte;
                            }
                        }

                        return newBytes;
                    }
                default:
                    throw new ArgumentException($"Invalid interlace method: {header.InterlaceMethod}.");
            }
        }

        private static byte SamplesPerPixel(ImageHeader header)
        {
            switch (header.ColorType)
            {
                case ColorType.None:
                    return 1;
                case ColorType.PaletteUsed:
                    return 1;
                case ColorType.ColorUsed:
                    return 3;
                case ColorType.AlphaChannelUsed:
                    return 2;
                case ColorType.ColorUsed | ColorType.AlphaChannelUsed:
                    return 4;
                default:
                    return 0;
            }
        }

        private static int BytesPerScanline(ImageHeader header, byte samplesPerPixel)
        {
            var width = header.Width;

            switch (header.BitDepth)
            {
                case 1:
                    return (width + 7) / 8;
                case 2:
                    return (width + 3) / 4;
                case 4:
                    return (width + 1) / 2;
                case 8:
                case 16:
                    return width * samplesPerPixel * (header.BitDepth / 8);
                default:
                    return 0;
            }
        }

        private static void ReverseFilter(byte[] data, FilterType type, int previousRowStartByteAbsolute, int rowStartByteAbsolute, int byteAbsolute, int rowByteIndex, int bytesPerPixel)
        {
            // Moved out of the switch for performance.
            if (type == FilterType.Up)
            {
                var above = previousRowStartByteAbsolute + rowByteIndex;
                if (above < 0)
                {
                    return;
                }

                data[byteAbsolute] += data[above];
                return;
            }

            var leftIndex = rowByteIndex - bytesPerPixel;

            if (type == FilterType.Sub)
            {
                if (leftIndex < 0)
                {
                    return;
                }

                data[byteAbsolute] += data[rowStartByteAbsolute + leftIndex];
                return;
            }

            if (type == FilterType.None)
                return;

            var leftValue = leftIndex >= 0 ? data[rowStartByteAbsolute + leftIndex] : (byte)0;
            var upIndex = previousRowStartByteAbsolute + rowByteIndex;
            var aboveValue = upIndex >= 0 ? data[upIndex] : (byte)0;

            if (type == FilterType.Average)
            {
                data[byteAbsolute] += (byte)((leftValue + aboveValue) / 2);
                return;
            }

            var index = previousRowStartByteAbsolute + rowByteIndex - bytesPerPixel;
            var aboveLeftValue = index < previousRowStartByteAbsolute || previousRowStartByteAbsolute < 0 ? (byte)0 : data[index];

            if (type == FilterType.Paeth)
            {
                data[byteAbsolute] += GetPaethValue(leftValue, aboveValue, aboveLeftValue);
                return;
            }

            throw new ArgumentException(nameof(type), type.ToString(), null);
        }

        /// <summary>
        /// Computes a simple linear function of the three neighboring pixels (left, above, upper left),
        /// then chooses as predictor the neighboring pixel closest to the computed value.
        /// </summary>
        private static byte GetPaethValue(byte a, byte b, byte c)
        {
            var p = a + b - c;
            var pa = Math.Abs(p - a);
            var pb = Math.Abs(p - b);
            var pc = Math.Abs(p - c);

            if (pa <= pb && pa <= pc)
            {
                return a;
            }

            return pb <= pc ? b : c;
        }
    }
}
