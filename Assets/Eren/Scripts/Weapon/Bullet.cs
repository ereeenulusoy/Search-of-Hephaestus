using UnityEngine;
using UnityEngine.XR;

public class Bullet : MonoBehaviour
{
    public float impactForce;
    private Rigidbody rb => GetComponent<Rigidbody>();
    [SerializeField] private GameObject bulletImpactVfx;


    public void BulletSetup(float impactForce)
    {
        this.impactForce = impactForce;
    }
 
    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            Vector3 force = rb.velocity.normalized * impactForce;
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody; 

            enemy.GetHit();
            enemy.HitImpact(force, collision.contacts[0].point, hitRigidbody);
        }
        CreateBulletImpactFX(collision);
        ObjectPool.instance.ReturnBullet(gameObject);
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
