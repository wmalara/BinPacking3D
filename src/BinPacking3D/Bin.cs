using System;
using System.Collections.Generic;
using System.Linq;
using BinPacking3D.Geometry;

namespace BinPacking3D
{
    public class Bin<T>
    {
        private readonly List<ItemInSpace<T>> items;
        
        public Bin(Cuboid box, uint maxWeight)
        {
            ValidateCtorArguments(box, maxWeight);

            Box = box;
            MaxWeight = maxWeight;
            items = new List<ItemInSpace<T>>();

            CuboidCoordinate = new CuboidInSpace(Point3D.Zero, Box);
            RefreshCalculatedValues();
        }

        private CuboidInSpace CuboidCoordinate { get; }

        public Cuboid Box { get; }
        public IEnumerable<ItemInSpace<T>> Items => items;
        public uint MaxWeight { get; }
        public uint ItemsWeight { get; private set; }
        public uint WeightCapacityLeft { get; private set; }
        public uint ItemsVolume { get; private set; }
        public uint VolumeCapacityLeft { get; private set; }

        public Bin<T> CloneEmpty()
        {
            return new Bin<T>(Box, MaxWeight);
        }

        public bool TryAddItem(Item<T> item)
        {
            if (WeightCapacityLeft < item.Weight)
                return false;
            
            var layerLevels = GenerateLayerLevels();
            var orientations = GenerateOrientations(item.Box).ToList();

            return layerLevels.Any(layerLevel => TryAddAtLevel(item, layerLevel, orientations));
        }

        private bool TryAddAtLevel(Item<T> item, uint layerLevel, List<Cuboid> orientations)
        {
            var startingPoints = GenerateAllVerticesAtLevel(layerLevel).ToList();

            return orientations.Any(orientation => TryAddInOrientation(item, orientation, startingPoints));
        }

        private bool TryAddInOrientation(Item<T> item, Cuboid orientation, List<Point3D> startingPoints)
        {
            return startingPoints.Any(startingPoint => TryAddAtStartingPoint(item, startingPoint, orientation));
        }

        private bool TryAddAtStartingPoint(Item<T> item, Point3D startingPoint, Cuboid orientation)
        {
            var placement = new CuboidInSpace(startingPoint, orientation);

            if (Collides(placement) == false)
            {
                AddItem(new ItemInSpace<T>(item, placement));
                return true;
            }

            return false;
        }

        private bool Collides(CuboidInSpace placement)
        {
            return CuboidCoordinate.Contains(placement) == false 
                || Items.Any(i => i.Coordinate.Intersects(placement));
        }

        private void AddItem(ItemInSpace<T> itemPlaced)
        {
            items.Add(itemPlaced);
            RefreshCalculatedValues();
        }

        private void RefreshCalculatedValues()
        {
            ItemsWeight = (uint)Items.Sum(i => i.Item.Weight);
            WeightCapacityLeft = MaxWeight - ItemsWeight;
            ItemsVolume = (uint)Items.Sum(i => i.Item.Box.Volume);
            VolumeCapacityLeft = Box.Volume - ItemsVolume;
        }

        private IEnumerable<Point3D> GenerateAllVerticesAtLevel(uint layerLevel)
        {
            return Enumerable.Repeat(new Point3D(0, layerLevel, 0), 1)
                .Concat(
                    Items
                    .Where(i => IntersectsY(i.Coordinate, layerLevel))
                    .SelectMany(i => GetVerticesAtLevel(i.Coordinate, layerLevel)))
                .Distinct()
                .OrderBy(p => p.Z)
                .ThenBy(p => p.X);
        }

        private static IEnumerable<Cuboid> GenerateOrientations(Cuboid itemBox)
        {
            return itemBox.GenerateOrientations().OrderBy(bp => bp.Height);
        }

        private IEnumerable<uint> GenerateLayerLevels()
        {
            return Enumerable.Repeat(0u, 1)
                .Concat(items.Select(i => i.Coordinate.Point1.Y))
                .Where(y => y < Box.Height)
                .Distinct()
                .OrderBy(y => y);
        }

        private bool IntersectsY(CuboidInSpace cuboidInSpace, uint y)
        {
            return cuboidInSpace.Point0.Y <= y && cuboidInSpace.Point1.Y >= y;
        }

        private IEnumerable<Point3D> GetVerticesAtLevel(CuboidInSpace cuboidInSpace, uint y)
        {
            yield return new Point3D(cuboidInSpace.Point0.X, y, cuboidInSpace.Point0.Z);
            yield return new Point3D(cuboidInSpace.Point1.X, y, cuboidInSpace.Point0.Z);
            yield return new Point3D(cuboidInSpace.Point1.X, y, cuboidInSpace.Point1.Z);
            yield return new Point3D(cuboidInSpace.Point0.X, y, cuboidInSpace.Point1.Z);
        }

        private static void ValidateCtorArguments(Cuboid box, uint maxWeight)
        {
            if (box.Volume == 0)
                throw new ArgumentException("Volume must be greater than 0", nameof(box));

            if (maxWeight == 0)
                throw new ArgumentException("Max weight must be greater than 0", nameof(maxWeight));
        }
    }
}