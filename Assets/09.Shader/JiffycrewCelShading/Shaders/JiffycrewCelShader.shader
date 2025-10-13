Shader "Jiffycrew/CelShading/JiffycrewCelShading"
{
	Properties
	{
		_MainTex("", 2D) = "white" {}

		_P0001("", float) = 0
		_P0002("", color) = (0, 0, 1, 1)
		_P0003("", color) = (1, 1, 1, 1)
		_P0004("", float) = 0
		_P0005("", color) = (1, 1, 1, 0)
		_P0006("", float) = 0
		
		_P0007("", Color) = (0, 0, 0, 0)
		_P0008("", float) = 1
		_P0009("", Color) = (0, 0, 0, 0)
		_P0010("", float) = 0
		
		_P0011("", 2D) = "white" {}
		_P0012("", 2D) = "white" {}
		_P0013("", 2D) = "black" {}
		_P0014("", 2D) = "white" {}
		_P0015("", 2D) = "white" {}
		_P0016("", 2D) = "black" {}
		_P0017("", Range(0, 2)) = 0.5

		_P0018("", Color) = (1, 1, 1, 0)
		_P0019("", float) = 0
		_P0020("", Range(0, 1)) = 0.7
		_P0021("", Range(0, 1)) = 0
		_P0022("", Range(0, 1)) = 1
		_P0023("", Range(0, 1)) = 0
		_P0024 ("", 2D) = "white" {}
		_P0025("", Range(-1, 1)) = 0
		_P0026("", Range( 0, 2)) = 1

		_P0027("", Color) = (1, 1, 1, 0)
		_P0028("", float) = 0
		_P0029("", Range(0, 1)) = 0.995
		_P0030("", Range(0, 1)) = 0
		_P0031("", Range(0, 2)) = 0
		_P0032 ("", 2D) = "white" {}
		_P0033("", Range(-1,1)) = 0
		_P0034("", Range( 0,2)) = 1
		_P0035("", Range(0,360)) = 0
		_P0036("", Range(-1,1)) = 0
		_P0037("", Range(-1,1)) = 0
		_P0038("", Range(-1,1)) = 0
		_P0039("", Range(-1,1)) = 0
		_P0040("", Range(0,1)) = 0
		_P0041("", Range(0,1)) = 0
		_P0042("", Range(0,10)) = 4
		_P0043("", Range(0,0.05)) = 0

		_P0044("", Color) = (1, 1, 1, 1)
		_P0045("", Range(0.5, 8)) = 4
		_P0046("", Range(0, 1)) = 0
		_P0049("", Range(0, 1)) = 0.7
		_P0050("", Range(0, 1)) = 0
		_P0051("", Range(0, 1)) = 1
		_P0052("", Range(0, 1)) = 0
		_P0053("", 2D) = "white" {}
		_P0054("", Range(-1, 1)) = 0
		_P0055("", Range( 0, 2)) = 1
		_P0056("", float) = 0

		_P0047("", Color) = (1.0, 1.0, 1.0, 1.0)

		_P0048("", Float) = 1.0

		_P0100("", Float) = 1
		_P0101("", Color) = (0.9, 0.9, 0.9, 1)

		_P0300("", Range(0, 1)) = 0.5

		[HideInInspector] _Mode("", Float) = 0.0
		[HideInInspector] _SrcBlend("", Float) = 1.0
		[HideInInspector] _DstBlend("", Float) = 0.0
		[HideInInspector] _ZWrite("", Float) = 1.0
		[HideInInspector] _OutlineConstMode("", Float) = 1.0 
		[HideInInspector] _OutlineTextureMode("", Float) = 1.0
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			"Queue"="Geometry"
		}	
		LOD 100

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex _F0021
			#pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
			#pragma multi_compile _ _M0040 _M0041
			#pragma multi_compile _ _M0050 _M0051
			#pragma multi_compile _ _M0060 _M0061

			#include "JiffycrewCelShader.cginc"

			fixed4 frag (_S0002 i) : SV_Target
			{
				float3 _V0001 = normalize(i._V0001);

				float4 _v0001 = _F0014(i);
				float3 _v0002 = _F0016(i);
				float3 _v0003 = _v0002 * tex2D(_P0200, i.uv);

				float3 _v0004 = (_v0001 + _v0003 + _F0019(_V0001,i.view)) ;

				_v0004 = _v0004 + tex2D(_P0013, i.uv);
#ifdef _ALPHATEST_ON
				clip(_v0001.a - _P0300);
#endif
				return float4(_v0004, _v0001.a*_P0018.a);
			}
			ENDCG
		}
		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			Cull Front
			Offset[_Offset1],[_Offset2]

			CGPROGRAM

			#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
			#pragma multi_compile _ _M0001
			#pragma multi_compile _ _M0002
			#pragma multi_compile _ _M0030 _M0031 _M0032
			#pragma multi_compile _ _M0040 _M0041
			#pragma multi_compile _ _M0050 _M0051
			#pragma multi_compile _ _M0060 _M0061
			#pragma multi_compile_instancing

			#pragma vertex _F0022
			#pragma fragment frag
			#include "JiffycrewCelShader.cginc"
			fixed4 frag(_S0002 _p0001) : SV_Target
			{
				float3 _V0001 = normalize(_p0001._V0001);

				float4 _v0001 = _F0014(_p0001);
				float3 _v0002 = _F0016(_p0001);
				float3 _v0003 = _v0002 * tex2D(_P0200, _p0001.uv);

				float3 _v0004 = (_v0001 + _v0003 + _F0019(_V0001,_p0001.view));

				_v0004 = _v0004 + tex2D(_P0013, _p0001.uv);

				float4 _v0005 = _P0101;
#ifdef _M0002
				_v0005 *= float4(_v0004, _v0001.a*_P0018.a);
#endif
#ifdef _ALPHATEST_ON
				clip(_v0005.a - _P0300);
#endif
				return _v0005;
			}
			ENDCG
		}

		// Pass to render object as a shadow caster
		Pass {
			Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
			#include "UnityCG.cginc"

			struct v2f {
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _MainTex_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			sampler2D _MainTex;
			fixed _P0300;

			float4 frag(v2f i) : SV_Target
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);

#ifdef _ALPHATEST_ON
				clip(texcol.a - _P0300);
#endif

				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG

		}
	}
	CustomEditor "JiffycrewCelShaderGUI"
}
