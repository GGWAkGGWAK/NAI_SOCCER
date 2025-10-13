Shader "Jamong/Diffuse_Only"
{
    Properties
    {  _Color ("Color", Color) = (1,1,1)
       _MainTex ("Albedo (RGB)", 2D) = "white" {}
       _BrightDark("Brightness", Range(-1,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
       
        CGPROGRAM
               #pragma surface surf ASHUE 

      
       

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			};
		 float4 _Color;
		 float _BrightDark;     
	        
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color; 
            o.Albedo = c.rgb + _BrightDark;
            o.Alpha = c.a;
			}
			float4 LightingASHUE (SurfaceOutput s, float3 lightDir, float atten) 
			{
			float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;
			ndotl = pow(ndotl,3);
			float4 final;
			final.rgb = ndotl * s.Albedo * _LightColor0.rgb * atten;
			final.a = s.Alpha;
			return final;
			
        }
        ENDCG
    }
    FallBack "Diffuse"
}
