using UnityEngine;
using UnityEngine.SceneManagement; // Sahne olaylarını yönetmek için

public class MusicManager : MonoBehaviour
{
    // ✨ Singleton Örneği
    public static MusicManager instance;

    // 🎶 Rastgele çalacak müzik parçaları
    public AudioClip[] backgroundMusic;

    private AudioSource audioSource;

    // 🚩 Durum Bayrağı: Ölüm sahnesi için müziğin duraklatılıp duraklatılmadığını tutar.
    private bool isPausedForDeathScene = false;

    private void Awake()
    {
        // ⭐ Singleton Kontrolü: Yalnızca bir örnek olmasına izin verir.
        if (instance == null)
        {
            instance = this;
            // Bu nesneyi sahne geçişlerinde yok etme.
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            // Sahne yükleme bittiğinde OnSceneLoaded metodunu çağıracak olay dinleyici ekle.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Zaten bir MusicManager varsa, yenisini yok et.
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Nesne yok edilmeden önce olay dinleyiciyi kaldır.
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void Start()
    {
        // Oyun başladığında rastgele müzik çalmaya başla.
        PlayRandomMusic();
    }

    void Update()
    {
        // 🔁 Müzik Döngüsü: Eğer bir parça bittiyse ve duraklatılmamışsa, yeni parça başlat.
        if (!audioSource.isPlaying && !isPausedForDeathScene)
        {
            PlayRandomMusic();
        }
    }

    // =================================================================================

    // Sahne yüklenmesi tamamlandığında çağrılan metot
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 💀 Ölüm Sahnesi Kontrolü
        if (scene.name == "Death")
        {
            StopMusicForDeath();
        }
        // 🏠 Ana Menü veya Oynanış Sahneleri Kontrolü
        else
        {
            // Eğer müzik durdurulmuşsa (Death sahnesinden gelindiğinde) devam ettir.
            if (isPausedForDeathScene)
            {
                ResumeMusic();
            }
            // NOT: Ana menüye kesintisiz geçiş yapılıyorsa müzik zaten çalmaya devam eder.
        }
    }

    // Rastgele bir müzik seçer ve çalar.
    public void PlayRandomMusic()
    {
        if (backgroundMusic.Length == 0 || audioSource == null) return;

        int randomIndex = Random.Range(0, backgroundMusic.Length);
        audioSource.clip = backgroundMusic[randomIndex];

        audioSource.Play();
    }

    // Ölüm sahnesine gidince müziği durdurur.
    public void StopMusicForDeath()
    {
        isPausedForDeathScene = true;
        audioSource.Stop();
    }

    // Ana menüye veya oynanışa dönünce müziği devam ettirir/başlatır.
    public void ResumeMusic()
    {
        isPausedForDeathScene = false; // Duraklatma durumunu sıfırla

        // Eğer şu anda müzik çalmıyorsa (yani durdurulmuşsa), yeni bir parça başlat.
        if (!audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }
}