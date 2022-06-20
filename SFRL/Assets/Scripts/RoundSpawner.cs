using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using TMPro;

public class RoundSpawner : MonoBehaviour
{
    public PlayerDamageHandler player;

    public enum SpawnState
    {
        spawning,
        waiting,
        counting
    }

    [System.Serializable]
    public class Round
    {
        public string name;       
        public int amount;
        public float rate;
        public Transform[] enemyPool;
    }

    public Round[] rounds;
    
    int nextRound = 0;
    public int roundCount = 1;

    public Transform[] spawnPoints;

    TMP_Text roundCountText;

    public float timeBetweenRounds = 5f;

    public float enemyStatsBuff = 0;
    float timesLooped = 1;
    public bool looped;

    float roundCountdown;

    float searchCountdown = 1f;

    SpawnState state = SpawnState.counting;

    void Awake()
    {
        roundCountText = GameObject.FindGameObjectWithTag("RoundCount").GetComponent<TextMeshProUGUI>();
        roundCountText.text = "Round:" + roundCount;
    }

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("ERROR NO SPAWNPOINTS DETECTED");
        }
        roundCountdown = timeBetweenRounds;
    }

    void Update()
    {
        if (state == SpawnState.waiting)
        {
            if (!EnemyIsAlive())
            {
                RoundCompleted();
            }
            else
            {
                return;
            }
        }

        if (roundCountdown <= 0)
        {
            if (player != null && state != SpawnState.spawning)
            {
                StartCoroutine(SpawnRound(rounds[nextRound]));
            }
        }
        else
        {
            roundCountdown -= Time.deltaTime;
        }
    }

    void RoundCompleted()
    {
        Debug.Log("Round Complete");
        roundCount++;
        roundCountText.text = "Round:" + roundCount;

        state = SpawnState.counting;
        roundCountdown = timeBetweenRounds;

        if (nextRound + 1 > rounds.Length - 1)
        {
            nextRound = 0;
            enemyStatsBuff = 1f * timesLooped;
            looped = true;
            timesLooped++;
            Debug.Log("All Rounds Complete! Looping...");
        }
        else
        {
            nextRound++;
        }       
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }            
        }
        return true;
    }

    IEnumerator SpawnRound (Round _round)
    {
        Debug.Log("Spawning Round: " + _round.name);
        state = SpawnState.spawning;
        if (looped == true)
        {
            _round.amount = _round.amount * 2;
        }
        if (_round.name == "Round 20")
        {
            _round.amount = 1;
        }

        for (int i = 0; i < _round.amount; i++)
        {
            if (player != null)
            {
            SpawnEnemy(_round.enemyPool[Random.Range(0, _round.enemyPool.Length)]);
            yield return new WaitForSeconds(1f / _round.rate);
            }
        }

        state = SpawnState.waiting;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {        
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);       
    }
}
