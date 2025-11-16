using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // Image bileþenleri için gerekli
using TMPro;      // TextMeshPro (TMP) bileþenleri için gerekli (varsa)

[System.Serializable]
public class PowerUp
{
    // Özelliðin adý (örneðin, Hýz Artýþý, Güç Kalkaný)
    public string powerUpAdi;

    // Uygulanacak etki deðeri (örneðin, %10 hýz artýþý)
    public float etkiDegeri;

    // Özelliði temsil eden görsel (Sprite veya Texture)
    public Sprite icon;
}