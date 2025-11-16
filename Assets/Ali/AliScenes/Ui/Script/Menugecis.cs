using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menugecis : MonoBehaviour
{
    public void SahneYukle(string sahneAdi)
    {
        // Eðer boþ bir isim verilirse yükleme yapma
        if (!string.IsNullOrEmpty(sahneAdi))
        {
            SceneManager.LoadScene(sahneAdi);
        }
    }
}
