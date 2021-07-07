#ifndef BIOSEARCHER_GENERATE_VALUE_FLAT
#define BIOSEARCHER_GENERATE_VALUE_FLAT

#include "InputOutput.hlsl"
#include "Noise.hlsl"

// todo: not in sync with CPU (+~)
float GenerateValue(float3 position)
{
    float result = 1;
    float3 normalizedPosition = float3(position.x, 0, position.z);

    // 400 - planet size (todo)
    float planetRadius = position.y / 400;
    
    // Mountains
    float preMountainNoise = 1;
    preMountainNoise *= 1 - GradientNoise(normalizedPosition * _CubesPerChunk1D * 0.005);
    preMountainNoise *= 1 - GradientNoise(normalizedPosition * _CubesPerChunk1D * 0.01) * 0.5;
    //preMountainNoise *= 1 - GradientNoise(normalizedPosition * cubesPerChunk * 0.02) * 0.2;
    //preMountainNoise *= 1 - GradientNoise(normalizedPosition * cubesPerChunk * 0.05) * 0.1;
    float preMountainMask = 1;
    preMountainMask *= 1 - GradientNoise(normalizedPosition * _CubesPerChunk1D * 0.0005);
    preMountainMask *= 1 - GradientNoise(normalizedPosition * _CubesPerChunk1D * 0.0008);
    preMountainMask *= 1 - GradientNoise(normalizedPosition * _CubesPerChunk1D * 0.001) * 0.75;
    preMountainMask *= 1 - GradientNoise(normalizedPosition * _CubesPerChunk1D * 0.002) * 0.5;
    float mountainMask = smoothstep(0.1, 0.8, preMountainMask);
    //float mountainMask = GradientNoise(normalizedPosition * cubesPerChunk * 0.0005);
    //planetRadius -= (Noise2Mountain(preMountainNoise) + 0.5) * mountainMask * 0.3;
    planetRadius -= (preMountainNoise + 0.5) * mountainMask * 0.3;
    //planetRadius -= preMountainNoise * mountainMask * 0.2;
    
    // Hills
    planetRadius -= GradientNoise(position * _CubesPerChunk1D * 0.02) /* * (1 - mountainMask)*/ * 0.002;
    planetRadius -= GradientNoise(position * _CubesPerChunk1D * 0.01) /* * (1 - mountainMask)*/ * 0.005;
    planetRadius -= GradientNoise(position * _CubesPerChunk1D * 0.005) /* * (1 - mountainMask)*/ * 0.01;
    
    result *= 1 - clamp(planetRadius, 0, 1);

    return result;
}

#endif
