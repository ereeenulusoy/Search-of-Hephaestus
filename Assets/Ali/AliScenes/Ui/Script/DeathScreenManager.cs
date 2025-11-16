using UnityEngine;
using UnityEngine.UI; // UI elemanlarý için gerekli
using System.Collections; // Coroutine'ler için gerekli
using TMPro; // TextMeshPro kullanýyorsanýz bu gerekli

public class DeathScreenManager : MonoBehaviour
{
    // === Inspector'dan Atanacak Alanlar ===
    [Header("UI Elemanlarý")]
    public TextMeshProUGUI deathText; // Eðer TextMeshPro kullanýyorsanýz (ÖNERÝLÝR)
    // public Text deathText; // Eðer standart Unity Text kullanýyorsanýz bunu kullanýn
    public GameObject buttonContainer; // Baþtan Baþla ve Ana Menü butonlarýnýn tutulduðu obje

    [Header("Animasyon Ayarlarý")]
    public float scaleDuration = 1.0f; // Yazýnýn büyüme süresi (saniye)
    public float buttonDelay = 1.5f; // Yazý büyüdükten sonra butonlarýn bekleme süresi
    public Vector3 startScale = new Vector3(0.01f, 0.01f, 0.01f); // Baþlangýç ölçeði
    public Vector3 endScale = new Vector3(1.0f, 1.0f, 1.0f); // Bitiþ ölçeði

    private float timeElapsed;

    void Start()
    {
        // 1. Yazýnýn baþlangýç ölçeðini ayarla
        if (deathText != null)
        {
            deathText.rectTransform.localScale = startScale;
        }

        // 2. Butonlarý baþlangýçta gizle
        if (buttonContainer != null)
        {
            buttonContainer.SetActive(false);
        }

        // 3. Animasyonu baþlat
        StartCoroutine(AnimateDeathScreen());
    }

    private IEnumerator AnimateDeathScreen()
    {
        // -------------------------
        // AÞAMA 1: Yazýyý Büyütme
        // -------------------------
        timeElapsed = 0f;

        while (timeElapsed < scaleDuration)
        {
            // Geçen zamana göre ilerleme oranýný hesapla (0'dan 1'e)
            float t = timeElapsed / scaleDuration;

            // Yumuþak bir geçiþ için Ease-Out kullan (isteðe baðlý)
            // t = t * t * (3f - 2f * t); // SmoothStep

            // Ölçeði baþlangýçtan bitiþe doðru LERP ile yumuþakça deðiþtir
            deathText.rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);

            timeElapsed += Time.deltaTime;
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Animasyon bittiðinde son ölçeði tam olarak ayarla
        deathText.rectTransform.localScale = endScale;

        // -------------------------
        // AÞAMA 2: Gecikme
        // -------------------------
        yield return new WaitForSeconds(buttonDelay);

        // -------------------------
        // AÞAMA 3: Butonlarý Gösterme
        // -------------------------
        if (buttonContainer != null)
        {
            buttonContainer.SetActive(true);
            // Butonlarýn fade-in (solarak ortaya çýkma) animasyonu da eklenebilir.
        }
    }
}