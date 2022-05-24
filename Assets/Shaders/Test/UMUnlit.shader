Shader "UM/UMUnlit"
{
	Properties
	{
		_BaseMap ("Texture", 2D) = "white" {}
		_BaseColor ("Color", Color) = (0.5, 0.5, 0.5, 0.5)
	}
	
	SubShader
	{
		Tags 
		{ 
			"RenderPipeline" = "UniversalPipeline"
			"Queue" = "Geometry"
		}

		HLSLINCLUDE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			CBUFFER_START(UnityPerMaterial)
		
				TEXTURE2D(_BaseMap);
				SAMPLER(sampler_point_repeat_BaseMap);
				float4 _BaseMap_ST;
				float4 _BaseColor;
		
			CBUFFER_END
		
		ENDHLSL
		
		Pass
		{
			Name "Forward"

			Tags 
		    {
		        "LightMode" = "UniversalForward" 
		    }
			
			//Cull Back
			//ZTest LEqual
			//ZWrite On
			//Offset 0, 0
			
			//Blend SrcAlpha OneMinusSrcAlpha
			//BlendOp Add
			
			HLSLPROGRAM
			
				#pragma vertex UnlitPassVertex
				#pragma fragment UnlitPassFragment
			
				struct Attributes
				{
					float4 positionOS : POSITION;
					float2 uv : TEXCOORD0;
				};
	
				struct Varyings
				{
					float4 positionCS : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

			
				Varyings UnlitPassVertex(Attributes input)
				{
					Varyings output;
					
					output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
					output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
					
					return output;
				}
				
				float4 UnlitPassFragment(Varyings input) : SV_Target
				{
					float4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_point_repeat_BaseMap, input.uv);
					
					return color * _BaseColor;
				}
			
			ENDHLSL
		}
	}
}