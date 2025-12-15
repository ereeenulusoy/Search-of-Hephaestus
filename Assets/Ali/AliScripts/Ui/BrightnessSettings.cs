using UnityEngine;
using UnityEngine.Rendering; // Volume bileþeni için
using UnityEngine.Rendering.Universal; // URP efektleri için
using UnityEngine.UI; // Slider kullanacaksanýz

public class BrightnessController : MonoBehaviour
{
    public Volume globalVolume; // Inspector'dan Global Volume'ü sürükleyin
    public Slider brightnessSlider; // UI Slider'ý buraya sürükleyin

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        // Volume profilinden Color Adjustments efektini bulmaya çalýþ
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            // Slider'a baþlangýç deðeri verelim (Mevcut exposure deðeri)
            // Genelde -1 ile +1 arasý deðerler idealdir.
            brightnessSlider.value = colorAdjustments.postExposure.value;

            // Slider her deðiþtiðinde fonksiyonu çalýþtýr
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
        else
        {
            Debug.LogWarning("Global Volume içinde Color Adjustments bulunamadý!");
        }
    }

    public void SetBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            // Slider deðerini Post Exposure'a ata
            colorAdjustments.postExposure.value = value;
        }
    }
}