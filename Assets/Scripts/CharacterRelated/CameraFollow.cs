using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    private Player player;

    private Transform targetPlayer;
    private float xMinValue, xMaxValue, yMinValue, yMaxValue;

    private void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        player = targetPlayer.GetComponent<Player>();
        Vector3 minTile = tileMap.CellToWorld(tileMap.cellBounds.min);
        Vector3 maxTile = tileMap.CellToWorld(tileMap.cellBounds.max);

        SetLimit(minTile, maxTile);
        player.Setlimit(minTile, maxTile);
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(targetPlayer.position.x, xMinValue, xMaxValue), Mathf.Clamp(targetPlayer.position.y, yMinValue, yMaxValue), -10);
   }

    private void SetLimit(Vector3 minTile, Vector3 maxTile)
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        xMinValue = minTile.x + width / 2;
        xMaxValue = maxTile.x - width / 2;

        yMinValue = minTile.y + height / 2;
        yMaxValue = maxTile.y - height / 2;

    }
}
