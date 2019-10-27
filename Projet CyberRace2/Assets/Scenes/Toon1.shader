Shader "Unlit/Toon1"
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
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members normal)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 normal = normalize(i.normal);
				float ndotl = dot(normal._WorldSpaceLightPos0);
				float ndotv = saturate(dot(normal, i.viewDir))

				float3 lut = tex2D(_ToonLut, float2(ndotl, 0));
				float3 rim = _RimColor * pow(1 - ndotv, _RimPower)*ndotl;

				float3 directDiffuse = lut * _LightColor0;
				float3 indirectDiffuse = unity_AmbientSky;

                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= directDiffuse + indirectDiffuse;
				col.rgb += rim;
               
                return col;
            }
            ENDCG
        }
    }
}
