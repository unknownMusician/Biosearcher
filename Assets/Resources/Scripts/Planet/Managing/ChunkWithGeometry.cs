using System.Collections;
using UnityEngine;

namespace Biosearcher.Planet.Managing
{
    public class ChunkWithGeometry : Chunk
    {
        protected Mesh mesh;
        protected GameObject chunkObject;

        protected internal ChunkWithGeometry(Vector3Int position, int size, ChunkHolder holder) : base(position, size, holder)
        {
            holder.ChunkManager.GenerateChunk(position, 1 << size, holder, out mesh, out chunkObject);
        }

        protected internal void Clear()
        {
            Object.Destroy(mesh);
            Object.Destroy(chunkObject);
        }

        protected internal override void DrawGizmos()
        {
            float colorValue = Size / 8f;
            Gizmos.color = new Color(colorValue, colorValue, colorValue);
            int fullSize = Size2ActualSize(Size);
            Gizmos.DrawWireCube(Position, new Vector3(fullSize, fullSize, fullSize));
        }
    }
}