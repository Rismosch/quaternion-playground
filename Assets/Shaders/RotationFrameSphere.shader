Shader "Unlit/RotationFrameSphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Red("Red", Color) = (0, 0, 0)
        _Green("Green", Color) = (0, 0, 0)
        _Blue("Blue", Color) = (0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Red;
            float4 _Green;
            float4 _Blue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = i.uv;
                float3 normal = i.normal;

                float3 chopped = normal - fmod(normal, 0.3);
                float3 red = _Red * chopped.x;
                float3 green = _Green * chopped.y;
                float3 blue = _Blue * chopped.z;
                float3 result = red + green + blue;

                return fixed4(result, 0.65);
            }

            ENDCG
        }
    }
}
