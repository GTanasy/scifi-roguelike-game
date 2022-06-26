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

    TMP_Text _gameOverTotalKills;
    TMP_Text _gameOverBossesKilled;
    TMP_Text _gameOverTotalItemsBought;
    TMP_Text _gameOverHealthItems;
    TMP_Text _gameOverWeaponItems;
    TMP_Text _gameOverMovementItems;

    TMP_Text _totalKills;
    TMP_Text _bossesKilled;
    TMP_Text _totalItemsBought;
    TMP_Text _healthItems;
    TMP_Text _weaponItems;
    TMP_Text _movementItems;

    TMP_Text _maxHealthText;
    TMP_Text _maxShieldText;
    TMP_Text _shieldRegenText;
    TMP_Text _damageText;
    TMP_Text _critChanceText;
    TMP_Text _attackSpeedText;
    TMP_Text _attackRangeText;
    TMP_Text _projectileSpeedText;
    TMP_Text _reloadSpeedText;
    TMP_Text _moveSpeedText;
    TMP_Text _dashLengthText;
    TMP_Text _dashSpeedText;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _healthMachine = GameObject.FindGameObjectWithTag("HealthVending");
        _weaponMachine = GameObject.FindGameObjectWithTag("WeaponVending");
        _movementMachine = GameObject.FindGameObjectWithTag("MovementVending");

        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");       

        tabMenu = GameObject.FindGameObjectWithTag("TabMenu");
        _totalKills = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/TotalKills").GetComponent<TextMeshProUGUI>();
        _bossesKilled = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/BossKills").GetComponent<TextMeshProUGUI>();
        _totalItemsBought = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/TotalItemsBought").GetComponent<TextMeshProUGUI>();
        _healthItems = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/HealthItems").GetComponent<TextMeshProUGUI>();
        _weaponItems = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/WeaponItems").GetComponent<TextMeshProUGUI>();
        _movementItems = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/MovementItems").GetComponent<TextMeshProUGUI>();

        _maxHealthText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/MaxHealth").GetComponent<TextMeshProUGUI>();
        _maxShieldText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/MaxShield").GetComponent<TextMeshProUGUI>();
        _shieldRegenText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/ShieldRegen").GetComponent<TextMeshProUGUI>();
        _damageText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/Damage").GetComponent<TextMeshProUGUI>();
        _critChanceText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/CritChance").GetComponent<TextMeshProUGUI>();
        _attackSpeedText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/AttackSpeed").GetComponent<TextMeshProUGUI>();
        _attackRangeText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/AttackRange").GetComponent<TextMeshProUGUI>();
        _projectileSpeedText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/ProjectileSpeed").GetComponent<TextMeshProUGUI>();
        _reloadSpeedText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/ReloadSpeed").GetComponent<TextMeshProUGUI>();
        _moveSpeedText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/MoveSpeed").GetComponent<TextMeshProUGUI>();
        _dashLengthText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/DashLength").GetComponent<TextMeshProUGUI>();
        _dashSpeedText = GameObject.Find("GameHandler/UI/Canvas/TabMenu/Stats/DashSpeed").GetComponent<TextMeshProUGUI>();     

        gameOverMenu = GameObject.FindGameObjectWithTag("GAMEOVER");
        roundsSurvived = GameObject.Find("GameHandler/UI/Canvas/GAMEOVER/RoundsSurvived").GetComponent<TextMeshProUGUI>();        
        roundSpawner = GameObject.Find("RoundManager").GetComponent<RoundSpawner>();

        _gameOverTotalKills = GameObject.Find("GameHandler/UI/Canvas/GAMEOVER/TotalKills").GetComponent<TextMeshProUGUI>();
        _gameOverBossesKilled = GameObject.Find("GameHandler/UI/Canvas/GAMEOVER/BossKills").GetComponent<TextMeshProUGUI>();
        _gameOverTotalItemsBought = GameObject.Find("GameHandler/UI/Canvas/GAMEOVER/TotalItemsBought").GetComponent<TextMeshProUGUI>();
        _gameOverHealthItems = GameObject.Find("GameHandler/UI/Canvas/GAMEOVER/HealthItems").GetComponent<TextMeshProUGUI>();
        _gameOverWeaponItems = GameObject.Find("GameHandler/UI/Canvas/GAMEOVER/WeaponItems").GetComponent<TextMeshProUGUI>();
        _gameOverMovementItems = GameObject.Find("GameHandler/UI/Canvas/GAMEOVER/MovementItems").GetComponent<TextMeshProUGUI>();

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
            Tooltip.HideTooltip_Static();
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

            _gameOverTotalKills.text = "Total Kills:" + kills;
            _gameOverBossesKilled.text = "Bosses Killed:" + bossKills;
            _gameOverTotalItemsBought.text = "Items Bought:" + (healthVending.itemsBought + weaponVending.itemsBought + movementVending.itemsBought);
            _gameOverHealthItems.text = "Health Items:" + healthVending.itemsBought;
            _gameOverWeaponItems.text = "Weapon Items:" + weaponVending.itemsBought;
            _gameOverMovementItems.text = "Movement Items:" + movementVending.itemsBought;
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

        _totalKills.text = "Total Kills:" + kills;
        _bossesKilled.text = "Bosses Killed:" + bossKills;
        _totalItemsBought.text = "Items Bought:" + (healthVending.itemsBought + weaponVending.itemsBought + movementVending.itemsBought);
        _healthItems.text = "Health Items:" + healthVending.itemsBought;
        _weaponItems.text = "Weapon Items:" + weaponVending.itemsBought;
        _movementItems.text = "Movement Items:" + movementVending.itemsBought;


        _maxHealthText.text = "Max Health:" + _playerHealth.maxHealth.Value;
        _maxShieldText.text = "Max Shield:" + _playerHealth.maxShield.Value;
        _shieldRegenText.text = "Shield Regen:" + Math.Round(_playerHealth.shieldRegenRate.Value, 1);

        _damageText.text = "Damage:" + Math.Round(_playerShooting.bulletDamage.Value, 1);
        _critChanceText.text = "Crit Chance:" + Math.Round(_playerShooting.criticalChance.Value, 1);
        _attackSpeedText.text = "Attack Speed:" + Math.Round(_playerShooting.attackSpeed.Value, 1);
        _attackRangeText.text = "Attack Range:0";
        _projectileSpeedText.text = "Projectile Speed:" + Math.Round(_playerShooting.bulletForce.Value, 1);
        _reloadSpeedText.text = "Reload Speed:" + Math.Round(_playerShooting.reloadDuration.Value, 2);

        _moveSpeedText.text = "Move Speed:" + Math.Round(_playerMovement.normalSpeed.Value, 1);
        _dashLengthText.text = "Dash Length:" + Math.Round(_playerMovement.dashLength.Value, 1);
        _dashSpeedText.text = "Dash Speed:" + Math.Round(_playerMovement.dashSpeed.Value, 1);
        tabMenu.SetActive(true);
    }

    void RemoveStatsMenu()
    {
        tabMenu.SetActive(false);
        Debug.Log("Removing Menu");
    }
}
