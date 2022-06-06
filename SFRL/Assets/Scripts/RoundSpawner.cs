using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;

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
    public GameObject itemSpawner;
    
    int nextRound = 0;

    public Transform[] spawnPoints;
    public Transform iSP;

    public float timeBetweenRounds = 5f;
    float roundCountdown;

    float searchCountdown = 1f;

    SpawnState state = SpawnState.counting;

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

        state = SpawnState.counting;
        roundCountdown = timeBetweenRounds;
        if(nextRound / 2 == 0 || nextRound == 0)
        {
            Instantiate(itemSpawner, iSP.position, iSP.rotation);
        }

        if (nextRound + 1 > rounds.Length - 1)
        {
            nextRound = 0;
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
        Debug.Log("Spawning Enemy: " + _enemy.name);

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);       
    }
}
