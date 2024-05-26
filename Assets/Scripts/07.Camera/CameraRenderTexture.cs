using UnityEngine;

public class CameraRenderTexture : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Camera renderCamera;
    public UnityEngine.UI.RawImage rawImage;

    public LayerMask renderLayerMask;

    void Start()
    {
        // Render Texture ����
        if (renderTexture == null)
        {
            Debug.LogError("Render Texture�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ī�޶� ����
        if (renderCamera == null)
        {
            Debug.LogError("Render ī�޶� �������� �ʾҽ��ϴ�.");
            return;
        }

        renderCamera.targetTexture = renderTexture;
        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        renderCamera.backgroundColor = Color.clear;
        renderCamera.cullingMask = renderLayerMask;

        //// Raw Image ����
        //if (rawImage == null)
        //{
        //    Debug.LogError("Raw Image�� �������� �ʾҽ��ϴ�.");
        //    return;
        //}

        //rawImage.texture = renderTexture;
    }

    //private void Update()
    //{
    //    if (renderCamera != null && renderTexture != null && rawImage != null)
    //    {
    //        renderCamera.Render();
    //    }
    //}
}
