using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] items; //for debugging purposes

    private static Inventory instance;
    public static Inventory MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        BagItem bag = (BagItem)Instantiate(items[0]);
        bag.Initialize(14);
        bag.Use();
    }

    private void Start()
    {
        
    }

}
