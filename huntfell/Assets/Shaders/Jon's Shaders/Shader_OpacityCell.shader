// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:32795,y:32720,varname:node_4013,prsc:2|diff-4243-OUT,spec-5638-RGB,normal-4297-RGB,emission-9461-OUT,alpha-1584-R;n:type:ShaderForge.SFN_LightVector,id:1079,x:31486,y:32911,varname:node_1079,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7642,x:31486,y:33031,prsc:2,pt:False;n:type:ShaderForge.SFN_ValueProperty,id:6112,x:32541,y:33242,ptovrint:False,ptlb:OutlineWidth,ptin:_OutlineWidth,varname:_OutlineWidth,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Color,id:8693,x:32541,y:33314,ptovrint:False,ptlb:OutLinecolor,ptin:_OutLinecolor,varname:_OutLinecolor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:5145,x:32130,y:32951,ptovrint:False,ptlb:Celltexture,ptin:_Celltexture,varname:_Celltexture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4e018ce494cc29d4eb07e81e36609a40,ntxv:0,isnm:False|UVIN-4121-OUT;n:type:ShaderForge.SFN_Append,id:4121,x:31933,y:32951,varname:node_4121,prsc:2|A-3023-OUT,B-3609-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7046,x:31679,y:32967,ptovrint:False,ptlb:constant,ptin:_constant,varname:_constant,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.4;n:type:ShaderForge.SFN_ValueProperty,id:3609,x:31679,y:33041,ptovrint:False,ptlb:AppendConstant,ptin:_AppendConstant,varname:_AppendConstant,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Dot,id:3023,x:31679,y:32803,varname:node_3023,prsc:2,dt:0|A-1079-OUT,B-7642-OUT;n:type:ShaderForge.SFN_Tex2d,id:6873,x:32130,y:32768,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:_Albedo,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bb757f3dcf8f5d04a80d39f577c92532,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9461,x:32429,y:32828,varname:node_9461,prsc:2|A-6873-RGB,B-5145-RGB;n:type:ShaderForge.SFN_Dot,id:9918,x:31677,y:32500,varname:node_9918,prsc:2,dt:0|A-2270-OUT,B-970-OUT;n:type:ShaderForge.SFN_NormalVector,id:970,x:31486,y:32623,prsc:2,pt:False;n:type:ShaderForge.SFN_Append,id:614,x:31934,y:32612,varname:node_614,prsc:2|A-9918-OUT,B-3609-OUT;n:type:ShaderForge.SFN_Tex2d,id:1670,x:32130,y:32595,ptovrint:False,ptlb:Celltexture_copy,ptin:_Celltexture_copy,varname:_Celltexture_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4e018ce494cc29d4eb07e81e36609a40,ntxv:0,isnm:False|UVIN-614-OUT;n:type:ShaderForge.SFN_Multiply,id:4243,x:32429,y:32706,varname:node_4243,prsc:2|A-1670-RGB,B-6873-RGB;n:type:ShaderForge.SFN_LightVector,id:2270,x:31486,y:32500,varname:node_2270,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:5638,x:33543,y:33000,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:_Specular,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0e44bd21b1e18c344b46119f70d6e48a,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4297,x:33543,y:32835,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:_Normal,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:250169623080bd44bbf30f270238be51,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:1584,x:32409,y:33017,ptovrint:False,ptlb:OpacityMap,ptin:_OpacityMap,varname:node_1584,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;proporder:4297-5638-3609-6873-1670-5145-1584;pass:END;sub:END;*/

Shader "Shader Forge/Jon's Shaders/Opacity Cell" {
    Properties {
        _Normal ("Normal", 2D) = "bump" {}
        _Specular ("Specular", 2D) = "bump" {}
        _AppendConstant ("AppendConstant", Float ) = 1
        _Albedo ("Albedo", 2D) = "white" {}
        _Celltexture_copy ("Celltexture_copy", 2D) = "white" {}
        _Celltexture ("Celltexture", 2D) = "white" {}
        _OpacityMap ("OpacityMap", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
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
            uniform sampler2D _Celltexture; uniform float4 _Celltexture_ST;
            uniform float _AppendConstant;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Celltexture_copy; uniform float4 _Celltexture_copy_ST;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _OpacityMap; uniform float4 _OpacityMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(i.uv0, _Specular));
                float3 specularColor = _Specular_var.rgb;
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float2 node_614 = float2(dot(lightDirection,i.normalDir),_AppendConstant);
                float4 _Celltexture_copy_var = tex2D(_Celltexture_copy,TRANSFORM_TEX(node_614, _Celltexture_copy));
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 diffuseColor = (_Celltexture_copy_var.rgb*_Albedo_var.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float2 node_4121 = float2(dot(lightDirection,i.normalDir),_AppendConstant);
                float4 _Celltexture_var = tex2D(_Celltexture,TRANSFORM_TEX(node_4121, _Celltexture));
                float3 emissive = (_Albedo_var.rgb*_Celltexture_var.rgb);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                float4 _OpacityMap_var = tex2D(_OpacityMap,TRANSFORM_TEX(i.uv0, _OpacityMap));
                fixed4 finalRGBA = fixed4(finalColor,_OpacityMap_var.r);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
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
            uniform sampler2D _Celltexture; uniform float4 _Celltexture_ST;
            uniform float _AppendConstant;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform sampler2D _Celltexture_copy; uniform float4 _Celltexture_copy_ST;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _OpacityMap; uniform float4 _OpacityMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(i.uv0, _Specular));
                float3 specularColor = _Specular_var.rgb;
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float2 node_614 = float2(dot(lightDirection,i.normalDir),_AppendConstant);
                float4 _Celltexture_copy_var = tex2D(_Celltexture_copy,TRANSFORM_TEX(node_614, _Celltexture_copy));
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 diffuseColor = (_Celltexture_copy_var.rgb*_Albedo_var.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                float4 _OpacityMap_var = tex2D(_OpacityMap,TRANSFORM_TEX(i.uv0, _OpacityMap));
                fixed4 finalRGBA = fixed4(finalColor * _OpacityMap_var.r,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
