using NUnit.Framework;
using EpiFlow.DataAccess;
using AutoMoq.Helpers;
using Shouldly;
using System.Linq;

namespace EpiFlow.UnitTests.DataAccess
{
    [TestFixture]
    public class WhenWeAreSearchingForSeries : AutoMoqTestFixture<SiteReader>
    {
        [Test, Combinatorial]
        public void EdgeCasesDoNotThrow(
            [Values("the office", "asdfljadsfl", null, "", StaticTestData.SuperLongString)]string searchString,
            [Values(true, false)]bool withDetails
            )
        {
            Should.NotThrow(() => Subject.SearchSeries(searchString, withDetails));
        }

        [Test, Combinatorial]
        public void NoResultsAreFound(
            [Values("asdfljadsfl", null, "")]string searchString,
            [Values(true, false)]bool withDetails
            )
        {
            Subject.SearchSeries(searchString, withDetails).ShouldBeEmpty();
        }

        [Test]
        public void ResultsAreFoundWithoutDetails()
        {
            var result = Subject.SearchSeries("the office", false).First();
            result.ShouldSatisfyAllConditions(
                () => result.Id.ShouldNotBeNull(),
                () => result.SeriesName.ShouldNotBeEmpty(),
                () => result.IMDB_ID.ShouldNotBeEmpty(),
                () => result.lastupdated.ShouldNotBeEmpty(),
                () => result.Overview.ShouldNotBeEmpty(),
                () => result.Seasons.ShouldBeEmpty()
                );
        }

        [Test]
        public void ResultsAreFoundWithDetails()
        {
            var result = Subject.SearchSeries("the office", true).First();
            result.ShouldSatisfyAllConditions(
                () => result.Id.ShouldNotBeNull(),
                () => result.SeriesName.ShouldNotBeEmpty(),
                () => result.IMDB_ID.ShouldNotBeEmpty(),
                () => result.lastupdated.ShouldNotBeEmpty(),
                () => result.Overview.ShouldNotBeEmpty(),
                () => result.Seasons.ShouldNotBeEmpty()
                );
        }
    }
}
