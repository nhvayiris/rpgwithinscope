using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer parentRenderer; //refers to the player
    private readonly List<Obstacles> obstacles = new List<Obstacles>();
    

    private void Start()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Obstacles o = collision.GetComponent<Obstacles>();
            if (obstacles.Count == 0 || o.tilemapRenderer.sortingOrder - 1 > parentRenderer.sortingOrder)
            {
                parentRenderer.sortingOrder = o.tilemapRenderer.sortingOrder - 1;
                o.FadeOut();
            }
            obstacles.Add(o);
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Obstacles o = collision.GetComponent<Obstacles>();
            o.FadeIn(); 
            obstacles.Remove(o);
            if (obstacles.Count == 0)
            {
                parentRenderer.sortingOrder = 2000;
            }
            else
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].tilemapRenderer.sortingOrder - 1;
            }
        }  
    }
}
