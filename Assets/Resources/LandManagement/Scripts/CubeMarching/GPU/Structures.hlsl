#ifndef BIOSEARCHER_STRUCTURES
#define BIOSEARCHER_STRUCTURES

struct MarchPoint
{
    float3 position;
    float value;
};

struct MarchCube
{
    MarchPoint points[8];
};

#endif
