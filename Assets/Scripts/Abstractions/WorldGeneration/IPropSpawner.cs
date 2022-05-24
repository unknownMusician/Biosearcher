using System.Collections.Generic;

namespace Biosearcher.WorldGeneration
{
    public interface IPropSpawner<in TInfo, TProp>
    {
        public void Spawn(List<TProp> spawned, TInfo info);
    }
}
