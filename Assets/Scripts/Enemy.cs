using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup;
    [SerializeField] private float initialAggroRange;

    private IState currentState;
    public float MyAttackRange { get; set; }
    public float MyAttackTime{ get; set; }
    public Vector3 MyStartPosition { get; set; }
    public float MyAggroRange { get; set; }
    public bool InRange { get { return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange; } }

    public override Transform Select()
    {
        healthGroup.alpha = 1;
        return base.Select();
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;
        base.DeSelect();
    }

    public override void TakeDamage(float damage, Transform source)
    {
        if(!(currentState is EvadeState))
        {
            SetTarget(source);
            base.TakeDamage(damage, source);
            OnHealthChanged(health.MyCurrentValue);
        }
    }
    protected override void Awake()
    {
        MyStartPosition = transform.position;
        MyAggroRange = initialAggroRange;
        MyAttackRange = 1;
        ChangeState(new IdleState());
        base.Awake();
    }
    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }
            currentState.Update();
        }
        base.Update();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initialAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initialAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }
}
