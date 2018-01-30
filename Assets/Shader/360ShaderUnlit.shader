Shader "Unlit/360ShaderUnlit"
{
	Properties
	{
		_MainTex("Base(RGB)", 2D) = "white"{}
		_Constrast("Contrast", float) = 0
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 300
		Cull Front
		//This is used to print the texture inside of the sphere
		CGPROGRAM
		#pragma surface surf SimpleLambert
		half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
		{
			half4 c;
			c.rgb = s.Albedo;
			return c;
		}

		sampler2D _MainTex;
		sampler2D _BumpMap;
		uniform float _Constrast;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			IN.uv_MainTex.x = 1 - IN.uv_MainTex.x;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			c = c * 0.5;
			//c = (c - 0.1)*_Constrast + 0.1;
			float whitefact = (c.r + c.g + c.b) / 3;

			o.Albedo = c.rgb; // *pow((1 - whitefact), (1 - whitefact));
			//o.Albedo = c.rgb * pow((0.5 *((c.r + c.b + c.b) / 3)), 0.6);
			//o.Albedo = c.rgb * pow(c.r, 0.4) * pow(c.g, 0.4) *pow(c.b, 0.4);
			
		}
		ENDCG
	}
	Fallback "Diffuse"
}