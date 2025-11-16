using UnityEngine;
using UnityEngine.Audio; // AudioMixer için gerekli
using UnityEngine.UI;
using TMPro;             // % Deðerini göstermek için gerekli

public class SoundSettings : MonoBehaviour
{
    // INSPECTOR'DA ATANACAK ALANLAR
    public AudioMixer mainMixer;     // Mixer nesnesini buraya sürükleyin
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;

    // % Deðerlerini göstermek için (Opsiyonel)
    public TextMeshProUGUI masterValueText;

    // Mixer'da expose ettiðiniz parametre isimleri
    private const string MASTER_PARAM = "MasterVolume";
    private const string MUSIC_PARAM = "MusicVolume";
    private const string SFX_PARAM = "SFXVolume";

    void Awake()
    {
        // Slider'lar hareket ettiðinde çaðrýlacak metotlarý ata
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        muteToggle.onValueChanged.AddListener(SetMute);

        // UI ve Oyuna kayýtlý ayarlarý yükle
        LoadSettings();
    }

    // --- SES SEVÝYESÝNÝ AYARLAMA METOTLARI ---

    // Ses seviyesini logaritmik olarak ayarlar
    private void SetVolume(string parameterName, float sliderValue)
    {
        float volume = 0f;

        // Slider deðeri 0 (sessiz) olduðunda -80 dB ayarla
        if (sliderValue <= 0.0001f) // 0'a çok yakýn bir deðer kontrolü
        {
            volume = -80f;
        }
        else
        {
            // Logaritmik dönüþüm formülü: dB = 20 * log10(deðer)
            volume = Mathf.Log10(sliderValue) * 20;
        }

        mainMixer.SetFloat(parameterName, volume);

        // UI'daki % deðerini güncelle (Örn: masterValueText'i)
        if (parameterName == MASTER_PARAM && masterValueText != null)
        {
            masterValueText.text = Mathf.RoundToInt(sliderValue * 100) + "%";
        }
    }

    public void SetMasterVolume(float value) => SetVolume(MASTER_PARAM, value);
    public void SetMusicVolume(float value) => SetVolume(MUSIC_PARAM, value);
    public void SetSFXVolume(float value) => SetVolume(SFX_PARAM, value);

    // Mute/Unmute Fonksiyonu
    public void SetMute(bool isMuted)
    {
        if (isMuted)
        {
            // Master sesi -80 dB (tamamen sessiz) ayarla
            mainMixer.SetFloat(MASTER_PARAM, -80f);
        }
        else
        {
            // Mute açýldýðýnda, en son kaydedilen slider deðerini uygula
            SetMasterVolume(masterSlider.value);
        }

        // Mute durumu Slider'a yansýmaz, ayrý kaydetmek gerekir
        PlayerPrefs.SetInt("MuteState", isMuted ? 1 : 0);
    }

    // --- KAYIT VE YÜKLEME ---

    public void SaveSettings()
    {
        // Slider deðerlerini (0-1 arasýnda) kaydet
        PlayerPrefs.SetFloat("MasterSliderValue", masterSlider.value);
        PlayerPrefs.SetFloat("MusicSliderValue", musicSlider.value);
        PlayerPrefs.SetFloat("SFXSliderValue", sfxSlider.value);
        // Mute durumunu da kaydet:
        PlayerPrefs.SetInt("MuteState", muteToggle.isOn ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Ses Ayarlarý Kaydedildi.");
    }

    public void LoadSettings()
    {
        // Varsayýlan: 0.75 (yüzde 75 ses)
        float masterVal = PlayerPrefs.GetFloat("MasterSliderValue", 0.75f);
        float musicVal = PlayerPrefs.GetFloat("MusicSliderValue", 0.75f);
        float sfxVal = PlayerPrefs.GetFloat("SFXSliderValue", 0.75f);
        int muteState = PlayerPrefs.GetInt("MuteState", 0); // 0 = Unmuted (Açýk)

        // UI'ý güncelle
        masterSlider.value = masterVal;
        musicSlider.value = musicVal;
        sfxSlider.value = sfxVal;
        muteToggle.isOn = muteState == 1;

        // Ayarlarý oyuna uygula
        SetMasterVolume(masterVal);
        SetMusicVolume(musicVal);
        SetSFXVolume(sfxVal);

        // Mute durumu yüklenirse, mute fonksiyonunu tekrar çaðýr (Sesi kesmek için)
        if (muteState == 1)
        {
            SetMute(true);
        }
    }
}