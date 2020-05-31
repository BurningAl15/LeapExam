using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrquestraDirector : MonoBehaviour
{
    public static OrquestraDirector _instance;

    [SerializeField] private bool someoneAttacked = false;
    [SerializeField] private bool isOrquestraAttackOn = false;
    
    private Enemy[] tempEnemies;
    [SerializeField] List<Enemy> activeEnemies=new List<Enemy>();
    
    [SerializeField] float maxDistance;
    [SerializeField] private CharacterController2D player;

    private Coroutine currentCoroutine = null;

    [SerializeField] private int n;
    
    [SerializeField] private float t ;
    private float orquestraAttackCooldownTimer = 0;
    
    [Range(0,1)]
    [SerializeField] private float r;
    
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

    }
    
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

    public bool SomeoneAttacked
    {
        get => someoneAttacked;
        set => someoneAttacked = value;
    }

    public void Wait_ForAttack(float _duration)
    {
        if (currentCoroutine == null)
            currentCoroutine = StartCoroutine(WaitForAttack(_duration));
    }

    void OrquestraAttack()
    {
        
    }
    
    IEnumerator WaitForAttack(float _duration)
    {
        someoneAttacked = true;
        if (Random.value > r)
        {
            isOrquestraAttackOn = true;
        }
        while (_duration > 0)
        {
            _duration -= Time.fixedDeltaTime;
            yield return null;
        }
        yield return null;
        someoneAttacked = false;
        currentCoroutine = null;
    }
}
