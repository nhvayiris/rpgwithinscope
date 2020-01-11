using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag", menuName ="Items/Bag", order =1)]
public class BagItem : Item, IUsable
{
    private int bagSlots;

    [SerializeField] private GameObject bagPrefab;

    public Bag MyBagScript { get; set; }

    public int Slots { get => Slots; }
    
    public Sprite MyIcon => throw new System.NotImplementedException();

    public void Initialize(int slots)
    {
        this.bagSlots = slots;
    }

    public void Use()
    {
        MyBagScript = Instantiate(bagPrefab, Inventory.MyInstance.transform).GetComponent<Bag>();
        MyBagScript.AddSlots(bagSlots);
            }
}
