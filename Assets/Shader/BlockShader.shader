Shader "Custom/BlockShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlockProcess ("Process", Range(0, 1.0)) = 0.0
        _BlockColor ("BlockColor", Color) = (1,1,1,1)
        _PathColor ("PathColor", Color) = (.5,.5,.5,.5)
        _ProcessColor ("ProcessColor", Color) = (.5,.5,.5,.5)
        [KeywordEnum(Horizontal, Vertical, L_Shape)] _Mode ("Fill Mode", Float) = 0
        [Toggle] _InvertHDirection ("Invert Horizontal", Float) = 0
        [Toggle] _InvertVDirection ("Invert Vertical", Float) = 0
        [Toggle] _ReverseFill ("Reverse Fill", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _MODE_HORIZONTAL _MODE_VERTICAL _MODE_L_SHAPE

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 _BlockColor;
            float4 _PathColor;
            float4 _ProcessColor;
            float _BlockProcess, _InvertHDirection, _InvertVDirection, _ReverseFill;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                if(_InvertHDirection > .5) uv.x = 1 - uv.x;
                if(_InvertVDirection > .5) uv.y = 1 - uv.y;

                float dist = 0;
                bool isInPath = false;

                #if _MODE_HORIZONTAL
                    isInPath = abs(uv.y - .5) < .1 && abs(uv.x - .5) < .45;
                    dist = (uv.x - .05) / .9;

                #elif _MODE_VERTICAL
                    isInPath = abs(uv.y - .5) < .45 && abs(uv.x - .5) < .1;
                    dist = (uv.y - .05) / .9;

                #elif _MODE_L_SHAPE
                    bool horizontal = (uv.x >= .05 && uv.x <= .5) && (abs(uv.y - .5) < .1);
                    bool vertical = (uv.y >= .5 - .1 && uv.y <= .95) && (abs(uv.x - .5) < .1);

                    isInPath = horizontal || vertical;

                    if(horizontal){
                        dist = lerp(0, .5 + .1, (uv.x - .05) / .45);
                    }else if(vertical){
                        dist = lerp(.5 + .1, 1.0, (uv.y - .5) / .45);
                    }
                #endif
                if(_ReverseFill > .5){
                    dist = 1.0 - dist;
                }

                fixed4 color = _BlockColor;
                if(isInPath){
                    color = (dist < _BlockProcess) ? _ProcessColor : _PathColor;
                }

                return color;
            }
            ENDCG
        }
    }
}
