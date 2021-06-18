using Biosearcher.LandManagement.Chunks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.LandManagement
{
    public class ChunkTracker : System.IDisposable
    {
        protected TriggerTracker triggerTracker;
        protected MonoBehaviour behaviour;
        protected LandSettings settings;
        protected Transform trigger;

        protected Dictionary<Vector3Int, ChunkWithGeometry>[] ChunkSize2GeometryChunks;
        protected Vector3Int[] ChunkSize2PlayerPosition;

        public ChunkTracker(MonoBehaviour behaviour, LandSettings settings, Transform trigger, float preGenerationDuration)
        {
            this.behaviour = behaviour;
            this.settings = settings;
            this.trigger = trigger;

            Initialize();
            behaviour.StartCoroutine(PreGenerating(preGenerationDuration));
        }

        protected IEnumerator PreGenerating(float duration)
        {
            yield return null;
            while (duration > 0)
            {
                TriggerPositionChanged(Chunk.RoundPosition(Vector3Int.RoundToInt(trigger.position), 0), 0, true);

                duration -= Time.deltaTime;
                yield return null;
            }
        }

        protected void Initialize()
        {
            int arraySize = settings.MaxHierarchySize + 1;
            ChunkSize2GeometryChunks = new Dictionary<Vector3Int, ChunkWithGeometry>[arraySize];
            ChunkSize2PlayerPosition = new Vector3Int[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                ChunkSize2GeometryChunks[i] = new Dictionary<Vector3Int, ChunkWithGeometry>();
                ChunkSize2PlayerPosition[i] = Chunk.RoundPosition(Vector3Int.RoundToInt(trigger.position), i);
            }

            TryCreateTracker(trigger);
        }

        protected void TryCreateTracker(Transform trigger)
        {
            if (trigger != null)
            {
                triggerTracker = new TriggerTracker(this, trigger, behaviour);
            }
            else
            {
                Debug.LogWarning("You should assign Trigger value in Land script");
            }
        }

        private void ForEach<T>(Queue<T> queue, UnityAction<T> action)
        {
            int size = queue.Count;
            for (int i = 0; i < size; i++)
            {
                action?.Invoke(queue.Dequeue());
            }
        }

        private void GetChunksToChange(
            int chunkSize,
            Vector3Int triggerPosition,
            out Queue<ChunkWithGeometry> chunksToUnUniteIntoParent,
            out Queue<ChunkWithGeometry> chunksToUniteIntoParent,
            out Queue<ChunkWithGeometry> chunksToDivide)
        {
            chunksToUnUniteIntoParent = new Queue<ChunkWithGeometry>();
            chunksToUniteIntoParent = new Queue<ChunkWithGeometry>();
            chunksToDivide = new Queue<ChunkWithGeometry>();

            foreach (KeyValuePair<Vector3Int, ChunkWithGeometry> chunkPair in ChunkSize2GeometryChunks[chunkSize])
            {
                if (Chunk.IsChunkWrongSize(chunkPair.Key, chunkPair.Value.HierarchySize, triggerPosition, out Chunk.WrongSizeType changeType))
                {
                    switch (changeType)
                    {
                        case Chunk.WrongSizeType.TooSmall:
                            chunksToUniteIntoParent.Enqueue(chunkPair.Value);
                            break;
                        case Chunk.WrongSizeType.TooLarge:
                            chunksToDivide.Enqueue(chunkPair.Value);
                            break;
                    }
                }
                else
                {
                    chunksToUnUniteIntoParent.Enqueue(chunkPair.Value);
                }
            }
        }

        protected void TriggerPositionChanged(Vector3Int triggerPosition, int chunkSize, bool isPreGenerating = false)
        {
            ChunkSize2PlayerPosition[chunkSize] = triggerPosition;

            GetChunksToChange(chunkSize, triggerPosition,
                out Queue<ChunkWithGeometry> chunksToUnUniteIntoParent,
                out Queue<ChunkWithGeometry> chunksToUniteIntoParent,
                out Queue<ChunkWithGeometry> chunksToDivide);

            ForEach(chunksToUnUniteIntoParent, chunk => chunk.TryUnUniteIntoParent());
            ForEach(chunksToUniteIntoParent, chunk => chunk.TryUniteIntoParent());
            ForEach(chunksToDivide, chunk => chunk.Divide());

            int nextChunkSize = chunkSize + 1;
            if (nextChunkSize > settings.MaxHierarchySize)
            {
                return;
            }
            Vector3Int nextSizePosition = Chunk.RoundPosition(triggerPosition, nextChunkSize);
            if (nextSizePosition != ChunkSize2PlayerPosition[nextChunkSize] || isPreGenerating)
            {
                TriggerPositionChanged(nextSizePosition, nextChunkSize, isPreGenerating);
            }
        }

        public void SetTriggerPosition(Vector3Int triggerPosition)
        {
            Vector3Int nextSizePosition = Chunk.RoundPosition(triggerPosition, 0);
            if (nextSizePosition != ChunkSize2PlayerPosition[0])
            {
                TriggerPositionChanged(nextSizePosition, 0);
            }
        }

        public void TrackChunk(ChunkWithGeometry chunk) => ChunkSize2GeometryChunks[chunk.HierarchySize].Add(chunk.Position, chunk);

        public void UnTrackChunk(ChunkWithGeometry chunk) => ChunkSize2GeometryChunks[chunk.HierarchySize].Remove(chunk.Position);

        public void Dispose() => triggerTracker.Dispose();
    }
}