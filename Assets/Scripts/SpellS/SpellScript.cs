using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    public Transform MyTarget { get; private set; }
    private Transform source;
    private int damage;
    private bool alive = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    public void Initialize(Transform target, int damage, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
    }
    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position;
            rb.velocity = direction.normalized * speed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "hitbox" && collision.transform == MyTarget)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage,source);
            GetComponent<Animator>().SetTrigger("Impact");
            rb.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}
