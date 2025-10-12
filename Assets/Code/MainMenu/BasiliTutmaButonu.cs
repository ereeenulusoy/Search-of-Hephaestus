using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BasiliTutmaButonu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image dolumBari;
    public float basiliTutmaSuresi = 10.0f;

    private float basiliTutmaSayaci = 0f;
    private bool mouseBasiliyor = false; 


    public void OnPointerDown(PointerEventData eventData)
    {
        mouseBasiliyor = true;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        mouseBasiliyor = false;
    }


    void Update()
    {

        bool fTusuBasili = Input.GetKey(KeyCode.F);


        if (mouseBasiliyor || fTusuBasili)
        {

            basiliTutmaSayaci += Time.deltaTime;


            dolumBari.fillAmount = basiliTutmaSayaci / basiliTutmaSuresi;

            if (basiliTutmaSayaci >= basiliTutmaSuresi)
            {

                enabled = false;


                SceneManager.LoadScene("MainMenu");
            }
        }
        else 
        {

            if (basiliTutmaSayaci > 0)
            {
                basiliTutmaSayaci = 0f;
                dolumBari.fillAmount = 0f;
            }
        }
    }
}