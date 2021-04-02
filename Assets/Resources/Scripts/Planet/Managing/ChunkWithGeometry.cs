using System.Collections;
using UnityEngine;

namespace Biosearcher.Planet.Managing
{
    public class ChunkWithGeometry : Chunk
    {
        GeometryHolder geometryHolder;

        protected internal ChunkWithGeometry(Vector3Int position, int size, ChunkHolder holder) : base(position, size, holder)
        {
            geometryHolder = holder.ChunkManager.GenerateChunk(position, 1 << size, holder);
        }

        protected internal void Clear()
        {
            geometryHolder.Geometry.Clear();
        }

        protected internal override void DrawGizmos()
        {
            float colorValue = Size / 8f;
            Gizmos.color = new Color(colorValue, colorValue, colorValue);
            int fullSize = Size2ActualSize(Size);
            Gizmos.DrawWireCube(Position, new Vector3(fullSize, fullSize, fullSize));
        }
    }

    public class GeometryHolder
    {
        public IGeometry Geometry { get; set; }
    }

    public interface IGeometry
    {
        void Clear();
    }

    public struct Geometry : IGeometry
    {
        public Mesh mesh;
        public GameObject chunkObject;

        public void Clear()
        {
            Object.Destroy(mesh);
            Object.Destroy(chunkObject);
        }
    }

    public struct GeometryCredit : IGeometry
    {
        public System.Action onCancel;

        public void Clear()
        {
            onCancel?.Invoke();
        }
    }
}