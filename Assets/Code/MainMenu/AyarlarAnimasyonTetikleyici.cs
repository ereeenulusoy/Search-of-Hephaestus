using UnityEngine;
using UnityEngine.EventSystems;


public class AyarlarAnimasyonTetikleyici : MonoBehaviour, IPointerEnterHandler
{
    public Animator ayalarAnimator;


    private bool animasyonOynatildi = false;


    public void OnPointerEnter(PointerEventData eventData)
    {

        if (ayalarAnimator != null && !animasyonOynatildi)
        {

            ayalarAnimator.SetTrigger("Baslat");


            animasyonOynatildi = true;
        }
    }
}