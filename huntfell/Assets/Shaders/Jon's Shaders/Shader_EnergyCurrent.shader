// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:Standard,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32765,y:32474,varname:node_4795,prsc:2|diff-9132-OUT,normal-4172-RGB,emission-9132-OUT,olwid-8135-OUT,olcol-4159-RGB;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31565,y:32662,ptovrint:False,ptlb:Electric Gradient,ptin:_ElectricGradient,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a65fdabb80940b647ba5e69ef5538613,ntxv:0,isnm:False|UVIN-3048-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:31871,y:32888,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-7364-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:31565,y:32833,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:31565,y:32991,ptovrint:True,ptlb:Electric Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Append,id:8955,x:29794,y:32847,varname:node_8955,prsc:2|A-7214-OUT,B-4155-OUT;n:type:ShaderForge.SFN_Time,id:9975,x:29794,y:33010,varname:node_9975,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7214,x:29607,y:32847,ptovrint:False,ptlb:U Speed,ptin:_USpeed,varname:node_7214,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:4155,x:29607,y:32956,ptovrint:False,ptlb:V Speed,ptin:_VSpeed,varname:node_4155,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:6694,x:29961,y:32847,varname:node_6694,prsc:2|A-8955-OUT,B-9975-T;n:type:ShaderForge.SFN_TexCoord,id:1661,x:29961,y:33000,varname:node_1661,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:5841,x:30142,y:32847,varname:node_5841,prsc:2|A-6694-OUT,B-1661-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:7799,x:30308,y:32847,varname:node_7799,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-5841-OUT,TEX-3828-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:3828,x:30142,y:33011,ptovrint:False,ptlb:Electric Noise,ptin:_ElectricNoise,varname:node_3828,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:9430,x:29795,y:33164,varname:node_9430,prsc:2|A-2500-OUT,B-8284-OUT;n:type:ShaderForge.SFN_Time,id:3274,x:29795,y:33326,varname:node_3274,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6445,x:29961,y:33164,varname:node_6445,prsc:2|A-9430-OUT,B-3274-T;n:type:ShaderForge.SFN_TexCoord,id:9503,x:29961,y:33316,varname:node_9503,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:2500,x:29607,y:33164,ptovrint:False,ptlb:U Speed 2,ptin:_USpeed2,varname:_uspeed_copy,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.2;n:type:ShaderForge.SFN_ValueProperty,id:8284,x:29607,y:33244,ptovrint:False,ptlb:V Speed 2,ptin:_VSpeed2,varname:_Vspeed_copy,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.2;n:type:ShaderForge.SFN_Add,id:2946,x:30142,y:33164,varname:node_2946,prsc:2|A-6445-OUT,B-9503-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:5968,x:30325,y:33164,varname:node_5968,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-2946-OUT,TEX-3828-TEX;n:type:ShaderForge.SFN_Slider,id:450,x:29804,y:32702,ptovrint:False,ptlb:Electric Dissolve,ptin:_ElectricDissolve,varname:node_450,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2820513,max:1;n:type:ShaderForge.SFN_OneMinus,id:2017,x:30142,y:32662,varname:node_2017,prsc:2|IN-450-OUT;n:type:ShaderForge.SFN_RemapRange,id:5395,x:30308,y:32662,varname:node_5395,prsc:2,frmn:0,frmx:1,tomn:-0.65,tomx:0.65|IN-2017-OUT;n:type:ShaderForge.SFN_Add,id:3930,x:30468,y:32662,varname:node_3930,prsc:2|A-5395-OUT,B-7799-R;n:type:ShaderForge.SFN_Add,id:6835,x:30485,y:32847,varname:node_6835,prsc:2|A-5395-OUT,B-5968-R;n:type:ShaderForge.SFN_Multiply,id:4928,x:30669,y:32662,varname:node_4928,prsc:2|A-3930-OUT,B-6835-OUT;n:type:ShaderForge.SFN_RemapRange,id:2080,x:30870,y:32662,varname:node_2080,prsc:2,frmn:0,frmx:1,tomn:-10,tomx:10|IN-4928-OUT;n:type:ShaderForge.SFN_Clamp01,id:7942,x:31031,y:32662,varname:node_7942,prsc:2|IN-2080-OUT;n:type:ShaderForge.SFN_OneMinus,id:6054,x:31207,y:32662,varname:node_6054,prsc:2|IN-7942-OUT;n:type:ShaderForge.SFN_Append,id:3048,x:31391,y:32662,varname:node_3048,prsc:2|A-6054-OUT,B-9660-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7364,x:31565,y:33175,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_7364,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:7;n:type:ShaderForge.SFN_Vector1,id:9660,x:31207,y:32784,varname:node_9660,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:148,x:31819,y:32662,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:node_148,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:ebf045037130c664a821b96ac7d48ed1,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4172,x:32063,y:32415,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_4172,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b3a991b2729e9f14290a16c3074bf445,ntxv:3,isnm:True;n:type:ShaderForge.SFN_ValueProperty,id:8135,x:32366,y:32906,ptovrint:False,ptlb:Outline Width,ptin:_OutlineWidth,varname:node_8135,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Color,id:4159,x:32366,y:32991,ptovrint:False,ptlb:Outline Color,ptin:_OutlineColor,varname:node_4159,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:9132,x:32164,y:32652,varname:node_9132,prsc:2|A-148-RGB,B-2393-OUT;proporder:148-4172-6074-3828-797-7364-450-7214-4155-2500-8284-8135-4159;pass:END;sub:END;*/

