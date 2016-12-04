using BinPacking3D.Geometry;
using Xunit;

namespace BinPacking3D.Tests
{
    public class CuboidTests
    {
        [Fact]
        public void dimensions_set()
        {
            var cuboid = new Cuboid(1,2,3);

            Assert.StrictEqual(1u, cuboid.Width);
            Assert.StrictEqual(2u, cuboid.Height);
            Assert.StrictEqual(3u, cuboid.Depth);
        }
        
        [Fact]
        public void volume_calculated()
        {
            var cuboid = new Cuboid(1, 2, 3);

            Assert.StrictEqual(6u, cuboid.Volume);
        }

        [Fact]
        public void rotates90DegX_correctly()
        {
            var cuboid = new Cuboid(1, 2, 3);

            var rotated = cuboid.Rotate90DegX();

            Assert.StrictEqual(1u, rotated.Width);
            Assert.StrictEqual(3u, rotated.Height);
            Assert.StrictEqual(2u, rotated.Depth);
        }

        [Fact]
        public void rotates90DegY_correctly()
        {
            var cuboid = new Cuboid(1, 2, 3);

            var rotated = cuboid.Rotate90DegY();

            Assert.StrictEqual(3u, rotated.Width);
            Assert.StrictEqual(2u, rotated.Height);
            Assert.StrictEqual(1u, rotated.Depth);
        }

        [Fact]
        public void rotates90DegZ_correctly()
        {
            var cuboid = new Cuboid(1, 2, 3);

            var rotated = cuboid.Rotate90DegZ();

            Assert.StrictEqual(2u, rotated.Width);
            Assert.StrictEqual(1u, rotated.Height);
            Assert.StrictEqual(3u, rotated.Depth);
        }
    }
}
