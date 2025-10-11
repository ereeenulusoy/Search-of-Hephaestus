using UnityEngine;
using UnityEngine.EventSystems;


public class MenuAnimasyonTetikleyici : MonoBehaviour, IPointerEnterHandler
{
    public Animator menuAnimator;


    private bool animasyonOynatildi = false;


    public void OnPointerEnter(PointerEventData eventData)
    {

        if (menuAnimator != null && !animasyonOynatildi)
        {

            menuAnimator.SetTrigger("Baslat");


            animasyonOynatildi = true;
        }
    }
}