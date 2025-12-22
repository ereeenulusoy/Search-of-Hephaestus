using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private Transform corridorPrefab;

    [SerializeField] private List<Transform> levelParts;
    private List<Transform> currentLevelParts;
    [SerializeField] private List<Transform> generatedLevelPart;
    [SerializeField] private SnapPoint nextSnapPoint;
    [SerializeField] private int maxAllowedSequencedCorridor;
    private SnapPoint defaultSnappoint;

    [Space]
    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;

    private int sequencedCorridorCount;

    // Art arda aynı prefab'ı engellemek için son seçilen prefab
    private Transform lastChosenPrefab;

    [Header("Navmesh")]//Eren
    [SerializeField] private NavMeshSurface navMeshSurface;
    private List<Enemy> spawnedEnemies = new List<Enemy>();

    private void Start()
    {
        spawnedEnemies.Clear(); //Eren
        defaultSnappoint = nextSnapPoint;
        InitializeGeneration();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            InitializeGeneration();
        }

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

    private void RegisterEnemiesInPart(Transform levelPart)
    {
        // Odanın içindeki tüm Enemy scriptine sahip robotları bul (kapalı olsalar bile)
        Enemy[] enemiesInPart = levelPart.GetComponentsInChildren<Enemy>(true);
        foreach (Enemy e in enemiesInPart)
        {
            spawnedEnemies.Add(e);
        }
    }

    [ContextMenu("Restart Generation")]
    private void InitializeGeneration()
    {
        StopAllCoroutines();
        sequencedCorridorCount = 0;

        nextSnapPoint = defaultSnappoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);

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
        spawnedEnemies.Clear();
    }

    private void FinishGeneration()
    {
        generationOver = true;

        Transform levelPart = Instantiate(lastLevelPart);

        generatedLevelPart.Add(levelPart);

        RegisterEnemiesInPart(levelPart);
        levelPart = DetectCollision(levelPart,true);

        if (levelPart != null)
        {
            StartCoroutine(FinalizeLevel());
        }
    }

    private IEnumerator FinalizeLevel()
    {
        // 1. NavMesh'i çalışma anında pişir (Baking)
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }

        yield return new WaitForSeconds(0.1f); // NavMesh'in oturması için milisaniyelik bekleme

        // 2. Robotları uyandır ve yerlerine mıhla
        foreach (Enemy enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                // Damage sistemi transform.root kontrolü yapıyorsa unparent etmek iyidir
                enemy.transform.parent = null;

                enemy.gameObject.SetActive(true); // Robotu aç

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = true;
                    // KRİTİK: Robotu olduğu yere ışınlanmaya zorla, başlangıca kaçmasın!
                    agent.Warp(enemy.transform.position);
                }
            }
        }
    }

    [ContextMenu("Create next level part")]
    private void GenerateNextLevelPart()
    {
        Transform chosenPrefab = ChooseRandomPart();

        if (!generationOver)
        {
            lastChosenPrefab = chosenPrefab;
        }

        Transform newPart = null;
        if (generationOver)
        {
            return;
        }
        else
        {
            newPart = Instantiate(chosenPrefab);
            RegisterEnemiesInPart(newPart);
            newPart = DetectCollision(newPart);

            if (newPart == null)
            {
                return;
            }
        }

        generatedLevelPart.Add(newPart);

        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();

        // Yerleştirme ve Hizalama
        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        // Bir sonraki noktayı ayarla
        nextSnapPoint = levelPartScript.GetExitPoint();
        levelPartScript.CloseOtherOneWithCap(nextSnapPoint);
    }
    private Transform DetectCollision(Transform newPart, bool lastPart = false)
    {
        LevelPart part = newPart.GetComponent<LevelPart>();
        part.SnapAndAlignPartTo(nextSnapPoint);

        Physics.SyncTransforms();
        bool collision = part.DetectCollision();

        if (collision && !lastPart)
        {
            currentLevelParts.Add(lastChosenPrefab);
            newPart.gameObject.SetActive(false);
            Destroy(newPart.gameObject);

            if (sequencedCorridorCount >= maxAllowedSequencedCorridor)
            {
                sequencedCorridorCount = 0;
                InitializeGeneration();
                return null;
            }

            newPart = Instantiate(corridorPrefab);

            LevelPart newLevelPart = newPart.GetComponent<LevelPart>();
            newLevelPart.SnapAndAlignPartTo(nextSnapPoint);

            if (newLevelPart.DetectCollision())
            {
                sequencedCorridorCount = 0;
                generatedLevelPart.Add(newPart);
                InitializeGeneration();
                return null;
            }
            else
            {
                sequencedCorridorCount++;
            }
        }
        else if (collision && lastPart)
        {
            sequencedCorridorCount = 0;
            generatedLevelPart.Add(newPart);
            InitializeGeneration();
            return null;
        }
        else if (!collision)
        {
            sequencedCorridorCount = 0;
        }
        return newPart;
    }

    // Art arda aynı prefab'ı engelleme mantığı
    private Transform ChooseRandomPart()
    {
        Transform choosenPart = null;

        // Son kullanılan hariç, kullanılabilir parçaların geçici listesi
        List<Transform> availableParts = new List<Transform>(currentLevelParts);

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
}