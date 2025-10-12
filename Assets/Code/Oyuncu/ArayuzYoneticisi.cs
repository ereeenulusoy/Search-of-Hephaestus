using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ArayuzYoneticisi : MonoBehaviour
{
    [Header("Kontrol Edilecek Paneller")]
    public GameObject anaGelisimPaneli;

    [Header("UI Referanslarý")]
    public TextMeshProUGUI paraGostergeText;
    public Button atakYukseltButonu;

  
    public TextMeshProUGUI atakButonText;

    [Header("Yükseltme Ayarlarý")]
    public int atakYukseltmeMaliyeti = 50;


    public Color satinAlinmisYaziRengi = Color.green;

    void Start()
    {
        UI_Guncelle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool panelAcikMi = !anaGelisimPaneli.activeSelf;
            anaGelisimPaneli.SetActive(panelAcikMi);
            if (panelAcikMi) UI_Guncelle();
        }
    }

    public void UI_Guncelle()
    {
        paraGostergeText.text = "Para: " + OyuncuKaynaklari.instance.para;

        if (OyuncuKaynaklari.instance.lazerBoomAtakGuncellemesiAlindi)
        {
            atakYukseltButonu.interactable = false; 


            atakButonText.color = satinAlinmisYaziRengi;
        }
        else
        {
            atakYukseltButonu.interactable = (OyuncuKaynaklari.instance.para >= atakYukseltmeMaliyeti);
        }
    }

    public void AtakYukseltSatinAl()
    {
        if (OyuncuKaynaklari.instance.ParaHarca(atakYukseltmeMaliyeti))
        {
            OyuncuKaynaklari.instance.lazerBoomAtakGuncellemesiAlindi = true;
            Debug.Log("LazerBoom Atak Yükseltmesi baþarýyla satýn alýndý!");
            UI_Guncelle();
        }
    }
}