// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "US/US_WaterS"
{
	Properties
	{
		_TextureSample0("Normal", 2D) = "bump" {}
		_TextureSample1("Normal2", 2D) = "bump" {}
		_Speed("Speed", Range( 0 , 1)) = 1
		_Float1("Speed2", Range( 0 , 1)) = 1
		_Float0("法线强度", Range( 0 , 1)) = 0
		_Color0("主色", Color) = (0.5754717,0.5754717,0.5754717,1)
		_Color1("边缘色", Color) = (1,0,0,0)
		_Float2("边缘范围", Float) = 1
		_Float3("整体透明度", Range( 0 , 1)) = 1

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		GrabPass{ }

		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
			uniform sampler2D _TextureSample0;
			uniform float _Speed;
			uniform float4 _TextureSample0_ST;
			uniform sampler2D _TextureSample1;
			uniform float _Float1;
			uniform float4 _TextureSample1_ST;
			uniform float _Float0;
			uniform float4 _Color1;
			uniform float4 _Color0;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _Float2;
			uniform float _Float3;
			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord = screenPos;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float4 screenPos = i.ase_texcoord;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 _Vector0 = float2(1,0);
				float2 uv0_TextureSample0 = i.ase_texcoord1.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float2 panner3 = ( ( _Speed * _Time.y ) * _Vector0 + uv0_TextureSample0);
				float2 appendResult27 = (float2(_Vector0.x , 0.0));
				float2 uv0_TextureSample1 = i.ase_texcoord1.xy * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
				float2 panner24 = ( ( _Float1 * _Time.y ) * appendResult27 + uv0_TextureSample1);
				float4 screenColor12 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_grabScreenPosNorm + float4( ( max( UnpackNormal( tex2D( _TextureSample0, panner3 ) ) , UnpackNormal( tex2D( _TextureSample1, panner24 ) ) ) * _Float0 ) , 0.0 ) ).xy/( ase_grabScreenPosNorm + float4( ( max( UnpackNormal( tex2D( _TextureSample0, panner3 ) ) , UnpackNormal( tex2D( _TextureSample1, panner24 ) ) ) * _Float0 ) , 0.0 ) ).w);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth32 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float distanceDepth32 = abs( ( screenDepth32 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Float2 ) );
				float4 lerpResult34 = lerp( _Color1 , _Color0 , saturate( distanceDepth32 ));
				
				
				finalColor = ( ( screenColor12 * lerpResult34 ) * _Float3 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17700
0;6;1920;1013;2637.005;1058.001;2.759972;True;True
Node;AmplifyShaderEditor.Vector2Node;4;-1735.805,52.45063;Inherit;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;26;-1492.882,1033.054;Inherit;False;Property;_Float1;Speed2;3;0;Create;False;0;0;False;0;1;0.283;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1493.763,382.1088;Inherit;False;Property;_Speed;Speed;2;0;Create;True;0;0;False;0;1;0.2262941;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;21;-1571.644,890.4965;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;5;-1480.764,178.6811;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;27;-1383.221,713.7017;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1242.874,798.0309;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1402.764,-157.3189;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1152,176;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1497.184,538.5681;Inherit;False;0;19;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;3;-1030.763,14.68115;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;24;-1061.001,641.4172;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-822.7917,-8.525062;Inherit;True;Property;_TextureSample0;Normal;0;0;Create;False;0;0;False;0;-1;None;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;bump;LockedToTexture2D;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-848.8467,414.1242;Inherit;True;Property;_TextureSample1;Normal2;1;0;Create;False;0;0;False;0;-1;None;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;bump;LockedToTexture2D;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;31;-410.3879,200.3316;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-346.817,442.8631;Inherit;False;Property;_Float0;法线强度;4;0;Create;False;0;0;False;0;0;0.768;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;49.35327,589.3331;Inherit;False;Property;_Float2;边缘范围;7;0;Create;False;0;0;False;0;1;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;32;291.704,558.9667;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-218.5972,81.93298;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GrabScreenPosition;10;-588.8169,-264.1369;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;11;130.1831,-22.1369;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;14;84.50768,180.581;Inherit;False;Property;_Color0;主色;5;0;Create;False;0;0;False;0;0.5754717,0.5754717,0.5754717,1;1,0.514151,0.514151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;33;45.55042,376.5556;Inherit;False;Property;_Color1;边缘色;6;0;Create;False;0;0;False;0;1,0,0,0;0,1,0.2333652,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;40;406.5504,408.5556;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;34;426.5504,181.5556;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;12;311.1831,-30.1369;Inherit;False;Global;_GrabScreen0;Grab Screen 0;3;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;621.1831,-43.1369;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;654.5504,154.5556;Inherit;False;Property;_Float3;整体透明度;8;0;Create;False;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;976.6675,28.12634;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;15;1183.865,45.6628;Float;False;True;-1;2;ASEMaterialInspector;100;1;US/US_WaterS;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Transparent=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;27;0;4;0
WireConnection;25;0;26;0
WireConnection;25;1;21;2
WireConnection;6;0;7;0
WireConnection;6;1;5;2
WireConnection;3;0;2;0
WireConnection;3;2;4;0
WireConnection;3;1;6;0
WireConnection;24;0;23;0
WireConnection;24;2;27;0
WireConnection;24;1;25;0
WireConnection;1;1;3;0
WireConnection;19;1;24;0
WireConnection;31;0;1;0
WireConnection;31;1;19;0
WireConnection;32;0;35;0
WireConnection;8;0;31;0
WireConnection;8;1;9;0
WireConnection;11;0;10;0
WireConnection;11;1;8;0
WireConnection;40;0;32;0
WireConnection;34;0;33;0
WireConnection;34;1;14;0
WireConnection;34;2;40;0
WireConnection;12;0;11;0
WireConnection;13;0;12;0
WireConnection;13;1;34;0
WireConnection;16;0;13;0
WireConnection;16;1;38;0
WireConnection;15;0;16;0
ASEEND*/
//CHKSM=DF54525FE8A506484ADD085462FC33A092E327CF