// This shader adds tessellation in URP
Shader "Example/URPUnlitShaderTessallated"
{

	// The properties block of the Unity shader. In this example this block is empty
	// because the output color is predefined in the fragment shader code.
	Properties
	{
		_Tess("Tessellation", Range(1, 32)) = 20
		_MaxTessDistance("Max Tess Distance", Range(1, 32)) = 20
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque" 
			"RenderPipeline" = "UniversalRenderPipeline"
		}

		Pass
		{
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct Attributes
			{
				float4 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct ControlPoint
			{
				float4 positionOS : INTERNALTESSPOS;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionOS : SV_POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			#include "CustomTessellation.hlsl"

			#pragma require tessellation

			// This line defines the name of the vertex shader. 
			#pragma vertex PreTessellationVertex
			// This line defines the name of the hull shader. 
			#pragma hull Hull
			// This line defines the name of the domain shader. 
			#pragma domain Domain
			// This line defines the name of the fragment shader. 
			#pragma fragment Fragment



			// Attributes
			// v TessellationVertexProgram (preTess)
			// ControlPoint
			// v domain (tess)
			// Varyings (Attributes)
			// v vert (vert)
			// Varyings
			// v frag (frag)
			// float4

			ControlPoint PreTessellationVertex(Attributes v)
			{
				ControlPoint p;

				p.positionOS = v.positionOS;
				p.normal = v.normal;
				p.color = v.color;
				p.uv = v.uv;

				return p;
			}

			//HULL_PASS(Hull, ControlPoint, "fractional_odd")
			//HULL_PASS(Hull, ControlPoint, "fractional_even")
			//HULL_PASS(Hull, ControlPoint, "pow2")
			HULL_PASS(Hull, ControlPoint, "integer")

			Varyings Vertex(Attributes input);

			[UNITY_domain("tri")]
			Varyings Domain(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
			{
				Attributes v;

				DOMAIN_TRANSFER_FIELD(v, patch, barycentricCoordinates, positionOS)
				DOMAIN_TRANSFER_FIELD(v, patch, barycentricCoordinates, normal)
				DOMAIN_TRANSFER_FIELD(v, patch, barycentricCoordinates, color)
				DOMAIN_TRANSFER_FIELD(v, patch, barycentricCoordinates, uv)

				return Vertex(v);
			}

			// after tesselation
			Varyings Vertex(Attributes input)
			{
				Varyings output;

				output.positionOS = TransformObjectToHClip(input.positionOS.xyz);
				output.color = input.color;
				output.normal = input.normal;
				output.uv = input.uv;

				return output;
			}

			// The fragment shader definition.            
			float4 Fragment(Varyings IN) : SV_Target
			{
				return float4(0.5f, 0.5f, 0.5f, 0.5f);
			}

			ENDHLSL
		}
	}
}
