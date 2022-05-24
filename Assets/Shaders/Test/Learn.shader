Shader "UM/Learn"
{
    Properties
    {
    //  [name] ("[name in inspector]", [type]) = [default value]
    //  _ExampleDir ("Example Vector", Vector) = (0, 1, 0, 0)
    //  _ExampleFloat ("Example Float (Vector1)", Float) = 0.5
        [MainTexture] _BaseMap ("Base Map", 2D) = "white" {}
        _BaseMap_ST ("Base Map Tiling & Offset", Vector) = (1.0, 1.0, 0.0, 0.0)
		[MainColor] _BaseColor ("Base Color", Color) = (0.0, 0.6, 0.7, 1.0)
		[Toggle(_ALPHATEST_ON)] _AlphaTestToggle ("Alpha Clipping", Float) = 0
        _Cutoff ("Alpha Cutoff", Float) = 0.5
    	_Metallic ("Metallic", Float) = 0.5
    	_Smoothness ("Smoothness", Float) = 0.5
    	_EmissionColor ("Emission", Color) = (0.0, 0.0, 0.0, 0.0)
    }
    
    SubShader
    {
        Tags
        {
        //  "RenderPipeline" = "HDRenderPipeline" (optional)
            "RenderPipeline" = "UniversalPipeline"
            
        //  "Queue" = "Background" // = (1000)
        //  "Queue" = "Geometry" // = (2000)
        //  "Queue" = "AlphaTest" // = (2450)
        //  "Queue" = "Transparent" // = (3000)
        //  "Queue" = "Overlay" // = (4000)
        //  "Queue" = "Overlay-1" // = (3999)
        //  "Queue" = "Overlay+1" // = (4001)
        //  [<= 2500] => Opaque
        //  [>= 2501] => Transparent
            "Queue" = "Geometry"
        }

        // Include code in all Passes        
	    HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // Include Lighting.hlsl
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
	
            CBUFFER_START(UnityPerMaterial)
	            //TEXTURE2D(_BaseMap);

	            // Sampler Filter Modes:
	            // - Point (or Nearest-Point) : The colour is taken from the nearest texel. The result is blocky/pixellated, but that if you’re sampling pixel art you’ll likely want to use this.
                // - Linear / Bilinear : The colour is taken as a weighted average of close texels, based on the distance to them.
                // - Trilinear : The same as Linear/Bilinear, but it is also blends between mipmap levels.
                // Sampler Wrap Modes:
	            // - Repeat : UV values outside of 0-1 will cause the texture to tile/repeat.
	            // - Clamp : UV values outside of 0-1 are clamped, causing the edges of the texture to stretch out.
	            // - Mirror : The texture tiles/repeats while also mirroring at each integer boundary.
	            // - Mirror Once : The texture is mirrored once, then clamps UV values lower than -1 and higher than 2.
	            SAMPLER(sampler_point_repeat_BaseMap);
	            float4 _BaseColor;
	    
	            // Tiling & Offset, x = TilingX, y = TilingY, z = OffsetX, w = OffsetY
                float4 _BaseMap_ST;
	    
	            // x = 1/width, y = 1/height, z = width, w = height.
	            //float4 _BaseMap_TexelSize;

	    		float _Metallic;
				float _Smoothness;
				float4 _EmissionColor;
	            float _Cutoff;
	            // etc.
            CBUFFER_END
	    ENDHLSL
        
        Pass
        {
        //  Name "UniversalForward" // Used to render objects in the Forward rendering path. Renders geometry with lighting.
        //  Name "ShadowCaster" // Used for casting shadows
        //  Name "DepthOnly" // Used by the Depth Prepass to create the Depth Texture (_CameraDepthTexture) if MSAA is enabled or the platform doesn’t support copying the depth buffer
        //  Name "DepthNormals" // Used by the Depth Normals Prepass to create the Depth Texture (_CameraDepthTexture) and Normals Texture (_CameraNormalsTexture) if a renderer feature requests it (via ConfigureInput(ScriptableRenderPassInput.Normal))
        //  Name "Meta" // Used during Lightmap Baking
        //  Name "Universal2D" // Used for rendering when the 2D Renderer is enabled
        //  Name "SRPDefaultUnlit" // Default if no LightMode tag is included in a Pass. Can be used to draw extra passes (in both forward/deferred rendering), however this can break SRP Batcher compatibility
        //  Name "UniversalGBuffer" // (FUTURE) Used to render objects in the Deferred rendering path. Renders geometry into multiple buffers without lighting. Lighting is handled later in the path.
        //  Name "UniversalForwardOnly" // (FUTURE) Similar to “UniversalForward”, but can be used to render objects as forward even in the Deferred path which is useful if the shader features data that won’t fit in the GBuffer, such as Clear Coat normals.
            Name "Forward"
            
        //  Cull Back (default) // Back faces are culled
	    //  Cull Front // Front faces are culled
        //  Cull Off // No faces are culled. Both sides are rendered.
	        Cull Off
            
        //  ZTest Less | Greater | LEqual (Default) | GEqual | Equal | NotEqual | Always  (optional)
	        ZTest LEqual
            	        
        //  ZWrite Off | On (Default)  (optional)
            ZWrite On
            
            // Factor scales the maximum Z slope, with respect to X or Y of the polygon, and units scale the minimum resolvable depth buffer value. This allows you to force one polygon to be drawn on top of another although they are actually in the same position. For example Offset 0, -1 pulls the polygon closer to the camera, ignoring the polygon’s slope, whereas Offset -1, -1 will pull the polygon even closer when looking at a grazing angle.
            Offset 0, 0
            
        //  Blend SrcFactor DstFactor
        //  or
        //  Blend SrcFactor DstFactor, SrcFactorA DstFactorA
        
            // finalValue = sourceFactor * sourceValue(shader) operation destinationFactor * destinationValue(buffer)
            
        //  SrcFactor, DstFactor values:
        //  Blend Off (Default)
        //  - One
        //  - Zero
        //  - SrcColor
        //  - SrcAlpha
        //  - DstColor
        //  - DstAlpha
        //  - OneMinusSrcColor
        //  - OneMinusSrcAlpha
        //  - OneMinusDstColor
        //  - OneMinusDstAlpha
            Blend SrcAlpha OneMinusSrcAlpha
            
        //  BlendOp Add (default)
        //  BlendOp Sub
        //  BlendOp RevSub
        //  BlendOp Min
        //  BlendOp Max
            BlendOp Add
            
            HLSLPROGRAM

                // todo
                //#pragma exclude_renderers gles gles3 glcore
                //#pragma target 4.5
            
                #pragma vertex UnlitPassVertex
                #pragma fragment LitPassFragment
            
                // Semantics:
                // - POSITION : Vertex position
                // - COLOR : Vertex colour
                // - TEXCOORD0-7 : UVs (aka texture coordinates). A mesh has 8 different UV channels accessed with a value from 0 to 7. Note that in C#, Mesh.uv corresponds to TEXCOORD0. Mesh.uv1 does not exist, the next channel is uv2 which corresponds to TEXCOORD1 and so on up to Mesh.uv8 and TEXCOORD7.
                // - NORMAL : Vertex Normals (used for lighting calculations. This is unlit currently so isn’t needed)
                // - TANGENT : Vertex Tangents (used to define “tangent space”, important for normal maps and parallax effects
                //
                // - SV_POSITION (for interpolation) : Clip space position from the vertex shader output
                // - SV_Target (final render) : Write the fragment/pixel colour to the current render target
            
                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float4 positionWS : POSITION;
                    float4 normalWS : NORMAL;
                	half fogFactor : TEXCOORD2;
                	half3 vertexSH : TEXCOORD1;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct FragOut
                {
                    // Default render target
                    half4 color : SV_Target0;
                    // Another render target (for MRT - Multiple Render Target)
                    half4 color2 : SV_Target1;
                };

            
				// Textures, Samplers 
				// (note, BaseMap, BumpMap and EmissionMap is being defined by the SurfaceInput.hlsl include)
				TEXTURE2D(_MetallicSpecGlossMap); 	SAMPLER(sampler_MetallicSpecGlossMap);
				TEXTURE2D(_OcclusionMap); 			SAMPLER(sampler_OcclusionMap);
				
				// Functions
				half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha) {
					half4 specGloss;
					#ifdef _METALLICSPECGLOSSMAP
						specGloss = SAMPLE_TEXTURE2D(_MetallicSpecGlossMap, sampler_MetallicSpecGlossMap, uv)
						#ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
							specGloss.a = albedoAlpha * _Smoothness;
						#else
							specGloss.a *= _Smoothness;
						#endif
					#else // _METALLICSPECGLOSSMAP
						#if _SPECULAR_SETUP
							specGloss.rgb = _SpecColor.rgb;
						#else
							specGloss.rgb = _Metallic.rrr;
						#endif
				
						#ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
							specGloss.a = albedoAlpha * _Smoothness;
						#else
							specGloss.a = _Smoothness;
						#endif
					#endif
					return specGloss;
				}
				
				half SampleOcclusion(float2 uv) {
					#ifdef _OCCLUSIONMAP
					#if defined(SHADER_API_GLES)
						return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
					#else
						half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
						return LerpWhiteTo(occ, _OcclusionStrength);
					#endif
					#else
						return 1.0;
					#endif
				}
				
				void InitializeSurfaceData(Varyings input, out SurfaceData surfaceData){
					surfaceData = (SurfaceData)0; // avoids "not completely initalized" errors
				
					half4 albedoAlpha = SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
					surfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
					surfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb * input.color.rgb;
				
					surfaceData.normalTS = SampleNormal(input.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
					surfaceData.emission = SampleEmission(input.uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
					surfaceData.occlusion = SampleOcclusion(input.uv);
						
					half4 specGloss = SampleMetallicSpecGloss(input.uv, albedoAlpha.a);
					#if _SPECULAR_SETUP
						surfaceData.metallic = 1.0h;
						surfaceData.specular = specGloss.rgb;
					#else
						surfaceData.metallic = specGloss.r;
						surfaceData.specular = half3(0.0h, 0.0h, 0.0h);
					#endif
					surfaceData.smoothness = specGloss.a;
				}
                
                void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
                {
                    // avoids "not completely initalized" errors
                	//inputData = (InputData)0;

                    inputData.positionWS = input.positionCS;
	
					#ifdef _NORMALMAP
						half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
						inputData.normalWS = TransformTangentToWorld(normalTS,half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));
					#else
						half3 viewDirWS = GetWorldSpaceNormalizeViewDir(inputData.positionWS);
						inputData.normalWS = input.normalWS;
					#endif

					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
					viewDirWS = SafeNormalize(viewDirWS);
				
					inputData.viewDirectionWS = viewDirWS;

					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						inputData.shadowCoord = input.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
					#else
						inputData.shadowCoord = float4(0, 0, 0, 0);
					#endif
				
					// Fog
					#ifdef _ADDITIONAL_LIGHTS_VERTEX
						inputData.fogCoord = input.fogFactorAndVertexLight.x;
    					inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
					#else
						inputData.fogCoord = input.fogFactor;
						inputData.vertexLighting = half3(0, 0, 0);
					#endif
				
					/* in v11/v12?, could use this :
					#ifdef _ADDITIONAL_LIGHTS_VERTEX
						inputData.fogCoord = InitializeInputDataFog(float4(inputData.positionWS, 1.0), input.fogFactorAndVertexLight.x);
						inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
					#else
						inputData.fogCoord = InitializeInputDataFog(float4(inputData.positionWS, 1.0), input.fogFactor);
						inputData.vertexLighting = half3(0, 0, 0);
					#endif
					// Which currently just seems to force re-evaluating fog per fragment
					*/
				
					inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
					inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
					inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);

                }
            
                Varyings UnlitPassVertex(Attributes input)
                {
                    Varyings output = (Varyings)0;
                    
                    // same as "VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);"
                    // which contains .positionCS, .positionWS, .positionVS and .positionNDC (aka screen position)
                    // (same exists for Normal)
                    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
					//output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                    
                    // same as "output.uv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;"
                    output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                    output.color = input.color;

                    return output;
                }
                
                FragOut LitPassFragment(Varyings input)
                {
                    // can use [FragOut output;], but then we need to explicitly initialize members
                    // ("(FragOut)0" returns struct initialized with zeros)
                    // output.color2 = output.color;
                    FragOut output = (FragOut)0;

                    const float2 uv = input.uv + float2(_Time.y / 2, 0.0);

	                // Sample BaseMap Texture :
                    const half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_point_repeat_BaseMap, uv);

	                // Tint texture result with Color property and vertex colours :
                    half4 color = baseMap * _BaseColor * input.color;

                    SurfaceData surfaceData;
                    InitializeSurfaceData(input, surfaceData);

                    InputData inputData;
                    InitializeInputData(input, surfaceData.normalTS, inputData);

                    color = UniversalFragmentPBR(inputData, surfaceData);

					color.rgb = MixFog(color.rgb, inputData.fogCoord);

                    if (color.a < _Cutoff)
                    {
                        discard;
                    }
                    
                    output.color = color;
                    
                    return output;
                }
            
            ENDHLSL
        }

		Pass {
			Name "ShadowCaster"
			
			Tags 
			{ 
				"LightMode" = "ShadowCaster" 
			}
		
			ZWrite On
			ZTest LEqual
		
			HLSLPROGRAM
			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment
		
			// Material Keywords
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		
			// GPU Instancing
			#pragma multi_compile_instancing
			// (Note, this doesn't support instancing for properties though. Same as URP/Lit)
			// #pragma multi_compile _ DOTS_INSTANCING_ON
			// (This was handled by LitInput.hlsl. I don't use DOTS so haven't bothered to support it)
		
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
			ENDHLSL
		}
    }
    
//  FallBack "Path/Name"
}
