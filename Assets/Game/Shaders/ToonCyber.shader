Shader "Toon/Cyber" 
{
    Properties 
    {
        _MainColor ("Main Color", Color) = (.5,.5,.5,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (0, 1)) = .005
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        
        Pass 
        {
           Cull Off
            
           CGPROGRAM
           
           #pragma vertex vert
           #pragma fragment frag
           #pragma multi_compile_fog

           #include "UnityCG.cginc"

            float4 _MainColor;

            struct appdata 
            {
                half4 vertex : POSITION;
                half3 normal : NORMAL;
            };
            
            struct v2f 
            {
                half4 pos : SV_POSITION;
                half factor : FLOAT;
                UNITY_FOG_COORDS(0)
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		        float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		        float f = dot(norm, float3(-1.0, -1.0, -1.0));
		        o.factor = f < 0.22362 ? 0 : (f < 0.75 ? 0.5 : 1);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
            	fixed4 c = fixed4(_MainColor.rgb * i.factor, 1);
            	UNITY_APPLY_FOG(i.fogCoord, c);
                return c;
            }
            
            ENDCG           
        }
        
        UsePass "Toon/Outline/OUTLINE"
    } 

    Fallback "VertexLit"
}