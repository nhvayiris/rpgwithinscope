﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string spellName;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("LMB clicks on spell");
            HandScript.MyInstance.TakeMovable(SpellBook.Instance.GetSpell(spellName));
        }
    }
}
