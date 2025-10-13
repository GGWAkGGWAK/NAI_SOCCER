
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"

float4 _P0011_ST;

float4 _P0002;
bool _P0001;
float4 _P0003;
bool _P0004;
float4 _P0005;
bool _P0006;

float4 _P0007;
float _P0008;
float4 _P0009;
float _P0010;

sampler2D _P0011;
sampler2D _P0016;
sampler2D _P0014;
sampler2D _P0012;
sampler2D _P0013;
sampler2D _P0015;
sampler2D _P0200;

float _P0017;
float4 _P0018;
bool _P0019;

float _P0020;
float _P0021;
float _P0023;
float _P0022;
sampler2D _P0024;
float _P0025;
float _P0026;
float4 _P0027;

float4 _P0044;
float _P0045;
float _P0046;
float _P0049;
float _P0050;
float _P0051;
float _P0052;
sampler2D _P0053;
float _P0054;
float _P0055;

bool _P0028;
float _P0029;
float _P0030;
float _P0031;
sampler2D _P0032;
float _P0033;
float _P0034;

float _P0035;
float _P0038;
float _P0039;
float _P0040;
float _P0041;
float _P0036;
float _P0037;
float _P0042;
float _P0043;

float4 _P0047;
float _P0048;
float _P0100;
float _ZSmooth;
float4 _P0101;

float _P0300;

struct _S0001 {
	float4 _V0002 : POSITION;
	float4 _V0003 : TANGENT;
	float3 _V0001 : NORMAL;
	float4 _V0004 : TEXCOORD0;
};

struct _S0002
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	half3 _V0001   : TEXCOORD1;
	half4 _V0003  : TEXCOORD2;
	float3 view : TEXCOORD3;
	float3 worldPos : TEXCOORD4;
	SHADOW_COORDS(5)
};
			
float3 _F0001(float3 _p0001, float3 _p0002, float3 _p0003, float _p0004, float _p0005, float _p0006)
{
	float3 _v0001 = _p0006 >= _p0005 ? _p0001 : _p0002;
	return _p0006 >= _p0004 ? _v0001 : _p0003;
}

float3 _F0002(float3 _p0001)
{
	//return _p0001 * 0.5f + 0.5f;
	return clamp(_p0001, 0.0, 1.0);
}

float3 _F0003(float3 _p0001)
{
	return _p0001 * 2 - 1;
}

float3 _F0004(float _p0001, float _p0002, float _p0003)
{
	float _v0001 = clamp((_p0003 - _p0001) / (_p0002 - _p0001), 0, 1);
	return _F0002(sin((_v0001 - 0.5f)*3.141592));
}

float3 _F0005(float3 _p0001, float3 _p0002 = float3(0,0,1))
{
	return dot(_p0001, _p0001) >= 0.000001f ? normalize(_p0001) : _p0002;
}

float3 _F0006(float3 _p0001, float3 _p0002)
{
	return _F0005(_p0001 + _p0002, float3(0, 0, 0));
}

float3 _F0007(float3 _p0001, float3 _p0002, float _p0003, float _p0004, float _p0005, float2 _p0006, float _p0007)
{
	float _v0001 = _F0003(_p0004 - _p0005);
	float _v0002 = _F0003(_p0004 + _p0005);

	float _v0003 = _F0004(_v0001, _v0002, _p0003);
	_v0003 = min(_p0007, clamp(_v0003, _p0006.x, _p0006.y));

	return lerp(_p0002, _p0001, _v0003);
}

float3 _F0008(float3 _p0001, float3 _p0002, float _p0003, sampler2D _p0004, float _p0005, float _p0006, float2 _p0007, float _p0008)
{
	float2 _v0001 = (_F0002(_p0003) - _p0005) * _p0006;
	_v0001.x = min(_p0008, clamp(_v0001.x, _p0007.x, _p0007.y));

	//return lerp(_p0002, _p0001, tex2D(_p0004, _v0001).r);
	return _p0001 * tex2D(_p0004, _v0001).rgb;
}

float3 _F0009(float3 _p0001, float3 _p0002, float _p0003) {
	float _v0001 = radians(_p0003);
	float3 _v0002 = float3(0,0,0);
	_v0002 += cos(_v0001) * _p0001;
	_v0002 += sin(_v0001) * cross(_p0002, _p0001);
	_v0002 += dot(_p0001, _p0002) * _p0002 * (1 - cos(_v0001));
				
	return _v0002;
}

