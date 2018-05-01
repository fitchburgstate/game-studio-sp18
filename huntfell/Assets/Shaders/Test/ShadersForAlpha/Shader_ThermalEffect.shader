// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|diff-2894-OUT,emission-4592-OUT,clip-3952-OUT,olwid-1592-OUT,olcol-6994-RGB;n:type:ShaderForge.SFN_Tex2d,id:6907,x:32288,y:32721,varname:node_6907,prsc:2,tex:271f5ee3273dd7f4fae6e204d4f8c4bf,ntxv:0,isnm:False|UVIN-7770-OUT,TEX-7043-TEX;n:type:ShaderForge.SFN_Append,id:4255,x:31712,y:32581,varname:node_4255,prsc:2|A-3952-OUT,B-3267-G;n:type:ShaderForge.SFN_Tex2d,id:4157,x:31687,y:33251,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_4157,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e958c6041cfe445e987c73751e8d4082,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:8021,x:31188,y:33049,ptovrint:False,ptlb:Dissolve amount,ptin:_Dissolveamount,varname:node_8021,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5016065,max:1;n:type:ShaderForge.SFN_Add,id:709,x:31974,y:33052,varname:node_709,prsc:2|A-3013-OUT,B-4157-R;n:type:ShaderForge.SFN_RemapRange,id:3013,x:31700,y:33049,varname:node_3013,prsc:2,frmn:0,frmx:1,tomn:-0.55,tomx:0.55|IN-4224-OUT;n:type:ShaderForge.SFN_OneMinus,id:4224,x:31519,y:33049,varname:node_4224,prsc:2|IN-8021-OUT;n:type:ShaderForge.SFN_RemapRange,id:3731,x:32162,y:33052,varname:node_3731,prsc:2,frmn:0,frmx:1,tomn:-4,tomx:4|IN-709-OUT;n:type:ShaderForge.SFN_Clamp01,id:3952,x:32343,y:33052,varname:node_3952,prsc:2|IN-3731-OUT;n:type:ShaderForge.SFN_Color,id:3267,x:31393,y:32578,ptovrint:False,ptlb:Yellow,ptin:_Yellow,varname:node_3267,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8014706,c2:0.7325409,c3:0.176795,c4:1;n:type:ShaderForge.SFN_Color,id:480,x:31393,y:32762,ptovrint:False,ptlb:Orange,ptin:_Orange,varname:node_480,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.3103448,c3:0,c4:1;n:type:ShaderForge.SFN_Append,id:6560,x:31712,y:32752,varname:node_6560,prsc:2|A-3952-OUT,B-480-G;n:type:ShaderForge.SFN_Tex2d,id:6861,x:32196,y:32390,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:node_6861,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4086-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:7043,x:31846,y:32877,ptovrint:False,ptlb:Ramp,ptin:_Ramp,varname:node_7043,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:271f5ee3273dd7f4fae6e204d4f8c4bf,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3504,x:31904,y:32647,varname:node_3504,prsc:2|A-4255-OUT,B-6560-OUT;n:type:ShaderForge.SFN_Vector1,id:1592,x:32260,y:33226,varname:node_1592,prsc:2,v1:0.015;n:type:ShaderForge.SFN_Color,id:6994,x:32260,y:33318,ptovrint:False,ptlb:OutLineColor,ptin:_OutLineColor,varname:node_6994,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.7858012,c3:0.5147059,c4:1;n:type:ShaderForge.SFN_Color,id:9333,x:32196,y:32234,ptovrint:False,ptlb:GrayScale,ptin:_GrayScale,varname:node_9333,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5808823,c2:0.5808823,c3:0.5808823,c4:1;n:type:ShaderForge.SFN_Add,id:2894,x:32450,y:32390,varname:node_2894,prsc:2|A-6861-RGB,B-9333-RGB;n:type:ShaderForge.SFN_TexCoord,id:3379,x:31619,y:32332,varname:node_3379,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:6983,x:31619,y:32214,varname:node_6983,prsc:2;n:type:ShaderForge.SFN_Add,id:4086,x:31904,y:32430,varname:node_4086,prsc:2|A-6983-T,B-3379-UVOUT;n:type:ShaderForge.SFN_Multiply,id:7770,x:32124,y:32585,varname:node_7770,prsc:2|A-4086-OUT,B-3504-OUT;n:type:ShaderForge.SFN_Multiply,id:4592,x:32489,y:32764,varname:node_4592,prsc:2|A-6907-RGB,B-2602-RGB;n:type:ShaderForge.SFN_Color,id:2602,x:32288,y:32875,ptovrint:False,ptlb:AdjustColor,ptin:_AdjustColor,varname:node_2602,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.6,c3:0,c4:1;proporder:4157-8021-3267-480-6861-7043-6994-9333-2602;pass:END;sub:END;*/

Shader "Shader Forge/ThermalDissolve" {
    Properties {
        _noise ("noise", 2D) = "white" {}
        _Dissolveamount ("Dissolve amount", Range(0, 1)) = 0.5016065
        _Yellow ("Yellow", Color) = (0.8014706,0.7325409,0.176795,1)
        _Orange ("Orange", Color) = (1,0.3103448,0,1)
        _Albedo ("Albedo", 2D) = "white" {}
        _Ramp ("Ramp", 2D) = "white" {}
        _OutLineColor ("OutLineColor", Color) = (1,0.7858012,0.5147059,1)
        _GrayScale ("GrayScale", Color) = (0.5808823,0.5808823,0.5808823,1)
        _AdjustColor ("AdjustColor", Color) = (1,0.6,0,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
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
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _Dissolveamount;
            uniform float4 _OutLineColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*0.015,1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_3952 = saturate(((((1.0 - _Dissolveamount)*1.1+-0.55)+_noise_var.r)*8.0+-4.0));
                clip(node_3952 - 0.5);
                return fixed4(_OutLineColor.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _Dissolveamount;
            uniform float4 _Yellow;
            uniform float4 _Orange;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform float4 _GrayScale;
            uniform float4 _AdjustColor;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_3952 = saturate(((((1.0 - _Dissolveamount)*1.1+-0.55)+_noise_var.r)*8.0+-4.0));
                clip(node_3952 - 0.5);
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
                float4 node_6983 = _Time;
                float2 node_4086 = (node_6983.g+i.uv0);
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(node_4086, _Albedo));
                float3 diffuseColor = (_Albedo_var.rgb+_GrayScale.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float2 node_7770 = (node_4086*(float2(node_3952,_Yellow.g)*float2(node_3952,_Orange.g)));
                float4 node_6907 = tex2D(_Ramp,TRANSFORM_TEX(node_7770, _Ramp));
                float3 emissive = (node_6907.rgb*_AdjustColor.rgb);
/// Final Color:
                float3 finalColor = diffuse + emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _Dissolveamount;
            uniform float4 _Yellow;
            uniform float4 _Orange;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform float4 _GrayScale;
            uniform float4 _AdjustColor;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_3952 = saturate(((((1.0 - _Dissolveamount)*1.1+-0.55)+_noise_var.r)*8.0+-4.0));
                clip(node_3952 - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_6983 = _Time;
                float2 node_4086 = (node_6983.g+i.uv0);
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(node_4086, _Albedo));
                float3 diffuseColor = (_Albedo_var.rgb+_GrayScale.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _Dissolveamount;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_3952 = saturate(((((1.0 - _Dissolveamount)*1.1+-0.55)+_noise_var.r)*8.0+-4.0));
                clip(node_3952 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
