using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HealthPotion", menuName ="Items/Potion", order =1)]
public class HealthPotion : Item, IUsable
{
    [SerializeField] private int health;
    public void Use()
    {
        //regenerates health if below max health
        if (Player.Instance.MyHealth.MyCurrentValue < Player.Instance.MyHealth.MyMaxValue)
        {
            Remove();
            Player.Instance.MyHealth.MyCurrentValue += health;
        }
        
       
    }

    
}
