using UnityEngine;
using UnityEngine.Audio; // Audio Mixer'ý kontrol etmek için bu kütüphane ÞART!
using UnityEngine.UI;    // Slider ve Button'larý kontrol etmek için bu kütüphane ÞART!
using UnityEngine.SceneManagement; // Sahne deðiþtirmek için bu kütüphane ÞART!

public class SesYoneticisi : MonoBehaviour
{
    [Header("Ses Kontrol Merkezi")]
    public AudioMixer anaMixer; // Project panelindeki AnaMixer'ý buraya sürükleyeceðiz.

    [Header("Arayüz Elemanlarý")]
    public Slider masterSlider;
    public Slider muzikSlider;
    public Slider sfxSlider;
    public Button geriButonu;

    // PlayerPrefs'te ayarlarý kaydetmek için kullanacaðýmýz anahtarlar
    private const string MASTER_KEY = "MasterVolume";
    private const string MUZIK_KEY = "MuzikVolume";
    private const string SFX_KEY = "SFXVolume";

    void Start()
    {
        // Kayýtlý ayarlarý yükle ve slider'lara ata
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_KEY, 0.75f); // Kayýt yoksa %75 (0.75) baþla
        muzikSlider.value = PlayerPrefs.GetFloat(MUZIK_KEY, 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_KEY, 0.75f);

        // Slider'lar her hareket ettirildiðinde ilgili fonksiyonu çaðýrmalarý için "dinleyici" ekle
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        muzikSlider.onValueChanged.AddListener(SetMuzikVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Geri butonuna týklanýldýðýnda GeriDon fonksiyonunu çaðýr
        geriButonu.onClick.AddListener(GeriDon);

        // Slider'larýn mevcut deðerlerini Audio Mixer'a uygula (Oyun baþlarken sesin doðru ayarlanmasý için)
        SetMasterVolume(masterSlider.value);
        SetMuzikVolume(muzikSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    // --- SES AYARLAMA FONKSÝYONLARI ---

    // Bu fonksiyonu MasterSlider çaðýracak
    public void SetMasterVolume(float sliderValue)
    {
        // Slider 0-1 arasý çalýþýr, Mixer ise -80 ile 0 arasý (logaritmik) çalýþýr.
        // Bu yüzden bir dönüþtürme yapmamýz gerekir.
        float volumeInDb = Mathf.Log10(sliderValue) * 20;

        // Eðer slider 0 yapýlýrsa ses -sonsuz (eksi sonsuz) olur, hata almamak için -80 (en kýsýk) yapýyoruz.
        if (sliderValue == 0)
        {
            volumeInDb = -80f;
        }

        anaMixer.SetFloat("MasterVolume", volumeInDb);
        PlayerPrefs.SetFloat(MASTER_KEY, sliderValue); // Ayarý kaydet
    }

    // Bu fonksiyonu MuzikSlider çaðýracak
    public void SetMuzikVolume(float sliderValue)
    {
        float volumeInDb = Mathf.Log10(sliderValue) * 20;
        if (sliderValue == 0) volumeInDb = -80f;

        anaMixer.SetFloat("MuzikVolume", volumeInDb);
        PlayerPrefs.SetFloat(MUZIK_KEY, sliderValue); // Ayarý kaydet
    }

    // Bu fonksiyonu SFXSlider çaðýracak
    public void SetSFXVolume(float sliderValue)
    {
        float volumeInDb = Mathf.Log10(sliderValue) * 20;
        if (sliderValue == 0) volumeInDb = -80f;

        anaMixer.SetFloat("SFXVolume", volumeInDb);
        PlayerPrefs.SetFloat(SFX_KEY, sliderValue); // Ayarý kaydet
    }

    // --- GERÝ DÖNME FONKSÝYONU ---

    // Bu fonksiyonu GeriButonu çaðýracak
    public void GeriDon()
    {
        // Ayarlarý kaydet (aslýnda her slider hareketinde kaydediyor ama garanti olsun)
        PlayerPrefs.Save();

        // Ana Menü sahnesine geri dön (Sahne adýnýn "MainMenu" olduðunu varsayýyoruz)
        SceneManager.LoadScene("MainMenu");
    }
}