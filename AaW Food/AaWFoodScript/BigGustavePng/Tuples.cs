namespace BigGustave
{
    struct VtupBSPP
    {
        public byte bytesPerPixel;
        public byte samplesPerPixel;

        public VtupBSPP(byte bytesPerPixel, byte samplesPerPixel)
        {
            this.bytesPerPixel = bytesPerPixel;
            this.samplesPerPixel = samplesPerPixel;
        }
    }

    struct VtupRGBA
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public VtupRGBA(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }

    struct VtupKeywordData
    {
        public string keyword;
        public byte[] data;

        public VtupKeywordData(string keyword, byte[] data)
        {
            this.keyword = keyword;
            this.data = data;
        }
    }

    struct VtupXY
    {
        public int x;
        public int y;

        public VtupXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
