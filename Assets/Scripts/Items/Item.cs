using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int stackSize;
    
    private Slot slot;


    public Sprite ItemIcon { get => itemIcon; }
    public int StackSize { get => stackSize; }
    protected Slot Slot { get => slot; set => slot = value; }
}
