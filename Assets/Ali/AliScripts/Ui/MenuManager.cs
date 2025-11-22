using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class MenuManager : MonoBehaviour
{
    public void SahneYukle(string sahneAdi)
    {
        // Eğer boş bir isim verilirse yükleme yapma
        if (!string.IsNullOrEmpty(sahneAdi))
        {
            SceneManager.LoadScene(sahneAdi);
        }
    }
    public GameObject CikisOnayPaneli;
    [Header("UI Kontrol")]
    public CanvasGroup[] MenuButonCanvasGruplari;
    public void QuitButonunaBasildi()
    {
        if (CikisOnayPaneli != null)
        {
            CikisOnayPaneli.SetActive(true);

            // ⭐️ DİZİ ÜZERİNDE DÖNGÜ: Tüm buton gruplarını kilitle
            foreach (CanvasGroup cg in MenuButonCanvasGruplari)
            {
                if (cg != null)
                {
                    cg.interactable = false;
                    cg.blocksRaycasts = false;
                }
            }
        }
    }
    public void OyundanCikisOnaylandi()
    {
    
        Application.Quit();

     
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void CikisIptalEdildi()
    {
        if (CikisOnayPaneli != null)
        {
            CikisOnayPaneli.SetActive(false);

            
            foreach (CanvasGroup cg in MenuButonCanvasGruplari)
            {
                if (cg != null)
                {
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                }
            }
        }
    }
    public ButtonScaler[] TumButonlar;
    public void ButonlariOdakla(ButtonScaler aktifButon)
    {
        foreach (ButtonScaler buton in TumButonlar)
        {
            if (buton == aktifButon)
            {
                buton.SetTargetScale(true);
            }
            else
            {
                buton.SetTargetScale(false);
            }
        }
    }
    public void ButonlariNormaleDondur()
    {
        foreach (ButtonScaler buton in TumButonlar)
        {
            
            buton.ResetTargetScale();
        }
    }
}
    