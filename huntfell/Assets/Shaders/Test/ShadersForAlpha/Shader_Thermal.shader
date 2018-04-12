// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|diff-5408-OUT,emission-3929-OUT;n:type:ShaderForge.SFN_LightVector,id:1631,x:31620,y:32671,varname:node_1631,prsc:2;n:type:ShaderForge.SFN_LightVector,id:4545,x:31620,y:32935,varname:node_4545,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7954,x:31620,y:33056,prsc:2,pt:False;n:type:ShaderForge.SFN_NormalVector,id:4048,x:31620,y:32795,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:2750,x:31816,y:32715,varname:node_2750,prsc:2,dt:0|A-1631-OUT,B-4048-OUT;n:type:ShaderForge.SFN_Dot,id:3299,x:31815,y:32984,varname:node_3299,prsc:2,dt:0|A-4545-OUT,B-7954-OUT;n:type:ShaderForge.SFN_Vector1,id:1150,x:31815,y:32889,varname:node_1150,prsc:2,v1:1;n:type:ShaderForge.SFN_Append,id:5612,x:32022,y:32715,varname:node_5612,prsc:2|A-2750-OUT,B-1150-OUT;n:type:ShaderForge.SFN_Append,id:604,x:32025,y:32984,varname:node_604,prsc:2|A-3299-OUT,B-1150-OUT;n:type:ShaderForge.SFN_Tex2d,id:9706,x:32204,y:32625,ptovrint:False,ptlb:Cell1,ptin:_Cell1,varname:node_9706,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4777b72b75245ef409ab20a0bfe6b040,ntxv:0,isnm:False|UVIN-5612-OUT;n:type:ShaderForge.SFN_Tex2d,id:6945,x:32209,y:33028,ptovrint:False,ptlb:Cell2,ptin:_Cell2,varname:node_6945,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-604-OUT;n:type:ShaderForge.SFN_Tex2d,id:2822,x:32209,y:32823,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_2822,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5408,x:32507,y:32712,varname:node_5408,prsc:2|A-2822-RGB,B-9706-RGB;n:type:ShaderForge.SFN_Multiply,id:3929,x:32505,y:33013,varname:node_3929,prsc:2|A-2822-RGB,B-8852-RGB,C-3232-OUT;n:type:ShaderForge.SFN_Clamp01,id:3232,x:32327,y:33261,varname:node_3232,prsc:2|IN-2564-OUT;n:type:ShaderForge.SFN_RemapRange,id:2564,x:32174,y:33261,varname:node_2564,prsc:2,frmn:0,frmx:1,tomn:-10,tomx:10|IN-9430-OUT;n:type:ShaderForge.SFN_Add,id:9430,x:32016,y:33261,varname:node_9430,prsc:2|A-1532-OUT,B-3175-R;n:type:ShaderForge.SFN_Tex2d,id:3175,x:31839,y:33463,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_4157,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e958c6041cfe445e987c73751e8d4082,ntxv:0,isnm:False;n:type:ShaderForge.SFN_RemapRange,id:1532,x:31852,y:33261,varname:node_1532,prsc:2,frmn:0,frmx:1,tomn:-0.55,tomx:0.55|IN-1257-OUT;n:type:ShaderForge.SFN_OneMinus,id:1257,x:31671,y:33261,varname:node_1257,prsc:2|IN-7424-OUT;n:type:ShaderForge.SFN_Slider,id:7424,x:31340,y:33261,ptovrint:False,ptlb:Dissolve amount,ptin:_Dissolveamount,varname:node_8021,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4877885,max:1;n:type:ShaderForge.SFN_Color,id:8852,x:32204,y:32445,ptovrint:False,ptlb:node_8852,ptin:_node_8852,varname:node_8852,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5220588,c3:0.5220588,c4:1;proporder:9706-2822-6945-7424-3175-8852;pass:END;sub:END;*/

Shader "Shader Forge/thermal" {
    Properties {
        _Cell1 ("Cell1", 2D) = "white" {}
        _Texture ("Texture", 2D) = "white" {}
        _Cell2 ("Cell2", 2D) = "white" {}
        _Dissolveamount ("Dissolve amount", Range(0, 1)) = 0.4877885
        _noise ("noise", 2D) = "white" {}
        _node_8852 ("node_8852", Color) = (1,0.5220588,0.5220588,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
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
            uniform sampler2D _Cell1; uniform float4 _Cell1_ST;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _Dissolveamount;
            uniform float4 _node_8852;
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
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float node_1150 = 1.0;
                float2 node_5612 = float2(dot(lightDirection,i.normalDir),node_1150);
                float4 _Cell1_var = tex2D(_Cell1,TRANSFORM_TEX(node_5612, _Cell1));
                float3 diffuseColor = (_Texture_var.rgb*_Cell1_var.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float3 emissive = (_Texture_var.rgb*_node_8852.rgb*saturate(((((1.0 - _Dissolveamount)*1.1+-0.55)+_noise_var.r)*20.0+-10.0)));
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
            uniform sampler2D _Cell1; uniform float4 _Cell1_ST;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _Dissolveamount;
            uniform float4 _node_8852;
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
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float node_1150 = 1.0;
                float2 node_5612 = float2(dot(lightDirection,i.normalDir),node_1150);
                float4 _Cell1_var = tex2D(_Cell1,TRANSFORM_TEX(node_5612, _Cell1));
                float3 diffuseColor = (_Texture_var.rgb*_Cell1_var.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
