using UnityEngine;

// Bu script, oyuncunun tüm ilerleme verilerini tutar.
// Singleton deseni kullanýr, yani sahnede sadece bir tane bulunur
// ve "OyuncuKaynaklari.instance" yazýlarak her yerden kolayca eriþilebilir.
public class OyuncuKaynaklari : MonoBehaviour
{
    // --- Singleton Kurulumu ---
    public static OyuncuKaynaklari instance;

    [Header("Ana Kaynaklar")]
    public int para = 100;

    // --- Oyuncu Ýstatistikleri ---
    // Bu deðiþkenleri, durdurma menüsündeki "Ýstatistikler" panelinde göstereceðiz.
    [Header("Oyuncu Ýstatistikleri")]
    public float maksimumCan = 100;
    public float canYenileme = 0;
    public float zirh = 0;
    public float kacýnmaSansi = 0.0f; // %0
    public float canCalma = 0.0f; // %0
    public float hasarBonus = 1.0f; // 1.0 = %100 hasar (yani +%0 bonus)
    public float kritikSans = 0.01f; // %1
    public float kritikHasar = 1.5f; // 1.5x
    public int mermiSayisiBonus = 0; // Ekstra mermi sayýsý
    public float mermiHiziBonus = 1.0f; // 1.0 = %100 hýz
    public float mermiToplamaArtisi = 1.0f; // 1.0 = %100 toplama
    // ... Buraya görseldeki diðer istatistikleri de ekleyebilirsin ...

    // --- Envanter ve Geliþtirmeler ---
    // Bu deðiþkenleri, durdurma menüsündeki "Envanter" panelinde ikon olarak göstereceðiz.
    [Header("Silah ve Özellik Sahipliði")]
    public bool lazerBoomSahipMi = false;
    public bool[] lazerBoomGelistirmeleriAlindi = new bool[4];

    // (Örnek: Kalple alýnan kalýcý özellikler)
    // public bool kaliciCanBonusuAlindi = false;
    // public bool kaliciHasarBonusuAlindi = false;


    // Awake metodu, oyun baþladýðýnda çalýþan ilk metotlardan biridir.
    void Awake()
    {
        // --- Singleton Kontrolü ---
        // Eðer sahnede "instance" (yani bu script'ten bir kopya) zaten varsa...
        if (instance != null)
        {
            // Yeni oluþturulan bu objeyi yok et, çünkü zaten bir tane var.
            Destroy(gameObject);
        }
        else // Eðer sahnede "instance" yoksa...
        {
            // Bu objeyi "instance" olarak ata.
            instance = this;
            // Bu objenin, yeni sahneler yüklendiðinde bile silinmemesini saðla.
            DontDestroyOnLoad(gameObject);
        }
    }

    // --- PARA KONTROL METODU ---

    /// <summary>
    /// Oyuncunun parasýndan belirtilen miktarý harcamayý dener.
    /// </summary>
    /// <param name="miktar">Harcanacak para miktarý</param>
    /// <returns>Para yeterliyse true, yetersizse false döner.</returns>
    public bool ParaHarca(int miktar)
    {
        // Eðer istenen miktarda veya daha fazla paramýz varsa...
        if (para >= miktar)
        {
            para -= miktar; // Parayý düþür.
            Debug.Log(miktar + " para harcandý. Kalan para: " + para);
            return true;    // Ýþlemin baþarýlý olduðunu bildir.
        }
        else // Eðer yeterli para yoksa...
        {
            Debug.Log("Yetersiz para! Harcama yapýlamadý.");
            return false;   // Ýþlemin baþarýsýz olduðunu bildir.
        }
    }

    // --- (ÝLERÝSÝ ÝÇÝN NOT) ---
    // Ýleride buraya "CanYenile()", "HasarHesapla()" gibi
    // bu istatistikleri kullanan metotlar da ekleyebiliriz.
}