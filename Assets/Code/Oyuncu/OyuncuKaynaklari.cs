using UnityEngine;

public class OyuncuKaynaklari : MonoBehaviour
{
    public static OyuncuKaynaklari instance;

    public int para = 100;


    public int canSeviyesi = 1;


    public bool lazerBoomAtakGuncellemesiAlindi = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ParaHarca(int miktar)
    {
        if (para >= miktar)
        {
            para -= miktar;
            return true;
        }
        return false;
    }
}