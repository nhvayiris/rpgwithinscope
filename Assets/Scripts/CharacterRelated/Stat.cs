using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;

    [SerializeField] private Text statValue;
    [SerializeField] private float lerpSpeed;
    
    private float currentFill;
    private float currentValue;
    public float MyMaxValue { get; set; }
    public float MyCurrentValue
    {
        get
        {
            return currentValue; 
        }
        set
        {
            if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            } else
            {
                currentValue = value;
            }
            currentFill = currentValue / MyMaxValue;
            if (statValue != null)
            {
                statValue.text = currentValue + "/" + MyMaxValue;
            }
        }
    } 

    void Start()
    {
        content = GetComponent<Image>();
    }

    void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }

        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }
}
