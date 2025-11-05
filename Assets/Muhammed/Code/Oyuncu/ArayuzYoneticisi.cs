using UnityEngine;
using UnityEngine.UI; // Button, Image gibi UI elemanlarý için ÞART!
using TMPro;      // TextMeshPro için ÞART!

public class ArayuzYoneticisi : MonoBehaviour
{
    [Header("Ana Paneller")]
    public GameObject anaGelisimPaneli;
    public GameObject ozelliklerPaneli;
    public Animator ozelliklerPanelAnimator;
    public GameObject silahGelistirmePaneli;
    public Animator silahPanelAnimator;

    [Header("Ana Geliþim Butonlarý")]
    public Button ozelliklerAcmaButonu;
    public Button silahGelistirmeAcmaButonu;

    [Header("Silah Geliþtirme Detaylarý")]
    public Image lazerSilahiIkon;
    public TextMeshProUGUI silahParaGostergeText; // Silah panelindeki para
    public Button[] lazerGelistirmeButonlari = new Button[4];

    [Header("Özellik Geliþtirme Detaylarý")] // BU BÖLÜM EKLENDÝ/GÜNCELLENDÝ
    public TextMeshProUGUI ozellikParaGostergeText; // Özellik panelindeki para
    public Button[] ozellikGelistirmeButonlari = new Button[4]; // Özellik butonlarý

    [Header("Görsel Ayarlar")]
    public Color ogeKilitliRenk = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    public Color satinAlinmisRenk = Color.black;

    [Header("Maliyetler")]
    public int[] lazerGelistirmeMaliyetleri = new int[4] { 10, 20, 30, 40 };
    public int[] ozellikGelistirmeMaliyetleri = new int[4] { 15, 25, 35, 45 }; // ÖZELLÝK MALÝYETLERÝ EKLENDÝ

    void Start()
    {
        ozelliklerPaneli.SetActive(false);
        silahGelistirmePaneli.SetActive(false);
        anaGelisimPaneli.SetActive(false);
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
            if (OyuncuKaynaklari.instance != null && !OyuncuKaynaklari.instance.lazerBoomSahipMi)
            {
                OyuncuKaynaklari.instance.lazerBoomSahipMi = true;
                Debug.Log("LazerBoom yerden alýndý!");
                UI_Guncelle();
            }
        }
    }

    public void UI_Guncelle()
    {
        if (OyuncuKaynaklari.instance == null) return; // Güvenlik kontrolü

        // --- Para Göstergelerini Güncelle ---
        string paraYazisi = "Para: " + OyuncuKaynaklari.instance.para;
        if (silahParaGostergeText != null) silahParaGostergeText.text = paraYazisi;
        if (ozellikParaGostergeText != null) ozellikParaGostergeText.text = paraYazisi; // Özellik paneli parasý eklendi

        // --- Silah Paneli Güncellemesi ---
        bool silahaSahipMi = OyuncuKaynaklari.instance.lazerBoomSahipMi;
        if (lazerSilahiIkon != null) lazerSilahiIkon.color = silahaSahipMi ? Color.white : ogeKilitliRenk;
        for (int i = 0; i < lazerGelistirmeButonlari.Length; i++)
        {
            if (lazerGelistirmeButonlari[i] == null) continue;
            if (!silahaSahipMi)
            {
                lazerGelistirmeButonlari[i].interactable = false;
                lazerGelistirmeButonlari[i].GetComponent<Image>().color = ogeKilitliRenk;
                continue;
            }
            if (OyuncuKaynaklari.instance.lazerBoomGelistirmeleriAlindi[i])
            {
                lazerGelistirmeButonlari[i].interactable = false;
                lazerGelistirmeButonlari[i].GetComponent<Image>().color = satinAlinmisRenk;
            }
            else
            {
                bool parasiYetiyor = OyuncuKaynaklari.instance.para >= lazerGelistirmeMaliyetleri[i];
                lazerGelistirmeButonlari[i].interactable = parasiYetiyor;
                lazerGelistirmeButonlari[i].GetComponent<Image>().color = parasiYetiyor ? Color.white : ogeKilitliRenk;
            }
        }

        // --- ÖZELLÝK PANELÝ GÜNCELLEMESÝ EKLENDÝ ---
        for (int i = 0; i < ozellikGelistirmeButonlari.Length; i++)
        {
            if (ozellikGelistirmeButonlari[i] == null) continue;

            // Özellik butonlarý için "silaha sahip mi" kontrolü yok
            if (OyuncuKaynaklari.instance.ozellikGelistirmeleriAlindi[i])
            {
                ozellikGelistirmeButonlari[i].interactable = false;
                ozellikGelistirmeButonlari[i].GetComponent<Image>().color = satinAlinmisRenk;
            }
            else
            {
                bool parasiYetiyor = OyuncuKaynaklari.instance.para >= ozellikGelistirmeMaliyetleri[i];
                ozellikGelistirmeButonlari[i].interactable = parasiYetiyor;
                ozellikGelistirmeButonlari[i].GetComponent<Image>().color = parasiYetiyor ? Color.white : ogeKilitliRenk;
            }
        }
    }

    // --- PANEL GEÇÝÞ METOTLARI ---
    public void OzelliklerPaneliniAc()
    {
        anaGelisimPaneli.SetActive(false);
        ozelliklerPaneli.SetActive(true);
        if (ozelliklerPanelAnimator != null) ozelliklerPanelAnimator.SetTrigger("Ac");
        UI_Guncelle(); // Özellikler paneli açýldýðýnda da UI güncellensin
    }

    public void SilahGelistirmePaneliniAc()
    {
        anaGelisimPaneli.SetActive(false);
        silahGelistirmePaneli.SetActive(true);
        if (silahPanelAnimator != null) silahPanelAnimator.SetTrigger("Ac");
        UI_Guncelle();
    }

    public void AnaGelisimPaneliniAc()
    {
        anaGelisimPaneli.SetActive(true);
        ozelliklerPaneli.SetActive(false);
        silahGelistirmePaneli.SetActive(false);
        UI_Guncelle();
    }

    // --- SATIN ALMA METOTLARI ---
    public void GelistirmeSatinAl(int gelistirmeIndex)
    {
        if (OyuncuKaynaklari.instance == null) return;
        int maliyet = lazerGelistirmeMaliyetleri[gelistirmeIndex];
        if (OyuncuKaynaklari.instance.ParaHarca(maliyet))
        {
            OyuncuKaynaklari.instance.lazerBoomGelistirmeleriAlindi[gelistirmeIndex] = true;
            UI_Guncelle();
        }
    }

    // ÖZELLÝK SATIN ALMA METODU EKLENDÝ
    public void OzellikSatinAl(int gelistirmeIndex)
    {
        if (OyuncuKaynaklari.instance == null) return;
        int maliyet = ozellikGelistirmeMaliyetleri[gelistirmeIndex];
        if (OyuncuKaynaklari.instance.ParaHarca(maliyet))
        {
            OyuncuKaynaklari.instance.ozellikGelistirmeleriAlindi[gelistirmeIndex] = true;
            UI_Guncelle();
        }
    }
}