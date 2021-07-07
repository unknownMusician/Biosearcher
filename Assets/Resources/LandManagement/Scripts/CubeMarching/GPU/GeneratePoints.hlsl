#ifndef BIOSEARCHER_GENERATE_POINTS
#define BIOSEARCHER_GENERATE_POINTS

#define NUMTHREADS 8

#include "Common.hlsl"
#include "InputOutput.hlsl"
#include "Noise.hlsl"
#include "Structures.hlsl"

[numthreads(NUMTHREADS, NUMTHREADS, NUMTHREADS)]
void GeneratePoints(uint3 threadId : SV_DispatchThreadID)
{
    if (threadId.x >= _PointsPerChunk1D || threadId.y >= _PointsPerChunk1D || threadId.z >= _PointsPerChunk1D)
    {
        return;
    }
    
    MarchPoint marchPoint;
    
    int pointIndex = MatrixId2ArrayId(threadId, _PointsPerChunk1D);
    
    int3 position = (threadId - _CubesPerChunk1D / 2u) * _CubeSize;
    
    marchPoint.position = position;
    marchPoint.value = GenerateValue(position + _ChunkPosition);
    
    // todo
    _Points[pointIndex] = marchPoint;
}

#endif
