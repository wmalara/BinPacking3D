using System.Collections.Generic;

namespace BinPacking3D.Geometry
{
    public struct Cuboid
    {
        public Cuboid(uint width, uint height, uint depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Volume = Width*Height*Depth;
        }

        public uint Width { get; }
        public uint Height { get; }
        public uint Depth { get; }
        public uint Volume { get; }

        public Cuboid Rotate90DegX()
        {
            return new Cuboid(Width, Depth, Height);
        }

        public Cuboid Rotate90DegY()
        {
            return new Cuboid(Depth, Height, Width);
        }

        public Cuboid Rotate90DegZ()
        {
            return new Cuboid(Height, Width, Depth);
        }

        public IEnumerable<Cuboid> GenerateOrientations()
        {
            yield return this;
            var xRotated = Rotate90DegX();
            yield return xRotated;
            var yRotated = Rotate90DegY();
            yield return yRotated;
            var zRotated = Rotate90DegZ();
            yield return zRotated;
            yield return xRotated.Rotate90DegZ();
            yield return yRotated.Rotate90DegZ();
        }
    }
}