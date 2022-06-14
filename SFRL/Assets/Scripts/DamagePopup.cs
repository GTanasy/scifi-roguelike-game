using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DamagePopup : MonoBehaviour
{
    const float DISAPPEAR_TIMER_MAX = 1f;
    TextMeshPro _textMesh;
    float disappearTimer;
    Color textColor;
    Vector3 moveVector;
    static int sortingOrder;

    public static DamagePopup Create(Vector3 position, float damageAmount, bool isCritialHit, bool isShield)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.damagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        Debug.Log(damagePopupTransform.GetComponent<DamagePopup>());
        damagePopup.Setup(damageAmount, isCritialHit, isShield);

        return damagePopup;
    }

    void Awake()
    {
        _textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(float damageAmount, bool isCritialHit, bool isShield)
    {
        _textMesh.SetText(Math.Round(damageAmount, 1).ToString());
        if (!isCritialHit && !isShield)
        {
            //Normal Hit
            _textMesh.fontSize = 4;
            textColor = new Color(255, 166, 0);
        }
        else if (isCritialHit && isShield){
            //Crit hit with shield
            _textMesh.fontSize = 8;
            textColor = new Color(0, 0, 255);
        }
        else if (isCritialHit && !isShield)
        {
            //Crit hit no shield
            _textMesh.fontSize = 8;
            textColor = new Color (255, 0, 0);
        }
        else
        {
            //Shield Hit
            _textMesh.fontSize = 4;
            textColor = new Color (0, 134, 255);
        }
        _textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        _textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(1, 1) * 10f;
    }

    void Update()
    {        
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            _textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
