using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using CG.SFRL.Characters;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int kills;
    public int bossKills;

    bool _isGameOver;
    bool _isGamePaused;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject tabMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] string _sceneName;
    GameObject _player;
    GameObject _healthMachine;
    GameObject _weaponMachine;
    GameObject _movementMachine;

    TMP_Text roundsSurvived;
    RoundSpawner roundSpawner;

    ItemVendingMachine healthVending;
    ItemVendingMachine weaponVending;
    ItemVendingMachine movementVending;

    PlayerDamageHandler _playerHealth;
    PlayerMovement _playerMovement;
    Shooting _playerShooting;

    TMP_Text gameOverTotalKills;
    TMP_Text gameOverBossesKilled;
    TMP_Text gameOverTotalItemsBought;
    TMP_Text gameOverHealthItems;
    TMP_Text gameOverWeaponItems;
    TMP_Text gameOverMovementItems;

    TMP_Text totalKills;
    TMP_Text bossesKilled;
    TMP_Text totalItemsBought;
    TMP_Text healthItems;
    TMP_Text weaponItems;
    TMP_Text movementItems;

    TMP_Text maxHealthText;
    TMP_Text maxShieldText;
    TMP_Text shieldRegenText;
    TMP_Text damageText;
    TMP_Text critChanceText;
    TMP_Text attackSpeedText;
    TMP_Text attackRangeText;
    TMP_Text projectileSpeedText;
    TMP_Text reloadSpeedText;
    TMP_Text moveSpeedText;
    TMP_Text dashLengthText;
    TMP_Text dashSpeedText;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _healthMachine = GameObject.FindGameObjectWithTag("HealthVending");
        _weaponMachine = GameObject.FindGameObjectWithTag("WeaponVending");
        _movementMachine = GameObject.FindGameObjectWithTag("MovementVending");

        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");       

        tabMenu = GameObject.FindGameObjectWithTag("TabMenu");
        totalKills = GameObject.Find("UI/TabMenu/TotalKills").GetComponent<TextMeshProUGUI>();
        bossesKilled = GameObject.Find("UI/TabMenu/BossKills").GetComponent<TextMeshProUGUI>();
        totalItemsBought = GameObject.Find("UI/TabMenu/TotalItemsBought").GetComponent<TextMeshProUGUI>();
        healthItems = GameObject.Find("UI/TabMenu/HealthItems").GetComponent<TextMeshProUGUI>();
        weaponItems = GameObject.Find("UI/TabMenu/WeaponItems").GetComponent<TextMeshProUGUI>();
        movementItems = GameObject.Find("UI/TabMenu/MovementItems").GetComponent<TextMeshProUGUI>();

        maxHealthText = GameObject.Find("UI/TabMenu/MaxHealth").GetComponent<TextMeshProUGUI>();
        maxShieldText = GameObject.Find("UI/TabMenu/MaxShield").GetComponent<TextMeshProUGUI>();
        shieldRegenText = GameObject.Find("UI/TabMenu/ShieldRegen").GetComponent<TextMeshProUGUI>();
        damageText = GameObject.Find("UI/TabMenu/Damage").GetComponent<TextMeshProUGUI>();
        critChanceText = GameObject.Find("UI/TabMenu/CritChance").GetComponent<TextMeshProUGUI>();
        attackSpeedText = GameObject.Find("UI/TabMenu/AttackSpeed").GetComponent<TextMeshProUGUI>();
        attackRangeText = GameObject.Find("UI/TabMenu/AttackRange").GetComponent<TextMeshProUGUI>();
        projectileSpeedText = GameObject.Find("UI/TabMenu/ProjectileSpeed").GetComponent<TextMeshProUGUI>();
        reloadSpeedText = GameObject.Find("UI/TabMenu/ReloadSpeed").GetComponent<TextMeshProUGUI>();
        moveSpeedText = GameObject.Find("UI/TabMenu/MoveSpeed").GetComponent<TextMeshProUGUI>();
        dashLengthText = GameObject.Find("UI/TabMenu/DashLength").GetComponent<TextMeshProUGUI>();
        dashSpeedText = GameObject.Find("UI/TabMenu/DashSpeed").GetComponent<TextMeshProUGUI>();     

        gameOverMenu = GameObject.FindGameObjectWithTag("GAMEOVER");
        roundsSurvived = GameObject.Find("UI/GAMEOVER/RoundsSurvived").GetComponent<TextMeshProUGUI>();        
        roundSpawner = GameObject.Find("RoundManager").GetComponent<RoundSpawner>();

        gameOverTotalKills = GameObject.Find("UI/GAMEOVER/TotalKills").GetComponent<TextMeshProUGUI>();
        gameOverBossesKilled = GameObject.Find("UI/GAMEOVER/BossKills").GetComponent<TextMeshProUGUI>();
        gameOverTotalItemsBought = GameObject.Find("UI/GAMEOVER/TotalItemsBought").GetComponent<TextMeshProUGUI>();
        gameOverHealthItems = GameObject.Find("UI/GAMEOVER/HealthItems").GetComponent<TextMeshProUGUI>();
        gameOverWeaponItems = GameObject.Find("UI/GAMEOVER/WeaponItems").GetComponent<TextMeshProUGUI>();
        gameOverMovementItems = GameObject.Find("UI/GAMEOVER/MovementItems").GetComponent<TextMeshProUGUI>();

    }

    void Start()
    {
        pauseMenu.SetActive(false);
        tabMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        healthVending = _healthMachine.GetComponent<ItemVendingMachine>();
        weaponVending = _weaponMachine.GetComponent<ItemVendingMachine>();
        movementVending = _movementMachine.GetComponent<ItemVendingMachine>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowStatsMenu();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            RemoveStatsMenu();
        }
    }


    public void GameOver()
    {
        if (_isGameOver == false)
        {
            _isGameOver = true;
            gameOverMenu.SetActive(true);
            roundsSurvived.text = "Survived " + roundSpawner.roundCount + " rounds";

            gameOverTotalKills.text = "Total Kills:" + kills;
            gameOverBossesKilled.text = "Bosses Killed:" + bossKills;
            gameOverTotalItemsBought.text = "Items Bought:" + (healthVending.itemsBought + weaponVending.itemsBought + movementVending.itemsBought);
            gameOverHealthItems.text = "Health Items:" + healthVending.itemsBought;
            gameOverWeaponItems.text = "Weapon Items:" + weaponVending.itemsBought;
            gameOverMovementItems.text = "Movement Items:" + movementVending.itemsBought;
            Debug.Log("GAME OVER");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        _player.GetComponent<PlayerMovement>().enabled = false;
        _player.GetComponent<PlayerAimWeapon>().enabled = false;
        _player.GetComponent<Shooting>().enabled = false;
        _player.GetComponent<Norman>().enabled = false;
        Time.timeScale = 0;
        _isGamePaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        _player.GetComponent<PlayerMovement>().enabled = true;
        _player.GetComponent<PlayerAimWeapon>().enabled = true;
        _player.GetComponent<Shooting>().enabled = true;
        _player.GetComponent<Norman>().enabled = true;
        Time.timeScale = 1;
        _isGamePaused = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }

    void ShowStatsMenu()
    {
        Debug.Log("Showing Stats");
        _playerHealth = _player.GetComponent<PlayerDamageHandler>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _playerShooting = _player.GetComponent<Shooting>();

        totalKills.text = "Total Kills:" + kills;
        bossesKilled.text = "Bosses Killed:" + bossKills;
        totalItemsBought.text = "Items Bought:" + (healthVending.itemsBought + weaponVending.itemsBought + movementVending.itemsBought);
        healthItems.text = "Health Items:" + healthVending.itemsBought;
        weaponItems.text = "Weapon Items:" + weaponVending.itemsBought;
        movementItems.text = "Movement Items:" + movementVending.itemsBought;


        maxHealthText.text = "Max Health:" + _playerHealth._maxHealth.Value;
        maxShieldText.text = "Max Shield:" + _playerHealth._maxShield.Value;
        shieldRegenText.text = "Shield Regen:" + Math.Round(_playerHealth._shieldRegenRate.Value, 1);

        damageText.text = "Damage:" + Math.Round(_playerShooting._bulletDamage.Value, 1);
        critChanceText.text = "Crit Chance:" + Math.Round(_playerShooting._criticalChance.Value, 1);
        attackSpeedText.text = "Attack Speed:" + Math.Round(_playerShooting._attackSpeed.Value, 1);
        attackRangeText.text = "Attack Range:0";
        projectileSpeedText.text = "Projectile Speed:" + Math.Round(_playerShooting._bulletForce.Value, 1);
        reloadSpeedText.text = "Reload Speed:" + Math.Round(_playerShooting._reloadDuration.Value, 2);

        moveSpeedText.text = "Move Speed:" + Math.Round(_playerMovement._normalSpeed.Value, 1);
        dashLengthText.text = "Dash Length:" + Math.Round(_playerMovement._dashLength.Value, 1);
        dashSpeedText.text = "Dash Speed:" + Math.Round(_playerMovement._dashSpeed.Value, 1);
        tabMenu.SetActive(true);
    }

    void RemoveStatsMenu()
    {
        tabMenu.SetActive(false);
        Debug.Log("Removing Menu");
    }
}
