using NUnit.Framework;
using System.Collections.Generic;
using static FilterAndRank.Ranking;

namespace FilterAndRank.Tests
{
    public class RankingTests
    {
        // TODO: This is a partial list of tests, and may be extended. You do NOT need to add any `@Category` annotations
        //       to your own new test methods.
        
        [Test]
        [Category("provided")]
        [Category("requirement-filterByCountry")]
        public void TestPersonsAreFilteredByOneCountry()
        {
            var people = new List<Person>
            {
                new Person(1, "name"),
                new Person(2, "name"),
                new Person(3, "name"),
                new Person(4, "name"),
                new Person(5, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                new CountryRanking(1, "country", 1),
                new CountryRanking(2, "country", 1),
                new CountryRanking(3, "notCountry", 1),
                new CountryRanking(4, "Country", 1), // case insensitive
                new CountryRanking(5, "cOUntry", 1) // case insensitive
            };

            var expectedResults = new List<RankedResult>()
            {
                new RankedResult(1, 1),
                new RankedResult(2, 1),
                new RankedResult(4, 1),
                new RankedResult(5, 1)
            };

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string> { "country" },
                1, int.MaxValue,
                int.MaxValue
            );

            // we don't care about ordering for this test
            CollectionAssert.AreEquivalent(expectedResults, new HashSet<RankedResult>(actualResults));
        }

        [Test]
        [Category("provided")]
        [Category("requirement-filterByCountry")]
        public void TestPersonsAreFilteredByMoreThankOneCountry()
        {
            var people = new List<Person>
            {
                new Person(1, "name"),
                new Person(2, "name"),
                new Person(3, "name"),
                new Person(4, "name"),
                new Person(5, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                new CountryRanking(1, "countryOne", 1),
                new CountryRanking(2, "countryTwo", 1),
                new CountryRanking(3, "notCountry", 2),
                new CountryRanking(4, "CountryOne", 2), // case insensitive
                new CountryRanking(5, "cOUntryTwo", 2) // case insensitive
            };

            var expectedResults = new List<RankedResult>()
            {
                new RankedResult(1, 1),
                new RankedResult(2, 1),
                new RankedResult(4, 2),
                new RankedResult(5, 2)
            };

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string>() { "countryOne", "countryTwo" },
                1, 10,
                maxCount: 100
            );

            // we don't care about ordering for this test
            CollectionAssert.AreEquivalent(expectedResults, new HashSet<RankedResult>(actualResults));
        }

        [Test]
        [Category("provided")]
        [Category("requirement-sortByRank")]
        public void TestPeopleAreSortedFirstByRank()
        {
            var people = new List<Person>
            {
                new Person(1, "name"),
                new Person(2, "name"),
                new Person(3, "name"),
                new Person(4, "name"),
                new Person(5, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                new CountryRanking(1, "country", 1),
                new CountryRanking(2, "country", 5),
                new CountryRanking(3, "country", 2),
                new CountryRanking(4, "country", 4),
                new CountryRanking(5, "country", 3)
            };

            var expectedResults = new List<RankedResult>()
            {
                new RankedResult(1, 1),
                new RankedResult(3, 2),
                new RankedResult(5, 3),
                new RankedResult(4, 4),
                new RankedResult(2, 5)
            };

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string>() { "country" },
                1, 10,
                100
            );

            CollectionAssert.AreEqual(expectedResults, actualResults);
        }


