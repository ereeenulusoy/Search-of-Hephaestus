using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct MeleeAttackData
{
    public string attackName;
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;

    [Range(1,10)]
    public float animationSpeed;
    public AttackType_Melee attackType;
}

public enum AttackType_Melee { Close, Charge}
public enum EnemyMelee_Type {Regular , Shield, Dodge, Thrower}

public class Enemy_Melee : Enemy
{

    public Enemy_Visuals visuals {  get; private set; } 
    #region States
    public IdleState_Melee idleState {  get; private set; }
    public MoveState_Melee moveState { get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public DeadState_Melee deadState { get; private set; }
    public AbilityState_Melee abilityState { get; private set; }

    #endregion

    [Header("Enemy Settings")]
    public EnemyMelee_Type meleeType;
    [SerializeField] private Transform shieldTransform;
    public float dodgeCooldown;
    private float lastTimeDodge = -10;

    [Header("Axe Throw Ability")]
    public GameObject axePrefab;
    public float axeFlySpeed;
    public float axeTimer;
    public float axeThrowCooldown;
    private float lastTimeAxeThrown;
    public Transform axeStartPoint;

    [Header("Attack Data")]
    public MeleeAttackData attackData;
    public List<MeleeAttackData> attackList;


    protected override void Awake()
    {
        base.Awake();
        visuals = GetComponent<Enemy_Visuals>();
        idleState = new IdleState_Melee(this , stateMachine, "Idle");
        moveState = new MoveState_Melee(this , stateMachine , "Move");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        deadState = new DeadState_Melee(this , stateMachine, "Idle");//Idle anim is just placeholder.we use radgoll.
        abilityState = new AbilityState_Melee(this, stateMachine, "AxeThrow");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        InitializePerk();
        visuals.SetupLook();
        UpdateAttackData();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (ShouldEnterBattleMode())
            EnterBattleMode();
        

    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;
        base.EnterBattleMode();
        stateMachine.ChangeState(recoveryState);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        moveSpeed = moveSpeed * .45f;
        EnableWeaponModel(false);
    }

    public void UpdateAttackData()
    {
        Enemy_WeaponModel currentWeapon = visuals.currentWeaponModel.GetComponent<Enemy_WeaponModel>();

        if (currentWeapon != null)
        {
            attackList = new List<MeleeAttackData>(currentWeapon.weaponData.attackData);   
            turnSpeed = currentWeapon.weaponData.turnSpeed; 
        }
    }
    private void InitializePerk()
    {
        if (meleeType == EnemyMelee_Type.Thrower)
        {
            visuals.SetupWeaponType(Enemy_MeleeWeaponType.Throw);
        }

        if (meleeType == EnemyMelee_Type.Shield)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
            visuals.SetupWeaponType(Enemy_MeleeWeaponType.OneHand);

        }
    }

    public override void GetHit()
    {
        base.GetHit();

        if(healthPoints <=0)
         stateMachine.ChangeState(deadState);
    }
    public void EnableWeaponModel(bool active)
    {   
       visuals.currentWeaponModel.gameObject.SetActive(active);
    }
    public void ActivateDodgeRoll()
    {
        if (meleeType != EnemyMelee_Type.Dodge)
            return;

        if (stateMachine.currentState != chaseState)
            return;

        if (Vector3.Distance(transform.position, player.position) < 2.25f)
            return;

        float dodgeAnimationDuration = GetAnimationClipDuration("Dodge Roll");


        if (Time.time > lastTimeDodge + dodgeCooldown + dodgeAnimationDuration)
        {
            lastTimeDodge = Time.time;
            anim.SetTrigger("Dodge");
        }
    }

    public bool CanThrowAxe()
    {
        if(meleeType != EnemyMelee_Type.Thrower)
           return false;

        if (Time.time < lastTimeAxeThrown + axeThrowCooldown)
        {
           lastTimeAxeThrown = Time.time;
           return true;
        }
        return false;
    }

    private float GetAnimationClipDuration(string clipName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if(clip.name == clipName)
                return clip.length;
        }

        Debug.Log(clipName + "named animation couldn't found..");
        return 0;
    }
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackData.attackRange;
    protected override void OnDrawGizmos()
    {
       base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }
}