Shader "Shader Forge/Jon's Shaders/Energy Current" {
    Properties {
        _Albedo ("Albedo", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _ElectricGradient ("Electric Gradient", 2D) = "white" {}
        _ElectricNoise ("Electric Noise", 2D) = "white" {}
        _TintColor ("Electric Color", Color) = (0.5,0.5,0.5,1)
        _Opacity ("Opacity", Float ) = 7
        _ElectricDissolve ("Electric Dissolve", Range(0, 1)) = 0.2820513
        [HideInInspector]_USpeed ("U Speed", Float ) = 0.2
        [HideInInspector]_VSpeed ("V Speed", Float ) = 0.1
        [HideInInspector]_USpeed2 ("U Speed 2", Float ) = -0.2
        [HideInInspector]_VSpeed2 ("V Speed 2", Float ) = -0.2
        _OutlineWidth ("Outline Width", Float ) = 0.1
        _OutlineColor ("Outline Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _OutlineWidth;
            uniform float4 _OutlineColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_OutlineWidth,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_OutlineColor.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _ElectricGradient; uniform float4 _ElectricGradient_ST;
            uniform float4 _TintColor;
            uniform float _USpeed;
            uniform float _VSpeed;
            uniform sampler2D _ElectricNoise; uniform float4 _ElectricNoise_ST;
            uniform float _USpeed2;
            uniform float _VSpeed2;
            uniform float _ElectricDissolve;
            uniform float _Opacity;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float node_5395 = ((1.0 - _ElectricDissolve)*1.3+-0.65);
                float4 node_9975 = _Time;
                float2 node_5841 = ((float2(_USpeed,_VSpeed)*node_9975.g)+i.uv0);
                float4 node_7799 = tex2D(_ElectricNoise,TRANSFORM_TEX(node_5841, _ElectricNoise));
                float4 node_3274 = _Time;
                float2 node_2946 = ((float2(_USpeed2,_VSpeed2)*node_3274.g)+i.uv0);
                float4 node_5968 = tex2D(_ElectricNoise,TRANSFORM_TEX(node_2946, _ElectricNoise));
                float2 node_3048 = float2((1.0 - saturate((((node_5395+node_7799.r)*(node_5395+node_5968.r))*20.0+-10.0))),0.0);
                float4 _ElectricGradient_var = tex2D(_ElectricGradient,TRANSFORM_TEX(node_3048, _ElectricGradient));
                float3 node_9132 = (_Albedo_var.rgb+(_ElectricGradient_var.rgb*i.vertexColor.rgb*_TintColor.rgb*_Opacity));
                float3 diffuseColor = node_9132;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = node_9132;
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _ElectricGradient; uniform float4 _ElectricGradient_ST;
            uniform float4 _TintColor;
            uniform float _USpeed;
            uniform float _VSpeed;
            uniform sampler2D _ElectricNoise; uniform float4 _ElectricNoise_ST;
            uniform float _USpeed2;
            uniform float _VSpeed2;
            uniform float _ElectricDissolve;
            uniform float _Opacity;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float node_5395 = ((1.0 - _ElectricDissolve)*1.3+-0.65);
                float4 node_9975 = _Time;
                float2 node_5841 = ((float2(_USpeed,_VSpeed)*node_9975.g)+i.uv0);
                float4 node_7799 = tex2D(_ElectricNoise,TRANSFORM_TEX(node_5841, _ElectricNoise));
                float4 node_3274 = _Time;
                float2 node_2946 = ((float2(_USpeed2,_VSpeed2)*node_3274.g)+i.uv0);
                float4 node_5968 = tex2D(_ElectricNoise,TRANSFORM_TEX(node_2946, _ElectricNoise));
                float2 node_3048 = float2((1.0 - saturate((((node_5395+node_7799.r)*(node_5395+node_5968.r))*20.0+-10.0))),0.0);
                float4 _ElectricGradient_var = tex2D(_ElectricGradient,TRANSFORM_TEX(node_3048, _ElectricGradient));
                float3 node_9132 = (_Albedo_var.rgb+(_ElectricGradient_var.rgb*i.vertexColor.rgb*_TintColor.rgb*_Opacity));
                float3 diffuseColor = node_9132;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
