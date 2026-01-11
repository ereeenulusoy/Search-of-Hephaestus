using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] int poolSize = 30;


    private Dictionary<GameObject, Queue<GameObject>> poolDictionary =
        new Dictionary<GameObject, Queue<GameObject>>();
    
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    public GameObject GetObject(GameObject prefab, Transform target)
    {
        if (poolDictionary.ContainsKey(prefab) == false)//bu tipte hiçbir key yoksa[Hiç kayýt yoksa] queue(value) oluþtur.
        {
            InitializeNewPool(prefab); 
        }
            
        if (poolDictionary[prefab].Count == 0) //Tüm objeler kullanýlýyorsa yenisini oluþtur.
        {
            CreateNewObject(prefab);
        }

        GameObject objectToGet = poolDictionary[prefab].Dequeue();

        objectToGet.transform.position = target.position;
        objectToGet.transform.parent = null;
        objectToGet.SetActive(true);
        return objectToGet;
    }


    public void ReturnObject(GameObject objectToReturn, float delay = .001f)
                     => StartCoroutine(DelayReturn(delay, objectToReturn));

    private IEnumerator DelayReturn(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool(objectToReturn);
    }
    private void ReturnToPool(GameObject objectToReturn)
    {
        GameObject originalPrefab = objectToReturn.GetComponent<PooledObject>().originalPrefab;
        //CreateNewObject kýsmý burada çalýþmaya baþlýyor.Bu yeni obje artýk orijinal bir obje.
        objectToReturn.SetActive(false);
       
        objectToReturn.transform.parent = transform;

        poolDictionary[originalPrefab].Enqueue(objectToReturn);

    }

    private void InitializeNewPool(GameObject prefab)
    {

        poolDictionary[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; ++i)
        {
            CreateNewObject(prefab);

        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);// CLONE YARATILDI.
        newObject.AddComponent<PooledObject>().originalPrefab = prefab;
        // YARATILAN OBJEDEKÝ SCRÝPTTE ORIGINALPREFAB'I TUTAN YER VAR. BURANIN ÝÇÝNE ORÝJÝNAL PREFAB'Ý ATICAZ.

        // BUNU YAPMA NEDENÝMÝZ OLUÞTURULAN PREFAB ORÝJÝNAL OLARAK GEÇMÝYOR. BU YÜZDEN DE KEY OLARAK KULLANILMIYOR.

        // BU DA OBJECT POOL OLARAK TEKRAR KULLANILAMAMASINA NEDEN OLUR. ReturnToPool'da kullaným amacý anlaþýlacak!!

        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);//object pool yapýlan hangi objeyse onun sýrasýna ekler.
    }
}
