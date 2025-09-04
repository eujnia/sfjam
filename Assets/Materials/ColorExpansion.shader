Shader "Custom/ExpandingColorLit"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _NewColor ("New Color", Color) = (1,0,0,1)
        _Point ("Effect Origin", Vector) = (0,0,0,0)
        _Progress ("Progress", Range(0,1)) = 0
        _Radius ("Radius", Float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        struct Input
        {
            float3 worldPos;
        };

        float4 _BaseColor;
        float4 _NewColor;
        float3 _Point;
        float _Progress;
        float _Radius;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float dist = distance(IN.worldPos, _Point);
            float threshold = _Progress * _Radius;

            float t = saturate(1 - smoothstep(threshold - 0.1, threshold, dist));

            float3 finalColor = lerp(_BaseColor.rgb, _NewColor.rgb, t);

            o.Albedo = finalColor;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
