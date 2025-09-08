Shader "Custom/ExpandingColorLit"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _NewColor ("New Color", Color) = (1,0,0,1)
        _Point ("Effect Origin", Vector) = (0,0,0,0)
        _Progress ("Progress", Range(0,1)) = 0
        _Radius ("Radius", Float) = 5
        _MainTex ("Texture", 2D) = "white" {} // Add this line
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
            float2 uv_MainTex; // Add this line
        };

        float4 _BaseColor;
        float4 _NewColor;
        float3 _Point;
        float _Progress;
        float _Radius;
        sampler2D _MainTex; // Add this line

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float dist = distance(IN.worldPos, _Point);
            float threshold = _Progress * _Radius;

            float t = saturate(1 - smoothstep(threshold - 0.1, threshold, dist));

            float4 texColor = tex2D(_MainTex, IN.uv_MainTex);

            // Calculate the grayscale value (luminosity) of the texture
            float grayscale = dot(texColor.rgb, float3(0.299, 0.587, 0.114));

            // Use the grayscale value to modulate the new color
            float3 baseCol = _BaseColor.rgb;
            float3 newCol = _NewColor.rgb;
            // Check if baseColor is white (all components are 1)
            bool isBaseWhite = all(baseCol == float3(1,1,1));
            bool isNewWhite = all(newCol == float3(1,1,1));
            bool isDisk = (_NewColor.r == 0 && _NewColor.a == 0.5);

            float3 modBaseColor = isBaseWhite ? baseCol : (isDisk ? texColor.rgb : baseCol * grayscale);
            float3 modNewColor = isNewWhite ? newCol : (isDisk ? texColor.rgb : newCol * grayscale);

            float3 finalColor = lerp(modBaseColor, modNewColor, t);

            o.Albedo = finalColor;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}