using Biosearcher.Refactoring;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandManagement.Chunks
{
    [NeedsRefactor("Split to unitialized chunk&geometry and itialized chunk&geometry.")]
    public struct Geometry
    {
        internal GameObject chunkObject;
        internal Mesh chunkMesh;
        internal IEnumerable<GameObject> plants;
        internal Ray[] normals;
    }
}