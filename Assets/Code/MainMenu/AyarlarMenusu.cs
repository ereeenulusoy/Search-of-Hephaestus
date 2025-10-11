using UnityEngine;
using UnityEngine.SceneManagement; 

public class AyarlarMenusu : MonoBehaviour
{
    public GameObject klavyePaneli;
    public GameObject sesPaneli;
    public GameObject goruntuPaneli;

    private void TumPanelleriKapat()
    {
        klavyePaneli.SetActive(false);
        sesPaneli.SetActive(false);
        goruntuPaneli.SetActive(false);
    }

    public void KlavyePaneliniAc()
    {
        TumPanelleriKapat();
        klavyePaneli.SetActive(true);
    }

    public void SesPaneliniAc()
    {
        TumPanelleriKapat();
        sesPaneli.SetActive(true);
    }

    public void GoruntuPaneliniAc()
    {
        TumPanelleriKapat();
        goruntuPaneli.SetActive(true);
    }

    public void PaneliKapat()
    {
        TumPanelleriKapat();
    }

    
    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("MainMenu");
    }
}