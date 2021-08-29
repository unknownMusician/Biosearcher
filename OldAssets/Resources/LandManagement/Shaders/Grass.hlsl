#ifndef BIOSEARCHER_GRASS
#define BIOSEARCHER_GRASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "NMGGeometryHelpers.hlsl"

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//////////// wtf

inline void InitializeStandardLitSurfaceData(inout SurfaceData outSurfaceData, float alpha, float3 albedo, float metallic, float smoothness, float3 normalTS, float occlusion, float3 emission)
{
    outSurfaceData.alpha = alpha;
    outSurfaceData.albedo = albedo;
    outSurfaceData.metallic = albedo;
    
    outSurfaceData.specular = half3(0.0h, 0.0h, 0.0h);
    
    outSurfaceData.smoothness = smoothness;
    outSurfaceData.normalTS = normalTS;
    outSurfaceData.occlusion = occlusion;
    outSurfaceData.emission = emission;
}

////////////

float random(float3 st)
{
    return frac(sin(dot(st,
                         float3(12.9898, 78.233, 34.045))) *
        48.5453123);
}

////////////

float3x3 AngleAxis3x3(float angle, float3 axis)
{
    float c, s;
    sincos(angle, s, c);

    float t = 1 - c;
    float x = axis.x;
    float y = axis.y;
    float z = axis.z;

    return float3x3(
        t * x * x + c, t * x * y - s * z, t * x * z + s * y,
        t * x * y + s * z, t * y * y + c, t * y * z - s * x,
        t * x * z - s * y, t * y * z + s * x, t * z * z + c
    );
}

////////////

struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
};

struct ControlPoint
{
    float4 positionOS : INTERNALTESSPOS;
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
    float colorLerp : TEXCOORD3;
};

#include "CustomGeometry.hlsl"
#include "CustomTessellation.hlsl"

TEXTURE2D(_MainTex); 
SAMPLER(sampler_MainTex);
float4 _MainTex_ST;

float _GrassHeight;
float _GrassWidth;
float _WindStrenght;
float4 _TestNoise;

float3 _BaseColor;
float3 _TopColor;
float _Metallic;
float _Smoothness;
float _Occlusion;
float3 _Emission;

ControlPoint PreTessellationVertex(Attributes v)
{
    ControlPoint p;
		
    p.positionOS = v.positionOS;
    p.uv = v.uv;
		
    return p;
}

//HULL_PASS(Hull, ControlPoint, "fractional_odd")
//HULL_PASS(Hull, ControlPoint, "fractional_even")
//HULL_PASS(Hull, ControlPoint, "pow2")
HULL_PASS(Hull, ControlPoint, "integer")

VertexOutput Vertex(Attributes input);

