Shader "Grid/RectOutLine" 
{
	Properties 
	{
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _Width("Width", Range(0.0, 1.0)) = 0.5
        _GridNumX("GridNumX", Int) = 10
        _GridNumY("GridNumY", Int) = 10
        _CenterX("CneterX", Float) = 0.5
        _CenterY("CneterY", Float) = 0.5
        _ScrollSpeed("Scroll Speed", Float) = 3
        _Freq("Freq", Float) = 3
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (0, 10)) = 1
    }
    SubShader 
    {
	    Tags { "Queue" = "Transparent" } 
	    UsePass "Grid/Rect/RECT"
	    UsePass "Toon/Outline/OUTLINE"
	}
}