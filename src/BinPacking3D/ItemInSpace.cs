using BinPacking3D.Geometry;

namespace BinPacking3D
{
    public class ItemInSpace<T>
    {
        public ItemInSpace(Item<T> stockItem, CuboidInSpace coordinate)
        {
            Item = stockItem;
            Coordinate = coordinate;
        }

        public Item<T> Item { get; }
        public CuboidInSpace Coordinate { get; }
    }
}