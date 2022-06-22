using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CG.SFRL.Enemy;
using CG.SFRL.Characters;

public class BossEnemy : MonoBehaviour
{
    public BasicEnemy enemyStats;
    EnemyDamageHandler _healthStat;    
    float _attackSpeed;
    float _timeBetweenShots;
    float _engagementRange;

    float timeBetweenStates;
    float maxTimeBetweenStates = 5f;

    bool _inLOS;

    bool _isEnraged = false;

    Transform _target;
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
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        _engagementRange = enemyStats.engagementRange;
        _attackSpeed = enemyStats.attackSpeed;
        agent.speed = enemyStats.speed;

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
        if (_target == null)
        {
            _state = default;
        }
    }

    void StateMachine()
    {
        if (_target != null)
        {
        Vector3 _lookDirection = (_target.position - transform.position).normalized;
        float _angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 0f;
        CorrectOrientation(_angle);
        switch (_state)
        {
            default:
            case State.Idle:
                agent.isStopped = false;
                agent.SetDestination(_roamPosition);

                if (_target != null && Vector3.Distance(_startingPosition, _target.position) > _engagementRange)
                {
                    _state = State.ChaseTarget;
                }
                float reachedPositionDistance = 10f;
                if (Vector3.Distance(transform.position, _roamPosition) < reachedPositionDistance)
                {
                    _roamPosition = GetRandomPosition();
                }

                break;
            case State.ChaseTarget:
                if (_target != null && Vector3.Distance(transform.position, _target.position) > _engagementRange)
                {
                    agent.SetDestination(_target.position);
                    _startingPosition = _target.position;
                    _state = State.Idle;
                }
                break;
            case State.Attack:
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(_angle));
                    if (raycastHit2D.collider != null)
                    {
                        if (raycastHit2D.collider.CompareTag("Player"))
                        {
                            _inLOS = true;
                        }
                        else
                        {
                            _inLOS = false;
                        }

                    }
                    agent.SetDestination(_target.position);
                if (_target != null && _inLOS == true && Vector3.Distance(transform.position, _target.position) < _engagementRange)
                {

                    if (Time.time > _timeBetweenShots)
                    {
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
                if (_target != null)
                { 
                if (Vector3.Distance(_startingPosition, _target.position) > _engagementRange)
                {
                    _state = State.ChaseTarget;
                }
                float reachedEnragedPositionDistance = 10f;
                if (Vector3.Distance(transform.position, _roamPosition) < reachedEnragedPositionDistance)
                {
                    _roamPosition = GetRandomPosition();
                }
                if (Vector3.Distance(transform.position, _target.position) < _engagementRange)
                {
                    if (Time.time > _timeBetweenShots)
                    {
                        _enemyAimWeapon.Shoot();
                        _timeBetweenShots = Time.time + (1 / _attackSpeed);
                    }

                }
        }
                break;
        }
        }
    }

    void EnrageCheck()
    {
        if (_healthStat.currentHealth <= _healthStat.maxHealth.Value / 2 && _isEnraged == false)
        {
            _healthStat.maxShield.AddModifier(new StatModifier(-_healthStat.maxShield.Value, StatModType.Flat, this));
            _state = State.Enraged;
            _isEnraged = true;
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
                _state = State.Attack;
                timeBetweenStates = maxTimeBetweenStates;
                return;
            }
            if (_state == State.Attack || _state == State.ChaseTarget)
            {
                _state = State.Idle;
                timeBetweenStates = maxTimeBetweenStates;
                return;
            }
            
        }
    }

    void CorrectOrientation(float _angle)
    {
        Vector3 _scale = Vector3.one;
        Vector3 _scaleBar = new Vector3();

        if (_angle < -90 || _angle > 90)
        {
            _scale.x = +1.0f;
            _scaleBar.x = -0.01f;
            _scaleBar.y = 0.01f;
            _scaleBar.z = 0.01f;
        }
        else
        {
            _scale.x = -1.0f;
            _scaleBar.x = +0.01f;
            _scaleBar.y = 0.01f;
            _scaleBar.z = 0.01f;
        }
        gameObject.GetComponent<Transform>().localScale = _scale;        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDamageHandler player = collision.GetComponent<PlayerDamageHandler>();
        if (player != null)
        {
            player.TakeDamage(30);
        }
        
    }

    static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
