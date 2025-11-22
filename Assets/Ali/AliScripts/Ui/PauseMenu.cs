using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   
    public GameObject pauseMenuUI;
    public static bool GameIsPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume(); // Eðer zaten duraklatýlmýþsa, devam et.
            }
            else
            {
                Pause(); // Duraklatýlmamýþsa, duraklat.
            }
        }
    }

    // --- BUTON FONKSÝYONLARI ---

    // 1. RESUME (Devam Et)
    // 1. RESUME (Devam Et)
    public void Resume()
    {
        // 1. UI Panelini kapat.
        pauseMenuUI.SetActive(false);

        // 2. Oyun zamanýný normal akýþýna geri döndür.
        Time.timeScale = 1f;

        // 3. Oyun durumunu güncelle.
        GameIsPaused = false;

        // 4. FARE ÝMLECÝ YÖNETÝMÝ: 
        // Oyun devam ederken imleci gizle ve kitle (CursorLockMode.Locked).
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    // 2. PAUSE (Duraklat)
    void Pause()
    {
        // 1. UI Panelini aç.
        pauseMenuUI.SetActive(true);

        // 2. Oyun zamanýný durdur (0'a ayarla).
        Time.timeScale = 0f;

        // 3. Oyun durumunu güncelle.
        GameIsPaused = true;

        // 4. FARE ÝMLECÝ YÖNETÝMÝ: 
        // Menü açýkken imleci göster (CursorLockMode.None).
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 3. RESET (Yeniden Baþlat)
    public void ResetGame()
    {
        // NOT: SceneManager'ý kullanmak için yukarýda using UnityEngine.SceneManagement; eklediðinizden emin olun.

        // 1. Oyunun normale döndüðünden emin olun.
        Time.timeScale = 1f;
        GameIsPaused = false;

        // 2. Mevcut sahneyi yeniden yükle.
        // SceneManager.GetActiveScene().buildIndex, o anki açýk olan sahnenin numarasýný alýr.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
      
       

    }
    public void LoadSettingsMenu()
    {
        Time.timeScale = 1f;
       

    }
}
