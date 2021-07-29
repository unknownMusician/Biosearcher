Shader "Custom/Grass"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _GrassHeight("Grass Height", Float) = 1.0
        _GrassWidth("Grass Width", Float) = 1.0
        _WindStrenght("Wind Strenght", Float) = 1.0
        _TestNoise("Test Noise", Vector) = (0.5, 0.5, 0.5, 0.5)

        _BaseColor("Base Color", Color) = (0.5, 0.5, 0.5, 0.5)
        _TopColor("Top Color", Color) = (0.5, 0.5, 0.5, 0.5)
        _Metallic("Metallic", Float) = 0.5
        _Smoothness("Smoothness", Float) = 0.5
        _Occlusion("Occlusion", Float) = 0.5
        _Emission("Emission", Color) = (0, 0, 0, 0)

        _Tess("Tessellation", Range(1, 32)) = 20
        _MinTessDistance("Min Tess Distance", Range(1, 100)) = 10
        _MaxTessDistance("Max Tess Distance", Range(1, 100)) = 20
    }

    Subshader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "ForwardLit"
            Tags 
            {
                "LightMode" = "UniversalForward"
            }
            Cull Off
            ZWrite On

            HLSLPROGRAM

            // Requires
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma require geometry
            #pragma require tesselation

            // Lighting and shadows
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #pragma vertex PreTessellationVertex
            #pragma hull Hull
            #pragma domain Domain
            #pragma geometry Geometry
            #pragma fragment Fragment

            #include "Grass.hlsl"
            
            ENDHLSL
        }
            
        Pass
        {
            Name "ShadowCaster"
            Tags 
            {
                "LightMode" = "ShadowCaster"
            }
            Cull Off
            ZWrite On

            HLSLPROGRAM

            // Requires
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma require geometry
            #pragma require tesselation

            // Lighting and shadows    
            #pragma multi_compile_shadowcaster

            #pragma vertex PreTessellationVertex
            #pragma hull Hull
            #pragma domain Domain
            #pragma geometry Geometry
            #pragma fragment Fragment

            #define SHADOW_CASTER_PASS

            #include "Grass.hlsl"
            
            ENDHLSL
        }
    }
}
