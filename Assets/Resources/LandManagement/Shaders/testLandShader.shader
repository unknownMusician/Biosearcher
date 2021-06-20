// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/testLandShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma require geometry
            #pragma vertex vert
            #pragma geometry geo
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float4 pos : SV_POSITION;
            };

            struct g2f
            {
	            float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(1)
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 vertexPosition : SV_Target0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

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

            float Noise2Mountain(float gradientNoise)
            {
                float x = gradientNoise;
                return x * x * x * x;
            }

            // todo: not in sync with CPU (+~)
            float GenerateValue(float3 position)
            {
                float cubesSize1D = 8;
                float result = 1;
                float3 normalizedPosition = normalize(position);
                
                // 400 - planet size (todo)
                float planetRadius = 0;
                
                // Mountains
                float preMountainNoise = 1;
                preMountainNoise *= GradientNoise(normalizedPosition * (cubesSize1D * (1 << 2)));
                preMountainNoise *= GradientNoise(normalizedPosition * (cubesSize1D * (1 << 3)));
                float mountainMask = smoothstep(0.3, 0.1, GradientNoise(normalizedPosition * (cubesSize1D / uint(1 << 1))));
                //planetRadius += (Noise2Mountain(preMountainNoise) + 0.5) * mountainMask;
                
                // Hills
                planetRadius += GradientNoise(normalizedPosition * (cubesSize1D * (1 << 0))) * (1 - mountainMask);

                // todo
                //result *= 1 - GradientNoise(position / (cubesSize1D * (1 << 1))) / 16;
                //result *= 1 - GradientNoise(position / (cubesSize1D * (1 << 2))) / 8;
                //result *= 1 - GradientNoise(position / (cubesSize1D * (1 << 3))) / 4;
                //result *= 1 - GradientNoise(position / (cubesSize1D * (1 << 4))) / 2;
                //result *= 1 - GradientNoise(position / (cubesSize1D * (1 << 5))) / 1.5f;

                result *= planetRadius;
    
                return result;
            }

            //v2f vert (appdata v)
            //{
            //    v2f o;
            //    o.vertex = UnityObjectToClipPos(v.vertex);
            //    o.vertexPosition = mul(unity_ObjectToWorld, v.vertex);
            //    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            //    UNITY_TRANSFER_FOG(o,o.vertex);
            //    return o;
            //}

            v2g vert(appdata v)
            {
                v2g g;
                g.pos = UnityObjectToClipPos(v.vertex);
                return g;
            }

            [maxvertexcount(3)]
            void geo(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
            {
                float3 pos = IN[0].pos;

                g2f o;
                
                o.pos = UnityObjectToClipPos(pos + float3(0.5, 0, 0));
                triStream.Append(o);
                
                o.pos = UnityObjectToClipPos(pos + float3(-0.5, 0, 0));
                triStream.Append(o);
                
                o.pos = UnityObjectToClipPos(pos + float3(0, 1, 0));
                triStream.Append(o);
            }

            fixed4 frag (g2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                //fixed4 col = float4(i.vertex.xyz / i.vertex.w, 1.0);
                return float4(1, 1, 0, 1);
                fixed4 col = GenerateValue(i.pos).xxxx;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
