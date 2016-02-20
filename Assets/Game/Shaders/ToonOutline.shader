Shader "Toon/Outline" 
{
    Properties 
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (0, 1)) = .005
    }
    
    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        Pass 
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
		    #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
		    struct appdata {
		        float4 vertex : POSITION;
		        float3 normal : NORMAL;
		    };

		    struct v2f 
		    {
		        float4 pos : SV_POSITION;
		        fixed4 color : COLOR;
		        UNITY_FOG_COORDS(0)
		    };
            
		    uniform float _Outline;
		    uniform float4 _OutlineColor;
    
		    v2f vert(appdata v) 
		    {
		        v2f o;
		        
		        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		        o.color = _OutlineColor;

		        float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		        float2 offset = TransformViewToProjection(norm.xy);
		        o.pos.xy += offset * _Outline / 1000.0;
		        
		        UNITY_TRANSFER_FOG(o, o.pos);
		        return o;
		    }
            
            fixed4 frag(v2f i) : SV_Target
            {
            	UNITY_APPLY_FOG(i.fogCoord, i.color);
                return i.color;
            }
            ENDCG
        }
    }
    
    Fallback "Toon/Basic"
}