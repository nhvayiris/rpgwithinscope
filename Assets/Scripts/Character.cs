using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] protected Transform hitBox;
    [SerializeField] protected Stat health;
    [SerializeField] private float initialHealth;

    public Transform MyTarget { get; set; }
    public Stat MyHealth { get { return health; } }
    public Animator MyAnimator { get; set; }
    public bool IsAlive { get { return health.MyCurrentValue > 0; } }
    public bool IsAttacking { get; set; }

    private Vector2 direction;
    protected Coroutine attackRoutine;
    private Rigidbody2D rb;

    public bool IsMoving
    {
        get
        {
            return direction != Vector2.zero;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

    protected virtual void Start()
    {
        health.Initialize(initialHealth, initialHealth);
    }
    protected virtual void Awake()
    {
        MyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (IsAlive)
        {
            rb.velocity = Direction.normalized * speed;
        }
        
    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");
                MyAnimator.SetFloat("x", direction.x);
                MyAnimator.SetFloat("y", direction.y);
            }
            else if (IsAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
                ActivateLayer("IdleLayer");
            }
        }
        else
        {
            ActivateLayer("DeathLayer");
            Debug.Log("Character is in death animation");
        }
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName),1);
    }

    

    public virtual void TakeDamage(float damage, Transform source)
    {
        health.MyCurrentValue -= damage;
        if(health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            rb.velocity = Direction;
            MyAnimator.SetTrigger("die"); 
        }
    }
} 