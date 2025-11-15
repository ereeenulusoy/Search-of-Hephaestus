using UnityEngine;

public class ObjectFading : MonoBehaviour
{
    private float originalOppacity;
    private Material mat;
    public bool DoFade = false;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalOppacity = mat.color.a;
    }


    void Update()
    {
        if (DoFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }

    }
    private void FadeNow()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
            Mathf.Lerp(currentColor.a, 0.1f, 7 * Time.deltaTime));
        mat.color = smoothColor;
    }

    private void ResetFade()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
            Mathf.Lerp(currentColor.a, originalOppacity, 7 * Time.deltaTime));
        mat.color = smoothColor;
    }
}
