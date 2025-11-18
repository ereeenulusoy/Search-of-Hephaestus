using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    public float buyumeCarpani = 1.2f;
    public float kuculmeCarpani = 0.9f;
    public float gecisHizi = 10f;
    private Vector3 mevcutHedefBoyut;
    private Vector3 baslangicBoyut;
    private RectTransform rectTransform;
    public MenuManager menuManager;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        baslangicBoyut = rectTransform.localScale;
        mevcutHedefBoyut = baslangicBoyut;
        rectTransform.localScale = baslangicBoyut;
    }
    void Update()
    {
        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            mevcutHedefBoyut,
            Time.deltaTime * gecisHizi
        );
    }
    public void SetTargetScale(bool shouldGrow)
    {
        if (shouldGrow)
        {
            
            mevcutHedefBoyut = baslangicBoyut * buyumeCarpani;
        }
        else
        {
            mevcutHedefBoyut = baslangicBoyut * kuculmeCarpani;
        }
    }
    public void ResetTargetScale()
    {
        mevcutHedefBoyut = baslangicBoyut;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (menuManager != null)
        {
            menuManager.ButonlariOdakla(this);
        }
    }
  public void OnPointerExit(PointerEventData eventData)
    {
        if (menuManager != null)
        {
            menuManager.ButonlariNormaleDondur();
        }
    }
}