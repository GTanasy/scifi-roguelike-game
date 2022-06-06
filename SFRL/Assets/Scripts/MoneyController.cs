using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    public static MoneyController moneyController;

    public TMP_Text creditText;

    public int credits;

    void Awake()
    {
        if (moneyController == null)
        {
            DontDestroyOnLoad(gameObject);
            moneyController = this;
        }
        else if (moneyController != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        creditText.text = "Credits: " + credits.ToString();
    }
}
