// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32765,y:32474,varname:node_4795,prsc:2|emission-2393-OUT,clip-2191-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32235,y:32601,ptovrint:False,ptlb:Gradient,ptin:_Gradient,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a65fdabb80940b647ba5e69ef5538613,ntxv:0,isnm:False|UVIN-3048-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32495,y:32793,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-7364-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32235,y:32772,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32235,y:32930,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Append,id:8955,x:30464,y:32786,varname:node_8955,prsc:2|A-7214-OUT,B-4155-OUT;n:type:ShaderForge.SFN_Time,id:9975,x:30464,y:32949,varname:node_9975,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7214,x:30277,y:32786,ptovrint:False,ptlb:u speed,ptin:_uspeed,varname:node_7214,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:4155,x:30277,y:32895,ptovrint:False,ptlb:V speed,ptin:_Vspeed,varname:node_4155,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:6694,x:30631,y:32786,varname:node_6694,prsc:2|A-8955-OUT,B-9975-T;n:type:ShaderForge.SFN_TexCoord,id:1661,x:30631,y:32939,varname:node_1661,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:5841,x:30812,y:32786,varname:node_5841,prsc:2|A-6694-OUT,B-1661-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:7799,x:30978,y:32786,varname:node_7799,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-5841-OUT,TEX-3828-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:3828,x:30812,y:32950,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_3828,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:9430,x:30465,y:33103,varname:node_9430,prsc:2|A-2500-OUT,B-8284-OUT;n:type:ShaderForge.SFN_Time,id:3274,x:30465,y:33265,varname:node_3274,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6445,x:30631,y:33103,varname:node_6445,prsc:2|A-9430-OUT,B-3274-T;n:type:ShaderForge.SFN_TexCoord,id:9503,x:30631,y:33255,varname:node_9503,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:2500,x:30277,y:33103,ptovrint:False,ptlb:u speed2,ptin:_uspeed2,varname:_uspeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.2;n:type:ShaderForge.SFN_ValueProperty,id:8284,x:30277,y:33183,ptovrint:False,ptlb:V speed2,ptin:_Vspeed2,varname:_Vspeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.2;n:type:ShaderForge.SFN_Add,id:2946,x:30812,y:33103,varname:node_2946,prsc:2|A-6445-OUT,B-9503-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:5968,x:30995,y:33103,varname:node_5968,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-2946-OUT,TEX-3828-TEX;n:type:ShaderForge.SFN_Slider,id:450,x:30474,y:32641,ptovrint:False,ptlb:Dissolve,ptin:_Dissolve,varname:node_450,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2820513,max:1;n:type:ShaderForge.SFN_OneMinus,id:2017,x:30812,y:32601,varname:node_2017,prsc:2|IN-450-OUT;n:type:ShaderForge.SFN_RemapRange,id:5395,x:30978,y:32601,varname:node_5395,prsc:2,frmn:0,frmx:1,tomn:-0.65,tomx:0.65|IN-2017-OUT;n:type:ShaderForge.SFN_Add,id:3930,x:31138,y:32601,varname:node_3930,prsc:2|A-5395-OUT,B-7799-R;n:type:ShaderForge.SFN_Add,id:6835,x:31155,y:32786,varname:node_6835,prsc:2|A-5395-OUT,B-5968-R;n:type:ShaderForge.SFN_Multiply,id:4928,x:31339,y:32601,varname:node_4928,prsc:2|A-3930-OUT,B-6835-OUT;n:type:ShaderForge.SFN_RemapRange,id:2080,x:31540,y:32601,varname:node_2080,prsc:2,frmn:0,frmx:1,tomn:-10,tomx:10|IN-4928-OUT;n:type:ShaderForge.SFN_Clamp01,id:7942,x:31701,y:32601,varname:node_7942,prsc:2|IN-2080-OUT;n:type:ShaderForge.SFN_OneMinus,id:6054,x:31877,y:32601,varname:node_6054,prsc:2|IN-7942-OUT;n:type:ShaderForge.SFN_Append,id:3048,x:32061,y:32601,varname:node_3048,prsc:2|A-6054-OUT,B-7679-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7679,x:31877,y:32757,ptovrint:False,ptlb:Don't Touch,ptin:_DontTouch,varname:node_7679,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:7364,x:32235,y:33113,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_7364,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:5295,x:32235,y:32472,ptovrint:False,ptlb:Strench,ptin:_Strench,varname:node_5295,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.7;n:type:ShaderForge.SFN_Multiply,id:2191,x:32474,y:32485,varname:node_2191,prsc:2|A-5295-OUT,B-6074-R;proporder:6074-797-7214-4155-3828-450-2500-8284-7679-7364-5295;pass:END;sub:END;*/

Shader "Shader Forge/Jon's Shaders/Energy Current" {
    Properties {
        _Gradient ("Gradient", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _uspeed ("u speed", Float ) = 0.2
        _Vspeed ("V speed", Float ) = 0.1
        _Noise ("Noise", 2D) = "white" {}
        _Dissolve ("Dissolve", Range(0, 1)) = 0.2820513
        _uspeed2 ("u speed2", Float ) = -0.2
        _Vspeed2 ("V speed2", Float ) = -0.2
        _DontTouch ("Don't Touch", Float ) = 0
        _Opacity ("Opacity", Float ) = 2
        _Strench ("Strench", Float ) = 0.7
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
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Gradient; uniform float4 _Gradient_ST;
            uniform float4 _TintColor;
            uniform float _uspeed;
            uniform float _Vspeed;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _uspeed2;
            uniform float _Vspeed2;
            uniform float _Dissolve;
            uniform float _DontTouch;
            uniform float _Opacity;
            uniform float _Strench;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float node_5395 = ((1.0 - _Dissolve)*1.3+-0.65);
                float4 node_9975 = _Time;
                float2 node_5841 = ((float2(_uspeed,_Vspeed)*node_9975.g)+i.uv0);
                float4 node_7799 = tex2D(_Noise,TRANSFORM_TEX(node_5841, _Noise));
                float4 node_3274 = _Time;
                float2 node_2946 = ((float2(_uspeed2,_Vspeed2)*node_3274.g)+i.uv0);
                float4 node_5968 = tex2D(_Noise,TRANSFORM_TEX(node_2946, _Noise));
                float2 node_3048 = float2((1.0 - saturate((((node_5395+node_7799.r)*(node_5395+node_5968.r))*20.0+-10.0))),_DontTouch);
                float4 _Gradient_var = tex2D(_Gradient,TRANSFORM_TEX(node_3048, _Gradient));
                clip((_Strench*_Gradient_var.r) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (_Gradient_var.rgb*i.vertexColor.rgb*_TintColor.rgb*_Opacity);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
