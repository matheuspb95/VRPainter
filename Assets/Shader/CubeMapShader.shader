Shader "Unlit/CubeMapShader"
{
   //Cubemap usado no shader
   Properties {
      _Cube("cube map", Cube) = "" {}
   }
   SubShader {
      Pass {
         //Renderiza o objeto por dentro
         cull front
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
                      
         #include "UnityCG.cginc"

         uniform samplerCUBE _Cube;

         struct appdata {
            float4 vertex : POSITION;
         };

         struct v2f {
            float3 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
         };
            
         v2f vert (appdata v) {
            v2f o;
            o.uv = v.vertex;
            o.vertex = UnityObjectToClipPos(v.vertex);
            return o;
         }
         //Renderiza o cubemap como textura do material
         fixed4 frag (v2f i) : COLOR {
            float4 result = texCUBE(_Cube, i.uv);
            return result;
         }
         ENDCG
      }
   }
}
