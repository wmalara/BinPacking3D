using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinPacking3D
{
    public class BinPacker
    {
        public Bin<T> Allocate<T>(Bin<T> bin, Item<T>[] items)
        {
            ValidateArguments(bin, items);
            
            var itemsSorted = SortItems(items);
            var binCloned = bin.CloneEmpty();

            var couldNotAdd = itemsSorted.Any(item => binCloned.TryAddItem(item) == false);

            if (couldNotAdd)
                throw new InvalidOperationException("Item could not be added to the bin");

            return binCloned;
        }

        private static void ValidateArguments<T>(Bin<T> bin, Item<T>[] items)
        {
            if (bin == null)
                throw new ArgumentNullException(nameof(bin));

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (items.Any(si => si.Weight > bin.MaxWeight))
                throw new ArgumentException("Some items are too heavy", nameof(items));

            var maxDimension = new[] { bin.Box.Width, bin.Box.Height, bin.Box.Depth }.Max();
            if (items.Any(si => new[] { si.Box.Width, si.Box.Height, si.Box.Depth}.Any(d => d > maxDimension)))
                throw new ArgumentException("Some items do not fit to the bin", nameof(bin));
        }
        
        private static IOrderedEnumerable<Item<T>> SortItems<T>(Item<T>[] stockItems)
        {
            return stockItems.OrderByDescending(si => si.Weight).ThenByDescending(si => si.Box.Volume);
        }        
    }
}
