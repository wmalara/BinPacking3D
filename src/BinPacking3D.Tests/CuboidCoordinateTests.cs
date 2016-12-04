using System;
using BinPacking3D.Geometry;
using Xunit;

namespace BinPacking3D.Tests
{
    public class CuboidCoordinateTests
    {
        [Fact]
        public void calculates_correct_point1()
        {
            var point0 = new Point3D(1, 1, 1);
            var cuboid = new Cuboid(1, 2, 3);

            var cuboidCoords = new CuboidInSpace(point0, cuboid);

            Assert.StrictEqual(2u, cuboidCoords.Point1.X);
            Assert.StrictEqual(3u, cuboidCoords.Point1.Y);
            Assert.StrictEqual(4u, cuboidCoords.Point1.Z);
        }

        [Fact]
        public void throws_if_negative_dimensions()
        {
            var point0 = new Point3D(2, 2, 2);
            var point1 = new Point3D(1, 2, 3);

            Assert.Throws<ArgumentException>(() => new CuboidInSpace(point0, point1));
        }

        [Fact]
        public void intersects_correctly()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(5, 5, 5));
            var cuboid2 = new CuboidInSpace(new Point3D(4, 4, 4), new Point3D(9, 9, 9));

            var intersects = cuboid1.Intersects(cuboid2);

            Assert.True(intersects);
        }

        [Fact]
        public void intersects_if_contains()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(10, 10, 10));
            var cuboid2 = new CuboidInSpace(new Point3D(4, 4, 4), new Point3D(9, 9, 9));

            var intersects = cuboid1.Intersects(cuboid2);

            Assert.True(intersects);
        }

        [Fact]
        public void intersects_if_same()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(10, 10, 10));
            var cuboid2 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(10, 10, 10));

            var intersects = cuboid1.Intersects(cuboid2);

            Assert.True(intersects);
        }

        [Fact]
        public void not_intersects_if_disjoint()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(5, 5, 5));
            var cuboid2 = new CuboidInSpace(new Point3D(6, 4, 4), new Point3D(9, 9, 9));

            var intersects = cuboid1.Intersects(cuboid2);

            Assert.False(intersects);
        }

        [Fact]
        public void not_intersects_if_touching()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(5, 5, 5));
            var cuboid2 = new CuboidInSpace(new Point3D(5, 0, 0), new Point3D(10, 10, 10));

            var intersects = cuboid1.Intersects(cuboid2);

            Assert.False(intersects);
        }

        [Fact]
        public void contains_if_inside()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(10, 10, 10));
            var cuboid2 = new CuboidInSpace(new Point3D(4, 4, 4), new Point3D(9, 9, 9));

            var contains = cuboid1.Contains(cuboid2);

            Assert.True(contains);
        }

        [Fact]
        public void contains_if_same()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(10, 10, 10));
            var cuboid2 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(10, 10, 10));

            var contains = cuboid1.Contains(cuboid2);

            Assert.True(contains);
        }

        [Fact]
        public void not_contains_if_disjoint()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(5, 5, 5));
            var cuboid2 = new CuboidInSpace(new Point3D(6, 4, 4), new Point3D(9, 9, 9));

            var contains = cuboid1.Contains(cuboid2);

            Assert.False(contains);
        }

        [Fact]
        public void contains_if_touching_inside()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(10,10,10));
            var cuboid2 = new CuboidInSpace(new Point3D(5,5,5), new Point3D(10,10,10));

            var contains = cuboid1.Contains(cuboid2);

            Assert.True(contains);
        }

        [Fact]
        public void contains_if_touching_outside()
        {
            var cuboid1 = new CuboidInSpace(new Point3D(0, 0, 0), new Point3D(5,5,5));
            var cuboid2 = new CuboidInSpace(new Point3D(5, 0, 0), new Point3D(10, 10, 10));

            var contains = cuboid1.Contains(cuboid2);

            Assert.False(contains);
        }
    }
}