float3 _F0010(float3 _p0001, float3 _V0001, float3 _V0003, float3 _p0002) {
	float3 _v0001 = _F0005(_F0009(_p0001, _V0001, _P0035));

	float3 _v0002 = _F0005(_v0001 + _P0036 * _V0003 + _P0037 * _p0002);
	float3 _v0003 = _F0005(_v0002 - _P0038 * dot(_v0002, _V0003) * _V0003 - _P0039 * dot(_v0002, _p0002) * _p0002);
	float3 _v0004 = _F0005(_v0003 - _P0040 * sign(dot(_v0003, _V0003)) * _V0003 - _P0041 * sign(dot(_v0003, _p0002)) * _p0002);
				

	float3 _v0005 = _F0005(dot(_v0004, _V0003) * _V0003 + dot(_v0004, _p0002) * _p0002);
	float _v0006 = min(acos(dot(_v0005, _V0003)), acos(dot(_v0005, _p0002)));
	_P0042 = 4;
	float _v0007 = pow(abs(sin(2 * _v0006)), _P0042);
	float3 _v0008 = _F0005(_v0004 - _P0043 * _v0007 * _v0005);

	return _v0008;
}

float3 _F0011()
{
	return _P0004 ? _P0003 : _LightColor0.rgb;
}

float3 _F0012()
{
	return _P0006 ? _P0005 : _LightColor0.rgb;
}

float3 _F0013()
{
	float3 _v0001 = float3(0, 0, 1);
	return _P0001 ? _F0005(_P0002, _v0001) : _F0005(_WorldSpaceLightPos0.rgb, _v0001);
}

float4 _F0014(_S0002 _p0001)
{
	fixed4 _v0001 = tex2D(_P0011, _p0001.uv);
	fixed4 _v0002 = tex2D(_P0016, _p0001.uv);
	float3 _v0003 = _v0001 * _F0011();
	float3 _v0004 = _v0002.rgb * _P0047;

	float _v0005 = 1.0;
	float3 _v0006 = float3(0, 0, 1);
	float3 _v0007 = float3(1, 0, 0);

	float3 _v0008 = _F0013();


	float3 _v0009 = cross(_v0008, _v0006);
	float3 _v0010 = cross(_v0008, _v0009);

	float _v0011 = 0.7;

	float3 _V0012 = _F0005(_v0008 + _v0005 * _v0009);
	float3 _V0013 = _F0005(_v0008 - _v0005 * _v0009);
	float3 _V0014 = _F0005(_v0008 + _v0005 * _v0010);
	float3 _V0015 = _F0005(_v0008 - _v0005 * _v0010);
	float3 _V0016 = _F0005(_v0008 + _v0011 * (_v0005 * _v0009 + _v0005 * _v0010));
	float3 _V0017 = _F0005(_v0008 + _v0011 * (_v0005 * _v0009 - _v0005 * _v0010));
	float3 _V0018 = _F0005(_v0008 - _v0011 * (_v0005 * _v0009 + _v0005 * _v0010));
	float3 _V0019 = _F0005(_v0008 - _v0011 * (_v0005 * _v0009 - _v0005 * _v0010));
	float3 _V0020 = _F0005(float3(_v0008.x, -_v0008.y, _v0008.z));

	float _V0021 = 1.5;

	float3 _V0001 = _F0005(_V0021 * _p0001._V0001 +float3(_v0008.x, _v0008.y, _v0008.z));
	float3 _v0021 = _F0005(_V0021 * _p0001._V0001 + _V0012);
	float3 _v0022 = _F0005(_V0021 * _p0001._V0001 + _V0013);
	float3 _v0023 = _F0005(_V0021 * _p0001._V0001 + _V0014);
	float3 _v0024 = _F0005(_V0021 * _p0001._V0001 + _V0015);
	float3 _v0025 = _F0005(_V0021 * _p0001._V0001 + _V0016);
	float3 _v0026 = _F0005(_V0021 * _p0001._V0001 + _V0017);
	float3 _v0027 = _F0005(_V0021 * _p0001._V0001 + _V0018);
	float3 _v0028 = _F0005(_V0021 * _p0001._V0001 + _V0019);
	float3 _v0029 = _F0005(_V0021 * _p0001._V0001 + _V0020);

	float _v0030 = dot(_F0013(), _V0001)
		+ clamp(dot(_F0005(_V0012, float3(0, 0, 1)), _v0021), 0, 1)
		+ clamp(dot(_F0005(_V0013, float3(0, 0, 1)), _v0024), 0, 1)
		+ clamp(dot(_F0005(_V0014, float3(0, 0, 1)), _v0021), 0, 1)
		+ clamp(dot(_F0005(_V0015, float3(0, 0, 1)), _v0024), 0, 1)
		+ clamp(dot(_F0005(_V0016, float3(0, 0, 1)), _v0025), 0, 1)
		+ clamp(dot(_F0005(_V0017, float3(0, 0, 1)), _v0026), 0, 1)
		+ clamp(dot(_F0005(_V0018, float3(0, 0, 1)), _v0027), 0, 1)
		+ clamp(dot(_F0005(_V0019, float3(0, 0, 1)), _v0028), 0, 1)
		+ clamp(dot(_F0005(_V0020, float3(0, 0, 1)), _v0029), 0, 1);

	_v0030 *= (SHADOW_ATTENUATION(_p0001));
	_v0030 /= 10.0;

	float4 _v0033 = tex2D(_P0012, _p0001.uv);
	float _v0034 = 1.0;

	float _v0031 = _v0033.r;
	float _v0032 = _v0033.g;

	float3 _v0035 = 0;
#ifdef _M0041
	_v0035 = _F0008(
		_v0003 * _P0018,
		_v0004,
		_v0030,
		_P0024,
		_P0025,
		_P0026,
		float2(_P0023, _P0022),
		_v0034
	);
#else
	_v0035 = _F0007(
		_v0003 * _P0018,
		_v0004,
		_v0030,
		_P0020,
		_P0021,
		float2(_P0023, _P0022),
		_v0034
	);
#endif

	return float4(_v0035 * _v0031 + _v0032 * _P0017, _v0001.a);
}

