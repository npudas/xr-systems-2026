Shader "Custom/MagnifierLens"
{
    Properties
    {
        _MagnifierRT("Magnifier RenderTexture", 2D) = "white" {}
        _Strength("Refraction Strength", Range(0,1)) = 0.1
        _IOR("Index of Refraction", Range(1.0, 2.0)) = 1.5
        _RollCorrection("Roll Correction", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };

            TEXTURE2D_X(_MagnifierRT);
            SAMPLER(sampler_MagnifierRT);

            float _Strength;
            float _IOR;

            Varyings vert (Attributes input)
            {
                Varyings output;
                float3 posWS = TransformObjectToWorld(input.positionOS);
                output.positionHCS = TransformWorldToHClip(posWS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);

                // viewDir = camera position - fragment position
                float3 camPosWS = GetWorldSpaceViewDir(posWS).xyz + posWS.xyz; // world camera pos
                output.viewDirWS = normalize(camPosWS - posWS.xyz);

                output.screenPos = ComputeScreenPos(output.positionHCS);
                return output;
            }

            float _RollCorrection;

            /*float2 RotateUV(float2 uv, float angle)
            {
                float rad = radians(angle);
                float s = sin(rad);
                float c = cos(rad);

                float2 centered = uv - 0.5;

                float2 rotated;
                rotated.x = centered.x * c - centered.y * s;
                rotated.y = centered.x * s + centered.y * c;

                return rotated + 0.5;
            }*/

            half4 frag(Varyings input) : SV_Target
            {
                float3 normal = normalize(input.normalWS);
                float3 viewDir = normalize(input.viewDirWS);

                // directional refraction
                float eta = 1.0 / _IOR;
                float3 refractedDir = refract(-viewDir, normal, eta);

                // map refracted direction to screen UV
                float2 screenUV = input.screenPos.xy / input.screenPos.w;

                // radial magnification
                float2 centeredUV = screenUV - 0.5;
                float r = length(centeredUV);
                float radialAmplification = 1.0 + r * 2.0;

                // final UV offset
                float2 offset = refractedDir.xy * _Strength * radialAmplification;
                float2 distortedUV = screenUV + offset;

                distortedUV = clamp(distortedUV, 0.001, 0.999);
                //float2 correctedUV = RotateUV(distortedUV, -_RollCorrection);
                half4 col = SAMPLE_TEXTURE2D_X(_MagnifierRT, sampler_MagnifierRT, distortedUV);

                return half4(col.rgb, 0.95); // slight transparency
            }

            ENDHLSL
        }
    }
}

/*Shader "Custom/MagnifierLens"
{
    Properties
    {
        [MainTexture] _MainTex ("Render Texture", 2D) = "white" {}
        [Magnification] _Magnification ("Magnification Strength", Range(-1.5, 1.5)) = 0.1
        [Edge Softness] _EdgeSoftness ("Edge Softness", Range(0.01, 0.2)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //float3 normal : NORMAL;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Magnification;
            float _EdgeSoftness;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // Get the vector from the camera to the object in object space
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(worldPos - _WorldSpaceCameraPos);
                
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Parallax shift
                float2 shiftedUV = i.uv + (i.viewDir.xy * _Magnification);

                // Create a vignette/mask so the edges don't look 'glitchy'
                float dist = distance(i.uv, float2(0.5, 0.5));
                float mask = smoothstep(0.5, 0.5 - _EdgeSoftness, dist);

                fixed4 col = tex2D(_MainTex, shiftedUV);
                
                // Add sheen
                float3 worldNormal = UnityObjectToWorldNormal(float3(0,0,1));
                float fresnel = pow(1.0 - saturate(dot(worldNormal, i.viewDir)), 3.0);
                col.rgb += fresnel * 0.5;
                // Darken the image near the physical edges of the lens mesh
                return col * mask;
            }
            ENDCG
        }
    }
}*/