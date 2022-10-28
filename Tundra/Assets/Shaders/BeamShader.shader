// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2595,x:32752,y:32685,varname:node_2595,prsc:2|emission-585-OUT,alpha-7577-OUT;n:type:ShaderForge.SFN_Multiply,id:5726,x:32116,y:32674,varname:node_5726,prsc:2|A-8558-OUT,B-4975-OUT;n:type:ShaderForge.SFN_Fresnel,id:8558,x:31930,y:32622,varname:node_8558,prsc:2|EXP-8317-OUT;n:type:ShaderForge.SFN_Vector1,id:8317,x:31772,y:32640,varname:node_8317,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:5431,x:32341,y:32785,varname:node_5431,prsc:2|A-5726-OUT,B-6863-OUT;n:type:ShaderForge.SFN_Vector1,id:6863,x:32100,y:32877,varname:node_6863,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:7577,x:32416,y:33061,varname:node_7577,prsc:2,v1:0.5;n:type:ShaderForge.SFN_OneMinus,id:585,x:32555,y:32785,varname:node_585,prsc:2|IN-5431-OUT;n:type:ShaderForge.SFN_OneMinus,id:4975,x:31930,y:32768,varname:node_4975,prsc:2|IN-4348-RGB;n:type:ShaderForge.SFN_Color,id:4348,x:31721,y:32768,ptovrint:False,ptlb:BorderColor,ptin:_BorderColor,varname:node_4348,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9,c2:0.7,c3:0,c4:1;proporder:4348;pass:END;sub:END;*/

Shader "Custom/BeamShader" {
    Properties {
        [HDR] _BorderColor ("BorderColor", Color) = (0.9,0.7,0,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 3.0
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _BorderColor)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _BorderColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _BorderColor );
                float3 emissive = (1.0 - ((pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0)*(1.0 - _BorderColor_var.rgb))*2.0));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,0.5);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