float3 _F0015(float3 _V0001, float3 _V0003, float _p0001) {
	return normalize(cross(_V0001, _V0003) * _p0001);
}

float3 _F0016(_S0002 _p0001)
{
	float3 _v0001 = _F0012();

	float3 _V0001 = normalize(_p0001._V0001);
	float3 _V0003 = normalize(_p0001._V0003.xyz);

	float3 _v0002 = _F0006(_F0013(), _p0001.view);

	float3 _v0003 = _F0015(_V0001, _V0003, _p0001._V0003.w);
	float3 _v0004 = _F0010(_v0002, _V0001, _V0003, _v0003);
	float _v0030 = dot(_V0001, _v0004);
	_v0030 *= (SHADOW_ATTENUATION(_p0001));

	float3 _v0035 = 0;
#ifdef _M0051
	_v0035 = _F0008(
		_v0001 * _P0027,
		0,
		_v0030,
		_P0032,
		_P0033,
		_P0034,
		float2(0, 1),
		1
	);
#else
	_v0035 = _F0007(
		_v0001 * _P0027,
		0,
		_v0030,
		_P0029,
		_P0030,
		float2(0, 1),
		1
	) * _P0031;
#endif
	return _v0035;
}

float _F0015(float3 _p0001, float3 _p0003, float _p0004, float _p0005)
{
	float _v0002 = dot(_p0001, _p0003);
	float _v0003 = sqrt(1.0 - _v0002 * _v0002);
	return pow(_v0003, _p0004) * _p0005;
}

float _F0017(float3 _p0001, float3 _p0002, float3 _p0003, float _p0004, float _p0005)
{
	float3 _v0001 = normalize(_p0003 + _p0002);
	float _v0002 = dot(_p0001, _v0001);
	float _v0003 = sqrt(1.0 - _v0002 * _v0002);
	float _v0004 = smoothstep(-1.0, 0.0, _v0002);
	return _v0004 * pow(_v0003, _p0004) * _p0005;
}

float3 _F0023(float3 _p0001, float3 _p0002, float _p0003)
{
	return normalize(_p0001 + _p0002 * _p0003);
}