[UNITY_domain("tri")]
VertexOutput Domain(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
{
    Attributes v;
    
	DOMAIN_TRANSFER_FIELD(v, patch, barycentricCoordinates, positionOS)
	DOMAIN_TRANSFER_FIELD(v, patch, barycentricCoordinates, uv)
			
    return Vertex(v);
}

VertexOutput Vertex(Attributes input)
{
    VertexOutput output = (VertexOutput) 0;
    
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    output.positionWS = vertexInput.positionWS;

    output.uv = TRANSFORM_TEX(input.uv, _MainTex);
    return output;
}

[maxvertexcount(9)]
void Geometry(triangle VertexOutput inputs[3], inout TriangleStream<GeometryOutput> outputStream)
{
    float3 centerWS = GetTriangleCenter(inputs[0].positionWS, inputs[1].positionWS, inputs[2].positionWS);
    float3 triNormal = GetNormalFromTriangle(inputs[0].positionWS, inputs[1].positionWS, inputs[2].positionWS);
    
    //if ((distance(centerWS, _WorldSpaceCameraPos) - _MinTessDistance) / (_MaxTessDistance - _MinTessDistance) < 0.1f)
    //{
        VertexOutput top = (VertexOutput) 0;
        VertexOutput leftLeg = (VertexOutput) 0;
        VertexOutput rightLeg = (VertexOutput) 0;
    
    //triNormal = normalize((normalize(centerWS) + triNormal) * 0.5f);
    
    //const float pi = 3.14159f;
    
        float3x3 rotation = AngleAxis3x3(random(centerWS * _TestNoise.x) * (PI * 2.0f), triNormal);
    
        float3 leftLegLocalPosition = mul(rotation, normalize(inputs[0].positionWS - centerWS) * _GrassWidth * 0.5f);
        centerWS += normalize(leftLegLocalPosition) * random(centerWS * _TestNoise.y) * _TestNoise.z;
        leftLeg.positionWS = leftLegLocalPosition + centerWS;
        rightLeg.positionWS = -leftLegLocalPosition + centerWS;
    
    //top.positionWS = centerWS + triNormal * _Height;
        top.positionWS = centerWS + triNormal * _GrassHeight;
    
        top.positionWS += float3(sin(_Time.y + top.positionWS.x), sin(2.5f * _Time.y + top.positionWS.y), sin(3.4568f * _Time.y + top.positionWS.z)) * _WindStrenght;

        top.uv = GetTriangleCenter(inputs[0].uv, inputs[1].uv, inputs[2].uv);

        SetupAndOutputTriangle(outputStream, leftLeg, rightLeg, top);
    //SetupAndOutputTriangle(outputStream, inputs[0], inputs[1], top);
    //SetupAndOutputTriangle(outputStream, inputs[1], inputs[2], top);
    //SetupAndOutputTriangle(outputStream, inputs[2], inputs[0], top);
    //}
    //else
    //{
    //    VertexOutput outputs[3] = { inputs[0], inputs[1], inputs[2] };
    //    [unroll]
    //    for (int i = 0; i < 3; i++)
    //    {
    //        outputs[i].positionWS += triNormal * _GrassHeight;
    //    }
    //    float rd = random(centerWS);
    //    SetupAndOutputTriangle(outputStream, outputs[0], outputs[1], outputs[2], 1.0f, 1.0f - rd, rd);
    //}
}

float4 Fragment(GeometryOutput input) : SV_Target
{
#ifdef SHADOW_CASTER_PASS
    return 0;
#endif
    
    SurfaceData surfaceData = (SurfaceData) 0;
    float3 albedo = lerp(_BaseColor, _TopColor, input.colorLerp);
    InitializeStandardLitSurfaceData(surfaceData, 1.0f, albedo, _Metallic, _Smoothness, normalize(mul(unity_ObjectToWorld, input.positionWS)), _Occlusion, _Emission);

    InputData lightingInput = (InputData) 0;
    lightingInput.positionWS = input.positionWS;
    lightingInput.normalWS = NormalizeNormalPerPixel(input.positionWS);
    lightingInput.viewDirectionWS = GetViewDirectionFromPosition(input.positionWS);
    lightingInput.shadowCoord = CalculateShadowCoord(input.positionWS, input.positionCS);
    
    half4 color = UniversalFragmentPBR(lightingInput, surfaceData);

    
    color.rgb += albedo * unity_AmbientSky.rgb;// + albedo * 0.1f;
    color.rgb = (lerp(_BaseColor, color.rgb, input.colorLerp) + color.rgb) * 0.5f;
    color.a = 1.0f;
    
    //float3 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).rgb;
    
    // lightingInput, albedo, specular, smoothness, emission, alpha
    //return UniversalFragmentBlinnPhong(lightingInput, albedo, 1, 0.5f, 0, 1);
    return color;
}



//// Used in Standard (Physically Based) shader
//half4 LitPassFragment(Varyings input) : SV_Target
//{
//    SurfaceData surfaceData;
//    InitializeStandardLitSurfaceData(input.uv, surfaceData);

//    InputData inputData;
//    InitializeInputData(input, surfaceData.normalTS, inputData);

//    half4 color = UniversalFragmentPBR(inputData, surfaceData);

//    color.rgb = MixFog(color.rgb, inputData.fogCoord);
//    color.a = OutputAlpha(color.a, _Surface);

//    return color;
//}

#endif