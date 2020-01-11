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
    
    private GameObject[] keybindButtons;
    private Stat healthStat;


    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }
    private void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();

        SetUsable(actionButtons[0], SpellBook.Instance.GetSpell("Fireball"));
        SetUsable(actionButtons[1], SpellBook.Instance.GetSpell("Frostbolt"));
        SetUsable(actionButtons[2], SpellBook.Instance.GetSpell("Thunderbolt"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
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

    public void OpenCloseMenu()
    {
        keybindMenu.alpha = keybindMenu.alpha > 0 ? 0 : 1;
        keybindMenu.blocksRaycasts = keybindMenu.blocksRaycasts == true ? false: true;
        Time.timeScale = Time.timeScale > 0 ? 0 : 1; //pause game
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

    public void SetUsable(ActionButton button, IUsable usable)
    {
        button.MyButton.image.sprite = usable.MyIcon;
        button.MyButton.image.color = Color.white;
        button.MyUsable = usable;
    }
}
