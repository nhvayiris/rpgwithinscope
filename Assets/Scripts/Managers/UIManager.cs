using System;
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
    
    [SerializeField] private ActionButton[] actionButtons;
    [SerializeField] private GameObject targetFrame;
    [SerializeField] private Image portraitFrame;
    [SerializeField] private CanvasGroup keybindMenu;
    [SerializeField] private CanvasGroup spellBook;

    private GameObject[] keybindButtons;
    private Stat healthStat;


    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }
    private void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(keybindMenu);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(spellBook);
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.MyInstance.OpenClose();
        }
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

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;


    }


    public void UpdateKeyText(string key, KeyCode code)
    {
        Text temp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        temp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }

        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }
}
