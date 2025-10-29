using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecimPaneliKontrol : MonoBehaviour
{
    [Header("Panel Referanslarý")]
    public GameObject secimPaneli;
    public Animator animasyonluArkaPlanAnimator;
    public CanvasGroup secenekGrubuCanvasGroup;

    [Header("Seçenek Elemanlarý")]
    public TextMeshProUGUI secim1Text;
    public TextMeshProUGUI secim2Text;
    public Image secim1GorselAlani;
    public Image secim2GorselAlani;

    // --- Ýtem Havuzu (Türkçe Karakterler Fonta Uygun Hale Getirildi) ---
    private string[] olasiItemler = new string[]
    {
        "Ekstra Can",
        "Hizli Kosma",
        "Cift Ziplama",
        "Guclu Mermi",
        "Kalkan",
        "Hizli Ates Etme"
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
                secimPaneli.SetActive(true);
                secenekGrubuCanvasGroup.alpha = 0;
                secenekGrubuCanvasGroup.interactable = false;
                animasyonluArkaPlanAnimator.Play("AtesliDöngü_Anim", -1, 0f);
            }
        }
    }

    // Animasyon Olayý tarafýndan çaðrýlacak
    public void AnimasyonBittiSecenekleriGoster()
    {
        YeniSecenekleriBelirle();
        secenekGrubuCanvasGroup.alpha = 1;
        secenekGrubuCanvasGroup.interactable = true;
        secenekGrubuCanvasGroup.blocksRaycasts = true;
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
        if (secim1GorselAlani != null) secim1GorselAlani.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f);
        if (secim2GorselAlani != null) secim2GorselAlani.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f);
    }

    public void SecimYap(int secimIndex)
    {
        if (secimIndex == 1) Debug.Log("Secim 1 yapýldý: " + sunulanItem1 + " alýndý!");
        else if (secimIndex == 2) Debug.Log("Secim 2 yapýldý: " + sunulanItem2 + " alýndý!");

        secimPaneli.SetActive(false);
    }
}