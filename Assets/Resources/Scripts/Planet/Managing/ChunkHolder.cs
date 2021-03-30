using System.Collections;
using UnityEngine;

namespace Biosearcher.Planet.Managing
{
    public class ChunkHolder
    {
        protected internal Chunk Chunk { get; protected set; }
        protected internal ChunkWithChunks Parent { get; protected set; }
        protected internal ChunkManager ChunkManager { get; protected set; }
        protected internal Transform Trigger { get; protected set; }

        protected bool isAlive = true;

        protected float DistanceToTrigger => (Trigger.position - Chunk.Position).magnitude;

        protected internal ChunkHolder(ChunkWithChunks parent, ChunkManager chunkManager)
        {
            Parent = parent;
            ChunkManager = chunkManager;
            Trigger = ChunkManager.Trigger;
        }

        protected internal void Initialize(ChunkWithGeometry chunk)
        {
            Chunk = chunk;

            StartUpdatePeriod(Chunk.Size2UpdatePeriod(chunk.Size));
        }
        protected internal void Initialize(ChunkWithChunks chunk)
        {
            Chunk = chunk;
        }
        protected internal void Initialize(Chunk chunk)
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

        protected internal void Clear()
        {
            isAlive = false;

            (Chunk as ChunkWithGeometry).Clear();
        }
        // todo: change all public to protected internal
        protected internal void Collapse() => Initialize(new ChunkWithGeometry(Chunk.Position, Chunk.Size, this));

        protected void StartUpdatePeriod(float period)
        {
            ChunkManager.StartCoroutine(UpdateChunk(period));
        }

        protected IEnumerator UpdateChunk(float period)
        {
            while (isAlive && Chunk is ChunkWithGeometry chunkWithGeometry)
            {
                if (DistanceToTrigger < Chunk.Size2DistanceRange(Chunk.Size).min && Chunk.Size > 0)
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
                if (DistanceToTrigger > Chunk.Size2DistanceRange(Chunk.Size).max)
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