using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);
public delegate void NPCRemoved();
public class NPC : Character
{
    public event HealthChanged HealthChanged;
    public event NPCRemoved NPCRemoved;
    
    [SerializeField] private Sprite portrait;
    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }
    public virtual void DeSelect()
    {
        HealthChanged -= new HealthChanged(UIManager.Instance.UpdateTargetFrame);
        NPCRemoved -= new NPCRemoved(UIManager.Instance.HideTargetFrame);
    }

    public virtual Transform Select()
    {
        return hitBox;
    }

    public void OnHealthChanged(float health)
    {
        if (HealthChanged != null)
        {
            HealthChanged(health);
        }
    }

    public void OnNPCRemoved()
    {
        if (NPCRemoved == null)
        {
            NPCRemoved();
        }

        Destroy(gameObject);
    }
}
