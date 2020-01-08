using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    private NPC currentTarget;
    private void Update()
    {
        ClickTarget();
    }
    private void ClickTarget()
    {
        //lmb 0, rmb 1
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));
            if (hit.collider != null)
            {
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = hit.collider.GetComponent<NPC>();
                player.MyTarget = currentTarget.Select();
                UIManager.Instance.ShowTargetFrame(currentTarget);
            }
            else
            {
                UIManager.Instance.HideTargetFrame();
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }
}
