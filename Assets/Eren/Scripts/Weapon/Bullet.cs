using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float impactForce;
    
    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    [SerializeField] private GameObject bulletImpactVfx;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled = false;

    private void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    public void BulletSetup(float flyDistance, float impactForce)
    {
        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        //trailRenderer.Clear(); LAZIM OLURSA BÝ BAK.
        trailRenderer.time = .2f;

        this.impactForce = impactForce;

        startPosition = transform.position;
        this.flyDistance = flyDistance;

    }

    private void Update()
    {
        FadeTrails();
        DisableBullet();
        ReturnBulletsToPool();

    }

    private void ReturnBulletsToPool()
    {
        if (trailRenderer.time <= 0)
            ReturnBulletToPool();
    }

    private void DisableBullet()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    private void FadeTrails()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
            trailRenderer.time -= 2 * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        ReturnBulletToPool();

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        EnemyShield shield = collision.gameObject.GetComponent<EnemyShield>();

        if (shield != null)
        {
            shield.ReduceDurability();
            return;
        }
        if (enemy != null)
        {
            Vector3 force = rb.velocity.normalized * impactForce;
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody;

            enemy.GetHit();
            enemy.DeathImpact(force, collision.contacts[0].point, hitRigidbody);
        }
        
    }

    private void ReturnBulletToPool() => ObjectPool.instance.ReturnObject(gameObject);

    private void CreateImpactFX(Collision collision)
    {
        if (collision.contacts.Length > 0)// çarpýlan yüzey sayýsý 0'dan büyükse
        {
            ContactPoint contact = collision.contacts[0];//List yapýyor.
            GameObject newImpactFx = ObjectPool.instance.GetObject(bulletImpactVfx);
               
            newImpactFx.transform.position = contact.point;
            newImpactFx.transform.rotation = Quaternion.LookRotation(contact.normal);

            ObjectPool.instance.ReturnObject(newImpactFx , 1);
        }
    }
}
