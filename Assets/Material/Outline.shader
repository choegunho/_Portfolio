Shader "Custom/SkinnedOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineWidth ("Outline Width", Float) = 0.02
    }

    SubShader
    {
        Tags { "Queue"="Geometry+1" "RenderType"="Opaque" }

        Cull Front

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineWidth;
            float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float3 normal = normalize(v.normal);
                v.vertex.xyz += normal * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}