float4 _F0024(_S0002 _p0001)
{
	fixed4 _v0001 = tex2D(_P0011, _p0001.uv);
	fixed4 _v0002 = tex2D(_P0016, _p0001.uv);
	float3 _v0003 = _v0001 * _F0011();
	float3 _v0004 = _v0002.rgb * _P0047;

	float _v0005 = 1.0;
	float3 _v0006 = float3(0, 0, 1);
	float3 _v0007 = float3(1, 0, 0);

	float3 _v0008 = _F0013();


	float3 _v0009 = cross(_v0008, _v0006);
	float3 _v0010 = cross(_v0008, _v0009);

	float _v0011 = 0.7;

	float3 _V0012 = _F0005(_v0008 + _v0005 * _v0009);
	float3 _V0013 = _F0005(_v0008 - _v0005 * _v0009);
	float3 _V0014 = _F0005(_v0008 + _v0005 * _v0010);
	float3 _V0015 = _F0005(_v0008 - _v0005 * _v0010);
	float3 _V0016 = _F0005(_v0008 + _v0011 * (_v0005 * _v0009 + _v0005 * _v0010));
	float3 _V0017 = _F0005(_v0008 + _v0011 * (_v0005 * _v0009 - _v0005 * _v0010));
	float3 _V0018 = _F0005(_v0008 - _v0011 * (_v0005 * _v0009 + _v0005 * _v0010));
	float3 _V0019 = _F0005(_v0008 - _v0011 * (_v0005 * _v0009 - _v0005 * _v0010));
	float3 _V0020 = _F0005(float3(_v0008.x, -_v0008.y, _v0008.z));

	float _V0021 = 1.5;

	float3 _V0001 = _F0005(_V0021 * _p0001._V0001 +float3(_v0008.x, _v0008.y, _v0008.z));
	float3 _v0021 = _F0005(_V0021 * _p0001._V0001 + _V0012);
	float3 _v0022 = _F0005(_V0021 * _p0001._V0001 + _V0013);
	float3 _v0023 = _F0005(_V0021 * _p0001._V0001 + _V0014);
	float3 _v0024 = _F0005(_V0021 * _p0001._V0001 + _V0015);
	float3 _v0025 = _F0005(_V0021 * _p0001._V0001 + _V0016);
	float3 _v0026 = _F0005(_V0021 * _p0001._V0001 + _V0017);
	float3 _v0027 = _F0005(_V0021 * _p0001._V0001 + _V0018);
	float3 _v0028 = _F0005(_V0021 * _p0001._V0001 + _V0019);
	float3 _v0029 = _F0005(_V0021 * _p0001._V0001 + _V0020);
	
	//float3 _V0003 = normalize(_p0001._V0003.xyz);
	float3 _V0002 = normalize(_p0001._V0003.xyz);
	float3 _V0003 = cross(_V0001, _V0002);
	//float3 _V0003 = GetTangent(_p0001);

	float3 _v0040 = _F0023(_V0003, _V0001, _P0037);
	float3 _v0041 = _F0023(_V0003, _v0021, _P0037);
	float3 _v0042 = _F0023(_V0003, _v0022, _P0037);
	float3 _v0043 = _F0023(_V0003, _v0023, _P0037);
	float3 _v0044 = _F0023(_V0003, _v0024, _P0037);
	float3 _v0045 = _F0023(_V0003, _v0025, _P0037);
	float3 _v0046 = _F0023(_V0003, _v0026, _P0037);
	float3 _v0047 = _F0023(_V0003, _v0027, _P0037);
	float3 _v0048 = _F0023(_V0003, _v0028, _P0037);
	float3 _v0049 = _F0023(_V0003, _v0029, _P0037);

	float _v0030 = _F0015(_v0040, _v0008, 1.0, 1.0);
		//+ clamp(_F0015(_v0041, _v0021, 1.0, 1.0), 0, 1)
		//+ clamp(_F0015(_v0042, _v0022, 1.0, 1.0), 0, 1)
		//+ clamp(_F0015(_v0043, _v0023, 1.0, 1.0), 0, 1)
		//+ clamp(_F0015(_v0044, _v0024, 1.0, 1.0), 0, 1)
		//+ clamp(_F0015(_v0045, _v0025, 1.0, 1.0), 0, 1)
		//+ clamp(_F0015(_v0046, _v0026, 1.0, 1.0), 0, 1)
		//+ clamp(_F0015(_v0047, _v0027, 1.0, 1.0), 0, 1)
		//+ clamp(_F0015(_v0048, _v0028, 1.0, 1.0), 0, 1)
		//+ clamp(_F0015(_v0049, _v0029, 1.0, 1.0), 0, 1);

	_v0030 *= (SHADOW_ATTENUATION(_p0001));
	//_v0030 /= 10.0;

	float4 _v0033 = tex2D(_P0012, _p0001.uv);
	float _v0034 = 1.0;

	float _v0031 = _v0033.r;
	float _v0032 = _v0033.g;

	float3 _v0035 = 0;
#ifdef _M0041
	_v0035 = _F0008(
		_v0003 * _P0018,
		_v0004,
		_v0030,
		_P0024,
		_P0025,
		_P0026,
		float2(_P0023, _P0022),
		_v0034
	);
#else
	_v0035 = _F0007(
		_v0003 * _P0018,
		_v0004,
		_v0030,
		_P0020,
		_P0021,
		float2(_P0023, _P0022),
		_v0034
	);
#endif

	return float4(_v0035 * _v0031 + _v0032 * _P0017, _v0001.a);
}

