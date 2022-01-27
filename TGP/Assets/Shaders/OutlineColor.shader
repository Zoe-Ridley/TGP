Shader "Unlit/OutlineGlow"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {} // The main texture 
		[HDR] _Color("Color", Color) = (1, 1, 1, 1) // The color of the outline glow
	}
	
		SubShader
		{
			Cull Off
			Blend One OneMinusSrcAlpha 
			
			Pass
			{
				CGPROGRAM
				
				#pragma vertex vertexFunc
				#pragma fragment fragmentFunc 
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				
				struct VS_OUTPUT
				{
					float4 pos : SV_POSITION; // vertex position in clip space
					half2 uv : TEXCOORD0;
				};
				
				VS_OUTPUT vertexFunc(appdata_base v)
				{
					VS_OUTPUT o;
					o.pos = UnityObjectToClipPos(v.vertex); // convert the vertex position to clip space
					o.uv = v.texcoord;
					
					return o;
				}
				
				fixed4 _Color;
				float4 _MainTex_TexelSize;
				
				fixed4 fragmentFunc(VS_OUTPUT i) : COLOR
				{
					half4 c = tex2D(_MainTex, i.uv); // The color of the texture
					c.rgb *= c.a; 
					half4 outlineC = _Color; // the color of the outline glow
					outlineC.a *= ceil(c.a);
					outlineC.rgb *= outlineC.a;

					/* Take the alpha value of pixels around the current pixel and if any of them is
					 * equal to 0 then we can assume that its near the edge and thus should have the
					 * glow outline applied to it. We subtract or add texel size to move between pixels.*/
					fixed upAlpha    = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
					fixed downAlpha  = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
					fixed leftAlpha  = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;
					fixed rightAlpha = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
					
					/* lerp between outline color and texture color based on alpha. We are using ceil to
					 * make sure that alpha will either be 1 or 0. if the value of alpha is anything above 0
					 * it will be rounded to 1 and if any of the alpha values is 0 the whole value will be 0
					 * and thus we will use the outline color otherwise if no alphas are 0 we will use the normal
					 * texture color */
					return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));
				}
				
				ENDCG
			}
		}
}