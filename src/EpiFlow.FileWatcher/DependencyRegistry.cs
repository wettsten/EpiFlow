using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using EpiFlow.Common;

namespace EpiFlow.FileWatcher
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
