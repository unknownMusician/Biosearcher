Shader "Hidden/HDRP/Sky/CustomSky"
{
    HLSLINCLUDE

    #pragma vertex Vert

    #pragma editor_sync_compilation
    #pragma target 4.5
    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch


    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Sky/SkyUtils.hlsl"

    float3 _SkyColor;
    float _NightSkyIntensity;
    float _StarsIntensity;
    float _GalaxyIntensity;
    float _OverallIntensity;
    float _StarsNoiseScale;
    float _StarsNoiseStep;

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID, UNITY_RAW_FAR_CLIP_VALUE);
        return output;
    }

    float Noise(float3 position)
    {
        float noise = (sin(dot(position, float3(12.9898f, 78.233f, 128.544f) * 2.0f) * length(position)) * 43758.5453f) % 1;
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

    float3 GetNightSky(float3 dir)
    {
        float stars = step(GradientNoise(dir * _StarsNoiseScale), _StarsNoiseStep) * _StarsIntensity;

        float galaxyNoise = GradientNoise(float3(dir.xyz * float3(1, 1, 2)) * 5);
        galaxyNoise *= GradientNoise(float3(dir.xyz * float3(1, 1, 2)) * 3);
        float galaxy = galaxyNoise * (1 - clamp(abs(dir.z * galaxyNoise * 10), 0, 1)) * _GalaxyIntensity;
        
        float3 result = stars.xxx + galaxy.xxx;

        return result;
    }

    float4 GetColorWithRotation(float3 dir, float exposure)
    {
        float3 color = GetNightSky(dir) * _NightSkyIntensity;
        color += _SkyColor;

        return float4(color * _OverallIntensity * exposure, 1.0);
    }

    float4 RenderSky(Varyings input, float exposure)
    {
        float3 viewDirWS = GetSkyViewDirWS(input.positionCS.xy);

        // Reverse it to point into the scene
        float3 dir = -viewDirWS;

        return GetColorWithRotation(dir, exposure);
    }

    float4 FragBaking(Varyings input) : SV_Target
    {
        return RenderSky(input, 1.0);
    }

    float4 FragRender(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        return RenderSky(input, GetCurrentExposureMultiplier());
    }

    ENDHLSL

    SubShader
    {
        // Regular New Sky
        // For cubemap
        Pass
        {
            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment FragBaking
            ENDHLSL
        }

        // For fullscreen Sky
        Pass
        {
            ZWrite Off
            ZTest LEqual
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment FragRender
            ENDHLSL
        }
    }
    Fallback Off
}