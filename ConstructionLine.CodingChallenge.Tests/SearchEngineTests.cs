using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        private List<Shirt> _shirts;

        [SetUp]
        public void SetUp()
        {
            _shirts = new List<Shirt>
                  {
                      new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                      new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                      new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                  };
        }

        [Test]
        public void Test()
        {
            var searchEngine = new SearchEngine(_shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red},
                Sizes = new List<Size> {Size.Small}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void WhenSearchOptionsEmptyReturnAllShirts()
        {
            var searchEngine = new SearchEngine(_shirts);

            var searchOptions = new SearchOptions();

            var results = searchEngine.Search(searchOptions);

            Assert.That(results.Shirts, Is.EqualTo(_shirts));

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts); 
        }

        [Test]
        public void WhenSearchOptionsIsNullThrowException()
        {
            var searchEngine = new SearchEngine(_shirts);

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(null));
        }
        
        [Test]
        public void WhenSearchOptionsColorsIsNullThrowException()
        {
            var searchEngine = new SearchEngine(_shirts);
            var searchOptions = new SearchOptions
                                    {
                                        Colors = null,
                                        Sizes = new List<Size> { Size.Small }
                                    };
            
            var exception = Assert.Throws<ArgumentException>(() => searchEngine.Search(searchOptions));
            Assert.That(exception.Message, Is.EqualTo($"{nameof(searchOptions.Colors)} cannot be null"));
        }
        
        [Test]
        public void WhenSearchOptionsSizesIsNullThrowException()
        {
            var searchEngine = new SearchEngine(_shirts);
            var searchOptions = new SearchOptions
                                    {
                                        Colors = new List<Color> {Color.Red},
                                        Sizes = null
                                    };
            
            var exception = Assert.Throws<ArgumentException>(() => searchEngine.Search(searchOptions));
            Assert.That(exception.Message, Is.EqualTo($"{nameof(searchOptions.Sizes)} cannot be null"));
        }
    }
}
