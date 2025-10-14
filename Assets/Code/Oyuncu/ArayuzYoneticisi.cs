using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArayuzYoneticisi : MonoBehaviour
{
    [Header("Ana Paneller")]
    public GameObject anaGelisimPaneli;
    public GameObject ozelliklerPaneli;
    public GameObject silahGelistirmePaneli;

    [Header("Ana Geliþim Butonlarý")]
    public Button ozelliklerAcmaButonu;
    public Button silahGelistirmeAcmaButonu; // Bu buton artýk hep aktif olacak

    [Header("Silah Geliþtirme Detaylarý")]
    public Image lazerSilahiIkon; // Ýkinci paneldeki silahýn resmi
    public TextMeshProUGUI paraGostergeText;
    public Button[] lazerGelistirmeButonlari = new Button[4];

    [Header("Görsel Ayarlar")]
    public Color ogeKilitliRenk = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    public Color satinAlinmisRenk = Color.black;

    [Header("Geliþtirme Maliyetleri")]
    public int[] lazerGelistirmeMaliyetleri = new int[4] { 10, 20, 30, 40 };

    void Start()
    {
        ozelliklerPaneli.SetActive(false);
        silahGelistirmePaneli.SetActive(false);
        UI_Guncelle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool panelAcikMi = !anaGelisimPaneli.activeSelf;
            anaGelisimPaneli.SetActive(panelAcikMi);
            if (panelAcikMi) AnaGelisimPaneliniAc();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!OyuncuKaynaklari.instance.lazerBoomSahipMi)
            {
                OyuncuKaynaklari.instance.lazerBoomSahipMi = true;
                Debug.Log("LazerBoom yerden alýndý!");
                UI_Guncelle();
            }
        }
    }

    public void UI_Guncelle()
    {
        // ÖNEMLÝ DÜZELTME: Ana menüdeki butonu kontrol eden satýr buradan kaldýrýldý.
        // O buton artýk her zaman Unity'nin kendi ayarlarýndan aktif olacak.

        // --- SADECE ÝKÝNCÝ PANELÝN ÝÇÝNÝ GÜNCELLE ---
        bool silahaSahipMi = OyuncuKaynaklari.instance.lazerBoomSahipMi;

        // Ýkinci paneldeki silah ikonunun rengini ayarla
        lazerSilahiIkon.color = silahaSahipMi ? Color.white : ogeKilitliRenk;

        // Para göstergesini güncelle
        paraGostergeText.text = "Para: " + OyuncuKaynaklari.instance.para;

        // Dört geliþtirme butonunu güncelle
        for (int i = 0; i < lazerGelistirmeButonlari.Length; i++)
        {
            if (lazerGelistirmeButonlari[i] == null) continue;

            // Geliþtirme butonlarý, SADECE silaha sahipsek aktif olabilir.
            if (!silahaSahipMi)
            {
                lazerGelistirmeButonlari[i].interactable = false;
                continue; // Silah yoksa aþaðýdaki kontrollere hiç girme
            }

            // Silah varsa, diðer kontrolleri yap
            if (OyuncuKaynaklari.instance.lazerBoomGelistirmeleriAlindi[i])
            {
                lazerGelistirmeButonlari[i].interactable = false;
                lazerGelistirmeButonlari[i].GetComponent<Image>().color = satinAlinmisRenk;
            }
            else
            {
                bool parasiYetiyor = OyuncuKaynaklari.instance.para >= lazerGelistirmeMaliyetleri[i];
                lazerGelistirmeButonlari[i].interactable = parasiYetiyor;
                lazerGelistirmeButonlari[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    // --- PANEL GEÇÝÞ VE SATIN ALMA METOTLARI ---
    public void OzelliklerPaneliniAc() 
    { 
        anaGelisimPaneli.SetActive(false);
        ozelliklerPaneli.SetActive(true);
    }

    public void SilahGelistirmePaneliniAc()
    {
        anaGelisimPaneli.SetActive(false);
        silahGelistirmePaneli.SetActive(true);
        UI_Guncelle();
    }

    public void AnaGelisimPaneliniAc()
    {
        anaGelisimPaneli.SetActive(true);
        ozelliklerPaneli.SetActive(false);
        silahGelistirmePaneli.SetActive(false);
        UI_Guncelle();
    }

    public void GelistirmeSatinAl(int gelistirmeIndex)
    {
        int maliyet = lazerGelistirmeMaliyetleri[gelistirmeIndex];
        if (OyuncuKaynaklari.instance.ParaHarca(maliyet))
        {
            OyuncuKaynaklari.instance.lazerBoomGelistirmeleriAlindi[gelistirmeIndex] = true;
            UI_Guncelle();
        }
    }
}