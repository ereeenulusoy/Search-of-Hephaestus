using UnityEngine;

// Bu script, oyuncunun tüm ilerleme verilerini tutar.
public class OyuncuKaynaklari : MonoBehaviour
{
    // Singleton deseni: Bu script'e her yerden "OyuncuKaynaklari.instance" yazarak kolayca erişiriz.
    public static OyuncuKaynaklari instance;

    // Oyuncunun ortak para birimi
    public int para = 100;

    // --- SİLAH SAHİPLİK DURUMU ---
    // Oyuncu başlangıçta LazerBoom silahına sahip değil (false).
    public bool lazerBoomSahipMi = false;

    // --- SİLAH YÜKSELTMELERİ ---
    // Silahın 4 adet geliştirme slotu var. Başlangıçta hiçbiri alınmamış (false).
    public bool[] lazerBoomGelistirmeleriAlindi = new bool[4];

    void Awake()
    {
        // Sahnede bu script'ten sadece bir tane olmasını sağlar.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişse bile bu obje silinmez, veriler kaybolmaz.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Para harcamak için kullanılan ana fonksiyon.
    public bool ParaHarca(int miktar)
    {
        // Eğer istenen miktarda veya daha fazla paramız varsa...
        if (para >= miktar)
        {
            para -= miktar; // Parayı düşür.
            return true;    // İşlemin başarılı olduğunu bildir.
        }
        // Eğer yeterli para yoksa...
        return false;   // İşlemin başarısız olduğunu bildir.
    }
}