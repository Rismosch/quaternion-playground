Shader "Unlit/OneSphereCoordinateSystem"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Position("Position", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            float4 _Position;

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

                // Coordinate System (unit sphere and x/y axis)
                float diff = abs(magic - length(coord));
                float isOnCircle = clamp(1 - diff * 100, 0, 1);
                float isOnXAxis = clamp(1 - abs(coord.y) * 100, 0, 1);
                float isOnYAxis = clamp(1 - abs(coord.x) * 100, 0, 1);
                float isOnCoordinateSystem = isOnCircle + isOnXAxis + isOnYAxis;

                // Projection (dotted sine/cosine lines)
                float2 pos = (coord - _Position * magic);
                isOnXAxis = clamp(1 - abs(pos.y) * 100, 0, 1);
                isOnYAxis = clamp(1 - abs(pos.x) * 100, 0, 1);
                float sinX = 0.25 + 0.5 * sin(100 * i.uv.x + 100 * _Time);
                float sinY = 0.25 + 0.5 * sin(100 * i.uv.y + 100 * _Time);
                float coordLength = length(coord) / magic;
                float isInsideCircle = coordLength < 1;
                isOnXAxis *= sinX * isInsideCircle;
                isOnYAxis *= sinY * isInsideCircle;
                isOnXAxis = lerp(0, isOnXAxis, sign(_Position.x) == sign(coord.x));
                isOnYAxis = lerp(0, isOnYAxis, sign(_Position.y) == sign(coord.y));
                float isOnProjection = isOnXAxis + isOnYAxis;

                // Hypothenuse
                float slopeDot = dot(_Position, float2(coord.y, -coord.x));
                float isOnSlope = clamp(1 - abs(slopeDot) * 50, 0, 1);
                isOnSlope *= isInsideCircle;
                isOnSlope *= lerp(0, isOnSlope, sign(_Position.x) == sign(coord.x));
                isOnSlope *= lerp(0, isOnSlope, sign(_Position.y) == sign(coord.y));

                float alpha = isOnCoordinateSystem + isOnProjection + isOnSlope;
                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}
