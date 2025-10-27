using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Sahne yönetimi için ÞART!

public class DurdurmaMenusu : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject durdurmaPaneli;
    // public GameObject ayarlarPaneli; // Ayarlar butonu için ileride

    [Header("Ana Menü Butonlarý")]
    public Button devamEtButonu;
    public Button ayarlarButonu;
    public Button yenidenBaslatButonu;
    public Button menuyeDonButonu;

    [Header("Envanter Referanslarý")]
    // Envanterdeki slotlarý buraya sürükle
    public Image[] envanterSlotlari;

    [Header("Ýstatistik Text Referanslarý")]
    public TextMeshProUGUI maxCanText;
    public TextMeshProUGUI canYenilemeText;
    public TextMeshProUGUI hasarText;
    public TextMeshProUGUI kritikSansText;
    public TextMeshProUGUI mermiSayisiText;
    // ... Diðer textler ...

    private bool oyunDurdu = false;

    void Start()
    {
        // Baþlangýçta panelin kapalý olduðundan emin ol
        durdurmaPaneli.SetActive(false);

        // Butonlarýn OnClick olaylarýný koddla atayalým (Daha temiz yöntem)
        devamEtButonu.onClick.AddListener(DevamEt);
        yenidenBaslatButonu.onClick.AddListener(YenidenBaslat);
        menuyeDonButonu.onClick.AddListener(MenuyeDon);
        // ayarlarButonu.onClick.AddListener(AyarlariAc);
    }

    void Update()
    {
        // ESC tuþunu dinle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleDuraklat();
        }
    }

    public void ToggleDuraklat()
    {
        oyunDurdu = !oyunDurdu;

        if (oyunDurdu)
        {
            // Oyunu Durdur
            Time.timeScale = 0f; // Zamaný durdurur!
            durdurmaPaneli.SetActive(true);
            ArayuzuGuncelle();
        }
        else
        {
            // Oyunu Devam Ettir
            Time.timeScale = 1f; // Zamaný normale döndür
            durdurmaPaneli.SetActive(false);
            // ayarlarPaneli.SetActive(false); // Ayarlar açýksa onu da kapat
        }
    }

    // Paneli her açtýðýmýzda verileri güncelleyen metot
    void ArayuzuGuncelle()
    {
        // --- ÝSTATÝSTÝKLERÝ DOLDUR ---
        OyuncuKaynaklari kaynak = OyuncuKaynaklari.instance; // Kýsayol

        maxCanText.text = "Maksimum Can: " + kaynak.maksimumCan;
        canYenilemeText.text = "Can Yenileme: " + kaynak.canYenileme;
        hasarText.text = "Hasar: " + (kaynak.hasarBonus * 100f) + "%";
        kritikSansText.text = "Kritik Þans: " + (kaynak.kritikSans * 100f) + "%";
        mermiSayisiText.text = "Mermi Sayýsý: +" + kaynak.mermiSayisiBonus;
        // ... Diðer textleri de buraya ekle ...


        // --- ENVANTERÝ DOLDUR (Ýsteklerine göre) ---
        // LazerBoom silahý alýndýysa ilk slotta göster
        if (envanterSlotlari.Length > 0)
        {
            envanterSlotlari[0].enabled = kaynak.lazerBoomSahipMi;
            // envanterSlotlari[0].sprite = [LazerBoom'un ikonu]; // Buraya ikonunu atayabilirsin
        }

        // LazerBoom'un ilk geliþtirmesi alýndýysa ikinci slotta göster
        if (envanterSlotlari.Length > 1)
        {
            envanterSlotlari[1].enabled = kaynak.lazerBoomGelistirmeleriAlindi[0];
            // envanterSlotlari[1].sprite = [Geliþtirme ikonu];
        }
        // ... Diðer envanter slotlarý ...
    }


    // --- BUTON FONKSÝYONLARI ---

    void DevamEt()
    {
        ToggleDuraklat();
    }

    void YenidenBaslat()
    {
        Time.timeScale = 1f; // Zamaný normale döndürmeyi unutma!
        // Mevcut sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MenuyeDon()
    {
        Time.timeScale = 1f; // Zamaný normale döndürmeyi unutma!
        SceneManager.LoadScene("MainMenu"); // Ana menü sahnenin adýnýn "MainMenu" olduðunu varsayýyoruz.
    }

    /*
    void AyarlariAc()
    {
        // Ayarlar panelini aç
    }
    */
}