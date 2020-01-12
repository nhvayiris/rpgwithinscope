using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{ 
    public IUsable MyUsable { get; set; }
    public Button MyButton { get; private set; }
    public Image MyIcon { get => icon; set => icon = value; }

    [SerializeField] private Image icon;

    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (MyUsable != null)
        {
            MyUsable.Use();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMovable != null && HandScript.MyInstance.MyMovable is IUsable)
            {
                SetUsable(HandScript.MyInstance.MyMovable as IUsable);
            }
        }
    }

    public void SetUsable(IUsable usable)
    {
        this.MyUsable = usable;
        
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
        MyIcon.color = Color.white;
    }
}