float3 _F0018(_S0002 _p0001)
{
	float3 _v0001 = _F0012();

	float3 _V0001 = normalize(_p0001._V0001);
	//float3 _V0003 = cross(_V0001, cross(_V0001, float3(0.0, 1.0, 0.0)));
	//float3 _V0003 = normalize(_p0001._V0003.xyz);
	float3 _V0002 = normalize(_p0001._V0003.xyz);
	float3 _V0003 = cross(_V0001, _V0002);
	
	float3 _v0003 = _F0023(_V0003, _V0001, _P0037);
	float _v0030 = _F0017(_v0003, normalize(_p0001.view), _F0013(), 1.0, 1.0);
	_v0030 *= (SHADOW_ATTENUATION(_p0001));
	
	float3 _v0035 = 0;
#ifdef _M0051
	_v0035 = _F0008(
		_v0001 * _P0027,
		0,
		_v0030,
		_P0032,
		_P0033,
		_P0034,
		float2(0, 1),
		1
	);
#else
	_v0035 = _F0007(
		_v0001 * _P0027,
		0,
		_v0030,
		_P0029,
		_P0030,
		float2(0, 1),
		1
	) * _P0031;
#endif
	return _v0035;
	//return _v0001 * _P0027 * _v0030;
}

float3 _F0019(float3 _V0001, float3 _p0001)
{
	float _v0001 = pow(1-clamp(dot(_V0001, _p0001), 0, 1), _P0045);
	float3 _v0002 = _P0044;
	
	float _v0034 = 1.0;
	float3 _v0035 = 0;
#ifdef _M0061
	_v0035 = _F0008(
		_v0002,
		0,
		_v0001 * _P0046,
		_P0053,
		_P0054,
		_P0055,
		float2(0, 1),
		_v0034
	);
#else
	_v0035 = _F0007(
		_v0002,
		0,
		_v0001 * _P0046,
		_P0049,
		_P0050,
		float2(0, 1),
		_v0034
	);
#endif

	return _v0035;
}

_S0001 _F0020(_S0001 v)
{
	return v;
}

_S0002 _F0021(_S0001 v)
{
	_S0002 o;
	o.pos = UnityObjectToClipPos(v._V0002);
	o.uv = TRANSFORM_TEX(v._V0004, _P0011);
	o._V0001 = UnityObjectToWorldNormal(v._V0001);

				
	float3 _V0003 = UnityObjectToWorldDir(v._V0003.xyz);
	half _v0001 = v._V0003.w * unity_WorldTransformParams.w;
	o._V0003 = float4(_V0003, _v0001);

	o.view = -normalize(mul(unity_ObjectToWorld, v._V0002).xyz - _WorldSpaceCameraPos.xyz);
	o.worldPos = mul(unity_ObjectToWorld, v._V0002);

	UNITY_TRANSFER_FOG(o, o.pos);
	TRANSFER_SHADOW(o)
	return o;
}

_S0002 _F0022(_S0001 v)
{
	_S0002 o;
	o.uv = TRANSFORM_TEX(v._V0004, _P0011);
	o._V0001 = UnityObjectToWorldNormal(v._V0001);

#ifdef JIFFYCREW_TANGENT_AS_NORMALS
	float3 _V0001 = v._V0003.xyz;
#else
	float3 _V0001 = v._V0001;
#endif

#ifdef _M0001
	float _v0001 = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, v._V0002));
	float4 _v0002 = float4(UnityObjectToViewPos(v._V0002 + float4(_V0001, 0) * _P0100 * 0.01 * _v0001), 1.0);
#else
	float4 _v0002 = float4(UnityObjectToViewPos(v._V0002 + float4(_V0001, 0) * _P0100 * 0.01), 1.0);
#endif
	o.pos = mul(UNITY_MATRIX_P, _v0002);

	float3 _V0003 = UnityObjectToWorldDir(v._V0003.xyz);
	half _v0003 = v._V0003.w * unity_WorldTransformParams.w;
	o._V0003 = float4(_V0003, _v0003);

	o.view = -normalize(mul(unity_ObjectToWorld, v._V0002).xyz - _WorldSpaceCameraPos.xyz);
	o.worldPos = mul(unity_ObjectToWorld, v._V0002);

	UNITY_TRANSFER_FOG(o, o.pos);
	TRANSFER_SHADOW(o)
	return o;
}
