using UnityEngine;
using UnityEngine.UI; // UI bileþenlerini kullanýyorsan bu gereklidir
using System.Collections; // Coroutine için gereklidir

public class UIMover : MonoBehaviour
{
    
    public Vector3 hedefKonum = new Vector3(100f, 200f, 0f);
    public float hareketSuresi = 1.0f;
    private RectTransform rectTransform; 
    private Vector3 baslangicKonumu; 
    void Start()
    {

        rectTransform = GetComponent<RectTransform>();

       
        baslangicKonumu = rectTransform.localPosition;

        
        StartCoroutine(YavasHareket(hedefKonum, hareketSuresi));
    }
    IEnumerator YavasHareket(Vector3 hedef, float sure)
    {
        float gecenZaman = 0;

        while (gecenZaman < sure)
        {
            gecenZaman += Time.deltaTime;

           
            float t = gecenZaman / sure;

            
          
            rectTransform.localPosition = Vector3.Lerp(baslangicKonumu, hedef, t);

            yield return null; 
        }

       
        rectTransform.localPosition = hedef;
    }
}