using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField] private Image icon;

    [SerializeField] private Text stackSize;

    public BagScript MyBag { get; set; } //reference to the bag that this slot belong to
    public bool IsEmpty { get => MyItems.Count == 0; }
    public bool IsFull 
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }
            return true;
        } 
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }
            return null;
        } 
    }

    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount { get => MyItems.Count; }

    public Text MyStackText { get => stackSize; }
    public ObservableStack<Item> MyItems { get => items; }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    public void RemoveItem(Item item)
    {
        if (MyItems.Count > 0) // or write as if (!IsEmpty), literally the same thing 
        {
            MyItems.Pop();
        }
    }

    public void Clear()
    {
        if (MyItems.Count > 0)
        {
            MyItems.Clear();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) 
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) //if there is nothing to move
            {
                HandScript.MyInstance.TakeMovable(MyItem as IMovable);
                InventoryScript.MyInstance.FromSlot = this;
            } 
            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty && (HandScript.MyInstance.MyMovable is Bag))
            {
                //dequip bag from inventory
                Bag bag = (Bag)HandScript.MyInstance.MyMovable;

                if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.Slots > 0)
                {
                    AddItem(bag);
                    bag.MyBagButton.RemoveBag();
                    HandScript.MyInstance.Drop();
                }

                
            }
            else if (InventoryScript.MyInstance.FromSlot !=null) //if there is something to move
            {
                //try to do different things to placethe item back into inventroy
                if (PutItemBack() || 
                    MergeItems(InventoryScript.MyInstance.FromSlot) || 
                    SwapItems(InventoryScript.MyInstance.FromSlot) || 
                    AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
            
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
        
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //check how many free slots do we have in the stack
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }
            return true;
        }
        return false;
    }

    public void UseItem()
    {
        if (MyItem is IUsable)
        {
            (MyItem as IUsable).Use();
        }
      
    }

    public bool StackItem (Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }

    private void UpdateSlot()
    {
        UIManager.Instance.UpdateStackSize(this);
    }

    private bool PutItemBack()
    {
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            //copy all the times we need to swap from A
            ObservableStack<Item> tempFrom = new ObservableStack<Item>(from.MyItems);
            
            //then we clear slot A
            from.MyItems.Clear();

            //take all items from slot B and copy them into A
            from.AddItems(MyItems);

            //clear B
            MyItems.Clear();

            //Move items from A copy to B
            AddItems(tempFrom);

            return true;

        }
        return false;
    }


}
