#ifndef BIOSEARCHER_GENERATE_POINTS
#define BIOSEARCHER_GENERATE_POINTS

#include "Common.hlsl"
#include "InputOutput.hlsl"
#include "Noise.hlsl"
#include "Structures.hlsl"

[numthreads(8, 8, 8)]
void GeneratePoints(uint3 threadId : SV_DispatchThreadID)
{
    if (threadId.x >= _PointsPerChunk || threadId.y >= _PointsPerChunk || threadId.z >= _PointsPerChunk)
    {
        return;
    }
    
    MarchPoint marchPoint;
    
    int pointIndex = MatrixId2ArrayId(threadId, _PointsPerChunk);
    
    int3 position = (threadId - uint(_PointsPerChunk - 1) / uint(2)) * _CubeSize;
    
    marchPoint.position = position;
    marchPoint.value = GenerateValue(position + _ChunkPosition);
    
    // todo
    _Points[pointIndex] = marchPoint;
}

#endif
