using NServiceBus;
using EpiFlow.Data.Raven;

namespace EpiFlow.Messages.Events
{
    public interface IEpisodeFoundEvent : IEvent
    {
        EpisodeConversion Episode { get; set; }
    }
}
