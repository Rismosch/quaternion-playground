Shader "Unlit/Line"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (0, 0, 0, 0)
        _Length("Length", Float) = 1.0
        _Mirror("Mirror (bool)", Float) = 1.0
        _Animate("Animate (bool)", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        cull Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
            float4 _Color;
            float _Mirror;
            float _Length;
            float _Animate;

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
                float2 coord = (i.uv - 0.5) * 2;

                float diff = abs(coord.y);
                float isOnXAxis = clamp(1 - diff * 75, 0, 1);

                float isOnLine = lerp(0, 1, abs(coord.x) < abs(_Length));

                float isOnPositiveX = (coord.x >= 0) * (_Length >= 0);
                float isOnNegativeX = (coord.x < 0) * (_Length < 0);
                float isOnSide = isOnPositiveX + isOnNegativeX;
                float isOnMirror = lerp(isOnSide, 1, _Mirror > 0);

                float animation = 0.25 + 0.5 * sin(100 * coord.x + 100 * _Time);
                float isOnAnimation = animation * _Animate + (1 - _Animate);

                float alpha = isOnXAxis * isOnLine * isOnMirror * isOnAnimation;

                return fixed4(_Color.xyz, alpha);
            }
            ENDCG
        }
    }
}
