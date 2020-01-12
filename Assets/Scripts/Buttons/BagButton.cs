using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    private Bag bag;
    
    [SerializeField] private Sprite full, empty;

    public Bag MyBag { get => bag; set { 
            if (value != null) { GetComponent<Image>().sprite = full; } 
            else { GetComponent<Image>().sprite = empty; }
            bag = value; } }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot != null && HandScript.MyInstance.MyMovable != null && HandScript.MyInstance.MyMovable is Bag)
            {
                if (MyBag != null)
                {
                    InventoryScript.MyInstance.SwapBags(MyBag, HandScript.MyInstance.MyMovable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)HandScript.MyInstance.MyMovable; //instantiate
                    tmp.MyBagButton = this; //set bag to this
                    tmp.Use();
                    MyBag = tmp;
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                HandScript.MyInstance.TakeMovable(MyBag);
            }
            else if (bag != null) //if we have a bag equipped
            {
                bag.MyBagScript.OpenClose(); //open or close the bag
            }
        }
        
    }

    public void RemoveBag()
    {
        InventoryScript.MyInstance.RemovedBag(MyBag); //removes the bag that is being picked for removal
        MyBag.MyBagButton = null; //make sure there is no more reference

        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            InventoryScript.MyInstance.AddItem(item);
        }
        MyBag = null; //make sure this bag does not have any reference to this bag anymore, so cannot open/close bag. And can be equipped again later
    }
}
