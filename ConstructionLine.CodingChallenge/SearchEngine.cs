using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
        }

        public SearchResults Search(SearchOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.Colors == null) throw new ArgumentException($"{nameof(options.Colors)} cannot be null");
            if (options.Sizes == null) throw new ArgumentException($"{nameof(options.Sizes)} cannot be null");

            var foundShirts = _shirts.Where(
                shirt =>
                    {
                        return (!options.Colors.Any() || options.Colors.Any(color => color.Id == shirt.Color.Id))
                               && (!options.Sizes.Any() || options.Sizes.Any(size => size.Id == shirt.Size.Id));
                    }).ToList();

            var foundColorCounts = foundShirts.GroupBy(shirt => shirt.Color).Select(
                group => new ColorCount
                             {
                                 Color = group.Key,
                                 Count = group.Count()
                             }).ToList();

            var foundSizeCounts = foundShirts.GroupBy(shirt => shirt.Size).Select(
                group => new SizeCount
                             {
                                 Size = group.Key,
                                 Count = group.Count()
                             }).ToList();

            var emptyColorCounts = Color.All
                .Where(color => foundColorCounts.All(foundColorCount => foundColorCount.Color != color))
                .Select(missingColor => new ColorCount { Color = missingColor, Count = 0 });

            var emptySizeCounts = Size.All
                .Where(size => foundSizeCounts.All(foundSizeCount => foundSizeCount.Size != size))
                .Select(missingSize => new SizeCount { Size = missingSize, Count = 0 });

            var searchResults = new SearchResults
                                    {
                                        Shirts = foundShirts,
                                        ColorCounts = foundColorCounts,
                                        SizeCounts = foundSizeCounts
                                    };

            searchResults.ColorCounts.AddRange(emptyColorCounts);
            searchResults.SizeCounts.AddRange(emptySizeCounts);

            return searchResults;
        }
    }
}