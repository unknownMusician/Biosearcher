using System.Collections;
using UnityEngine;

namespace Biosearcher.Land.Managing
{
    public class ChunkHolder
    {
        protected internal Chunk Chunk { get; protected set; }
        protected internal ChunkWithChunks Parent { get; }
        protected internal ChunkManager ChunkManager { get; }
        protected internal Transform Trigger { get; }

        protected bool isAlive = true;

        protected float DistanceToTrigger => (Trigger.position - Chunk.Position).magnitude;

        public ChunkHolder(ChunkWithChunks parent, ChunkManager chunkManager)
        {
            Parent = parent;
            ChunkManager = chunkManager;
            Trigger = ChunkManager.Trigger;
        }

        public void Initialize(ChunkWithGeometry chunk)
        {
            Chunk = chunk;

            StartUpdatePeriod(Chunk.size2UpdatePeriod[chunk.Size]);
        }
        public void Initialize(ChunkWithChunks chunk)
        {
            Chunk = chunk;

            // todo
        }
        public void Initialize(Chunk chunk)
        {
            if (chunk is ChunkWithChunks chunkWithChunks)
            {
                Initialize(chunkWithChunks);
            }
            else if (chunk is ChunkWithGeometry chunkWithGeometry)
            {
                Initialize(chunkWithGeometry);
            }
        }

        public void Clear()
        {
            isAlive = false;

            (Chunk as ChunkWithGeometry).Clear();
            // todo
        }
        // todo: change all public to internal protected
        public void Collapse() => Initialize(new ChunkWithGeometry(Chunk.Position, Chunk.Size, this));

        protected void StartUpdatePeriod(float period)
        {
            ChunkManager.StartCoroutine(Update(period));
        }

        protected IEnumerator Update(float period)
        {
            while (isAlive && Chunk is ChunkWithGeometry chunkWithGeometry)
            {
                if (DistanceToTrigger < Chunk.size2DistanceRange[Chunk.Size, 0] && Chunk.Size > 0)
                {
                    chunkWithGeometry.Clear();
                    Chunk = new ChunkWithChunks(Chunk.Position, Chunk.Size, this);
                    break;
                }
                if(Parent == null)
                {
                    yield return new WaitForSeconds(period);
                    continue;
                }
                if (DistanceToTrigger > Chunk.size2DistanceRange[Chunk.Size, 1])
                {
                    Parent.TryCollapse(this);
                }
                else
                {
                    Parent.UnTryCollapse(this);
                }
                yield return new WaitForSeconds(period);
            }
        }
    }
}