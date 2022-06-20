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
        creditText = GameObject.Find("UI/Credits").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (creditText == null)
        {
            credits = 0;
            if (GameObject.Find("UI/Credits") != null)
            {
                creditText = GameObject.Find("UI/Credits").GetComponent<TextMeshProUGUI>();
            }
        }
        creditText.text = "Credits: " + credits.ToString();
    }
}
