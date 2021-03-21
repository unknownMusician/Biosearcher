using System.Collections;
using UnityEngine;

namespace Biosearcher.Land.Managing
{
    public class ChunkWithGeometry : Chunk
    {
        protected Mesh mesh;
        protected GameObject chunkObject;

        public ChunkWithGeometry(Vector3Int position, int size, ChunkHolder holder) : base(position, size, holder)
        {
            holder.ChunkManager.GenerateChunk(position, 1 << size, out mesh, out chunkObject);
            // todo: draw geometry
        }

        public void Clear()
        {
            Object.Destroy(mesh);
            Object.Destroy(chunkObject);
            Debug.Log("Cleared mesh and obj"); // todo
            Debug.DrawLine(chunkObject.transform.position, chunkObject.transform.position + Vector3.up, Color.blue, 20); // todo
            // todo
        }

        public override void DrawGizmos()
        {
            float colorValue = Size / 8f;
            Gizmos.color = new Color(colorValue, colorValue, colorValue);
            int fullSize = 6 * (1 << Size);
            Gizmos.DrawWireCube(Position, new Vector3(fullSize, fullSize, fullSize));
        }
    }
}