        [Test]
        [Category("provided")]
        [Category("requirement-sortByCountryOrdinal")]
        public void TestPeopleAreSortedSecondByCountryOrdinal()
        {
            var people = new List<Person>
            {
                new Person(1, "name"),
                new Person(3, "name"),
                new Person(4, "name"),
                new Person(2, "name"),
                new Person(5, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                new CountryRanking(3, "55_third", 1),
                new CountryRanking(4, "99_first", 2),
                new CountryRanking(5, "01_second", 2),
                new CountryRanking(2, "01_second", 1),
                new CountryRanking(1, "99_first", 1)
            };

            var expectedResults = new List<RankedResult>()
            {
                new RankedResult(1, 1),
                new RankedResult(2, 1),
                new RankedResult(3, 1),
                new RankedResult(4, 2),
                new RankedResult(5, 2)
            };

            // TODO: what about case insensitivity when checking ordinal, does this test for that?!?

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string>() { "99_first", "01_second", "55_third" },
                1, 10,
                100
            );

            CollectionAssert.AreEqual(expectedResults, actualResults);
        }


        [Test]
        [Category("user")]
        [Category("requirement-sortByName")]
        public void TestPeopleAreSortedThirdByName()
        {
            var people = new List<Person>
            {
                new Person(1, "nameOne"),
                new Person(2, "nameTwo"),
                new Person(4, "nameFour"),
                new Person(5, "nameFive"),
                new Person(3, "nameThree"),
                new Person(13, "xnameThree"),
                new Person(11, "xnameOne"),
                new Person(12, "xnameTwo"),
                new Person(14, "xnameFour"),
                new Person(15, "xnameFive")
            };

            var rankingData = new List<CountryRanking>
            {
                new CountryRanking(3, "country", 1),
                new CountryRanking(4, "country", 1),
                new CountryRanking(1, "country", 1),
                new CountryRanking(5, "country", 1),
                new CountryRanking(2, "country", 1),
                new CountryRanking(14, "country", 2),
                new CountryRanking(15, "country", 2),
                new CountryRanking(12, "country", 2),
                new CountryRanking(13, "country", 2),
                new CountryRanking(11, "country", 2)
            };

            var expectedResults = new List<RankedResult>()
            {
                new RankedResult(5, 1),
                new RankedResult(4, 1),
                new RankedResult(1, 1),
                new RankedResult(3, 1),
                new RankedResult(2, 1),
                new RankedResult(15, 2),
                new RankedResult(14, 2),
                new RankedResult(11, 2),
                new RankedResult(13, 2),
                new RankedResult(12, 2)
            };

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string> { "country" },
                1, int.MaxValue,
                int.MaxValue
            );

            CollectionAssert.AreEqual(expectedResults, actualResults);
        }

        [Test]
        [Category("provided")]
        [Category("requirement-filterByRank")]
        public void TestFilteringByRank()
        {
            var people = new List<Person>
            {
                new Person(1, "name"),
                new Person(2, "name"),
                new Person(3, "name"),
                new Person(4, "name"),
                new Person(5, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                new CountryRanking(1, "country", 1),
                new CountryRanking(2, "country", 2),
                new CountryRanking(3, "country", 3),
                new CountryRanking(4, "country", 4),
                new CountryRanking(5, "country", 5)
            };

            var expectedResults = new HashSet<RankedResult>()
            {
                new RankedResult(2, 2),
                new RankedResult(3, 3),
                new RankedResult(4, 4)
            };

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string>() { "country" },
                2, 4,
                100
            );

            // we don't care about ordering for this test
            CollectionAssert.AreEquivalent(expectedResults, new HashSet<RankedResult>(actualResults));
        }


        [Test]
        [Category("provided")]
        [Category("requirement-maxCount")]
        public void TestWhenThereAreLessResultsThanMaxCount_withTies()
        {
            var people = new List<Person>
            {
                new Person(1, "name"),
                new Person(2, "name"),
                new Person(3, "name"),
                new Person(4, "name"),
                new Person(5, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                new CountryRanking(1, "countryOne", 1),
                new CountryRanking(2, "countryTwo", 1),
                new CountryRanking(3, "countryThree", 1),
                new CountryRanking(4, "countryOne", 2),
                new CountryRanking(5, "countryTwo", 2)
            };

            var expectedResults = new HashSet<RankedResult>()
            {
                new RankedResult(1, 1),
                new RankedResult(2, 1),
                new RankedResult(4, 2),
                new RankedResult(5, 2)
            };

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string>() { "countryOne", "countryTwo" },
                1, 15,
                4
            );

            // we don't care about ordering for this test
            CollectionAssert.AreEquivalent(expectedResults, new HashSet<RankedResult>(actualResults));
        }

