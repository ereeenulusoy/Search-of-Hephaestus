using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFFERENCED_BULLET_SPEED = 20f;
    private Player player;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;
    private Transform aim;
    private void Start()
    {
        player = GetComponent<Player>();
        player.controls.Character.Fire.performed += context => Shoot();
        aim = player.aim.Aim();
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
        
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
      
        rbNewBullet.mass = REFFERENCED_BULLET_SPEED / bulletSpeed;
        rbNewBullet.velocity = BulletDirection() * bulletSpeed;

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
    
   
}
