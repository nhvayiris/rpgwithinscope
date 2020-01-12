using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;
    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }
            return instance;
        }
    }

    public IMovable MyMovable { get; set; }
    private Image icon;
    [SerializeField] private Vector3 offset;

    public void TakeMovable (IMovable movable)
    {
        this.MyMovable = movable;
        icon.sprite = movable.MyIcon;
        icon.color = Color.white;
    }

    private void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        icon.transform.position = Input.mousePosition+offset;
        DeleteItem();
    }

    public IMovable Put()
    {
        IMovable temp = MyMovable;
        MyMovable = null;
        icon.color = new Color(0, 0, 0, 0);
        return temp;
    }

    public void Drop()
    {
        MyMovable = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    private void DeleteItem()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMovable != null)
        {
            if (MyMovable is Item && InventoryScript.MyInstance.FromSlot != null)
            {
                (MyMovable as Item).MySlot.Clear();
            }

            Drop();
            InventoryScript.MyInstance.FromSlot = null;
        }
    }
}
