Shader "BombardRange"
{
	//https://sakaf.net/posts/progress-shaders/ の記事コードを参考・改変したもの
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Degree("Degree", Range(0, 360)) = 0
	}
	SubShader {
		Tags {
			"Queue"="Transparent"
			"RenderType"="Trasparent"
		}
		LOD 200

		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			float _Degree;

			#define PI 3.14159265f
			#define PI_H 1.57079633f

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				const float deg = _Degree / 360;
				const float2 center = float2(0.5, 0.5);
				fixed4 color = _Color;

				float a = step(distance(center, i.uv), 0.5);

				float2 pos = (i.uv - center) * 2.0;
				float angle = -(atan2(pos.y, pos.x) - PI_H) / (PI * 2.0);
				angle = angle < 0 ? angle + 1.0 : angle;

				a *= step(angle, deg);
				color.a *= a;

				return color;
			}
			ENDCG
		}
	}
}
