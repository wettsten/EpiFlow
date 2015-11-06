using NUnit.Framework;
using EpiFlow.DataAccess;
using AutoMoq.Helpers;
using Shouldly;

namespace EpiFlow.UnitTests.DataAccess
{
    [TestFixture]
    public class WhenWeAreGettingAnEpisode : AutoMoqTestFixture<SiteReader>
    {
        [Test]
        public void EdgeCasesDoNotThrow([Values(int.MinValue, 0, int.MaxValue)]int inputData)
        {
            Should.NotThrow(() => Subject.GetEpisode(inputData));
        }

        [Test]
        public void NoEpisodeIsFound()
        {
            Subject.GetEpisode(0).ShouldBeNull();
        }

        [Test]
        public void EpisodeIsFound()
        {
            var result = Subject.GetEpisode(272173);
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