        [Test]
        [Category("user")]
        [Category("requirement-maxCount")]
        public void TestWhenThereAreMoreResultsThanMaxCount_butNoRankTies()
        {
            var people = new List<Person>
            {
                new Person(1, "name"),
                new Person(2, "name"),
                new Person(5, "name"),
                new Person(3, "name"),
                new Person(4, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                // odd numbering used here to ensure no interaction between rank and maxCount
                new CountryRanking(2, "country", 7),
                new CountryRanking(1, "country", 9),
                new CountryRanking(3, "country", 5),
                new CountryRanking(4, "country", 3),
                new CountryRanking(5, "country", 1)
            };

            var expectedResults = new HashSet<RankedResult>()
            {
                new RankedResult(5, 1),
                new RankedResult(4, 3),
                new RankedResult(3, 5),
                new RankedResult(2, 7),
                new RankedResult(1, 9)
            };

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string> { "country" },
                1, int.MaxValue,
                expectedResults.Count + 1
            );

            // we don't care about ordering for this test
            CollectionAssert.AreEquivalent(expectedResults, new HashSet<RankedResult>(actualResults));
        }

        [Test]
        [Category("user")]
        [Category("requirement-maxCount")]
        public void TestWhenThereAreMoreResultsThanMaxCount_andWithRankTies()
        {
            var people = new List<Person>
            {
                new Person(3, "name"),
                new Person(4, "name"),
                new Person(1, "name"),
                new Person(2, "name"),
                new Person(5, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                // odd numbering used here to ensure no interaction between rank and maxCount
                new CountryRanking(3, "country", 5),
                new CountryRanking(1, "country", 9),
                new CountryRanking(5, "country", 1),
                new CountryRanking(2, "country", 5),
                new CountryRanking(4, "country", 1)
            };

            var expectedResults = new HashSet<RankedResult>()
            {
                new RankedResult(4, 1),
                new RankedResult(5, 1),
                new RankedResult(2, 5),
                new RankedResult(3, 5)
            };

            var actualResults = FilterByCountryWithRank(
                people,
                rankingData,
                new List<string> { "country" },
                1, int.MaxValue,
                3
            );

            // we don't care about ordering for this test
            CollectionAssert.AreEquivalent(expectedResults, new HashSet<RankedResult>(actualResults));
        }


        [Test]
        [Category("provided")]
        [Category("edgecase-zeroResults")]
        public void TestWhenNoResults()
        {
            var people = new List<Person>
            {
                new Person(1, "name")
            };

            var rankingData = new List<CountryRanking>
            {
                new CountryRanking(1, "country", 10)
            };
            
            {
                // one option
                var actualResults = FilterByCountryWithRank(
                    people,
                    rankingData,
                    new List<string>() { "notFound" },
                    int.MinValue, int.MaxValue,
                    int.MaxValue
                );

                CollectionAssert.IsEmpty(actualResults);
            }

            {
                // another option
                var actualResults = FilterByCountryWithRank(
                    people,
                    rankingData,
                    new List<string>() { "country" },
                    0, 0,
                    int.MaxValue
                );

                CollectionAssert.IsEmpty(actualResults);
            }

            {
                // and another option
                var actualResults = FilterByCountryWithRank(
                    people,
                    rankingData,
                    new List<string>() { "country" },
                    int.MinValue, int.MaxValue,
                    0
                );

                CollectionAssert.IsEmpty(actualResults);
            }
        }

        // TODO: what other cases are missing?
        //  - Are all requirements checked?
        //  - Little details of each requirement?
        //  - Edge cases?
    }
}