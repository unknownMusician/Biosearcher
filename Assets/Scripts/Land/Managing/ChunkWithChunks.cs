using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.Land.Managing
{
    public class ChunkWithChunks : Chunk
    {
        protected ChunkHolder[] children;
        protected List<ChunkHolder> willingToCollapse = new List<ChunkHolder>();

        public ChunkWithChunks(Vector3Int position, int size, ChunkHolder holder) : base(position, size, holder)
        {
            children = new ChunkHolder[8];
            for (int z = 0; z < 2; z++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        int newSize = size - 1;
                        int newActualSize = (1 << newSize) * 6;
                        int deltaX = newActualSize / 2 * (x * 2 - 1);
                        int deltaY = newActualSize / 2 * (y * 2 - 1);
                        int deltaZ = newActualSize / 2 * (z * 2 - 1);
                        Vector3Int newPosition = position + new Vector3Int(deltaX, deltaY, deltaZ);
                        var child = new ChunkHolder(this, holder.ChunkManager);
                        child.Initialize(Chunk.Create(newPosition, newSize, holder, holder.Trigger.position));
                        children[x + y * 2 + z * 4] = child;
                    }
                }
            }
        }

        // todo: remove
        protected void Clear()
        {
            foreach (ChunkHolder childholder in children)
            {
                childholder.Clear();
            }
        }

        public override void DrawGizmos()
        {
            foreach (ChunkHolder childholder in children)
            {
                childholder.Chunk.DrawGizmos();
            }
        }

        public void TryCollapse(ChunkHolder child)
        {
            if (!willingToCollapse.Contains(child))
            {
                willingToCollapse.Add(child);
            }
            TryCollapse();
        }

        public void UnTryCollapse(ChunkHolder child)
        {
            // todo: check if is an actual child
            if (willingToCollapse.Contains(child))
            {
                willingToCollapse.Remove(child);
            }
        }

        protected void TryCollapse()
        {
            if(willingToCollapse.Count != children.Length || willingToCollapse.Count < 8)
            {
                return;
            }
            Clear();
            holder.Collapse();
        }
    }
}