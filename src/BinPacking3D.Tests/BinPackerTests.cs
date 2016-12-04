using BinPacking3D;
using BinPacking3D.Geometry;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace BinPacking3D.Tests
{
    public class BinPackerTests
    {
        private readonly ITestOutputHelper output;
        
        public BinPackerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void returns_bin()
        {
            var items = new[] { new Item<string>("a", new Cuboid(1, 1, 1), 1) };
            var bin = new Bin<string>(new Cuboid(1, 1, 1), 1);
            var packer = new BinPacker();

            var binWithItems = packer.Allocate(bin, items);

            Assert.NotNull(binWithItems);
        }

        [Fact]
        public void returns_bin_with_items()
        {
            var items = new[] { new Item<string>("a", new Cuboid(1, 1, 1), 1) };
            var bin = new Bin<string>(new Cuboid(1, 1, 1), 1);
            var packer = new BinPacker();

            var binWithItems = packer.Allocate(bin, items);

            Assert.NotNull(binWithItems.Items);
        }

        [Fact]
        public void all_items_are_allocated()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(1,1,1), 1),
                new Item<string>("b", new Cuboid(2,1,1), 2),
                new Item<string>("c", new Cuboid(1,2,1), 3)
            };
            var bin = new Bin<string>(new Cuboid(10, 10, 10), 10);
            var packer = new BinPacker();

            var binWithItems = packer.Allocate(bin, items);

            var sortedItems = items.Select(si => si.Content).OrderBy(si => si);
            var sortedAllocatedItems = binWithItems.Items.Select(i => i.Item.Content).OrderBy(si => si);
            Assert.True(sortedItems.SequenceEqual(sortedAllocatedItems));
        }

        [Fact]
        public void throws_if_any_item_doesnt_fit_by_weight()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(1,2,1), 3)
            };
            var bin = new Bin<string>(new Cuboid(2, 2, 2), 2);
            var packer = new BinPacker();

            Assert.Throws<ArgumentException>(() => packer.Allocate(bin, items));
        }

        [Fact]
        public void throws_if_any_item_doesnt_fit_by_any_dimension()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(1,3,1), 1)
            };
            var bin = new Bin<string>(new Cuboid(2, 2, 2), 2);
            var packer = new BinPacker();

            Assert.Throws<ArgumentException>(() => packer.Allocate(bin, items));
        }

        [Fact]
        public void fits_1_cube_to_bin()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(10,10,10), 1)
            };
            var bin = new Bin<string>(new Cuboid(20, 20, 20), 4);
            var packer = new BinPacker();

            var binWithItems = packer.Allocate(bin, items);
            
            Assert.StrictEqual(1, binWithItems.Items.Count());
            Assert.True(binWithItems.Items.First().Item == items[0]);
        }

        [Fact]
        public void fits_2_cubes_to_bin()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("a", new Cuboid(10,10,10), 1)
            };
            var bin = new Bin<string>(new Cuboid(20, 10, 10), 4);
            var packer = new BinPacker();

            var binWithItems = packer.Allocate(bin, items);
            
            Assert.StrictEqual(2, binWithItems.Items.Count());
        }

        [Fact]
        public void throws_if_there_is_not_enough_space()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("a", new Cuboid(10,10,10), 1)
            };
            var bin = new Bin<string>(new Cuboid(20, 10, 10), 4);
            var packer = new BinPacker();

            Assert.Throws<InvalidOperationException>(() => packer.Allocate(bin, items));
        }
        
        [Fact]
        public void performance_test()
        {
            var items = Enumerable.Repeat(new Item<string>("a", new Cuboid(2, 2, 2), 1), 100).ToArray();
            var bin = new Bin<string>(new Cuboid(1000, 1000, 1000), 1000);
            var packer = new BinPacker();
            var sw = new Stopwatch();

            sw.Start();
            var binWithItems = packer.Allocate(bin, items);
            sw.Stop();

            this.output.WriteLine($"Execution time: {sw.ElapsedMilliseconds} ms");
        }
    }
}
