using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections; // IEnumerator için gerekli

public class VideoSlaytGosterisi : MonoBehaviour
{
    // Inspector'da atamak üzere videolarýn listesi
    public VideoClip[] videoKlipleri;

    // Videonun oynatýlacaðý Raw Image UI bileþeni
    public RawImage rawImage;

    // Video Oynatýcý bileþeni (Raw Image nesnesine eklenecek)
    private VideoPlayer videoPlayer;

    private int mevcutKlipIndex = 0;

    void Start()
    {
        // 1. Video Oynatýcý Bileþenini Ekle
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false; // Otomatik baþlamasýn
        videoPlayer.isLooping = false; // Tek tek döngü yapacaðýmýz için kapalý kalsýn
        videoPlayer.renderMode = VideoRenderMode.RenderTexture; // Render Texture kullanacaðýz

        // 2. Video için bir Render Texture oluþtur
        // Boyutlarý ekranýnýza veya video boyutlarýna göre ayarlayýn
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
        videoPlayer.targetTexture = renderTexture;
        rawImage.texture = renderTexture; // Raw Image'in kaynaðý bu texture olacak

        // 3. Oynatýcý hazýr olunca sýradaki klibi oynatma fonksiyonunu çaðýr
        videoPlayer.loopPointReached += OynatmaBitti;

        // 4. Slayt gösterisini baþlat
        StartCoroutine(VideoSlaytBaslat());
    }

    // Videolarý sýrayla oynatan Coroutine
    IEnumerator VideoSlaytBaslat()
    {
        while (true) // Sonsuz döngü (Loop)
        {
            if (videoKlipleri.Length == 0)
            {
                Debug.LogWarning("Oynatýlacak video klibi yok!");
                yield break; // Döngüyü sonlandýr
            }

            // Mevcut klibi ata ve oynat
            videoPlayer.clip = videoKlipleri[mevcutKlipIndex];
            videoPlayer.Play();

            // Video klibinin bitmesini bekle
            // videoPlayer.clip.length uzunluðunu kullanmak daha garantilidir
            yield return new WaitForSeconds((float)videoKlipleri[mevcutKlipIndex].length);

            // Oynatma bittikten sonra sýradaki klibe geç
            mevcutKlipIndex++;
            if (mevcutKlipIndex >= videoKlipleri.Length)
            {
                mevcutKlipIndex = 0; // Baþa dön (Loop)
            }
        }
    }

    // Opsiyonel: Oynatma bitince sýradaki klibi oynatmak için kullanýlabilir
    private void OynatmaBitti(VideoPlayer vp)
    {
        // Coroutine ile hallettiðimiz için bu metot zorunlu deðil
    }
}