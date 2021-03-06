Shader "Sprites/Custom/TengenToppaSpriteShadow"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5
        _FlashColor("Flash Color", Color) = (1,1,1,1)
        _FlashAmount("Flash Amount", Range(0,1)) = 0
			//
		_Red("Red Shift", Range(-2.0, 2.0)) = 0.0
		_Green("Green Shift", Range(-2.0, 2.0)) = 0.0
		_Blue("Blue Shift", Range(-2.0, 2.0)) = 0.0
		_Brightness("Brightness", Range(0.0, 10.0)) = 1.0
		_Contrast("Contrast", Range(0.0, 10.0)) = 1.0
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
    }
        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            LOD 200
            Cull Off
            Lighting On
            ZWrite Off
			//Blend One OneMinusSrcAlpha

			Blend SrcAlpha OneMinusSrcAlpha


			CGPROGRAM
			#pragma surface surf NoLighting addshadow alphatest:_Cutoff
			sampler2D _MainTex;
			//fixed4 _Color;
			struct Input
			{
				float2 uv_MainTex;
				float4 color : COLOR;
			};
			fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
			{
				fixed4 c;
				c.rgb = s.Albedo;
				c.a = s.Alpha;
				return c;
			}

			half3 AdjustContrast(half3 color, half contrast) {
				return saturate(lerp(half3(0.5, 0.5, 0.5), color, contrast));
			}

			fixed _Red;
			fixed _Green;
			fixed _Blue;
			fixed _Contrast;
			fixed _Brightness;

			fixed4 _FlashColor;
			float _FlashAmount;
			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				half3 rgb = AdjustContrast(
					half3(
						c.r - _Red * (2 * c.r - c.g - c.b),
						c.g - _Green * (2 * c.g - c.r - c.b),
						c.b - _Blue * (2 * c.b - c.r - c.g)
						),
					_Contrast
				);
				c.rgb = fixed4(rgb.rgb * _Brightness, c.a) * IN.color;

				c.rgb = lerp(c.rgb, _FlashColor.rgb, _FlashAmount);
				c.rgb *= c.a;
				o.Albedo = c.rgb * IN.color.rgb;
				o.Alpha = c.a * IN.color.a;
			}
			ENDCG

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"
				//#include "AutoLight.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;
				fixed4 _FlashColor;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}



				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;
				float _FlashAmount;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

				#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
				#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

					c.rgb = lerp(c.rgb, _FlashColor.rgb, _FlashAmount);
					c.rgb *= c.a;
					return c;
				}


			ENDCG
			}


        }
            Fallback "Legacy Shaders/Transparent/Cutout/VertexUnlit"
}
