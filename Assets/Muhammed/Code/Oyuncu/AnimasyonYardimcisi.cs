using UnityEngine;

// Bu scriptin tek görevi, Animasyon Olayý tetiklendiðinde ana kontrol scriptine haber vermek.
public class AnimasyonYardimcisi : MonoBehaviour
{
    // Ana kontrol scriptimize ulaþmak için bir referans
    private SecimPaneliKontrol secimPaneliKontrol;

    void Start()
    {
        // Oyuna baþlarken ArayuzYoneticisi üzerindeki SecimPaneliKontrol scriptini bul ve sakla.
        secimPaneliKontrol = FindObjectOfType<SecimPaneliKontrol>();
    }

    // Animasyon Olayý BU FONKSÝYONU ÇAÐIRACAK
    public void AnimasyonBittiHaberiVer()
    {
        // Eðer ana scripti bulduysak...
        if (secimPaneliKontrol != null)
        {
            // Ana scriptin içindeki asýl fonksiyonu çaðýr!
            secimPaneliKontrol.AnimasyonBittiSecenekleriGoster();
        }
        else
        {
            Debug.LogError("SecimPaneliKontrol bulunamadý!");
        }
    }
}