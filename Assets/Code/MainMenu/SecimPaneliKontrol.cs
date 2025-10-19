using UnityEngine;
using TMPro; // TextMeshPro için ÞART!
// Video kütüphanesine artýk ihtiyacýmýz yok!

public class SecimPaneliKontrol : MonoBehaviour
{
    [Header("Panel Referanslarý")]
    public GameObject secimPaneli;
    // VideoOynatici referansýna artýk ihtiyacýmýz yok!

    [Header("Seçenek Elemanlarý")]
    public TextMeshProUGUI secim1Text;
    public TextMeshProUGUI secim2Text;

    // --- Ýtem Havuzu ---
    private string[] olasiItemler = new string[]
    {
        "Ekstra Can", "Hýzlý Koþma", "Çift Zýplama", "Güçlü Mermi", "Kalkan", "Hýzlý Ateþ Etme"
    };

    private string sunulanItem1;
    private string sunulanItem2;

    void Start()
    {
        // Oyun baþýnda panel kapalý olsun
        if (secimPaneli != null) secimPaneli.SetActive(false);
    }

    void Update()
    {
        // 'H' tuþuna (Kalp toplama) basýldýðýnda
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!secimPaneli.activeSelf)
            {
                // 1. Rastgele itemleri belirle
                YeniSecenekleriBelirle();

                // 2. Paneli aç
                secimPaneli.SetActive(true);
                // Panel açýldýðý an, içindeki Animator otomatik olarak oynamaya baþlayacak!
                Debug.Log("Özellik Seçim Paneli açýldý!");
            }
        }
    }

    // Ýtem havuzundan iki FARKLI item seçen metot
    void YeniSecenekleriBelirle()
    {
        int index1 = Random.Range(0, olasiItemler.Length);
        sunulanItem1 = olasiItemler[index1];
        int index2;
        do { index2 = Random.Range(0, olasiItemler.Length); } while (index1 == index2);
        sunulanItem2 = olasiItemler[index2];
        secim1Text.text = sunulanItem1;
        secim2Text.text = sunulanItem2;
    }

    // --- BUTON FONKSÝYONLARI ---
    public void SecimYap(int secimIndex)
    {
        if (secimIndex == 1) Debug.Log("Seçim 1 yapýldý: " + sunulanItem1 + " alýndý!");
        else if (secimIndex == 2) Debug.Log("Seçim 2 yapýldý: " + sunulanItem2 + " alýndý!");

        // Paneli kapat. Panel kapandýðý an, içindeki Animator otomatik olarak duracak.
        secimPaneli.SetActive(false);
    }
}