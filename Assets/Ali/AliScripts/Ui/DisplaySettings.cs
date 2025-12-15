using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DisplaySettings : MonoBehaviour
{
    [Header("UI Elemanlarý")]
    public Slider brightnessSlider;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;

    [Header("URP Ayarlarý")]
    // Artýk bunu Inspector'dan atamak zorunda deðilsiniz, kod kendi bulacak.
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    private Resolution[] resolutions;

    void Awake() // Start yerine Awake daha güvenlidir
    {
        // 1. EÐER GLOBAL VOLUME ATANMAMIÞSA OTOMATÝK BUL
        if (globalVolume == null)
        {
            // Sahnedeki ilk Volume bileþenini bulur
            globalVolume = FindObjectOfType<Volume>();
        }

        // 2. PROFÝLÝ VE EFEKTÝ KONTROL ET
        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
        {
            // Baþarýlý, efekt bulundu.
        }
        else
        {
            // Eðer yeni sahnede Color Adjustments yoksa hata vermesin ama uyarsýn
            Debug.LogWarning("Bu sahnede Global Volume veya Color Adjustments efekti bulunamadý!");
        }

        // Diðer baþlangýç iþlemleri...
        LoadAvailableResolutions();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        vsyncToggle.onValueChanged.AddListener(SetVSync);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);

        // 3. AYARLARI HEMEN YÜKLE VE UYGULA
        LoadSettings();
    }

    // --- (Diðer fonksiyonlar aynen kalacak) ---
    // A. Çözünürlükleri Hazýrlama
    void LoadAvailableResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    public void SetVSync(bool isVSync) => QualitySettings.vSyncCount = isVSync ? 1 : 0;

    public void SetBrightness(float value)
    {
        // colorAdjustments her sahnede yeniden bulunduðu için null olmaz
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("BrightnessValue", brightnessSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutionDropdown.value);
        int fullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0);
        int vsync = PlayerPrefs.GetInt("VSync", QualitySettings.vSyncCount);
        float brightness = PlayerPrefs.GetFloat("BrightnessValue", 0f);

        resolutionDropdown.value = resIndex;
        fullscreenToggle.isOn = fullscreen == 1;
        vsyncToggle.isOn = vsync == 1;

        // Önemli: Slider'ý güncellediðimizde 'OnValueChanged' tetiklenir 
        // ve SetBrightness otomatik çalýþýr.
        brightnessSlider.value = brightness;

        SetResolution(resIndex);
        SetFullscreen(fullscreen == 1);
        SetVSync(vsync == 1);

        // Ekstra garanti olsun diye manuel de çaðýrabiliriz
        SetBrightness(brightness);
    }
}