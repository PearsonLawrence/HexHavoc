Shader "Custom/SeeThroughWalls"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _IsPlayer ("Is Player", Float) = 0.0
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        ZWrite On
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            uniform float4 _Color;
            uniform float _IsPlayer;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _Color;
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                if (_IsPlayer > 0.5)
                {
                    // If the object is the player, render with a different color
                    return i.color;
                }
                else
                {
                    // Render other objects normally, but with alpha based on depth
                    fixed4 col = i.color;
                    col.a = 1.0 - i.pos.z;
                    return col;
                }
            }
            ENDCG
        }
    }
}
