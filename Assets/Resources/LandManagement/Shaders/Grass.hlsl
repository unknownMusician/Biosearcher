#ifndef BIOSEARCHER_GRASS
#define BIOSEARCHER_GRASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "NMGGeometryHelpers.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
};

struct VertexOutput
{
    float3 positionWS : TEXCOORD0;
    float2 uv : TEXCOORD1;
};

struct GeometryOutput
{
    float3 positionWS : TEXCOORD0;
    float2 uv : TEXCOORD1;
    float3 normalWS : TEXCOORD2;
    float4 positionCS : SV_POSITION;
};

TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_ST;

float _Height;

VertexOutput Vertex(Attributes input)
{
    VertexOutput output = (VertexOutput)0;
    
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    output.positionWS = vertexInput.positionWS;

    output.uv = TRANSFORM_TEX(input.uv, _MainTex);
    return output;
}

GeometryOutput SetupVertex(float3 positionWS, float3 normalWS, float2 uv)
{
    GeometryOutput output = (GeometryOutput)0;
    output.positionWS = positionWS;
    output.normalWS = normalWS;
    output.uv = uv;

    output.positionCS = CalculatePositionCSWithShadowCasterLogic(positionWS, normalWS);
    return output;
}

void SetupAndOutputTriangle(inout TriangleStream<GeometryOutput> outputStream, VertexOutput a, VertexOutput b, VertexOutput c)
{
    outputStream.RestartStrip();
    
    float3 normalWS = GetNormalFromTriangle(a.positionWS, b.positionWS, c.positionWS);

    outputStream.Append(SetupVertex(a.positionWS, normalWS, a.uv));
    outputStream.Append(SetupVertex(b.positionWS, normalWS, b.uv));
    outputStream.Append(SetupVertex(c.positionWS, normalWS, c.uv));
}

[maxvertexcount(9)]
void Geometry(triangle VertexOutput inputs[3], inout TriangleStream<GeometryOutput> outputStream)
{
    VertexOutput center = (VertexOutput)0;
    
    float3 triNormal = GetNormalFromTriangle(inputs[0].positionWS, inputs[1].positionWS, inputs[2].positionWS);
    
    center.positionWS = GetTriangleCenter(inputs[0].positionWS, inputs[1].positionWS, inputs[2].positionWS) + triNormal * _Height;

    center.uv = GetTriangleCenter(inputs[0].uv, inputs[1].uv, inputs[2].uv);

    SetupAndOutputTriangle(outputStream, inputs[0], inputs[1], center);
    SetupAndOutputTriangle(outputStream, inputs[1], inputs[2], center);
    SetupAndOutputTriangle(outputStream, inputs[2], inputs[0], center);

}

float4 Fragment(GeometryOutput input) : SV_Target
{
    #ifdef SHADOW_CASTER_PASS
    return 0;
    #endif
    
    InputData lightingInput = (InputData)0;
    lightingInput.positionWS = input.positionWS;
    lightingInput.normalWS = NormalizeNormalPerPixel(input.positionWS);
    lightingInput.viewDirectionWS = GetViewDirectionFromPosition(input.positionWS);
    lightingInput.shadowCoord = CalculateShadowCoord(input.positionWS, input.positionCS);
    
    float3 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).rgb;
    
    // lightingInput, albedo, specular, smoothness, emission, alpha
    return UniversalFragmentBlinnPhong(lightingInput, albedo, 1, 0, 0, 1);

}

#endif