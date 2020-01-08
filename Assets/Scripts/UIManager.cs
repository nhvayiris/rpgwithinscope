using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    
    [SerializeField] private Button[] actionButtons;
    [SerializeField] private GameObject targetFrame;
    [SerializeField] private Image portraitFrame;
    [SerializeField] private CanvasGroup keybindMenu;

    private Stat healthStat;
    private KeyCode action1, action2, action3;
    

    
    private void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }

    private void Update()
    {
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
        }

        if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(1);
        }

        if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(2);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }

    }

    private void ActionButtonOnClick(int btnIndex) 
        {
            actionButtons[btnIndex].onClick.Invoke();
        } 

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);
        portraitFrame.sprite = target.MyPortrait;
        target.HealthChanged += new HealthChanged(UpdateTargetFrame);
        target.NPCRemoved += new NPCRemoved(HideTargetFrame);
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float value)
    {
        healthStat.MyCurrentValue = value;
    }

    public void OpenCloseMenu()
    {
        keybindMenu.alpha = keybindMenu.alpha > 0 ? 0 : 1;
        keybindMenu.blocksRaycasts = keybindMenu.blocksRaycasts == true ? false: true;
        Time.timeScale = Time.timeScale > 0 ? 0 : 1; //pause game
    }
}
