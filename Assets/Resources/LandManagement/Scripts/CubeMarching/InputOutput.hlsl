#ifndef BIOSEARCHER_INPUT_OUTPUT
#define BIOSEARCHER_INPUT_OUTPUT

#include "Structures.hlsl"

// EdgeConnection lists the index of the endpoint vertices for each 
// of the 12 edges of the cube.
// edgeConnection[12][2]
uniform Texture2D<float> _EdgeIndex2PointIndexesT;

// For any edge, if one vertex is inside of the surface and the other 
// is outside of the surface then the edge intersects the surface.
// For each of the 8 vertices of the cube can be two possible states,
// either inside or outside of the surface.
// For any cube the are 2^8=256 possible sets of vertex states.
// This table lists the edges intersected by the surface for all 256 
// possible vertex states. There are 12 edges.  
// For each entry in the table, if edge #n is intersected, then bit #n is set to 1.
// cubeEdgeFlags[256]
uniform Texture2D<float> _PointsHash2EdgesHashT;

// For each of the possible vertex states listed in cubeEdgeFlags there is a specific triangulation
// of the edge intersection points.  triangleConnectionTable lists all of them in the form of
// 0-5 edge triples with the list terminated by the invalid value -1.
// For example: triangleConnectionTable[3] list the 2 triangles formed when corner[0] 
// and corner[1] are inside of the surface, but the rest of the cube is not.
// triangleConnectionTable[256][16]
uniform Texture2D<float> _PointsHash2EdgesIndexesT;


uniform RWStructuredBuffer<MarchPoint> _Points;
uniform RWStructuredBuffer<float4> _MeshV3T1;
uniform float3 _ChunkPosition;
uniform float _SurfaceValue;
uniform int _CubeSize;

uniform uint _PointsPerChunk;
uniform uint _CubesPerChunk;

uniform float _Seed;

#endif
