Shader "Unlit/OutLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

		_OutlineTex("Texture de ta ligne",2D) = "white" {}
		_OutlineColor("Couleur de la ligne",Color) = (1,1,1,1)
		_OutlineWidth("Epaisseur",Range(1.0,10.0))=1.1//en gros un multiplie le model par ce chiffre pour creer un model en background puis, on trace le vrai model par dessu
    }
    SubShader
    {
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100
			ZWrite off
			Blend SrcAlpha OneMinusSrcAlpha
		Pass
	{
		CGPROGRAM
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

	sampler2D _OutlineTex;
	float4 _OutlineColor;
	float _OutlineWidth;

	v2f vert(appdata v)//donc, ici c<est les veretex
	{
		//on grossit le model * l<epaisseur de la ligne
		v.vertex.xyz *= _OutlineWidth;
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;//on se fou du uv map
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target//ici c<est les couleurs
	{
		//on set la couleur et la texture par celle du outline
		
		// sample the texture
		fixed4 col = tex2D(_OutlineTex, i.uv)*_OutlineColor;
	// apply fog
	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
	}
		ENDCG
	}
        Pass//La deuxiemme pass ici c<est le shader par defaultqui se draw par dessu ce qu<on a fait
        {
            CGPROGRAM
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
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
