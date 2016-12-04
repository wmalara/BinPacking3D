using System.Linq;
using BinPacking3D.Geometry;
using Xunit;

namespace BinPacking3D.Tests
{
    public class BinTests
    {
        [Fact]
        public void doesnt_take_more_weight_than_can_contain()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(1,1,1), 2),
                new Item<string>("b", new Cuboid(1,1,1), 2),
                new Item<string>("b", new Cuboid(1,1,1), 2)
            };

            var bin = new Bin<string>(new Cuboid(2, 2, 2), 5);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }

            Assert.StrictEqual(2, bin.Items.Count());
            Assert.StrictEqual(4u, bin.ItemsWeight);
        }


        [Fact]
        public void sum_of_items_volume_doesnt_exceed_boxes_maximum()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(2, 2, 2), 1),
                new Item<string>("b", new Cuboid(2, 2, 2), 1),
                new Item<string>("b", new Cuboid(2, 2, 2), 1)
            };

            var bin = new Bin<string>(new Cuboid(5, 2, 3), 5);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }

            Assert.StrictEqual(2, bin.Items.Count());
            Assert.StrictEqual(16u, bin.ItemsVolume);
        }

        [Fact]
        public void item_not_added_if_too_big()
        {
            var stockItem = new Item<string>("a", new Cuboid(11, 10, 10), 1);

            var bin = new Bin<string>(new Cuboid(10, 10, 10), 4);

            bin.TryAddItem(stockItem);

            Assert.StrictEqual(0, bin.Items.Count());
        }

        [Fact]
        public void two_cubes_have_correct_coordinates()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("a", new Cuboid(10,10,10), 1)
            };

            var bin = new Bin<string>(new Cuboid(11, 11, 21), 4);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }

            var firstItem = bin.Items.ElementAt(0).Coordinate;
            Assert.StrictEqual(new Point3D(0, 0, 0), firstItem.Point0);
            Assert.StrictEqual(new Point3D(10, 10, 10), firstItem.Point1);

            var secondItem = bin.Items.ElementAt(1).Coordinate;
            Assert.StrictEqual(new Point3D(0, 0, 10), secondItem.Point0);
            Assert.StrictEqual(new Point3D(10, 10, 20), secondItem.Point1);
        }

        [Fact]
        public void three_items_on_bottom_have_correct_coordinates()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("b", new Cuboid(20,10,10), 1)
            };

            var bin = new Bin<string>(new Cuboid(20, 20, 20), 4);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }

            var firstItem = bin.Items.ElementAt(0).Coordinate;
            Assert.StrictEqual(new Point3D(0, 0, 0), firstItem.Point0);
            Assert.StrictEqual(new Point3D(10, 10, 10), firstItem.Point1);

            var secondItem = bin.Items.ElementAt(1).Coordinate;
            Assert.StrictEqual(new Point3D(10, 0, 0), secondItem.Point0);
            Assert.StrictEqual(new Point3D(20, 10, 10), secondItem.Point1);

            var thirdItem = bin.Items.ElementAt(2).Coordinate;
            Assert.StrictEqual(new Point3D(0, 0, 10), thirdItem.Point0);
            Assert.StrictEqual(new Point3D(20, 10, 20), thirdItem.Point1);
        }

        [Fact]
        public void two_items_on_bottom_have_correct_coordinates()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(11,10,10), 1),
                new Item<string>("b", new Cuboid(10,11,10), 1)
            };

            var bin = new Bin<string>(new Cuboid(20, 20, 20), 4);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }

            var firstItem = bin.Items.ElementAt(0).Coordinate;
            Assert.StrictEqual(new Point3D(0, 0, 0), firstItem.Point0);
            Assert.StrictEqual(new Point3D(11, 10, 10), firstItem.Point1);

            var secondItem = bin.Items.ElementAt(1).Coordinate;
            Assert.StrictEqual(new Point3D(00, 0, 10), secondItem.Point0);
            Assert.StrictEqual(new Point3D(11, 10, 20), secondItem.Point1);
        }

        [Fact]
        public void item_correctly_rotated_to_fit()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("b", new Cuboid(10,20,10), 1)
            };

            var bin = new Bin<string>(new Cuboid(20, 20, 20), 4);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }

            var thirdItem = bin.Items.ElementAt(2).Coordinate;
            Assert.StrictEqual(new Point3D(0, 0, 10), thirdItem.Point0);
            Assert.StrictEqual(new Point3D(20, 10, 20), thirdItem.Point1);
        }

        [Fact]
        public void item_correctly_rotated_to_fit2()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("a", new Cuboid(10,10,10), 1),
                new Item<string>("b", new Cuboid(10,10,20), 1)
            };

            var bin = new Bin<string>(new Cuboid(20, 20, 20), 4);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }
            
            var item4 = bin.Items.ElementAt(3).Coordinate;
            Assert.StrictEqual(new Point3D(10, 0, 10), item4.Point0);
            Assert.StrictEqual(new Point3D(20, 20, 20), item4.Point1);

            Assert.True(bin.Items.All(i => i.Coordinate.Point0.Y == 0));
        }


        [Fact]
        public void item_not_added_if_stack_too_high()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(20,10,10), 1),
                new Item<string>("b", new Cuboid(10,10,20), 1),
                new Item<string>("c", new Cuboid(11,11,11), 1),
                new Item<string>("d", new Cuboid(10,10,20), 1)
            };

            var bin = new Bin<string>(new Cuboid(20, 30, 20), 4);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }
            
            Assert.StrictEqual(3, bin.Items.Count());
            Assert.False(bin.Items.Any(i => i.Item.Content == "d"));
        }

        [Fact]
        public void item_put_on_lower_item()
        {
            var items = new Item<string>[]
            {
                new Item<string>("a", new Cuboid(20,10,10), 1),
                new Item<string>("b", new Cuboid(10,20,5), 1),
                new Item<string>("c", new Cuboid(10,20,10), 1)
            };

            var bin = new Bin<string>(new Cuboid(20, 20, 20), 4);

            foreach (var stockItem in items)
            {
                bin.TryAddItem(stockItem);
            }

            var item3 = bin.Items.ElementAt(2).Coordinate;
            Assert.StrictEqual(new Point3D(0, 5, 10), item3.Point0);
            Assert.StrictEqual(new Point3D(20, 15, 20), item3.Point1);
        }
    }
}
