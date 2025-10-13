Shader "Custom/FOWShader2"
{

    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _AlphaTex("Alpha (A)", 2D) = "white" {}
    }
        SubShader{
        Blend SrcAlpha OneMinusSrcAlpha

        Pass{
        SetTexture[_MainTex]{ Combine texture}
        SetTexture[_AlphaTex]{Combine previous - texture}
        }
    }
    
}

