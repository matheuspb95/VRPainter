﻿Shader "Custom/Paint Diffuse" {

	Properties{
		_MainTex("Main Texture", Cube) = "white" {}
		_PaintMap("PaintMap", 2D) = "white" {} // texture to paint on
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

		Pass{
			cull front

			Lighting Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag


			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float3 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

			struct appdata {
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;

			};

			sampler2D _PaintMap;
			samplerCUBE _MainTex;
			float4 _MainTex_ST;
			v2f vert(appdata v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				//o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv0 = v.vertex;

				o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;// lightmap uvs

				return o;
			}

			half4 frag(v2f o) : COLOR{
				half4 main_color = half4(1,1,1,1); // main texture
				half4 paint = (tex2D(_PaintMap, o.uv1)); // painted on texture
				main_color *= paint; // add paint to main;
				return main_color;
			}
			ENDCG
		}
	}
}