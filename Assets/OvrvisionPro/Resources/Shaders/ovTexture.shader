Shader "Ovrvision/ovTexture" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SampleDistance ("Sample Distance", float) = 1.0
		_Threshold ("Threshold", float) = 0.1
	}
	SubShader {
		Tags { "Queue" = "Background+1" "RenderType"="Background" }
		LOD 200
		
		Pass {
			Lighting Off
			ZWrite Off
			ZTest Always
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			float _SampleDistance;
			float _Threshold;

			half4 frag (v2f_img i) : COLOR0
			{
				return tex2D(_MainTex, i.uv);
				/*
				//discard;

				float2 duvs[2] = {
					float2(_MainTex_TexelSize.x, 0)  * _SampleDistance,
					float2(0, -_MainTex_TexelSize.y) * _SampleDistance
				};
				half4 col1 = tex2D(_MainTex, i.uv);

				half col = 0;
				for (int j = 0; j < 2; j++)
				{
					half2 uv2 = i.uv + duvs[j];
					half4 col2 = tex2D(_MainTex, uv2);

					half3 dCol = col1.rgb - col2.rgb;
					if (length(dCol) > _Threshold)
					{
						col = 1.0;
					}
				}
				return half4(0, col, 0, 0.01);
				*/
			}
			ENDCG
		}
	} 
	FallBack Off
}
