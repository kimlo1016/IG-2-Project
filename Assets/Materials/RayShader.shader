// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RayShader"
{
    Properties
    {
        _BeginColor("Begin Color", Color) = (1,1,1,1)
        _EndColor("End Color", Color) = (1,1,1,1)
    }

        SubShader
    {
        Tags
        {
            "Queue" = "Background"
            "IgnoreProjector" = "True"
        }

        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // ���ĺ����� ���ؼ� �ʿ���.

        ZWrite Off

        Pass
        {
            CGPROGRAM
        
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #pragma target 3.0

            fixed4 _BeginColor;
            fixed4 _EndColor;

            struct v2f // vertex to fragment
            {
                float4 pos : SV_POSITION;  // 3D ������Ʈ�� ���� ��ǥ
                float4 col : COLOR;  // 3D ������Ʈ�� ����
            };

            // ������ ���������� ����
            // 1. ���ؽ� �ε� : fbx ���� ���Ϸκ��� 3D ������Ʈ�� ���ؽ��� GPU�� �ε���.
            // 2. ���ؽ� ���̴� : ���ؽ��� ������ ������. ���� ���� ��ȯ, �� ��ȯ, �ø� ����� �Ͼ.
            // 3. �����Ͷ����� : ���� ��ȯ �� ���ؽ��� ������ �������� ������� �ȼ��� ������Ŵ
            // 4. �ȼ� ���̴�(�����׸�Ʈ ���̴�) : ���� ������ ����. ���� ���� ���� ���� ��.

            // ���ؽ� ���̴�
            v2f vert(appdata_full v)
            {
                // ������� ���� ���� ����
                v2f o;

                // ���� ��ȯ
                o.pos = UnityObjectToClipPos(v.vertex);

                // �÷� ���ϱ�
                _EndColor.a = 0;

                // ���� �������� ���� ������ ����.
                // ������ ���� �����δ� �ؽ��� ��ǥ�� x���� �̿�(0.0 ~ 1.0)
                // ���� ���� �������� ��.
                o.col = lerp(_BeginColor, _EndColor, v.texcoord.x);

                return o;
            }


            // �����׸�Ʈ ���̴�
            float4 frag(v2f i) : COLOR
            {
                // ���ؽ� ���̴����� �̹� ������ ���߱� ������ �ٷ� ����ϸ� ��
                return i.col;
            }

            ENDCG
        }
    }
}
