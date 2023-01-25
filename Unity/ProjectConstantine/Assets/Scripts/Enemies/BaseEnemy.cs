using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviourBase
{
    [Header("Debugging")]
    //public bool Stop;
    public bool DrawDebugLines;

    [Header("Enemy Base")]
    [Tooltip("Where on the mesh the player will shoot the enemy")]
    public GameObject AttackTarget;
    public BaseEnemyData EnemyData;

    private bool _foundPlayer = false;
    private bool _canAttack = true;
    private int _currentHealth;
    private float _attackCooldownCurrent;

    private GameObject _player;

    private void Start()
    {
        _currentHealth = EnemyData.MaxHealth;
        _player = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
    }

    private void Update()
    {
        _attackCooldownCurrent -= Time.deltaTime;
        _canAttack = _attackCooldownCurrent <= 0.0f;

        if(DrawDebugLines)
        {
            DebugLines();
        }

        if(!_foundPlayer &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyData.DetectionRange)
        {
            _foundPlayer = true;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        LogDebug($"I got hit - Attack Damage: {damage} - Current Health: {_currentHealth}");

        //LoadFloatingCombatText(damage);
        if(_currentHealth <= 0)
        {
            LogDebug("I Died");
            EventManager.GetInstance().OnEnemyDeath();
            Destroy(gameObject);
            return;
        }

        //_healthBar.UpdateHealth((float)_currentHealth / EnemyObj.MaxHealth);
    }

    private void DebugLines()
    {
        var rotation = gameObject.transform.rotation;
        rotation *= Quaternion.Euler(90, 0, 0);
        /*
        //Attack Stop Range
        Debug.DrawCircle(
            gameObject.transform.position,
            rotation,
            EnemyObj.AttackRange * .75f,
            Color.blue);

        //Attack Range
        Debug.DrawArc(
            90 - (EnemyObj.AttackWidth / 2),
            90 + (EnemyObj.AttackWidth / 2),
            gameObject.transform.position,
            rotation,
            EnemyObj.AttackRange,
            Color.red,
            false,
            true);
        */
        if(_foundPlayer)
        {
            return;
        }

        //Detection Range
        Debug.DrawCircle(
            gameObject.transform.position,
            rotation,
            EnemyData.DetectionRange,
            Color.yellow);
    }
}
