using UnityEngine;
using UnityEngine.UI;

public class UiOption : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button buttonCameraShake;
    public Image buttonImg;
    public Sprite[] buttonSprites;

    private int buttonOn = 0;
    private int buttonOff = 1;
    private int buttonState = -1;

    private void OnEnable()
    {
        buttonState = ParamManager.IsCameraShaking ? buttonOn : buttonOff;
        buttonImg.sprite = buttonSprites[buttonState];
    }

    private void Start()
    {
        if (buttonState == -1)
        {
            buttonState = buttonOn;
            buttonImg.sprite = buttonSprites[buttonState];
        }

        bgmSlider.onValueChanged.AddListener((float value) =>
        {
            ParamManager.BgmValue = value;
        });

        sfxSlider.onValueChanged.AddListener((float value) =>
        {
            ParamManager.SfxValue = value;
        });
    }

    public void CameraShakeToggle()
    {
        if(buttonState == buttonOn)
        {
            buttonState = buttonOff;
            ParamManager.IsCameraShaking = false;
        }
        else
        {
            buttonState = buttonOn;
            ParamManager.IsCameraShaking = true;
        }
        buttonImg.sprite = buttonSprites[buttonState];
    }

    
}
