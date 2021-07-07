#ifndef BIOSEARCHER_COMMON
#define BIOSEARCHER_COMMON

#include "Structures.hlsl"
#include "InputOutput.hlsl"

uint MatrixId2ArrayId(uint x, uint y, uint z, int base)
{
    return x + y * base + z * base * base;
}

uint MatrixId2ArrayId(uint3 id, int base)
{
    return MatrixId2ArrayId(id.x, id.y, id.z, base);
}

MarchCube GenerateCube(RWStructuredBuffer<MarchPoint> points, uint3 id)
{
    MarchPoint cubePoints[8];
    //int counter = 0;
    
    int x, y, z;
    for (int i = 0; i < 8; i++)
    {
        int x = i & 1;
        int z = (i & 2) >> 1;
        int y = (i & 4) >> 2;
        uint localXIndex = ((1 - x) & z) | (x & (1 - z));
        uint localYIndex = y;
        uint localZIndex = 1 - z;
        cubePoints[i] = points[MatrixId2ArrayId(id.x + localXIndex, id.y + localYIndex, id.z + localZIndex, _PointsPerChunk1D)];
    }
    
    MarchCube marchCube;
    marchCube.points = cubePoints;
    return marchCube;
}

int GetPointsHash(MarchCube cube, float surfaceValue)
{
    int cubeIndex = 0;
    // Find which vertices are inside of the surface and which are outside
    for (int i = 0; i < 8; i++)
    {
        if (cube.points[i].value <= surfaceValue)
        {
            cubeIndex |= 1 << i;
        }
    }
    return cubeIndex;
}

float3 Interpolate(MarchPoint point1, MarchPoint point2, float surfaceValue)
{
    float lerp = (surfaceValue - point1.value) / (point2.value - point1.value);
    float3 deltaPosition = point2.position - point1.position;
    return point1.position + deltaPosition * lerp;

    // return (point1.Position + point2.Position) / 2;
}

void AddFace(float3 face[3], uint faceId, uint cubeArrayId)
{
    if (cubeArrayId >= _CubesPerChunk1D * _CubesPerChunk1D * _CubesPerChunk1D)
    {
        return;
    }
    
    int faceGlobalId = cubeArrayId * 5 + faceId;
    
    for (int i = 0; i < 3; i++)
    {
        int vertexId = faceId * 3 + i;
        int vertexGlobalId = faceGlobalId * 3 + i;
        _MeshV3T1[vertexId + cubeArrayId * 15] = float4(face[i], vertexGlobalId); // todo: warning (if dimensions == 8)
    }
}

void March(MarchCube cube, float surfaceValue, uint cubeArrayId)
{
    int pointsHash = GetPointsHash(cube, surfaceValue);
    
    // Find which edges are intersected by the surface
    int edgesHash = _PointsHash2EdgesHashT[uint2(pointsHash, 0)];
    
    // If the cube is entirely inside or outside of the surface, then there will be no intersections
    if (edgesHash == 0)
    {
        return;
    }
        
    // Save the triangles that were found. There can be up to five per cube
    for (int i = 0; i < 5; i++)
    {
        if (_PointsHash2EdgesIndexesT[uint2(3 * i, pointsHash)] == 20.0)
        {
            break;
        }

        float3 face[3];
        
        for (int j = 0; j < 3; j++)
        {
            int edgeIndex = _PointsHash2EdgesIndexesT[uint2(3 * i + j, pointsHash)];
            
            int edgePoint1Index = _EdgeIndex2PointIndexesT[uint2(0, edgeIndex)];
            int edgePoint2Index = _EdgeIndex2PointIndexesT[uint2(1, edgeIndex)];
            
            MarchPoint edgePoint1 = cube.points[edgePoint1Index];
            MarchPoint edgePoint2 = cube.points[edgePoint2Index];

            face[j] = Interpolate(edgePoint1, edgePoint2, surfaceValue);
        }

        AddFace(face, i, cubeArrayId);
    }
}

void PrepareBuffer(uint cubeIndex)
{
    if (cubeIndex >= _CubesPerChunk1D * _CubesPerChunk1D * _CubesPerChunk1D)
    {
        return;
    }
    
    for (int i = 0; i < 15; i++)
    {
        _MeshV3T1[i + cubeIndex * 15] = float4(0, 0, 0, -1);
    }
}

#endif
