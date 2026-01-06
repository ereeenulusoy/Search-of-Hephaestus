using UnityEngine;
using UnityEngine.Rendering.Universal; // URP için gerekli

public class MGFall_EffectToggle : MonoBehaviour
{
    [Header("Renderer Data Ayarlarý")]
    [SerializeField] private ScriptableRendererData rendererData;
    [SerializeField] private string featureName = "New FullScreen Pass Renderer Feature";

    private ScriptableRendererFeature effectFeature;

    private void Awake()
    {
        if (rendererData == null)
        {
            Debug.LogError("Renderer Data atanmamýþ! Lütfen Inspector üzerinden sürükleyin.");
            return;
        }

        // Feature'ý bul
        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature.name == featureName)
            {
                effectFeature = feature;
                break;
            }
        }
    }

    private void OnEnable()
    {
        SetEffectState(true);
    }

    private void OnDisable()
    {
        SetEffectState(false);
    }

    private void OnApplicationQuit()
    {
        SetEffectState(false);
    }

    private void SetEffectState(bool state)
    {
        if (effectFeature != null)
        {
            // .isActive yerine .SetActive() metodunu kullanýyoruz
            effectFeature.SetActive(state);

            // Deðiþikliðin hemen yansýmasý için renderer'ý iþaretliyoruz
            rendererData.SetDirty();
        }
    }
} // Sýnýfýn sonu (End-of-file hatasýný önlemek için bu parantez çok önemli)