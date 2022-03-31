// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "IIM/TD"
{
	Properties
	{
		[PerRendererData]_MainTex("Main Texture", 2D) = "white" {}
		_PerlinTex("Bruit de Perlin", 2D) = "white" {}

		_Color("Tint", Color) = (1,1,1,1)
		_Threshold("Threshold", Range(0,1)) = 0.5
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

	}
	SubShader
	{
		// No culling or depth
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
			CGPROGRAM

			#include "UnitySprites.cginc"

			struct dv2f
			{
				float2 texcoord : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color    : COLOR;
			};

			dv2f vert(appdata_t IN)
			{
				dv2f OUT;

				OUT.vertex = float4(IN.vertex.xy * _Flip, IN.vertex.z, 1.0);
				OUT.vertex = UnityObjectToClipPos(OUT.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color * _RendererColor;

				#ifdef PIXELSNAP_ON
							OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _PerlinTex;
			float _Threshold;

			fixed4 frag(dv2f IN) : SV_Target
			{
				//fixed4 col = SampleSpriteTexture(IN.texcoord) * IN.color;
				//col.rgb *= col.a;
				fixed4 colMain = tex2D(_MainTex, IN.texcoord);
				fixed4 colPerlin = tex2D(_PerlinTex, IN.texcoord);
				if (colPerlin.r > _Threshold)
					return colMain;
				else
					return fixed4(0, 0, 0, 0);
				//return colMain * colPerlin;
			}

				// Time.y pour le shader wave de demain

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			ENDCG
		}
	}
}
