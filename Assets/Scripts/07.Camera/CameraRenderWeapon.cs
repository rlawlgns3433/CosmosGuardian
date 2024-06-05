using UnityEngine;

public class CameraRenderWeapon : MonoBehaviour
{
    public CameraRenderTexture cameraRenderTexture;
    public GameObject weaponParent;
    public GameObject[] weapons;
    public int selectedWeaponIndex = 0;
    public int currentWeaponIndex = 0;
    private float rotationSpeed = 360;
    private Vector3 rot = new Vector3(0, -1, 0);

    private void Start()
    {
        currentWeaponIndex = ParamManager.selectedWeaponIndex;   
    }

    void Update()
    {
        if (cameraRenderTexture.renderCamera != null && cameraRenderTexture.renderTexture != null && cameraRenderTexture.rawImage != null)
        {
            gameObject.transform.Rotate(rot * rotationSpeed * Time.deltaTime);

            cameraRenderTexture.renderCamera.Render();
        }
    }

    public void SetWeapon(int selectedWeaponIndex)
    {
        for(int i = 0; i < weapons.Length; ++i)
        {
            weapons[i].SetActive(false);
        }
        weapons[selectedWeaponIndex].SetActive(true);
    }
}