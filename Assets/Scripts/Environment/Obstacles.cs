using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Obstacles : MonoBehaviour, IComparable<Obstacles>
{
    public Tilemap MyTileMap { get; set; } //color
    public TilemapRenderer tilemapRenderer;  //sorting
    private Color defaultColor;
    private Color fadedColor;

    public int CompareTo(Obstacles other)
    {
        if (tilemapRenderer.sortingOrder > other.tilemapRenderer.sortingOrder)
        {
            return 1;
        }
        else if (tilemapRenderer.sortingOrder < other.tilemapRenderer.sortingOrder)
        {
            return -1;
        }
        return 0;
    }

    private void Start()
    {
        MyTileMap = transform.parent.parent.GetComponent<Tilemap>();
        tilemapRenderer = transform.parent.parent.GetComponent<TilemapRenderer>();
        defaultColor = MyTileMap.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
    }

    public void FadeOut()
    {
        MyTileMap.color = fadedColor;
    }

    public void FadeIn()
    {
        MyTileMap.color = defaultColor;
    }
}
