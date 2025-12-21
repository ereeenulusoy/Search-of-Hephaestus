using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject uiPanel; // Açýlacak olan Panel'i buraya sürükleyin
    public bool cursorAcilsinMi = true; // Panel açýlýnca mouse görünsün mü?

    // Oyuncu alana girdiðinde çalýþýr
    private void OnTriggerEnter(Collider other)
    {
        // Çarpan objenin etiketi "Player" mý?
        if (other.CompareTag("Player"))
        {
            uiPanel.SetActive(true); // Paneli aç

            if (cursorAcilsinMi)
            {
                Cursor.lockState = CursorLockMode.None; // Mouse'u serbest býrak
                Cursor.visible = true; // Mouse'u görünür yap
            }
        }
    }

    // Oyuncu alandan çýktýðýnda çalýþýr
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiPanel.SetActive(false); // Paneli kapat

            if (cursorAcilsinMi)
            {
                // Oyuna geri dönüldüðü için mouse'u tekrar kilitle (FPS oyunlarý için)
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}