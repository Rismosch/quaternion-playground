Shader "Unlit/ProjectedOneSphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
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
                float2 coord = (i.uv - 0.5) * 2;
                float magic = 0.9;

                // North
                float north = coord.y - 0.5;
                float isOnNorth = clamp(1 - abs(north) * 50, 0, 1);
                isOnNorth *= lerp(0, isOnNorth, coord.x < magic);
                isOnNorth *= lerp(0, isOnNorth, coord.x > -magic);

                // South
                float south = coord.y + 0.5;
                float isOnSouth = clamp(1 - abs(south) * 50, 0, 1);
                isOnSouth *= lerp(0, isOnSouth, coord.x < magic);
                isOnSouth *= lerp(0, isOnSouth, coord.x > -magic);

                // Equator Left
                float2 left_position = coord + float2(0.5, 0);
                float left_distance = length(left_position);
                float left_clamped = 1 - left_distance * 40;
                float left_alpha = lerp(0, left_clamped, left_distance < 0.05) * 5;

                // Equator Right
                float2 right_position = coord - float2(0.5, 0);
                float right_distance = length(right_position);
                float right_clamped = 1 - right_distance * 40;
                float right_alpha = lerp(0, right_clamped, right_distance < 0.05) * 5;

                float alpha = isOnNorth + isOnSouth + left_alpha + right_alpha;
                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}
