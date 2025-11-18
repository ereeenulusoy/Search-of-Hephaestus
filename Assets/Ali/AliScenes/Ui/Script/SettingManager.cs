using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Button ve Toggle için
using System.Collections; // Gerekirse Coroutine için

public class SettingManager : MonoBehaviour
{
    // Kategori Panelleri
    public GameObject displayPanel;
    public GameObject keyboardPanel;
    public GameObject soundsPanel;
    public void OpenPanel(GameObject panelToOpen)
    {
        
        displayPanel.SetActive(false);
        keyboardPanel.SetActive(false);
        soundsPanel.SetActive(false);

       
        panelToOpen.SetActive(true);
    }

    // Hiyerarþi'deki butonlara baðlanacak metotlar:
    public void OpenDisplaySettings() => OpenPanel(displayPanel);
    public void OpenKeyboardSettings() => OpenPanel(keyboardPanel);
    public void OpenSoundSettings() => OpenPanel(soundsPanel);

 
}