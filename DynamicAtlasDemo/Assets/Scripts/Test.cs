using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class Test : MonoBehaviour
{
    public Texture2D sourceTexture1, sourceTexture2;
    private Texture2D destinationTexture;

    void Start()
    {
        Debug.LogError("AAAAAAAAAA");
        destinationTexture = new Texture2D(1024, 1024);
        Graphics.CopyTexture(sourceTexture1, 0, 0, 0, 0, 275, 607, destinationTexture, 0, 0, 0, 0);
        Graphics.CopyTexture(sourceTexture2, 0, 0, 0, 0, 275, 607, destinationTexture, 0, 0, 275, 0);

        byte[] datas = destinationTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/拷贝图片.png", datas);
    }
}
