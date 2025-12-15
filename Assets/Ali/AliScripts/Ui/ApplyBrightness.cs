using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ApplyBrightness : MonoBehaviour
{
    public Volume globalVolume; // Inspector'dan o sahnenin Volume'ünü sürükleyin

    void Start()
    {
        // Eðer inspector'dan atamayý unuttuysanýz otomatik bulmayý dener
        if (globalVolume == null) globalVolume = GetComponent<Volume>();
        if (globalVolume == null) globalVolume = FindObjectOfType<Volume>();

        // Kayýtlý parlaklýk deðerini çek (Varsayýlan 0)
        float savedBrightness = PlayerPrefs.GetFloat("BrightnessValue", 0f);

        // Deðeri uygula
        if (globalVolume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.postExposure.value = savedBrightness;
        }
    }
}
