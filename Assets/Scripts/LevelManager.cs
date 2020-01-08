using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform map;
    [SerializeField] private Texture2D[] mapData;
    [SerializeField] private Sprite defaultTile;
    [SerializeField] private MapElement[] mapElements; //refers to tile

    private readonly Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();
    [SerializeField] private SpriteAtlas waterAtlas;
    public Vector2 WorldStartPos { get => Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); }


    private void Start()
    {
        
    }

    [Serializable]
    public class MapElement
    {
        [SerializeField] private string tileTag;
        [SerializeField] private Color color;
        [SerializeField] private GameObject elementPrefab;

        public string MyTileTag { get => tileTag; }
        public Color MyColor { get => color; }
        public GameObject ElementPrefab { get => elementPrefab; }
    }

    public struct Point
    {
        public int MyX { get; set; }
        public int MyY { get; set; }

        public Point(int x, int y)
        {
            this.MyX = x;
            this.MyY = y;
        }
    }

}