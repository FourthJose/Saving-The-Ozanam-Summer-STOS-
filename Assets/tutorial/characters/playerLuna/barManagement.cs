using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barManagement : MonoBehaviour
{
    public Gradient colRange;
    public Slider slider;
    public Image fill;

    public void setMaxHP(int HP)
    {
        slider.maxValue = HP;
        slider.value = HP;

        fill.color = colRange.Evaluate(1f);
    }

    public void setHealth(int HP)
    {
        slider.value = HP;

        fill.color = colRange.Evaluate(slider.normalizedValue);
    }
}