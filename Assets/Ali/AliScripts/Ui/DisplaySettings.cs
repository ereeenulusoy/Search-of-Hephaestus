using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class DisplaySettings : MonoBehaviour
{
    public Slider brightnessSlider;
    public PostProcessVolume postProcessVolume;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;
    private FloatParameter postExposure = new FloatParameter { value = 0f };

    private Resolution[] resolutions; // Cihazýn desteklediði çözünürlükler listesi

    void Awake()
    {
        // 1. Çözünürlük listesini al ve Dropdown'ý doldur
        LoadAvailableResolutions();

        // 2. UI elemanlarýna dinleyici (listener) ekle
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        vsyncToggle.onValueChanged.AddListener(SetVSync);

        // 3. Kayýtlý Ayarlarý Yükle (Eðer daha önce kaydedilmiþse)
        LoadSettings();
    }

    // --- AÞAMA AÞAMA METOTLAR ---

    // A. Çözünürlükleri Hazýrlama
    void LoadAvailableResolutions()
    {
        // Unity'den cihazýn desteklediði tüm çözünürlükleri al
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            // Dropdown için "1920x1080" gibi bir metin oluþtur
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            // Eðer bu çözünürlük, þu anki kullanýlan çözünürlükse, index'i kaydet
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Dropdown'a seçenekleri ekle ve þu anki çözünürlüðü seçili yap
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // B. Ayarlarý Uygulama

    // Çözünürlüðü Uygula
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        // Unity'nin Screen API'ýný kullanarak çözünürlüðü deðiþtir
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log($"Çözünürlük ayarlandý: {resolution.width}x{resolution.height}");
    }

    // Tam Ekraný Uygula
    public void SetFullscreen(bool isFullscreen)
    {
        // Unity'nin Screen API'ýný kullanarak tam ekran durumunu deðiþtir
        Screen.fullScreen = isFullscreen;
        Debug.Log($"Tam Ekran: {isFullscreen}");
    }

    // VSync'i Uygula
    public void SetVSync(bool isVSync)
    {
        // QualitySettings API'ýný kullanarak VSync'i aç/kapat
        // 1: Açýk (monitör hýzýyla senkronize), 0: Kapalý
        QualitySettings.vSyncCount = isVSync ? 1 : 0;
        Debug.Log($"VSync: {isVSync}");
    }
    public void SetBrightness(float value)
    {
        // Slider deðerini -3 ile 3 arasýnda bir Post Exposure deðerine dönüþtürmek yaygýndýr
        // Bu aralýk, parlaklýkta anlamlý bir deðiþiklik saðlar.
        float exposureValue = Mathf.Lerp(-3f, 3f, value);

        // 1. Post Process Profile'ý al (Color Grading efekti için)
        ColorGrading colorGrading;
        if (postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            // 2. Post Exposure deðerini ayarla
            // Eðer Post Exposure deðerini Inspector'da kontrol edilebilir yapmadýysanýz, bu satýr hata verecektir.
            colorGrading.postExposure.Override(exposureValue);
        }

        Debug.Log($"Parlaklýk (Post Exposure) ayarlandý: {exposureValue}");
        // PlayerPrefs'e kaydetme mantýðý SaveSettings() içinde yer almalýdýr.
    }

    // C. Kaydetme ve Yükleme

    // Ayarlarý kalýcý olarak kaydet (APPLY butonuna baðlayýn)
    public void SaveSettings()
    {
        // Seçilen Dropdown index'ini ve Toggle durumlarýný kaydet
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("BrightnessValue", brightnessSlider.value);

        // Tüm deðiþiklikleri diske yaz
        PlayerPrefs.Save();
        Debug.Log("Display Ayarlarý Kaydedildi.");
    }

    // Kayýtlý ayarlarý yükle (Menü açýldýðýnda veya CANCEL'a basýldýðýnda çaðrýlýr)
    public void LoadSettings()
    {
        // Varsayýlan deðerler: Son kaydedilen deðerler, yoksa mevcut deðerler
        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutionDropdown.value);
        int fullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0);
        int vsync = PlayerPrefs.GetInt("VSync", QualitySettings.vSyncCount);
        float brightness = PlayerPrefs.GetFloat("BrightnessValue", 0.5f); // 0.5f varsayýlan orta deðer


        // UI elemanlarýný yüklenen deðerlere ayarla
        resolutionDropdown.value = resIndex;
        fullscreenToggle.isOn = fullscreen == 1;
        vsyncToggle.isOn = vsync == 1;
        brightnessSlider.value = brightness;
        SetBrightness(brightness);
        SetResolution(resIndex);
        SetFullscreen(fullscreen == 1);
        SetVSync(vsync == 1);

        resolutionDropdown.RefreshShownValue();
    }
}