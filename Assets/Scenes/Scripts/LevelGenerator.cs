using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Unity Inspector'da ayarlanacak seviye parçaları listesi
    [SerializeField] private List<Transform> LevelParts = new List<Transform>();

    // Bir sonraki parçanın bağlanacağı mevcut bitiş noktası
    [SerializeField] private Transform NextSnapPoint;

    // Başlangıçta seviye parçalarının doğru şekilde oluşturulması için Start metodu
    private void Start()
    {
        // EĞER NEXTSNAPPOINT TANIMLI DEĞİLSE İLK PARÇAYI OLUŞTUR
        if (NextSnapPoint == null && LevelParts.Count > 0)
        {
            // İlk parçayı (0,0,0)'da oluştur
            Transform initialPart = Instantiate(ChooseRandomPart());

            // İlk parçanın çıkışını bir sonraki parça için ayarla
            NextSnapPoint = GetSnapPointOfType(initialPart, SnapPointType.Exit);
        }

        // YALNIZCA BİR SONRAKİ PARÇAYI OLUŞTUR
        // Not: Bu satır, NextSnapPoint'i ilk parçanın çıkışına ayarladıktan sonra 
        // çalıştırılırsa, ikinci bir parça oluşturur.
        GenerateNextLevelPart();

        // Eğer sadece tek bir parçanın (yani ilk parçanın) oluşmasını istiyorsanız, 
        // GenerateNextLevelPart() çağrısını tamamen silebilirsiniz.
    }

    [ContextMenu("Create next level part")]
    private void GenerateNextLevelPart()
    {
        if (NextSnapPoint == null)
        {
            Debug.LogError("NextSnapPoint ayarlanmamış! Lütfen ilk parçanın çıkışını ayarlayın.");
            return;
        }

        // 1. Rastgele bir seviye parçası oluştur
        Transform newPart = Instantiate(ChooseRandomPart());

        // 2. Yeni parçanın "Giriş" SnapPoint'ini bul
        Transform newPartEnterSnap = GetSnapPointOfType(newPart, SnapPointType.Enter);

        if (newPartEnterSnap == null)
        {
            Debug.LogError("Yeni parçada 'Enter' SnapPoint bulunamadı! Parça: " + newPart.name);
            Destroy(newPart.gameObject);
            return;
        }

        // 3. Hizalama (Alignment) hesaplaması:
        //    Yeni parçayı ne kadar hareket ettirmemiz gerektiğini bul.
        //    Hedef Konum (NextSnapPoint) - Kaynak Konum (newPartEnterSnap)
        Vector3 offset = NextSnapPoint.position - newPartEnterSnap.position;

        // 4. Yeni parçayı doğru konuma taşı (rotasyonu koru)
        newPart.position += offset;

        // 5. Bir sonraki parça için NextSnapPoint'i güncelle:
        //    Yeni parçanın "Çıkış" SnapPoint'ini bul ve NextSnapPoint'e ata.
        NextSnapPoint = GetSnapPointOfType(newPart, SnapPointType.Exit);

        if (NextSnapPoint == null)
        {
            Debug.LogError("Yeni parçada 'Exit' SnapPoint bulunamadı! Seviye oluşturma durduruldu.");
        }
    }

    // Seçim ve kaldırma mantığı
    private Transform ChooseRandomPart()
    {
        if (LevelParts.Count == 0) return null;

        int randomIndex = Random.Range(0, LevelParts.Count);
        Transform choosePart = LevelParts[randomIndex];

        // Parçaların tekrar kullanılabilmesi için bu satırı yorumlayabilir veya kaldırabilirsiniz
        // LevelParts.RemoveAt(randomIndex); 

        return choosePart;
    }

    // Bir parçanın içindeki belirli bir türdeki SnapPoint'i bulan yardımcı metot
    private Transform GetSnapPointOfType(Transform part, SnapPointType type)
    {
        // Parçanın içindeki SnapPoint bileşenlerini tara
        SnapPoint[] snapPoints = part.GetComponentsInChildren<SnapPoint>();

        foreach (SnapPoint sp in snapPoints)
        {
            if (sp.pointType == type)
            {
                // SnapPoint'in kendi Transform'unu döndür
                return sp.transform;
            }
        }
        return null;
    }
}