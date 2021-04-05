using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.Planet.Managing
{
    // todo
    public class ChunkDistanceManager
    {
        //protected Dictionary<Vector3Int, ChunkHolder> chunks = new Dictionary<Vector3Int, ChunkHolder>();
        //public Vector3Int TriggerPosition { get; set; }
        //public Vector3 TriggerPositionFloat
        //{
        //    set => TriggerPosition = Vector3Int.RoundToInt(value);
        //}

        //public ChunkDistanceManager()
        //{
        //    // todo
        //}

        //public void Initialize(ChunkHolder mainChunkHolder)
        //{
        //    ChunkWithGeometry chunk = mainChunkHolder.Chunk as ChunkWithGeometry;
        //    float distanceToTrigger = DistanceToTrigger(chunk.Position);

        //    if (distanceToTrigger < Chunk.Size2DistanceRange(chunk.Size).min && chunk.Size > 0)
        //    {
        //        chunk.Clear();
        //        mainChunkHolder.Chunk = new ChunkWithChunks(chunk.Position, chunk.Size, mainChunkHolder);
        //        break;
        //    }
        //    if (Parent == null)
        //    {
        //        yield return new WaitForSeconds(period);
        //        continue;
        //    }
        //    if (DistanceToTrigger > Chunk.Size2DistanceRange(chunk.Size).max)
        //    {
        //        Parent.TryCollapse(this);
        //    }
        //    else
        //    {
        //        Parent.UnTryCollapse(this);
        //    }
        //}

        //protected float DistanceToTrigger(Vector3Int chunkPosition) => (TriggerPosition - chunkPosition).magnitude;
    }
}