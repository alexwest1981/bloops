using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;



public class SettingsManager : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject resetProgressPromt;
    [SerializeField] private Slider pushMagnitudeSlider;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private ButtonSound buttonSound;

    [Header(" Actions ")]
    public static Action<float> onPushMagnitudeChanged;
    public static Action<bool> onSFXValueChanged;
    public static Action<bool> onMusicValueChanged;
    public Toggle cameraShakeToggle;

    [Header(" Data ")]
    private const string lastPushMagnitudeKey = "lastPushMagnitude";
    private const string sfxActiveKey = "sfxActiveKey";
    private const string musicActiveKey = "musicActiveKey";
    private const string cameraShakeActiveKey = "cameraShakeActiveKey";
    private bool canSave;


    private void Awake()
    {
        LoadData();
        cameraShakeToggle.onValueChanged.AddListener(ToggleCameraShake);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        /*SliderValueChangedCallback();
        ToggleCallback(sfxToggle.isOn);*/

        Initialize();

        yield return new WaitForSeconds(1f);

        canSave = true;

    }


    private void Initialize()
    {
        onPushMagnitudeChanged?.Invoke(pushMagnitudeSlider.value);
        onSFXValueChanged?.Invoke(sfxToggle.isOn);
        onMusicValueChanged?.Invoke(musicToggle.isOn);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetProgressButtonCallback()
    {
        resetProgressPromt.SetActive(true);
    }

    public void ResetProgressYes()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }   

    public void ResetProgressNo()
    {
        resetProgressPromt.SetActive(false);
    }

    public void SliderValueChangedCallback()
    {
        onPushMagnitudeChanged?.Invoke(pushMagnitudeSlider.value);
        SaveData();
    }

    public void ToggleCameraShake(bool isCameraShakeEnabled)
    {
        CameraShake cameraShake = UnityEngine.Object.FindFirstObjectByType<CameraShake>();
        if (cameraShake != null)
        {
            cameraShake.ToggleShake(isCameraShakeEnabled);

            // Spara kameraskakningens aktiveringsstatus i PlayerPrefs
            PlayerPrefs.SetInt(cameraShakeActiveKey, isCameraShakeEnabled ? 1 : 0);
        }
    }

    public void ToggleSFX(bool isSFXEnabled)
    {
        onSFXValueChanged?.Invoke(isSFXEnabled);
        SaveData();

        // Stäng av knappklicksljudet baserat på toggle-status
        buttonSound.ToggleButtonClickSound(isSFXEnabled);
    }

    public void ToggleCallback(bool sfxActive)
    {
        onSFXValueChanged?.Invoke(sfxActive);
        SaveData();
    }

    public void ToggleMusicCallback(bool musicActive)
    {
        onMusicValueChanged?.Invoke(musicActive);
        SaveData();
    }

    private void LoadData()
    {
        pushMagnitudeSlider.value = PlayerPrefs.GetFloat(lastPushMagnitudeKey);
        sfxToggle.isOn = PlayerPrefs.GetInt(sfxActiveKey) == 1;
        musicToggle.isOn = PlayerPrefs.GetInt(musicActiveKey) == 1;
        cameraShakeToggle.isOn = PlayerPrefs.GetInt(cameraShakeActiveKey, 1) == 1;
    }

    private void SaveData()
    {
        if (!canSave)
            return;

        PlayerPrefs.SetFloat(lastPushMagnitudeKey, pushMagnitudeSlider.value);

        int musicValue = musicToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt(musicActiveKey, musicValue);

        int sfxValue = sfxToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt(sfxActiveKey, sfxValue);

        // Använd rätt toggle-variabel här
        int isCameraShakeEnabled = cameraShakeToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt(cameraShakeActiveKey, isCameraShakeEnabled);
    }

}
