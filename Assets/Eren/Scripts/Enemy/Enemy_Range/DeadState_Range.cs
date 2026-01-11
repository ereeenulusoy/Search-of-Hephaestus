using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Range : EnemyState
{
    private Enemy_Range enemy;
    public DeadState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        enemy.ragdoll.RagdollActive(true);
    }

    public override void Update()
    {
        base.Update();
    }
}
