Shader "Jamong/Eff_Holo"
{
	Properties
	{
		_BumpMap("NormalMap", 2D) = "white" {}
		_EmissionColor("Emission Color", Color) = (0, 1, 1, 0)
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

			CGPROGRAM

			#pragma surface surf nolight noambient alpha:fade

			sampler2D _BumpMap;
			half4 _EmissionColor;

			struct Input
			{
				float2 uv_BumpMap;
				float3 viewDir;
				float3 worldPos;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Emission = _EmissionColor;
				float rim = saturate(dot(o.Normal, IN.viewDir));
				rim = saturate(pow(1 - rim, 1.2) + pow(frac(IN.worldPos.g * 0.25 - (_Time.y * 0.5)), 5)* 0.1);
				o.Alpha = rim;
			}
			float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
			{
				return float4(0, 0, 0, s.Alpha);
			}
			ENDCG
		}
			FallBack "Transparent/Diffuse"
}