using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GecisYoneticisi : MonoBehaviour
{
    // === Inspector'da Atanacak Bileþenler ===
    public VideoPlayer videoPlayer;
    public RawImage karakterRawImage;
    public TextMeshProUGUI baslikText;
    public string sonrakiSahneAdi = "YeniSahneAdi";

    // === Animasyon Ayarlarý ===
    [Header("Hareket Ayarlarý")]
    public float karakterHareketSuresi = 1.0f; // Karakterin hareket süresi
    public float yaziHareketSuresi = 1.5f;     // Yazýnýn hareket/açýlma süresi
    public float yaziGecikmeSuresi = 0.5f;     // Yazýnýn baþlamasý için bekleme süresi
    public Vector2 karakterHedefPozisyon = new Vector2(-800f, 0f);
    public Vector2 yaziHedefPozisyon = new Vector2(800f, 0f);

    [Header("Kaybolma (Fade) Ayarlarý")]
    public float kaybolmaSuresi = 0.5f;

    // --- Baþlangýç ve Abone Olma ---
    void Start()
    {
        if (videoPlayer == null || karakterRawImage == null || baslikText == null)
        {
            Debug.LogError("HATA: Gerekli tüm bileþenler Inspector'da atanmadý!");
            return;
        }

        // Yazýyý baþlangýçta tamamen görünmez yap
        Color yaziRengi = baslikText.color;
        yaziRengi.a = 0f;
        baslikText.color = yaziRengi;

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Video bittiðinde olayý kaldýr ve animasyonu baþlat
        videoPlayer.loopPointReached -= OnVideoFinished;
        StartCoroutine(GecisAnimasyonu());
    }

    IEnumerator GecisAnimasyonu()
    {
        // Baþlangýç deðerlerini kaydet
        Vector2 karakterBaslangicPos = karakterRawImage.rectTransform.anchoredPosition;
        Vector2 yaziBaslangicPos = baslikText.rectTransform.anchoredPosition;
        Color karakterBaslangicRengi = karakterRawImage.color;
        Color yaziBaslangicRengi = baslikText.color;

        Color yaziHedefRengi = yaziBaslangicRengi;
        yaziHedefRengi.a = 1f;

        // Toplam Hareket Süresini Belirle
        float toplamHareketSuresi = Mathf.Max(karakterHareketSuresi, yaziGecikmeSuresi + yaziHareketSuresi);
        float gecenZaman = 0f;

        // ==========================================================
        // ADIM 1: KARAKTER VE YAZI HAREKETÝ (Eþzamanlý Döngü)
        // ==========================================================
        while (gecenZaman < toplamHareketSuresi)
        {
            gecenZaman += Time.deltaTime;

            // --- 1. Karakter Hareketi ---
            float karakterOran = Mathf.Clamp01(gecenZaman / karakterHareketSuresi);
            karakterRawImage.rectTransform.anchoredPosition = Vector2.Lerp(
                karakterBaslangicPos,
                karakterHedefPozisyon,
                karakterOran
            );

            // --- 2. Yazý Hareketi ve Alpha (Gecikmeli Baþlar) ---
            if (gecenZaman > yaziGecikmeSuresi)
            {
                float yaziGecenZaman = gecenZaman - yaziGecikmeSuresi;
                float yaziOran = Mathf.Clamp01(yaziGecenZaman / yaziHareketSuresi);

                baslikText.rectTransform.anchoredPosition = Vector2.Lerp(
                    yaziBaslangicPos,
                    yaziHedefPozisyon,
                    yaziOran
                );

                baslikText.color = Color.Lerp(
                    yaziBaslangicRengi,
                    yaziHedefRengi,
                    yaziOran
                );
            }

            yield return null;
        }

        // ÝLK KÝLÝTLEME: Hareket bittiðinde pozisyonlarý kesinleþtir.
        karakterRawImage.rectTransform.anchoredPosition = karakterHedefPozisyon;
        baslikText.rectTransform.anchoredPosition = yaziHedefPozisyon;
        baslikText.color = yaziHedefRengi;


        // ==========================================================
        // ADIM 2: KAYBOLMA (FADE OUT)
        // ==========================================================
        gecenZaman = 0f;
        Color hedefRengi = karakterBaslangicRengi;
        hedefRengi.a = 0f; // Kaybolma hedefi (Alpha 0)

        while (gecenZaman < kaybolmaSuresi)
        {
            gecenZaman += Time.deltaTime;
            float oran = Mathf.Clamp01(gecenZaman / kaybolmaSuresi);

            // Karakteri ve Yazýyý kaybet
            karakterRawImage.color = Color.Lerp(karakterBaslangicRengi, hedefRengi, oran);

            Color anlikYaziRengi = baslikText.color;
            anlikYaziRengi.a = Mathf.Lerp(1f, 0f, oran);
            baslikText.color = anlikYaziRengi;

            yield return null;
        }

        // ÝKÝNCÝ KÝLÝTLEME: Kaybolma tamamlandýðýnda Alpha deðerlerini kesinleþtir.
        karakterRawImage.color = hedefRengi;
        baslikText.color = new Color(baslikText.color.r, baslikText.color.g, baslikText.color.b, 0f);

        // ==========================================================
        // ADIM 3: SAHNE GEÇÝÞÝ
        // ==========================================================
        SceneManager.LoadScene(sonrakiSahneAdi);
    }
}