Shader "Sprites/Diffuse Flash"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _SelfIllum("Self Illumination",Range(0.0,1.0)) = 0.0
        _FlashAmount("Flash Amount",Range(0.0,1.0)) = 0.0
        _DisappearRange("Disappear Range",Int) = 1
        _Disappear("Disappear Amount",Range(0.0,1.0)) = 0.0

        _AtlasStartHeight("AtlasStartHeight",Range(0.0,1.0)) = 0.0
        _AtlasEndHeight("AtlasEndHeight",Range(0.0,1.0)) = 1.0

        _Color("Tint", Color) = (1,1,1,1)

        _Red("Red Shift", Range(-2.0, 2.0)) = 0.0
        _Green("Green Shift", Range(-2.0, 2.0)) = 0.0
        _Blue("Blue Shift", Range(-2.0, 2.0)) = 0.0
        _Brightness("Brightness", Range(0.0, 10.0)) = 1.0
        _Contrast("Contrast", Range(0.0, 10.0)) = 1.0

        _ColorPalette("Color Palette", 2D) = "white" {}
        _SwapColorRatio("SwapColorRatio",Range(0.0,1.0)) = 0.0

        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
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

            Cull Off
            Lighting Off
            ZWrite Off
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha


            Pass
            {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile _ PIXELSNAP_ON
                #include "UnityCG.cginc"

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
                float _Disappear;
                float _DisappearRange;

                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.texcoord = IN.texcoord;
                    OUT.color = IN.color;

                    #ifdef PIXELSNAP_ON
                    OUT.vertex = UnityPixelSnap(OUT.vertex);
                    #endif
                    //OUT.texcoord.x* OUT.texcoord.y
                    //_SinTime* OUT.texcoord.x* OUT.texcoord.y;
                    //OUT.vertex.y = lerp(OUT.vertex.y, OUT.vertex.y - sin(OUT.texcoord.x * 100), _Disappear * OUT.texcoord.y);//_Disappear * (OUT.texcoord.x * OUT.texcoord.y));
                    return OUT;
                }

                sampler2D _MainTex;
                sampler2D _AlphaTex;
                sampler2D _ColorPalette;
                float _AlphaSplitEnabled;
                float _FlashAmount;

                fixed _Red;
                fixed _Green;
                fixed _Blue;
                fixed _Contrast;
                fixed _Brightness;
                fixed _SwapColorRatio;
                float _AtlasStartHeight;
                float _AtlasEndHeight;

                fixed4 SampleSpriteTexture(float2 uv)
                {
                    fixed4 color = tex2D(_MainTex, uv);
                    //fixed4 color = tex2D(_MainTex, float2(uv.x, _Disappear * uv.x * uv.y));

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                    if (_AlphaSplitEnabled)
                        color.a = tex2D(_AlphaTex, uv).r;
#endif  //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

                    return color;
                }

                half3 AdjustContrast(half3 color, half contrast) {
                    return saturate(lerp(half3(0.5, 0.5, 0.5), color, contrast));
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                    
                    fixed4 swapC = tex2D(_ColorPalette, float2(c.r, c.g));
                    c.rgb = lerp(c.rgb, swapC.rgb, _SwapColorRatio);

                    c.rgb = lerp(c.rgb, _Color, _FlashAmount);
                    c.rgb *= c.a;

                    // Si In.texcoord.Y < ratioDisappearence alors on désaffiche
                    float ratioDisappearence = (1 - _Disappear *( _AtlasEndHeight - _AtlasStartHeight)) - _AtlasStartHeight;
                    c.a *= clamp((ratioDisappearence - IN.texcoord.y) * 1000, 0, 1);
                    return c;
                }
            ENDCG
            }
                /*
            CGPROGRAM
            #pragma surface surf Lambert alpha vertex:vert
            #pragma multi_compile DUMMY PIXELSNAP_ON

            sampler2D _MainTex;
            fixed4 _Color;
            float _FlashAmount,_SelfIllum;

            struct Input
            {
                float2 uv_MainTex;
                fixed4 color;
            };

            void vert(inout appdata_full v, out Input o)
            {
                #if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
                v.vertex = UnityPixelSnap(v.vertex);
                #endif
                v.normal = float3(0,0,-1);

                UNITY_INITIALIZE_OUTPUT(Input, o);
                o.color = _Color;
            }

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
                o.Albedo = lerp(c.rgb,float3(1.0,1.0,1.0),_FlashAmount);
                //o.Emission = lerp(c.rgb,float3(1.0,1.0,1.0),_FlashAmount) * _SelfIllum;
                o.Alpha = c.a;
            }
            ENDCG*/
        }

            Fallback "Transparent/VertexLit"
}
