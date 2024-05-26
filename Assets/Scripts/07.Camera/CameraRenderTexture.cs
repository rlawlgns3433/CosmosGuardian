using UnityEngine;

public class CameraRenderTexture : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Camera renderCamera;
    public UnityEngine.UI.RawImage rawImage;

    public LayerMask renderLayerMask;

    void Start()
    {
        // Render Texture 설정
        if (renderTexture == null)
        {
            Debug.LogError("Render Texture가 설정되지 않았습니다.");
            return;
        }

        // 카메라 설정
        if (renderCamera == null)
        {
            Debug.LogError("Render 카메라가 설정되지 않았습니다.");
            return;
        }

        renderCamera.targetTexture = renderTexture;
        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        renderCamera.backgroundColor = Color.clear;
        renderCamera.cullingMask = renderLayerMask;

        //// Raw Image 설정
        //if (rawImage == null)
        //{
        //    Debug.LogError("Raw Image가 설정되지 않았습니다.");
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
