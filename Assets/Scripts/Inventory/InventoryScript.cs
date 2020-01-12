using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    private static InventoryScript instance;
    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }
    }


    [SerializeField] private Item[] items; //for debugging purposes
    [SerializeField] private BagButton[] bagButtons;
    
    private List<Bag> bags = new List<Bag>();
    private SlotScript fromSlot;
    public bool CanAddBag { get => bags.Count < 5; }
    public SlotScript FromSlot
    {
        get => fromSlot;
        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }
    public int MyEmptySlotCount 
    { 
        get 
        { 
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }
            return count;
        } 
    }

    private void PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
    }

    private bool PlaceInStack(Item item) 
    {
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slots in bag.MyBagScript.MySlots)
            {
                if (slots.StackItem(item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    

    private void Awake()
    {
        MakeBag();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) //debug
        {
            Debug.Log("J Is Pressed");
            MakeBag();
        }

        if (Input.GetKeyDown(KeyCode.K)) //debug
        {
            Debug.Log("K Is Pressed");
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(15);
            AddItem(bag);
        }

        if (Input.GetKeyDown(KeyCode.L)) //debug
        {
            Debug.Log("L Is Pressed");
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);

        }
    }

    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag; 
                bags.Add(bag);
                bag.MyBagButton = bagButton;
                break;
            }
        }
    }

    public void RemovedBag(Bag bag)
    {
        bags.Remove(bag); //remove specific bag in the list of bags within the inventory
        Destroy(bag.MyBagScript.gameObject);
    }

    public void OpenClose()
    {
        bool closedBag = bags.Find( x => !x.MyBagScript.IsOpen );
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.isActiveAndEnabled != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    private void MakeBag()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(15);
        bag.Use();
    }

    public void AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
        }
        PlaceInEmpty(item);
    }

}
