using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandManagement.CubeMarching.CPU
{
    internal struct TempPointsBuffer
    {
        internal MarchPoint[] points;
        internal Vector3 chunkPosition;
        internal int cubeSize;
    }

    internal struct TempMeshBuffer
    {
        internal MarchPoint[] points;
        internal List<Vector3> vertices;
        internal List<int> triangles;
        internal int counter;
    }

    internal struct ConstantBuffer
    {
        // EdgeConnection lists the index of the endpoint vertices for each 
        // of the 12 edges of the cube.
        // edgeConnection[12][2]
        internal int[,] edgeIndex2PointIndexes;

        // For any edge, if one vertex is inside of the surface and the other 
        // is outside of the surface then the edge intersects the surface.
        // For each of the 8 vertices of the cube can be two possible states,
        // either inside or outside of the surface.
        // For any cube the are 2^8=256 possible sets of vertex states.
        // This table lists the edges intersected by the surface for all 256 
        // possible vertex states. There are 12 edges.  
        // For each entry in the table, if edge #n is intersected, then bit #n is set to 1.
        // cubeEdgeFlags[256]
        internal int[] pointsHash2EdgesHash;

        // For each of the possible vertex states listed in cubeEdgeFlags there is a specific triangulation
        // of the edge intersection points.  triangleConnectionTable lists all of them in the form of
        // 0-5 edge triples with the list terminated by the invalid value -1.
        // For example: triangleConnectionTable[3] list the 2 triangles formed when corner[0] 
        // and corner[1] are inside of the surface, but the rest of the cube is not.
        // triangleConnectionTable[256][16]
        internal int[,] pointsHash2EdgesIndexes;

        internal float surfaceValue;
        internal int pointsPerChunk1D;
        internal int cubesPerChunk1D;
        internal float seed;

    }
}
