using UnityEngine;

// Oyuncunun TÜM verilerini tutan ana "beyin".
public class OyuncuKaynaklari : MonoBehaviour
{
    // --- Singleton Kurulumu ---
    public static OyuncuKaynaklari instance;

    [Header("Ana Kaynaklar")]
    public int para = 100;

    // --- Durdurma Menüsü İstatistikleri ---
    [Header("Oyuncu İstatistikleri")]
    public float maksimumCan = 100;
    public float canYenileme = 0;
    public float zirh = 0;
    public float kacınmaSansi = 0.0f;
    public float canCalma = 0.0f;
    public float hasarBonus = 1.0f;
    public float kritikSans = 0.01f;
    public float kritikHasar = 1.5f;
    public int mermiSayisiBonus = 0;
    public float mermiHiziBonus = 1.0f;
    public float mermiToplamaArtisi = 1.0f;
    // ... (Diğer istatistikler buraya eklenebilir) ...

    // --- Gelişim Paneli & Envanter Verileri ---
    [Header("Gelişim ve Envanter")]
    public bool lazerBoomSahipMi = false;
    // Dizilerin boyutunu burada tanımlamak önemli!
    public bool[] lazerBoomGelistirmeleriAlindi = new bool[4];
    public bool[] ozellikGelistirmeleriAlindi = new bool[4];

    void Awake()
    {
        // Singleton Kontrolü
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // --- DİZİLERİ BAŞLATMA (ÖNEMLİ!) ---
        // Dizilerin null olmadığından emin olalım.
        // Boyutları Inspector'dan ayarlanacağı için genellikle gerekmez,
        // ama garanti olması için eklenebilir.
        // if (lazerBoomGelistirmeleriAlindi == null || lazerBoomGelistirmeleriAlindi.Length != 4)
        //     lazerBoomGelistirmeleriAlindi = new bool[4];
        // if (ozellikGelistirmeleriAlindi == null || ozellikGelistirmeleriAlindi.Length != 4)
        //     ozellikGelistirmeleriAlindi = new bool[4];
    }

    // --- PARA KONTROL METODU ---
    public bool ParaHarca(int miktar)
    {
        if (para >= miktar)
        {
            para -= miktar;
            return true;
        }
        return false;
    }
}