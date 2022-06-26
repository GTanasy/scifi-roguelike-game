using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{

    public TMP_Text creditText;

    public int credits;

    void Awake()
    {
        creditText = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Credits").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (creditText == null)
        {
            credits = 0;
            if (GameObject.Find("GameHandler/UI/Canvas/Credits") != null)
            {
                creditText = GameObject.Find("GameHandler/UI/Canvas/Credits").GetComponent<TextMeshProUGUI>();
            }
        }
        creditText.text = "Credits: " + credits.ToString();
    }
}
