using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    public GameObject endCapPrefab;

    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private List<Transform> levelParts;
    private List<Transform> currentLevelParts;
    [SerializeField] private List<Transform> generatedLevelPart;
    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnappoint;

    // EndCap için kalan SnapPoint'leri takip etme listesi
    private List<SnapPoint> activeSnapPoints;

    [Space]
    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;

    // Art arda aynı prefab'ı engellemek için son seçilen prefab
    private Transform lastChosenPrefab;

    private void Start()
    {
        defaultSnappoint = nextSnapPoint;
        InitializeGeneration();
    }

    private void Update()
    {
        if (generationOver)
            return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            if (currentLevelParts.Count > 0)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevelPart();
            }
            else if (generationOver == false)
            {
                FinishGeneration();
            }

        }
    }

    [ContextMenu("Restart Generation")]
    private void InitializeGeneration()
    {
        nextSnapPoint = defaultSnappoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);
        activeSnapPoints = new List<SnapPoint>();

        // Son seçilen prefab'ı sıfırla
        lastChosenPrefab = null;

        DestroyGeneratedParts();
    }

    private void DestroyGeneratedParts()
    {
        foreach (Transform t in generatedLevelPart)
        {
            Destroy(t.gameObject);
        }
        generatedLevelPart = new List<Transform>();
    }

    private void FinishGeneration()
    {
        generationOver = true;
        Transform levelPart = Instantiate(lastLevelPart);
        LevelPart levelPartScript = levelPart.GetComponent<LevelPart>();

        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        // Bitişten sonra EndCap temizliğini yap
        CloseOpenEnds();
    }


    [ContextMenu("Create next level part")]
    private void GenerateNextLevelPart()
    {
        Transform chosenPrefab = ChooseRandomPart();

        Transform newPart = null;
        if (generationOver)
            newPart = Instantiate(lastLevelPart);
        else
            newPart = Instantiate(chosenPrefab);

        generatedLevelPart.Add(newPart);

        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();

        // Yerleştirme ve Hizalama
        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        // **DUVAR KALDIRMA KODLARI BU NOKTADAN ÇIKARILMIŞTIR**

        // Çakışma Kontrolü
        if (levelPartScript.IntersectionDetected())
        {
            InitializeGeneration();
            return;
        }

        // Başarılı yerleştirme sonrası, son seçilen prefab'ı kaydet
        if (!generationOver)
        {
            lastChosenPrefab = chosenPrefab;
        }

        // SnapPoint Takibi ve İlerleme
        // Kullanılan SnapPoint'i listeden çıkar
        activeSnapPoints.Remove(nextSnapPoint);

        // Yeni parça üzerindeki tüm SnapPoint'leri listeye ekle.
        SnapPoint[] newPoints = newPart.GetComponentsInChildren<SnapPoint>();
        foreach (SnapPoint point in newPoints)
        {
            activeSnapPoints.Add(point);
        }

        // Bir sonraki noktayı ayarla
        nextSnapPoint = levelPartScript.GetExitPoint();
    }

    // Art arda aynı prefab'ı engelleme mantığı
    private Transform ChooseRandomPart()
    {
        Transform choosenPart = null;

        // Son kullanılan hariç, kullanılabilir parçaların geçici listesi
        List<Transform> availableParts = new List<Transform>(currentLevelParts);

        if (lastChosenPrefab != null)
        {
            // Eğer son kullanılan parça mevcutsa, onu seçim havuzundan geçici olarak çıkar.
            availableParts.Remove(lastChosenPrefab);
        }

        // Kalan parçalardan rastgele seçim yap
        if (availableParts.Count > 0)
        {
            int randomIndex = Random.Range(0, availableParts.Count);
            choosenPart = availableParts[randomIndex];

            // Orijinal stok listesinden kaldır (bu parça artık kullanıldı)
            currentLevelParts.Remove(choosenPart);
        }
        else if (currentLevelParts.Count > 0)
        {
            // Eğer çıkarma sonrası boş kaldıysa (çok nadir), kalan tek parçayı seçmek zorunda kalırız.
            choosenPart = currentLevelParts[0];
            currentLevelParts.RemoveAt(0);
        }

        return choosenPart;
    }

    // Açık kalan uçları EndCap ile kapatma mantığı
    public void CloseOpenEnds()
    {
        // Oluşturma bittiğinde kalan her SnapPoint'i EndCap ile kapatır.
        foreach (SnapPoint snapPoint in activeSnapPoints)
        {
            if (snapPoint != null)
            {
                GameObject endCap = Instantiate(endCapPrefab, snapPoint.transform.position, snapPoint.transform.rotation);
                endCap.transform.SetParent(this.transform);
                Destroy(snapPoint.gameObject);
            }
        }
        activeSnapPoints.Clear();
        Debug.Log("Harita oluşturma tamamlandı. Kalan tüm açık uçlar EndCap ile kapatıldı.");
    }
}