using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    private static KeybindManager instance; //singleton
    private string bindName;

    public static KeybindManager MyInstance 
    {
        get
        {
            if (instance == null) 
            {
                instance = FindObjectOfType<KeybindManager>();
            } 
            return instance; 
        } 
    }

    public Dictionary<string, KeyCode> Keybinds { get; private set; }
    public Dictionary<string, KeyCode> ActionBinds { get; private set; }



    private void Start()
    {
        Keybinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("Up", KeyCode.W);
        BindKey("Left", KeyCode.A);
        BindKey("Down", KeyCode.S);
        BindKey("Right", KeyCode.D);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;
        if (key.Contains("ACT"))
        {
            currentDictionary = ActionBinds;
        }
        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            UIManager.Instance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;
            currentDictionary[myKey] = KeyCode.None;
            UIManager.Instance.UpdateKeyText(key, KeyCode.None);

        }
        currentDictionary[key] = keyBind;
        UIManager.Instance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;

    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI() 
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                BindKey(bindName, e.keyCode); 
            }
        }
    }
}
