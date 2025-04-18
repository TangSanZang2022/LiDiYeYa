// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "US/US_WaterJ"
{
	Properties
	{
		_TextureSample0("Normal", 2D) = "bump" {}
		_TextureSample1("Normal2", 2D) = "bump" {}
		_Speed("Speed", Range( 0 , 1)) = 1
		_Float1("Speed2", Range( 0 , 1)) = 1
		_Float0("法线强度", Range( 0 , 1)) = 0
		[HDR]_Color0("主色", Color) = (0.5754717,0.5754717,0.5754717,1)
		[HDR]_Color1("边缘色", Color) = (1,0,0,0)
		_Float2("边缘范围", Float) = 1
		_Float3("整体透明度", Range( 0 , 1)) = 1
		_Float4("填充", Range( 0 , 1)) = 0.5167453
		[Toggle(_KEYWORD0_ON)] _Keyword0("反向填充", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _KEYWORD0_ON
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _TextureSample0;
		uniform half _Speed;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample1;
		uniform half _Float1;
		uniform float4 _TextureSample1_ST;
		uniform half _Float0;
		uniform half4 _Color1;
		uniform half4 _Color0;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform half _Float2;
		uniform half _Float4;
		uniform half _Float3;


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


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			half4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			half2 _Vector0 = half2(0,1);
			float2 uv0_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			half2 panner3 = ( ( _Speed * _Time.y ) * _Vector0 + uv0_TextureSample0);
			half2 appendResult27 = (half2(_Vector0.x , 0.0));
			float2 uv0_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			half2 panner24 = ( ( _Float1 * _Time.y ) * appendResult27 + uv0_TextureSample1);
			half4 screenColor12 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_grabScreenPosNorm + half4( ( max( UnpackNormal( tex2D( _TextureSample0, panner3 ) ) , UnpackNormal( tex2D( _TextureSample1, panner24 ) ) ) * _Float0 ) , 0.0 ) ).xy/( ase_grabScreenPosNorm + half4( ( max( UnpackNormal( tex2D( _TextureSample0, panner3 ) ) , UnpackNormal( tex2D( _TextureSample1, panner24 ) ) ) * _Float0 ) , 0.0 ) ).w);
			half4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth32 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			half distanceDepth32 = abs( ( screenDepth32 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Float2 ) );
			half4 lerpResult34 = lerp( _Color1 , _Color0 , saturate( distanceDepth32 ));
			o.Emission = ( screenColor12 * lerpResult34 ).rgb;
			half temp_output_42_0 = ( 0.0 + i.uv_texcoord.y );
			#ifdef _KEYWORD0_ON
				half staticSwitch53 = temp_output_42_0;
			#else
				half staticSwitch53 = ( 1.0 - temp_output_42_0 );
			#endif
			o.Alpha = ( saturate( step( staticSwitch53 , _Float4 ) ) * _Float3 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
2054;115;1920;1007;1072.632;115.868;1.127439;True;True
Node;AmplifyShaderEditor.RangedFloatNode;26;-1492.882,1033.054;Inherit;False;Property;_Float1;Speed2;3;0;Create;False;0;0;False;0;1;0.283;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1493.763,382.1088;Inherit;False;Property;_Speed;Speed;2;0;Create;True;0;0;False;0;1;0.2262941;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;21;-1571.644,890.4965;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;5;-1480.764,178.6811;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;4;-1825.805,53.45063;Inherit;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1497.184,538.5681;Inherit;False;0;19;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;27;-1383.221,713.7017;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1242.874,798.0309;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1402.764,-157.3189;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1152,176;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;3;-1030.763,14.68115;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;24;-1061.001,641.4172;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-247.6478,742.8333;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-848.8467,414.1242;Inherit;True;Property;_TextureSample1;Normal2;1;0;Create;False;0;0;False;0;-1;None;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;bump;LockedToTexture2D;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-45.45789,745.4367;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-822.7917,-8.525062;Inherit;True;Property;_TextureSample0;Normal;0;0;Create;False;0;0;False;0;-1;None;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;bump;LockedToTexture2D;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;31;-410.3879,200.3316;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;35;12.98603,582.3394;Inherit;False;Property;_Float2;边缘范围;7;0;Create;False;0;0;False;0;1;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;52;300.6218,815.8232;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-346.817,442.8631;Inherit;False;Property;_Float0;法线强度;4;0;Create;False;0;0;False;0;0;0.768;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;32;255.3367,551.973;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;16.4042,1117.824;Inherit;False;Property;_Float4;填充;9;0;Create;False;0;0;False;0;0.5167453;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;53;501.6218,727.8232;Inherit;False;Property;_Keyword0;反向填充;10;0;Create;False;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;10;-588.8169,-264.1369;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-218.5972,81.93298;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;14;48.14043,173.5873;Inherit;False;Property;_Color0;主色;5;1;[HDR];Create;False;0;0;False;0;0.5754717,0.5754717,0.5754717,1;1,0.514151,0.514151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;40;370.1831,401.5619;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;43;771.5314,799.428;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;130.1831,-22.1369;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;33;9.183173,369.5619;Inherit;False;Property;_Color1;边缘色;6;1;[HDR];Create;False;0;0;False;0;1,0,0,0;0,1,0.2333651,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;45;984.4415,627.8136;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;1192.336,691.8907;Inherit;False;Property;_Float3;整体透明度;8;0;Create;False;0;0;False;0;1;0.762;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;12;311.1831,-30.1369;Inherit;False;Global;_GrabScreen0;Grab Screen 0;3;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;34;390.1831,174.5619;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;1062.319,235.8676;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;621.1831,-43.1369;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;1590.406,243.4737;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;51;2084.698,10.1603;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;US/US_WaterJ;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
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
WireConnection;19;1;24;0
WireConnection;42;1;41;2
WireConnection;1;1;3;0
WireConnection;31;0;1;0
WireConnection;31;1;19;0
WireConnection;52;0;42;0
WireConnection;32;0;35;0
WireConnection;53;1;52;0
WireConnection;53;0;42;0
WireConnection;8;0;31;0
WireConnection;8;1;9;0
WireConnection;40;0;32;0
WireConnection;43;0;53;0
WireConnection;43;1;44;0
WireConnection;11;0;10;0
WireConnection;11;1;8;0
WireConnection;45;0;43;0
WireConnection;12;0;11;0
WireConnection;34;0;33;0
WireConnection;34;1;14;0
WireConnection;34;2;40;0
WireConnection;13;0;12;0
WireConnection;13;1;34;0
WireConnection;16;0;45;0
WireConnection;16;1;38;0
WireConnection;51;2;13;0
WireConnection;51;9;16;0
ASEEND*/
//CHKSM=A2735AEFE9894B087845C0D462F79D46D31B315A