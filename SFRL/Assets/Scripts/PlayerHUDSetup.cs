using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CG.SFRL.Characters;

public class PlayerHUDSetup : MonoBehaviour
{
    [SerializeField] private BasicCharacter _characterIcons;

    Image _ability1Icon;
    Image _ability2Icon;

    Image _gunIcon;

    Image _dashIcon;

    void Awake()
    {
        _ability1Icon = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Ability1/Button").GetComponent<Image>();
        _ability2Icon = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Ability2/Button").GetComponent<Image>();
        _gunIcon = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/GunIcon").GetComponent<Image>();
        _dashIcon = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Roll/Button").GetComponent<Image>();
    }

    void Start()
    {
        _ability1Icon.sprite = _characterIcons.ability1Image;
        _ability2Icon.sprite = _characterIcons.ability2Image;
        _gunIcon.sprite = _characterIcons.gunIcon;
        _dashIcon.sprite = _characterIcons.dashIcon;
    }
}
