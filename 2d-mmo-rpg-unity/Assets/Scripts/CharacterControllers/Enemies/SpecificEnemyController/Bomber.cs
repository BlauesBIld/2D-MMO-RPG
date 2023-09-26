using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : EnemyBaseController
{
    public Animator animator;
    public string type = "Bomber";

    public int dieHash = Animator.StringToHash("attack");

    public override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    public override void Die()
    {
        animator.SetTrigger(dieHash);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
