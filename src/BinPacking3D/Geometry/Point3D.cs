namespace BinPacking3D.Geometry
{
    public struct Point3D
    {
        public Point3D(uint x, uint y, uint z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static readonly Point3D Zero = new Point3D(0, 0, 0);

        public uint X { get; }
        public uint Y { get; }
        public uint Z { get; }
    }
}