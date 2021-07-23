Shader "Custom/Grass"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Height("Height", Float) = 1
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
            Cull Back

            HLSLPROGRAM

            // Requires
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma require geometry

            // Lighting and shadows
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #pragma vertex Vertex
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

            HLSLPROGRAM

            // Requires
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma require geometry

            // Lighting and shadows    
            #pragma multi_compile_shadowcaster

            #pragma vertex Vertex
            #pragma geometry Geometry
            #pragma fragment Fragment

            #define SHADOW_CASTER_PASS

            #include "Grass.hlsl"
            
            ENDHLSL
        }
    }
}
