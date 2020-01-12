using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);
public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged ItemCountChangedEvent;
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

    public int MyTotalSlotCount { 
        get 
        { 
            int count = 0;
            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }

            return count;
        }
    }

    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    private void PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                OnItemCountChanged(item);
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
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }

        return false;
    }

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(15);
        AddItem(bag);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) //debug
        {
            Debug.Log("J Is Pressed");
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(8);
            AddItem(bag);
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

    public void AddBag(Bag bag, BagButton bagButton) //overload function
    {
        bags.Add(bag);
        bagButton.MyBag = bag;
    }

    public void RemovedBag(Bag bag)
    {
        bags.Remove(bag); //remove specific bag in the list of bags within the inventory
        Destroy(bag.MyBagScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyTotalSlotCount - oldBag.Slots) + newBag.Slots;
        if (newSlotCount - MyFullSlotCount >= 0)
        {
            //do swap
            List<Item> bagItems = oldBag.MyBagScript.GetItems();
            RemovedBag(oldBag);
            newBag.MyBagButton = oldBag.MyBagButton;
            newBag.Use();

            foreach (Item item in bagItems)
            {
                if (item != newBag) // no duplicate bags
                {
                    AddItem(item);
                }
            }

            AddItem(oldBag);
            HandScript.MyInstance.Drop();
            MyInstance.fromSlot = null;
        }
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

    public Stack<IUsable> GetUsables(IUsable type)
    {
        Stack<IUsable> usables = new Stack<IUsable>();
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach (Item item in slot.MyItems)
                    {
                        usables.Push(item as IUsable);
                    }
                }
            }
        }

        return usables;
    }

    public void OnItemCountChanged(Item item)
    {
        if (ItemCountChangedEvent != null)
        {
            ItemCountChangedEvent.Invoke(item);
        }
    }

}
