using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrquestraDirector : MonoBehaviour
{
    public static OrquestraDirector _instance;

    [SerializeField] private bool isOrquestraAttackOn = false;
    
    private Enemy[] tempEnemies;
    [SerializeField] List<Enemy> activeEnemies=new List<Enemy>();
    
    [SerializeField] float maxDistance;
    public CharacterController2D player;

    private Coroutine currentCoroutine = null;

    [SerializeField] private int n;
    [SerializeField] private int attacksToUnlockOrquestra = 0;
    
    [SerializeField] private float t ;
    [SerializeField] private float orquestraAttackCooldownTimer = -1;
    
    [Range(0,1)]
    [SerializeField] private float r;

    private string enemyType = "none";

    public bool IsOrquestraAttackOn
    {
        get => isOrquestraAttackOn;
    }
    
    public string EnemyType
    {
        get => enemyType;
    }
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        r = 1 - r;
        tempEnemies = FindObjectsOfType<Enemy>();
        FilterEnemies();
    }
    
    #region Orquestra Attack Methods

    public void Wait_ForAttack(float _duration, Enemy _enemy)
    {
        attacksToUnlockOrquestra--;
        if (currentCoroutine == null && !isOrquestraAttackOn && attacksToUnlockOrquestra<=0 && orquestraAttackCooldownTimer<=0)
            currentCoroutine = StartCoroutine(WaitForAttack(_duration, _enemy));
    }

    
    IEnumerator WaitForAttack(float _duration,Enemy enemy)
    {
        // someoneAttacked = true;
        enemyType = "none";
        
        if (Random.value > r)
        {
            isOrquestraAttackOn = true;
            enemyType = enemy.enemyType;
            
            FilterEnemies();
            
            // Enemy tempEnemy = null;
            //If any of filtered enemies has the same tag of the current attacking enemy, 
            //Stop searching and orders to the found enemy to attack  
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                for (int j = 0; j < activeEnemies[i].enemyAttack.attackTags.Length; j++)
                {
                    if (activeEnemies[i].enemyAttack.reactionTags.Contains(enemy.enemyAttack.attackTags[j]))
                    {
                        activeEnemies[i].Attack();
                        activeEnemies[i].CanAttackInOrquestra = true;
                        OrquestraMessage_Attack(enemy, activeEnemies[i]);
                        break;
                    }
                }
            }

            OrquestraAttack();
            
            
            while (_duration > 0)
            {
                _duration -= Time.fixedDeltaTime;
                yield return null;
            }

            print("La ventana de bloqueo de ataque generada por el ataque del enemigo" + enemy.name + " ha terminado");
        }
        yield return null;
        currentCoroutine = null;
    }

    void OrquestraAttack()
    {
        attacksToUnlockOrquestra = n;
        orquestraAttackCooldownTimer = t;
    }

    #endregion
    
    private void Update()
    {
        if (orquestraAttackCooldownTimer >= 0)
        {
            orquestraAttackCooldownTimer -= Time.deltaTime;
        }
        else
        {
            if (attacksToUnlockOrquestra <= 0)
            {
                isOrquestraAttackOn = false;
                orquestraAttackCooldownTimer = -1;
            }
        }
    }

  

    #region Filter Enemies

    void FilterEnemies()
    {
        tempEnemies = tempEnemies.OrderBy(c => GetDistance(c)).ToArray();

        activeEnemies.Clear();
        
        //Remove enemies by direction
        for (int i = 0; i < tempEnemies.Length; i++)
        {
            if (IsInRange(tempEnemies[i]) && tempEnemies[i].IsEnemyAlive())
            {
                activeEnemies.Add(tempEnemies[i]);
            }
        }
    }
    
    bool IsInRange(Enemy _tempEnemy)
    {
        float _tempDistance = Vector3.Distance(_tempEnemy.transform.position, player.transform.position);
        return _tempDistance<maxDistance?true:false;
    }
    
    float GetDistance(Enemy _tempEnemy)
    {
        float _tempDistance = Vector3.Distance(_tempEnemy.transform.position, player.transform.position);
        return _tempDistance;
    }

    #endregion
    
    #region Orquestra Messages

    public void OrquestraMessage_SameType(string _enemyType)
    {
        if (isOrquestraAttackOn)
        {
            if (enemyType.Contains(_enemyType))
                print("Un enemigo de su mismo tipo está atacando");
        }
    }

    public void OrquestraMessage_Attack(Enemy a, Enemy b)
    {
        if (isOrquestraAttackOn)
        {
            print("El sistema ha originado un ataque de orquesta entre enemigo " + a.name + " y " + b.name);
        }
    }

    void OrquestraMessage_UnlockCooldown()
    {
        print(" La ventana de tiempo de bloqueo de ataque en orquesta ha terminado");
    }
    
    void OrquestraMessage_UnlockAttack()
    {
        print("El número mínimo de ataques simples para desbloquear un ataque orquesta ha sido alcanzado");
    }

    #endregion

  
}
