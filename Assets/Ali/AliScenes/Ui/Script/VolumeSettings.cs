using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSlider;

    private void Start()
    {
        // 1. Oyun ilk kez çalýþýyorsa (PlayerPrefs kaydý yoksa) varsayýlan deðerleri kaydet.
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            // Eðer herhangi bir kayýt yoksa, mevcut Slider deðerlerini kaydet.
            SetMasterVolume();
            SetMusicVolume();
            SetSfxVolume();
        }

        // 2. PlayerPrefs kaydý varsa, deðerleri yükle ve AudioMixer'a uygula.
        // Bu tek çaðrý, daha önce kaydedilen tüm ses seviyelerini yükleyecektir.
        LoadVolume();
    }

    // Ses seviyelerini kaydýrýcýdan AudioMixer'a ayarlar ve kaydeder.
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        // Slider deðeri (0.0001 - 1) logaritmik desibele dönüþtürülür.
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    // Ses seviyelerini kaydýrýcýdan AudioMixer'a ayarlar ve kaydeder.
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    // Ses seviyelerini kaydýrýcýdan AudioMixer'a ayarlar ve kaydeder.
    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    // Kaydedilen deðerleri PlayerPrefs'ten okur, Slider'lara uygular ve ses seviyelerini günceller.
    private void LoadVolume()
    {
        // 1. Müzik Yükleme
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", musicSlider.value);
        SetMusicVolume();

        // 2. SFX Yükleme
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", sfxSlider.value);
        SetSfxVolume();

        // 3. Master Yükleme (Hata Düzeltildi!)
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", masterSlider.value); // masterSlider kullanýlýyor
        SetMasterVolume();
    }
}