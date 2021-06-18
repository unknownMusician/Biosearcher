#ifndef BIOSEARCHER_NOISE
#define BIOSEARCHER_NOISE

float Noise(float3 position)
{
    float noise = frac(sin(dot(position, float3(12.9898f, 78.233f, 128.544f) * 2.0f) * length(position)) * 43758.5453f);
    return abs(noise);
}

float3 SmoothNoise(float3 x)
{
    return x * x * x * (x * (x * 6 - 15) + 10);
}

float GradientNoise(float3 position)
{
    int3 wholePart = floor(position);
    // todo
    float3 fractPart = position % 1;
    // todo
    fractPart += float3(position.x < 0 && fractPart.x != 0 ? 1 : 0, position.y < 0 && fractPart.y != 0 ? 1 : 0, position.z < 0 && fractPart.z != 0 ? 1 : 0);
    fractPart = SmoothNoise(fractPart);
    float noisesZ[2];
    for (int z = 0; z < 2; z++)
    {
        float noisesY[2];
        for (int y = 0; y < 2; y++)
        {
            float noisesX[2];
            for (int x = 0; x < 2; x++)
            {
                noisesX[x] = Noise(wholePart + int3(x, y, z));
            }
            noisesY[y] = lerp(noisesX[0], noisesX[1], fractPart.x);
        }
        noisesZ[z] = lerp(noisesY[0], noisesY[1], fractPart.y);
    }
    return lerp(noisesZ[0], noisesZ[1], fractPart.z);
}

float Noise2Mountain(float gradientNoise)
{
    float x = gradientNoise;
    return x * x * x * x;
}

#endif
