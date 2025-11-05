using UnityEngine;
using UnityEngine.SceneManagement; 
public class SahneYoneticisi : MonoBehaviour
{
    
    public void OyunuBaslat()
    {
      
        SceneManager.LoadScene("Oyun");
    }

    
    public void AyarlariAc()
    {
       
        SceneManager.LoadScene("Ayarlar");
    }

   
    public void OyundanCik()
    {
       
        Debug.Log("Oyundan çýkma komutu çalýþtý!");

       
        Application.Quit();
    }
}