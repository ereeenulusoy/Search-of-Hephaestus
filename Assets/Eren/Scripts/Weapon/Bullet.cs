using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float impactForce;

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    // Opsiyonel Görseller (Biri olabilir, ikisi de olabilir)
    private TrailRenderer trailRenderer;
    private ParticleSystem ps;

    [SerializeField] private GameObject bulletImpactVfx;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    protected virtual void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Varsa bileþenleri al, yoksa null kalýr
        trailRenderer = GetComponent<TrailRenderer>();
        ps = GetComponentInChildren<ParticleSystem>(); // VFX genelde child objede olur
    }

    public void BulletSetup(float flyDistance = 100, float impactForce = 100)
    {
        this.impactForce = impactForce;

        bulletDisabled = false;

        if (cd != null) cd.enabled = true;
        if (meshRenderer != null) meshRenderer.enabled = true;

        // Trail varsa sýfýrla
        if (trailRenderer != null)
        {
            trailRenderer.Clear();
            trailRenderer.time = 0.2f;
        }

        // Particle System varsa sýfýrla ve oynat
        if (ps != null)
        {
            ps.Clear(); // Eskileri sil
            ps.Play();  // Yeniden baþlat
        }

        startPosition = transform.position;
        this.flyDistance = flyDistance;
    }

    protected virtual void Update()
    {
        FadeTrails();
        DisableBullet();
        ReturnBulletsToPool();
    }

    // Enemy_Bullet tarafýndan ezilebilsin diye 'virtual' yaptýk
    protected void ReturnBulletsToPool()
    {
        // Mermi görevini bitirdiyse (menzili aþtýysa)...
        if (bulletDisabled)
        {
            // 1. Trail varsa ve sönmediyse bekle
            if (trailRenderer != null && trailRenderer.time > 0) return;

            // 2. Alev topu varsa ve sönmediyse bekle (IsAlive true ise yaþýyordur)
            if (ps != null && ps.IsAlive(true)) return;

            // Hepsi bittiyse havuza dön
            ReturnBulletToPool();
        }
    }

    protected void DisableBullet()
    {
        // Menzil kontrolü
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            if (cd != null) cd.enabled = false;
            if (meshRenderer != null) meshRenderer.enabled = false;

            // Alev topu varsa yeni üretim yapmasýn ama var olanlar süzülsün
            if (ps != null) ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            bulletDisabled = true;
        }
    }

    protected void FadeTrails()
    {
        // Sadece TrailRenderer varsa çalýþýr
        if (trailRenderer != null)
        {
            if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
                trailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX();
        ReturnBulletToPool(); // Çarpýnca hemen dön

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        Enemy_Shield shield = collision.gameObject.GetComponent<Enemy_Shield>();

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

    protected void ReturnBulletToPool() => ObjectPool.instance.ReturnObject(gameObject);

    protected void CreateImpactFX()
    {
        if (bulletImpactVfx != null)
        {
            GameObject newImpactFx = ObjectPool.instance.GetObject(bulletImpactVfx, transform);
            ObjectPool.instance.ReturnObject(newImpactFx, 1);
        }
    }
}