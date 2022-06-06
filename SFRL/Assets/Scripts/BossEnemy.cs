using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CG.SFRL.Enemy;
using CG.SFRL.Characters;

public class BossEnemy : MonoBehaviour
{
    public BasicEnemy _enemyStats;
    EnemyDamageHandler _healthStat;
    PlayerDamageHandler _player;
    float _attackSpeed;
    float _timeBetweenShots;
    float _engagementRange;

    float timeBetweenStates;
    float maxTimeBetweenStates = 5f;

    bool isEnraged = false;

    Transform target;
    Vector3 _startingPosition;
    Vector3 _roamPosition;
    NavMeshAgent agent;

    EnemyAimWeapon _enemyAimWeapon;


    State _state;

    enum State
    {
        Idle,        
        RunRandom,
        ChaseTarget,
        Attack,
        Enraged,
        EnragedAttack,
    }

    void Awake()
    {
        _enemyAimWeapon = gameObject.GetComponent<EnemyAimWeapon>();
        if (GameObject.FindGameObjectWithTag("Player").transform != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        _engagementRange = _enemyStats.engagementRange;
        _attackSpeed = _enemyStats.attackSpeed;
        agent.speed = _enemyStats.speed;

        _state = State.Idle;
        _timeBetweenShots = 1 / _attackSpeed;

        timeBetweenStates = maxTimeBetweenStates;

        _startingPosition = transform.position;
        _roamPosition = GetRandomPosition();

        _healthStat = gameObject.GetComponent<EnemyDamageHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        TimerBetweenStates();
        StateMachine();
        EnrageCheck();
        if (target == null)
        {
            _state = default;
        }
    }

    void StateMachine()
    {
        switch (_state)
        {
            default:
            case State.Idle:
                agent.isStopped = false;
                agent.SetDestination(_roamPosition);

                if (target != null && Vector3.Distance(_startingPosition, target.position) > _engagementRange)
                {
                    _state = State.ChaseTarget;
                }
                float reachedPositionDistance = 5f;
                if (Vector3.Distance(transform.position, _roamPosition) < reachedPositionDistance)
                {
                    _roamPosition = GetRandomPosition();
                }

                break;
            case State.ChaseTarget:
                if (target != null && Vector3.Distance(transform.position, target.position) > _engagementRange)
                {
                    agent.SetDestination(target.position);
                    _startingPosition = target.position;
                    _state = State.Idle;
                }
                break;
            case State.Attack:
                if (target != null && Vector3.Distance(transform.position, target.position) < _engagementRange)
                {
                    if (Time.time > _timeBetweenShots)
                    {
                        _enemyAimWeapon.HandleAim();
                        agent.isStopped = true;
                        _enemyAimWeapon.Shoot();
                        _timeBetweenShots = Time.time + (1 / _attackSpeed);
                    }

                }
                else
                {
                    agent.isStopped = false;
                }
                break;
            case State.Enraged:
                agent.speed = 50;
                _attackSpeed = 30;
                _state = State.EnragedAttack;
                break;
            case State.EnragedAttack:
                agent.isStopped = false;
                agent.SetDestination(_roamPosition);
                if (target != null)
                { 
                if (Vector3.Distance(_startingPosition, target.position) > _engagementRange)
                {
                    _state = State.ChaseTarget;
                }
                float reachedEnragedPositionDistance = 5f;
                if (Vector3.Distance(transform.position, _roamPosition) < reachedEnragedPositionDistance)
                {
                    _roamPosition = GetRandomPosition();
                }
                if (Vector3.Distance(transform.position, target.position) < _engagementRange)
                {
                    if (Time.time > _timeBetweenShots)
                    {
                        _enemyAimWeapon.HandleAim();
                        _enemyAimWeapon.Shoot();
                        _timeBetweenShots = Time.time + (1 / _attackSpeed);
                    }

                }
        }
                break;
        }
    }

    void EnrageCheck()
    {
        if (_healthStat._currentHealth <= _healthStat._maxHealth / 2 && isEnraged == false)
        {
            _state = State.Enraged;
            isEnraged = true;
        }
    }

    Vector3 GetRandomPosition()
    {
        return _startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized 
            * Random.Range(1f, 10f);
    }

    void TimerBetweenStates()
    {               
        
        if (timeBetweenStates > 0)
        {
            timeBetweenStates -= Time.deltaTime;
        }
        if (timeBetweenStates < 0)
        {
            if (_state == State.Idle)
            {
                Debug.Log("Switch to Attack");
                _state = State.Attack;
                timeBetweenStates = maxTimeBetweenStates;
                return;
            }
            if (_state == State.Attack || _state == State.ChaseTarget)
            {
                Debug.Log("Switch to Idle");
                _state = State.Idle;
                timeBetweenStates = maxTimeBetweenStates;
                return;
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDamageHandler player = collision.GetComponent<PlayerDamageHandler>();
        if (player != null)
        {
            player.TakeDamage(30);
        }
        
    }
}
