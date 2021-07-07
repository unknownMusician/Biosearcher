#ifndef BIOSEARCHER_GENERATE_MESH
#define BIOSEARCHER_GENERATE_MESH

#include "Common.hlsl"
#include "InputOutput.hlsl"

[numthreads(7, 7, 7)]
void GenerateMesh(uint3 threadId : SV_DispatchThreadID)
{
    if (threadId.x >= _CubesPerChunk1D || threadId.y >= _CubesPerChunk1D || threadId.z >= _CubesPerChunk1D)
    {
        return;
    }
    
    uint cubeArrayId = MatrixId2ArrayId(threadId, _CubesPerChunk1D);
    
    //float testValue = float(PointsHash2EdgesHashT[uint2(threadId.x + threadId.y * 6 + threadId.z * 6 * 6, 0)]);
    //float testValue = 56;
    //meshV3T1[cubeIndex] = float4(marchCube.points[0].position, 0);
    //meshTriangles[uint2(1, 0)] = 8.0;
    
    //return;
    
    PrepareBuffer(cubeArrayId);
    
    MarchCube marchCube = GenerateCube(_Points, threadId);
    
    March(marchCube, _SurfaceValue, cubeArrayId);
}

#endif
