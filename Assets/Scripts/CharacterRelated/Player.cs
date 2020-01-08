using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Stat stamina;
    [SerializeField] private Stat aether;
    [SerializeField] private float maxAether;
    [SerializeField] private Transform[] exitPoints;
    [SerializeField] private Block[] blocks;
    
    private SpellBook spellBook;
    private int exitIndex = 2;
    
    private float initialStamina = 100;
    private float initialAether = 0;
    private Vector3 min, max;

    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
        stamina.Initialize(initialStamina, initialStamina);
        aether.Initialize(initialAether, maxAether);
        base.Start();
    }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
            Mathf.Clamp(transform.position.y, min.y, max.y),
            transform.position.z);
        base.Update();
    }

    private void GetInput()
    {
        Direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }

        // Debugging Key
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            stamina.MyCurrentValue -= 10;
            aether.MyCurrentValue += 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            stamina.MyCurrentValue += 10;
            aether.MyCurrentValue -= 10;
        }
        if (IsMoving)
        {
            StopAttack();
        }
    }

    public void Setlimit(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MyTarget;
        Spell newSpell = spellBook.CastSpell(spellIndex);
        IsAttacking = true;
        MyAnimator.SetBool("Attack", IsAttacking);
        yield return new WaitForSeconds(newSpell.MyCastTime); //test cast time for debugging
        if (MyTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget, newSpell.MyDamage, transform);
        }
        StopAttack();
    }

    public void CastSpell(int spellIndex)
    {
        Block();
        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !(IsAttacking && IsMoving) && InLineOfSight())  ///rw
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
        } 
    }

    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector2 targetDirection = (MyTarget.transform.position - transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), LayerMask.GetMask("Block"));
            if (hit.collider == null)
            {
                return true;
            }
        }
        return false;
    }

    private void Block()
    {
        foreach(Block b in blocks)
        {
            b.Deactivate();
        }
        blocks[exitIndex].Activate();
    }

    public void StopAttack()
    {
        spellBook.StopCasting();

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            IsAttacking = false;
            MyAnimator.SetBool("Attack", IsAttacking);
        }
    }
}
