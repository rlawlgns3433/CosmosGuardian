using UnityEngine;
using UnityEngine.UI;

public class UiOption : MonoBehaviour
{
    public Lobby lobby;
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
        ParamManager.saveData = SaveLoadSystem.Load() as SaveDataV1;

        buttonState = ParamManager.IsCameraShaking ? buttonOn : buttonOff;
        buttonImg.sprite = buttonSprites[buttonState];

        if (ParamManager.saveData.playerOption.bgmValue != -1)
        {
            bgmSlider.value = ParamManager.saveData.playerOption.bgmValue;
            ParamManager.BgmValue = bgmSlider.value;
        }

        if (ParamManager.saveData.playerOption.sfxValue != -1)
        {
            sfxSlider.value = ParamManager.saveData.playerOption.sfxValue;
            ParamManager.SfxValue = sfxSlider.value;
        }
    }

    private void OnDisable()
    {
        SaveLoadSystem.Save(ParamManager.saveData);
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
            lobby.audioSource.volume = ParamManager.BgmValue;
        });

        sfxSlider.onValueChanged.AddListener((float value) =>
        {
            ParamManager.SfxValue = value;
        });
    }

    public void CameraShakeToggle()
    {
        if (buttonState == buttonOn)
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