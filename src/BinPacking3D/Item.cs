using BinPacking3D.Geometry;

namespace BinPacking3D
{
    public class Item<T>
    {
        public Item(T content, Cuboid box, uint weight)
        {
            Content = content;
            Box = box;
            Weight = weight;
        }

        public T Content { get; }
        public Cuboid Box { get; }
        public uint Weight { get; }
    }
}