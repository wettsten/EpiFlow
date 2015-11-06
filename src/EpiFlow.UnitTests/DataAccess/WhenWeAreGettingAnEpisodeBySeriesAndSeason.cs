using NUnit.Framework;
using EpiFlow.DataAccess;
using AutoMoq.Helpers;
using Shouldly;

namespace EpiFlow.UnitTests.DataAccess
{
    [TestFixture]
    public class WhenWeAreGettingAnEpisodeBySeriesAndSeason : AutoMoqTestFixture<SiteReader>
    {
        [Test, Combinatorial]
        public void EdgeCasesDoNotThrow(
            [Values(int.MinValue, 0, int.MaxValue)]int seriesId,
            [Values(int.MinValue, 0, int.MaxValue)]int seasonNumber,
            [Values(int.MinValue, 0, int.MaxValue)]int episodeNumber)
        {
            Should.NotThrow(() => Subject.GetSeasonEpisode(seriesId, seasonNumber, episodeNumber));
        }

        [Test]
        public void NoEpisodeIsFound()
        {
            Subject.GetSeasonEpisode(0, 0, 0).ShouldBeNull();
        }

        [Test]
        public void EpisodeIsFound()
        {
            var result = Subject.GetSeasonEpisode(78107, 1, 4);
            result.ShouldSatisfyAllConditions(
                () => result.Id.ShouldNotBeNull(),
                () => result.EpisodeName.ShouldNotBeEmpty(),
                () => result.SeasonNumber.ShouldNotBeNull(),
                () => result.EpisodeNumber.ShouldNotBeNull(),
                () => result.Overview.ShouldNotBeEmpty()
                );
        }
    }
}
