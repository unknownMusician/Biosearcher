using Biosearcher.Common;
using Biosearcher.LandManagement.Chunks;
using Biosearcher.LandManagement.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

namespace Biosearcher.LandManagement
{
    public class ChunkTracker : System.IDisposable
    {
        protected TriggerTracker _triggerTracker;
        protected LandSettings _settings;
        protected Transform _trigger;

        protected Dictionary<Vector3Int, ChunkWithGeometry>[] _chunkSize2GeometryChunks;
        protected Vector3Int[] _chunkSize2PlayerPosition;

        public ChunkTracker(LandSettings settings, Transform trigger)
        {
            _settings = settings;
            _trigger = trigger;

            Initialize();
            CommonMonoBehaviour.StartCoroutine(PreGenerating(settings.PreGenerationDuration));
        }

        protected IEnumerator PreGenerating(float duration)
        {
            yield return null;
            while (duration > 0)
            {
                TriggerPositionChanged(Chunk.RoundPosition(Vector3Int.RoundToInt(_trigger.position), 0), 0, true);

                duration -= Time.deltaTime;
                yield return null;
            }
        }

        protected void Initialize()
        {
            int arraySize = _settings.MaxHierarchySize + 1;
            _chunkSize2GeometryChunks = new Dictionary<Vector3Int, ChunkWithGeometry>[arraySize];
            _chunkSize2PlayerPosition = new Vector3Int[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                _chunkSize2GeometryChunks[i] = new Dictionary<Vector3Int, ChunkWithGeometry>();
                _chunkSize2PlayerPosition[i] = Chunk.RoundPosition(Vector3Int.RoundToInt(_trigger.position), i);
            }

            TryCreateTracker(_trigger);
        }

        protected void TryCreateTracker(Transform trigger)
        {
            if (trigger != null)
            {
                _triggerTracker = new TriggerTracker(this, trigger);
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

            foreach (KeyValuePair<Vector3Int, ChunkWithGeometry> chunkPair in _chunkSize2GeometryChunks[chunkSize])
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
            Profiler.BeginSample("ChunkTracker.TriggerPositionChanged");
            _chunkSize2PlayerPosition[chunkSize] = triggerPosition;

            GetChunksToChange(chunkSize, triggerPosition,
                out Queue<ChunkWithGeometry> chunksToUnUniteIntoParent,
                out Queue<ChunkWithGeometry> chunksToUniteIntoParent,
                out Queue<ChunkWithGeometry> chunksToDivide);

            ForEach(chunksToUnUniteIntoParent, chunk => chunk.TryUnUniteIntoParent());
            ForEach(chunksToUniteIntoParent, chunk => chunk.TryUniteIntoParent());
            ForEach(chunksToDivide, chunk => chunk.Divide());

            chunkSize++;
            if (chunkSize > _settings.MaxHierarchySize)
            {
                Profiler.EndSample();
                return;
            }
            triggerPosition = Chunk.RoundPosition(triggerPosition, chunkSize);
            if (triggerPosition != _chunkSize2PlayerPosition[chunkSize] || isPreGenerating)
            {
                Profiler.EndSample();
                TriggerPositionChanged(triggerPosition, chunkSize, isPreGenerating);
            }
            else
            {
                Profiler.EndSample();
            }
        }

        public void SetTriggerPosition(Vector3Int triggerPosition)
        {
            Vector3Int nextSizePosition = Chunk.RoundPosition(triggerPosition, 0);
            if (nextSizePosition != _chunkSize2PlayerPosition[0])
            {
                TriggerPositionChanged(nextSizePosition, 0);
            }
        }

        public void TrackChunk(ChunkWithGeometry chunk) => _chunkSize2GeometryChunks[chunk.HierarchySize].Add(chunk.Position, chunk);
        public void UnTrackChunk(ChunkWithGeometry chunk) => _chunkSize2GeometryChunks[chunk.HierarchySize].Remove(chunk.Position);

        public void Dispose() => _triggerTracker.Dispose();
    }
}