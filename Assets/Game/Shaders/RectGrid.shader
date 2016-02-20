Shader "Grid/Rect" {
	Properties {
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _Width("Width", Range(0.0, 1.0)) = 0.5
        _GridNumX("GridNumX", Int) = 10
        _GridNumY("GridNumY", Int) = 10
        _CenterX("CneterX", Float) = 0.5
        _CenterY("CneterY", Float) = 0.5
        _ScrollSpeed("Scroll Speed", Float) = 3
        _Freq("Freq", Float) = 3
    }
    SubShader {
	    Tags { "Queue" = "Transparent" } 
        Pass {
        	Name "RECT"
      		Blend SrcAlpha OneMinusSrcAlpha
      		ZWrite On
      		Cull Off
      		
            CGPROGRAM
            
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            uniform half4 _Color;
            uniform half _Width;
            uniform int _GridNumX;
            uniform int _GridNumY;
            uniform half _CenterX;
            uniform half _CenterY;
            uniform half _ScrollSpeed;
            uniform half _Freq;
            
		    struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
		    
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord;
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
            	half t = _Time.y;
            	half x  = i.uv.x;
            	half y  = i.uv.y;
            	
            	half r0 = -1 + 2 * _Freq * fmod(t / _Freq, 1);
            	half aspect = half(_GridNumY) / _GridNumX * 0.5;
            	half r = sqrt(pow(x - _CenterX, 2) + aspect * pow(y - _CenterY, 2));
            	half circle = clamp(pow(0.1 / abs(r - r0), 1), 0.0, 3.0);
            	
            	y -= _ScrollSpeed / _GridNumY * fmod(t, 1);
            	
            	half x0 = fmod(x * _GridNumX, 1.0);
            	half y0 = fmod(y * _GridNumY, 1.0);
            	half p  = pow(1 / _Width, 2);
            	half x1 = pow(x0,       p);
            	half y1 = pow(y0,       p);
            	half x2 = pow(1.0 - x0, p);
            	half y2 = pow(1.0 - y0, p);
            	half4 grid = ((x1 + x2) + (y1 + y2)) * 0.5 * _Color;
            	
            	half alpha = (0.75 + circle) * _Color.a;
            	
				half4 col = grid * alpha;
            	
            	UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
    }
}