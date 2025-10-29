using UnityEngine;
using UnityEngine.UI; // Image referansý için ÞART!
// Artýk using UnityEngine.Video; satýrýna ÝHTÝYACIMIZ YOK!
using TMPro;

public class SecimPaneliKontrol : MonoBehaviour
{
    [Header("Panel Referanslarý")]
    public GameObject secimPaneli;
    // VideoOynatici referansý SÝLÝNDÝ!

    [Header("Seçenek Elemanlarý")]
    public TextMeshProUGUI secim1Text;
    public TextMeshProUGUI secim2Text;
    public Image secim1GorselAlani;
    public Image secim2GorselAlani;

    // --- Ýtem Havuzu ---
    private string[] olasiItemler = new string[]
    {
        "Ekstra Can", "Hýzlý Koþma", "Çift Zýplama", "Güçlü Mermi", "Kalkan", "Hýzlý Ateþ Etme"
    };
    private string sunulanItem1;
    private string sunulanItem2;

    void Start()
    {
        if (secimPaneli != null) secimPaneli.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!secimPaneli.activeSelf)
            {
                YeniSecenekleriBelirle();

                // Sadece paneli açýyoruz. Ýçindeki Animator otomatik baþlayacak.
                secimPaneli.SetActive(true);
                // videoOynatici.Stop/Play satýrlarý SÝLÝNDÝ!
            }
        }
    }

    void YeniSecenekleriBelirle()
    {
        int index1 = Random.Range(0, olasiItemler.Length);
        sunulanItem1 = olasiItemler[index1];
        int index2;
        do { index2 = Random.Range(0, olasiItemler.Length); } while (index1 == index2);
        sunulanItem2 = olasiItemler[index2];

        secim1Text.text = sunulanItem1;
        secim2Text.text = sunulanItem2;

        if (secim1GorselAlani != null) // Null kontrolü eklendi
            secim1GorselAlani.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f);
        if (secim2GorselAlani != null) // Null kontrolü eklendi
            secim2GorselAlani.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f);
    }

    public void SecimYap(int secimIndex)
    {
        if (secimIndex == 1) Debug.Log("Seçim 1 yapýldý: " + sunulanItem1 + " alýndý!");
        else if (secimIndex == 2) Debug.Log("Seçim 2 yapýldý: " + sunulanItem2 + " alýndý!");

        // Sadece paneli kapatýyoruz. Ýçindeki Animator otomatik duracak.
        secimPaneli.SetActive(false);
        // videoOynatici.Stop satýrý SÝLÝNDÝ!
    }
}