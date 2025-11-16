using UnityEngine;
using System.Collections;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    // === Inspector'dan Atanacak Alanlar ===
    [Header("Gecikme Ayarları")]
    // ✨ YENİ: Animasyonun başlamadan önceki bekleme süresi (saniye)
    public float startDelay = 2.0f;

    [Header("Yazım Ayarları")]
    public float delayPerCharacter = 0.05f; // Her karakterin yazılma süresi (saniye)
    public float endDelay = 1.0f; // Efekt bitince ekstra bekleme süresi

    // ... (diğer değişkenler aynı kalacak) ...
    private TextMeshProUGUI textComponent;
    private string fullText;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI bileşeni bulunamadı!");
            enabled = false;
            return;
        }

        fullText = textComponent.text;
        textComponent.text = "";
    }

    void Start()
    {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        // 1. ADIM: Başlangıç gecikmesini uygula
        if (startDelay > 0)
        {
            yield return new WaitForSeconds(startDelay);
        }

        // 2. ADIM: Metin yazma döngüsü başlar
        foreach (char character in fullText)
        {
            textComponent.text += character;
            yield return new WaitForSeconds(delayPerCharacter);
        }

        // Metin yazma işlemi bitti.

        // 3. ADIM: Bitiş gecikmesi
        if (endDelay > 0)
        {
            yield return new WaitForSeconds(endDelay);
        }

        Debug.Log("Yazım efekti tamamlandı: " + fullText);
    }
}