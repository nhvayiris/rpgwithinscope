using System;
using UnityEngine;

[Serializable]
public class Spell : IUsable
{
    [SerializeField] private string name;
    [SerializeField] private int damage;
    [SerializeField] private Sprite icon;
    [SerializeField] private float speed;
    [SerializeField] private float castTime;
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Color barColor;

    public string MyName { get => name;}
    public int MyDamage { get => damage;}
    public Sprite MyIcon { get => icon;}
    public float MySpeed { get => speed;}
    public float MyCastTime { get => castTime;}
    public GameObject MySpellPrefab { get => spellPrefab;}
    public Color MyBarColor { get => barColor;}

    public void Use()
    {
        Player.Instance.CastSpell(MyName);
    }
}
