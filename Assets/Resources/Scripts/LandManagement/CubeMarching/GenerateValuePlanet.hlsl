#ifndef BIOSEARCHER_GENERATE_VALUE_PLANET
#define BIOSEARCHER_GENERATE_VALUE_PLANET

#include "InputOutput.hlsl"
#include "Noise.hlsl"

// todo: not in sync with CPU (+~)
float GenerateValue(float3 position)
{
    float result = 1;
    float3 normalizedPosition = normalize(position);

    // 400 - planet size (todo)
    float planetRadius = length(position) / 400;
    //float planetRadius = length(position*2) - 400;

    //planetRadius += GradientNoise(normalizedPosition / (cubesPerChunk * (1 << 5))) / 1.5f;
    
    // Mountains
    float preMountainNoise = 1;
    preMountainNoise *= GradientNoise(normalizedPosition * (_CubesPerChunk * (1 << 2)));
    preMountainNoise *= GradientNoise(normalizedPosition * (_CubesPerChunk * (1 << 3)));
    float mountainMask = smoothstep(0.3, 0.1, GradientNoise(normalizedPosition * (_CubesPerChunk / uint(1 << 1))));
    planetRadius -= (Noise2Mountain(preMountainNoise) + 0.5) * mountainMask * 0.15;
    
    // Hills
    planetRadius -= GradientNoise(normalizedPosition * (_CubesPerChunk * (1 << 0))) * (1 - mountainMask) * 0.02;
    // todo
    //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 1))) / 16;
    //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 2))) / 8;
    //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 3))) / 4;
    //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 4))) / 2;
    //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 5))) / 1.5f;
    
    result *= 1 - clamp(planetRadius, 0, 1);

    return result;
}

#endif
