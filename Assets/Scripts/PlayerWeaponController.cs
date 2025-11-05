using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform aim;
    private void Start()
    {
        player = GetComponent<Player>();
        player.controls.Character.Fire.performed += context => Shoot();
    }
    private void Update()
    {
        if (player.movement.isStillDashing)
            return;
        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);
    }
    private void Shoot()
    {
        if (player.movement.isStillDashing)
            return;
        
        
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        // yeni bir bullet oluþtururken prefabteki bulleti , nerede doðacaðýný ve hangi yönde doðacaðýný seçiyoruz.
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.velocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 5f);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    public Vector3 BulletDirection()
    {
        Vector3 direction = (aim.position - gunPoint.position).normalized;
       
        if(!player.aim.GetIsAimPricesly() && player.aim.Target() == null)
        direction.y = 0;
        

        return direction;
    }

    public Transform GunPoint() => gunPoint;
    // lazer için Gunpointe ve Bullet Direction'a ihtiyacýmýz var. Bunlarý weapon'dan çekip aim'de kullanacaðýz.
   
}
