// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:2,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32765,y:32474,varname:node_4795,prsc:2|diff-5128-OUT,emission-7350-OUT,amdfl-7350-OUT,custl-7350-OUT,clip-1561-G;n:type:ShaderForge.SFN_Tex2d,id:1561,x:32224,y:32909,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_1561,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1a2b4c4c8de549741ae0f4f6f36c02fa,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6822,x:32031,y:32483,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:node_6822,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1a2b4c4c8de549741ae0f4f6f36c02fa,ntxv:0,isnm:False|UVIN-5519-OUT;n:type:ShaderForge.SFN_Tex2d,id:1432,x:32035,y:32016,ptovrint:False,ptlb:Emmission,ptin:_Emmission,varname:_Custom_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1a2b4c4c8de549741ae0f4f6f36c02fa,ntxv:0,isnm:False|UVIN-5519-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1210,x:32035,y:32194,ptovrint:False,ptlb:EmmissionValue,ptin:_EmmissionValue,varname:node_1210,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:3926,x:32243,y:32087,varname:node_3926,prsc:2|A-1432-RGB,B-1210-OUT;n:type:ShaderForge.SFN_Color,id:2062,x:32224,y:32686,ptovrint:False,ptlb:CustomColor,ptin:_CustomColor,varname:node_2062,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0.006896496,c4:1;n:type:ShaderForge.SFN_Multiply,id:7350,x:32401,y:32515,varname:node_7350,prsc:2|A-8411-OUT,B-2062-RGB;n:type:ShaderForge.SFN_Color,id:6898,x:32243,y:32242,ptovrint:False,ptlb:EmmissionColor,ptin:_EmmissionColor,varname:node_6898,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5586207,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:5128,x:32447,y:32150,varname:node_5128,prsc:2|A-3926-OUT,B-6898-RGB;n:type:ShaderForge.SFN_ValueProperty,id:2286,x:32031,y:32664,ptovrint:False,ptlb:AlbedoValue,ptin:_AlbedoValue,varname:_EmmissionValue_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:8411,x:32224,y:32515,varname:node_8411,prsc:2|A-6822-RGB,B-2286-OUT;n:type:ShaderForge.SFN_TexCoord,id:2964,x:31550,y:32619,varname:node_2964,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:5325,x:31550,y:32470,varname:node_5325,prsc:2;n:type:ShaderForge.SFN_Add,id:5519,x:31818,y:32483,varname:node_5519,prsc:2|A-5325-TSL,B-2964-UVOUT;proporder:6822-1561-1432-1210-2062-6898-2286;pass:END;sub:END;*/

Shader "Shader Forge/Jon's Shaders/Quake" {
    Properties {
        _Albedo ("Albedo", 2D) = "white" {}
        _Opacity ("Opacity", 2D) = "white" {}
        _Emmission ("Emmission", 2D) = "white" {}
        _EmmissionValue ("EmmissionValue", Float ) = 0.5
        _CustomColor ("CustomColor", Color) = (0,1,0.006896496,1)
        _EmmissionColor ("EmmissionColor", Color) = (1,0.5586207,0,1)
        _AlbedoValue ("AlbedoValue", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Opacity; uniform float4 _Opacity_ST;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Emmission; uniform float4 _Emmission_ST;
            uniform float _EmmissionValue;
            uniform float4 _CustomColor;
            uniform float4 _EmmissionColor;
            uniform float _AlbedoValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _Opacity_var = tex2D(_Opacity,TRANSFORM_TEX(i.uv0, _Opacity));
                clip(_Opacity_var.g - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_5325 = _Time;
                float2 node_5519 = (node_5325.r+i.uv0);
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(node_5519, _Albedo));
                float3 node_7350 = ((_Albedo_var.rgb*_AlbedoValue)*_CustomColor.rgb);
                indirectDiffuse += node_7350; // Diffuse Ambient Light
                float4 _Emmission_var = tex2D(_Emmission,TRANSFORM_TEX(node_5519, _Emmission));
                float3 diffuseColor = ((_Emmission_var.rgb*_EmmissionValue)*_EmmissionColor.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = node_7350;
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
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Opacity; uniform float4 _Opacity_ST;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Emmission; uniform float4 _Emmission_ST;
            uniform float _EmmissionValue;
            uniform float4 _CustomColor;
            uniform float4 _EmmissionColor;
            uniform float _AlbedoValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _Opacity_var = tex2D(_Opacity,TRANSFORM_TEX(i.uv0, _Opacity));
                clip(_Opacity_var.g - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_5325 = _Time;
                float2 node_5519 = (node_5325.r+i.uv0);
                float4 _Emmission_var = tex2D(_Emmission,TRANSFORM_TEX(node_5519, _Emmission));
                float3 diffuseColor = ((_Emmission_var.rgb*_EmmissionValue)*_EmmissionColor.rgb);
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
