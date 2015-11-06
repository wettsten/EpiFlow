using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using EpiFlow.Common;

namespace EpiFlow.ManualUI
{
    public class DependencyRegistry : Registry
    {
        public DependencyRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                    scan.AssemblyContainingType<IEpisodeSearcher>();
                });
        }
    }
}
