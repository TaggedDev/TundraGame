Shader "Custom/SnowShaders" {
	Properties{
		[Header(Tesselation)]
		_MaxTessDistance("Max Tessellation Distance", Range(10,100)) = 50
		_Tess("Tessellation", Range(0,32)) = 20
		[Space]
		[Header(Snow)]
		[HDR]_Color("Snow Color", Color) = (0.5,0.5,0.5,1)	
		_SnowHeight("Snow Height", Range(0,2)) = 0.3
		[Space]
		[Header(Snow Path)]
		_SnowPathStrength("Snow Path Depth", Range(0,4)) = 2
	}

	SubShader{
		Tags{ "RenderType"= "Opaque" "LightMode" = "ForwardBase" }
		

		CGPROGRAM
		
		#pragma surface surf Lambert vertex:vert addshadow nolightmap tessellate:tessDistance fullforwardshadows
		#pragma target 4.0
		#pragma require tessellation tessHW
		#include "Tessellation.cginc"
		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	

		uniform float3 _Position;
		uniform sampler2D _GlobalEffectRT;
		uniform float _OrthographicCamSize;

		float _Tess;
		float _MaxTessDistance;

		float CalcDistanceTessFactor(float4 vertex, float minDist, float maxDist, float tess)
		{
			float3 worldPosition = mul(unity_ObjectToWorld, vertex).xyz;
			float dist = distance(worldPosition, _WorldSpaceCameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0);
			return f * tess;
		}

		float4 DistanceBasedTess(float4 v0, float4 v1, float4 v2, float minDist, float maxDist, float tess)
		{
			float3 f;
			f.x = CalcDistanceTessFactor(v0, minDist, maxDist, tess);
			f.y = CalcDistanceTessFactor(v1, minDist, maxDist, tess);
			f.z = CalcDistanceTessFactor(v2, minDist, maxDist, tess);

			return UnityCalcTriEdgeTessFactors(f);
		}

		float4 tessDistance(appdata_full v0, appdata_full v1, appdata_full v2)
		{
			float minDist = 10.0;
			float maxDist = _MaxTessDistance;

			return DistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
		}

		float4 _Color;
		float _SnowHeight, _SnowPathStrength;
		float4 _PathColorIn, _PathColorOut;
		float _PathBlending;

		struct Input {
			float2 uv_MainTex : TEXCOORD0;
			float3 worldPos; // world position built-in value
			float4 position : SV_POSITION;
		};



		void vert(inout appdata_full v)
		{	
			
			float3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
			// Effects RenderTexture Reading
			float2 uv = worldPosition.xz - _Position.xz;
			uv = uv / (_OrthographicCamSize * 2);
			uv += 0.5;			
			float4 RTEffect = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
			
			// smoothstep edges to prevent bleeding
			RTEffect *=  smoothstep(0.99, 0.9, uv.x) * smoothstep(0.99, 0.9,1- uv.x);
			RTEffect *=  smoothstep(0.99, 0.9, uv.y) * smoothstep(0.99, 0.9,1- uv.y);
			
			
			// move vertices up where snow is, and where there is no path	
			v.vertex.xyz += normalize(v.normal) * _SnowHeight * saturate(1 - RTEffect.g * _SnowPathStrength);
		}
		void surf(Input IN, inout SurfaceOutput o) {
			// Effects RenderTexture Reading
			float2 uv = IN.worldPos.xz - _Position.xz;
			uv /= (_OrthographicCamSize * 2);
			uv += 0.5;

			float4 effect = tex2D(_GlobalEffectRT, float2 (uv.x, uv.y));
			effect *=  smoothstep(0.99, 0.9, uv.x) * smoothstep(0.99, 0.9,1- uv.x);
			effect *=  smoothstep(0.99, 0.9, uv.y) * smoothstep(0.99, 0.9,1- uv.y);
			
			// Compute the normal from the interpolated world position.
			half3 normal = normalize(cross(ddy(IN.worldPos), ddx(IN.worldPos)));

			half nl = saturate(dot(normal, _WorldSpaceLightPos0.xyz));

			o.Albedo = fixed4(_Color * (_LightColor0.rgb * nl + ShadeSH9(half4(normal.xyz, 1))), 1);
		}


		ENDCG
	}

	Fallback "Diffuse"
}