Shader "Custom/URP/BumpedSpecularRim_OriginalLike"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}

        _SpecColorTexture ("Specular Color", 2D) = "black" {}
        _Shininess ("Shininess", Range(0.03, 1)) = 0.078125

        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0)
        _RimPower ("Rim Power", Range(0.5,8)) = 3
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
        }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float2 uv         : TEXCOORD1;
                float3 normalWS   : TEXCOORD2;
                float3 tangentWS  : TEXCOORD3;
                float3 bitangentWS: TEXCOORD4;
                float3 viewDirWS  : TEXCOORD5;
            };

            TEXTURE2D(_MainTex);             SAMPLER(sampler_MainTex);
            TEXTURE2D(_BumpMap);             SAMPLER(sampler_BumpMap);
            TEXTURE2D(_SpecColorTexture);    SAMPLER(sampler_SpecColorTexture);

            float4 _Color;
            float _Shininess;
            float4 _RimColor;
            float _RimPower;

            Varyings vert (Attributes v)
            {
                Varyings o;

                VertexPositionInputs pos = GetVertexPositionInputs(v.positionOS.xyz);
                VertexNormalInputs nor = GetVertexNormalInputs(v.normalOS, v.tangentOS);

                o.positionCS = pos.positionCS;
                o.positionWS = pos.positionWS;
                o.uv = v.uv;

                o.normalWS = nor.normalWS;
                o.tangentWS = nor.tangentWS;
                o.bitangentWS = nor.bitangentWS;

                o.viewDirWS = GetWorldSpaceViewDir(pos.positionWS);

                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // ===== Texture =====
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                half3 albedo = tex.rgb * _Color.rgb;
                half gloss = tex.a;

                // ===== Normal =====
                float3 normalTS = UnpackNormal(
                    SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.uv)
                );

                float3x3 TBN = float3x3(
                    normalize(i.tangentWS),
                    normalize(i.bitangentWS),
                    normalize(i.normalWS)
                );

                float3 normalWS = normalize(mul(normalTS, TBN));

                // ===== View / Light =====
                float3 viewDir = normalize(i.viewDirWS);
                Light light = GetMainLight();
                float3 lightDir = normalize(-light.direction);

                // ===== Diffuse =====
                float NdotL = saturate(dot(normalWS, lightDir));
                float3 diffuse = albedo * NdotL * light.color;

                // ===== Specular (원본 BlinnPhong 대응) =====
                float3 halfDir = normalize(lightDir + viewDir);
                float spec = pow(
                    saturate(dot(normalWS, halfDir)),
                    _Shininess * 128
                );

                float3 specColor =
                    SAMPLE_TEXTURE2D(_SpecColorTexture, sampler_SpecColorTexture, i.uv).rgb;

                float3 specular = spec * specColor * light.color * gloss;

                // ===== Rim (Emission 방식 그대로) =====
                float rim = 1.0 - saturate(dot(viewDir, normalWS));
                float3 rimColor = _RimColor.rgb * pow(rim, _RimPower);

                float3 finalColor =
                    diffuse +
                    specular +
                    rimColor;

                return half4(finalColor, tex.a * _Color.a);
            }
            ENDHLSL
        }
    }
}
