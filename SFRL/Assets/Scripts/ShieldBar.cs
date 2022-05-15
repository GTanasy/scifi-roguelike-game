using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{

    public Slider _slider;

    public void SetMaxShield(int shield)
    {
        _slider.maxValue = shield;
        _slider.value = shield;
    }

    public void SetShield(int shield)
    {
        _slider.value = shield;
    }
}
