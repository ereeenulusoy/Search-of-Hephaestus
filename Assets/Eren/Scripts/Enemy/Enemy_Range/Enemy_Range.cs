using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Range : Enemy

{
    [SerializeField] private Transform weaponHolder;

    public float fireRate = 0.7f; //Bullets Per Second
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float bulletSpeed = 20f;


    public IdleState_Range idleState {  get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public DeadState_Range deadState { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine,"Battle");
        deadState = new DeadState_Range(this, stateMachine, "Idle"); //Idle is a placeholder.
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void GetHit()
    {
        base.GetHit();
        if(healthPoints <= 0 && stateMachine.currentState != deadState)
          stateMachine.ChangeState(deadState);
    }
    public override void AnimationSpecialAttackTrigger()
    {
        base.AnimationSpecialAttackTrigger();

        FireSingleBullet(); 
    }

    public void TriggerFire()
    {
        anim.SetTrigger("Shoot");

    }

    public void FireSingleBullet()
    {
        Vector3 bulletDirection = ((player.position + Vector3.up) - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab, gunPoint);
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Enemy_Bullet>().BulletSetup();

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = 20 / bulletSpeed;
        rbNewBullet.velocity = bulletDirection * bulletSpeed; // bu iki formül momentum'un her zaman 20'ye sabit olmasýný saðlar.
    }
    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;
        base.EnterBattleMode();
        stateMachine.ChangeState(battleState);
    }
}
