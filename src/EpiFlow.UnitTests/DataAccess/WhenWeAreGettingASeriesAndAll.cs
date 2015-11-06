using NUnit.Framework;
using EpiFlow.DataAccess;
using AutoMoq.Helpers;
using Shouldly;

namespace EpiFlow.UnitTests.DataAccess
{
    [TestFixture]
    public class WhenWeAreGettingASeriesAndAll : AutoMoqTestFixture<SiteReader>
    {
        [Test]
        public void EdgeCasesDoNotThrow([Values(int.MinValue, 0, int.MaxValue)]int inputData)
        {
            Should.NotThrow(() => Subject.GetSeriesAll(inputData));
        }

        [Test]
        public void NoSeriesIsFound()
        {
            Subject.GetSeriesAll(0).ShouldBeNull();
        }

        [Test]
        public void SeriesIsFound()
        {
            var result = Subject.GetSeriesAll(78107);
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
