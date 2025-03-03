using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicTMP : MonoBehaviour
{
    private TMP_Text tmp;
    public float offsetX;
    public float amplitudeA;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMP_Text>();
        tmp.text = "Sworder<color=#FF0000>T</color>";
        tmp.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the text mesh padding every frame after the text's attribute has been updated
        tmp.UpdateMeshPadding();

        var text = tmp.textInfo;
        for (int i = 0; i < text.characterCount; i++)
        {
            var charInfo = text.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }
            // Get the index of the vertex used by the current character
            var verts = text.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++)
            {
                // �ַ�����ʼ�������� + ��ǰ�����ƫ����
                var orig = verts[charInfo.vertexIndex + j];
                // Dynamic animation
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * offsetX) * amplitudeA, 0);
            }
        }
        // ���¶���д������
        for (int i = 0; i < text.meshInfo.Length; i++)
        {
            var meshInfo = text.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices; // ���޸ĺ��CPU������ ͬ����GPU��
            tmp.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
