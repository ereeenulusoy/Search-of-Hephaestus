using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // Image bileþenleri için gerekli
using TMPro;      // TextMeshPro (TMP) bileþenleri için gerekli (varsa)

public class PowerUpManager : MonoBehaviour
{
    // Inspector'da dolduracaðýnýz tüm olasý özelliklerin listesi
    public List<PowerUp> tumPowerUplar;

    // Unity Inspector'da sürükleyip býrakacaðýnýz, animasyonun bittiði boþ bölgelerin referanslarý
    public GameObject bosBolge1;
    public GameObject bosBolge2;

    // Rastgele 2 özellik seçip atayan metod
    public void RastgeleOzellikleriAta()
    {
        // 1. Olasý özelliklerin bir kopyasýný al
        List<PowerUp> secilebilirler = new List<PowerUp>(tumPowerUplar);

        // 2. Birinci boþ bölge için rastgele özellik seç
        PowerUp secilen1 = SecimYap(secilebilirler);
        // Seçilen özelliði bölgeye uygula (bu kýsým görsel ve metin atamasýdýr)
        Uygula(bosBolge1, secilen1);

        // 3. Seçilen özelliði listeden çýkar (iki bölgede ayný özellik çýkmasýn diye)
        secilebilirler.Remove(secilen1);

        // 4. Ýkinci boþ bölge için kalanlar arasýndan rastgele özellik seç
        PowerUp secilen2 = SecimYap(secilebilirler);
        Uygula(bosBolge2, secilen2);
    }

    // Listeden rastgele bir PowerUp seçen yardýmcý metod
    private PowerUp SecimYap(List<PowerUp> liste)
    {
        int randomIndex = Random.Range(0, liste.Count);
        return liste[randomIndex];
    }

    // Seçilen PowerUp verilerini boþ bölgedeki bileþenlere yazan metod
    private void Uygula(GameObject bolge, PowerUp ozellik)
    {
        // --- ICON GÖRSELÝNÝ AYARLAMA ---
        // Eðer Image bileþeni doðrudan 'bolge' GameObject'i üzerindeyse:
        Image iconImage = bolge.GetComponent<Image>();

        // Eðer Image bileþeni 'bolge'nin çocuk objelerinden birindeyse (daha yaygýndýr):
        if (iconImage == null)
        {
            iconImage = bolge.GetComponentInChildren<Image>();
        }

        if (iconImage != null)
        {
            iconImage.sprite = ozellik.icon;
            // Eðer Image'i önceden kapatmýþsanýz, burada açmayý unutmayýn
            iconImage.gameObject.SetActive(true);
        }


        // --- ÖZELLÝK ADINI AYARLAMA (TextMeshPro kullanýlýyorsa) ---
        TMP_Text adText = bolge.GetComponentInChildren<TMP_Text>();

        if (adText != null)
        {
            adText.text = ozellik.powerUpAdi;
            adText.gameObject.SetActive(true);
        }
        // Eðer TextMeshPro yerine normal Unity Text kullanýyorsanýz:
        else
        {
            Text adTextNormal = bolge.GetComponentInChildren<Text>();
            if (adTextNormal != null)
            {
                adTextNormal.text = ozellik.powerUpAdi;
                adTextNormal.gameObject.SetActive(true);
            }
        }
    }
}