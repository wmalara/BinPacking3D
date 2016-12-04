using System;

namespace BinPacking3D.Geometry
{
    public struct CuboidInSpace
    {
        public CuboidInSpace(Point3D point0, Point3D point1)
        {
            if (point0.X > point1.X || point0.Y > point1.Y || point0.Z > point1.Z)
                throw new ArgumentException("Cuboid cannot have negative dimensions");

            Point0 = point0;
            Point1 = point1;
        }

        public CuboidInSpace(Point3D point0, Cuboid cuboid)
            : this(point0, new Point3D(point0.X + cuboid.Width, point0.Y + cuboid.Height, point0.Z + cuboid.Depth))
        {
        }

        public Point3D Point0 { get; }
        public Point3D Point1 { get; }

        public bool Intersects(CuboidInSpace other)
        {
            return (Point0.X < other.Point1.X && Point1.X > other.Point0.X)
                   && (Point0.Y < other.Point1.Y && Point1.Y > other.Point0.Y)
                   && (Point0.Z < other.Point1.Z && Point1.Z > other.Point0.Z);
        }

        public bool Contains(CuboidInSpace other)
        {
            return (Point0.X <= other.Point0.X && Point1.X >= other.Point1.X)
                   && (Point0.Y <= other.Point0.Y && Point1.Y >= other.Point1.Y)
                   && (Point0.Z <= other.Point0.Z && Point1.Z >= other.Point1.Z);
        }

        public Cuboid ToCuboid()
        {
            return new Cuboid(Point1.X - Point0.X, Point1.Y - Point0.Y, Point1.Z - Point0.Z);
        }
    }
}