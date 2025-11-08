using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    private Rigidbody rb => GetComponent<Rigidbody>();
    [SerializeField] private GameObject bulletImpactVfx;

 
    private void OnCollisionEnter(Collision collision)
    {
        CreateBulletImpactFX(collision);
        Destroy(gameObject);
    }

    private void CreateBulletImpactFX(Collision collision)
    {
        if (collision.contacts.Length > 0)// çarpýlan yüzey sayýsý 0'dan büyükse
        {
            ContactPoint contact = collision.contacts[0];//List yapýyor.
            GameObject newbulletImpactFx = Instantiate(bulletImpactVfx, contact.point, Quaternion.LookRotation(contact.normal));

            Destroy(newbulletImpactFx,1f);
        }
    }
}
