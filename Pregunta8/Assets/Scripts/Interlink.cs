using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Interlink : MonoBehaviour
{
    public static Interlink _instance;
    
    private bool firstInterlink;
    private bool secondInterlink;

    private Enemy[] tempEnemies;
    [SerializeField] List<Enemy> linkedEnemies=new List<Enemy>();
    [SerializeField] List<Enemy> activeEnemies=new List<Enemy>();

    [SerializeField] private CharacterController2D player;
    private Coroutine currentCoroutine = null;

    [SerializeField] private int n = 0;

    [Range(0,1)]
    [SerializeField] private float percentageChance1;
    [Range(0,1)]
    [SerializeField] private float percentageChance2;

    [SerializeField] private float maxDistance;

    [SerializeField] private bool testing;
    
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance !=null)
            Destroy(this.gameObject);

        tempEnemies = FindObjectsOfType<Enemy>();

        if (testing)
        {
            percentageChance1 = 0;
            percentageChance2 = 0;
        }
        else
        {
            percentageChance1 = 1 - percentageChance1;
            percentageChance2 = 1 - percentageChance2;
        }
        
        FilterEnemies();
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

    public void TryInterlink(Enemy _hittedEnemy)
    {
        if (!firstInterlink)
        {
            if (Random.value > percentageChance1)
            {
                firstInterlink = true;
                if(!linkedEnemies.Contains(_hittedEnemy))
                    linkedEnemies.Add(_hittedEnemy);
                activeEnemies.Remove(_hittedEnemy);
                // n++;
                int randomEnemy = Random.Range(0, activeEnemies.Count);
                CallSpark(_hittedEnemy,activeEnemies[randomEnemy]);
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
    
    void CallSpark(Enemy a,Enemy b)
    {
        print("Call Spark");
        if (currentCoroutine == null)
            currentCoroutine = StartCoroutine(SparkMovement(.5f, a, b));
    }

    IEnumerator SparkMovement(float _duration, Enemy a, Enemy b)
    {
        if(!linkedEnemies.Contains(b))
            linkedEnemies.Add(b);
        n++;
        activeEnemies.Remove(b);
        
        bool contains = false;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            for (int j = 0; j < linkedEnemies.Count; j++)
            {
                if (activeEnemies[i].id == linkedEnemies[j].id)
                    contains = true;
                break;
            }
        }

        if (!contains)
        {
            GameObject spark = ObjectPooler._instance.GetPooledObject("Spark");
            float maxDuration=_duration;

            Vector3 center = CenterPoint(b.transform.position, a.transform.position, .5f);
            
            //Spark moves from a point to other in a curve
            while (_duration>0)
            {
                if (spark != null)
                {
                    spark.transform.position =SimpleBezier(b.transform.position, center,a.transform.position,_duration/maxDuration);
                    spark.transform.rotation = Quaternion.identity;
                    spark.SetActive(true);
                }
                
                _duration -= Time.fixedDeltaTime;
                yield return null;
            }

            b.GetComponent<Enemy>().DoDamage(10);
            
            spark.SetActive(false);
            
            yield return null;
            print("Spark Moving");

            int randomEnemy = Random.Range(0, activeEnemies.Count);

            if (!secondInterlink)
            {
                if (randomEnemy < activeEnemies.Count)
                {
                    if (Random.value > percentageChance2)
                    {
                        _duration = maxDuration;
                        
                        center = CenterPoint(activeEnemies[randomEnemy].transform.position, a.transform.position, .5f);
                    
                        spark = ObjectPooler._instance.GetPooledObject("Spark");
                    
                        while (_duration>0)
                        {
                            if (spark != null)
                            {
                                spark.transform.position = SimpleBezier(activeEnemies[randomEnemy].transform.position, center,a.transform.position,_duration/maxDuration);
                                spark.transform.rotation = Quaternion.identity;
                                spark.SetActive(true);
                            }
                
                            _duration -= Time.fixedDeltaTime;
                            yield return null;
                        }
                        //When spark arrives to active enemy's position, do damage and turn off
                        spark.SetActive(false);
                        activeEnemies[randomEnemy].GetComponent<Enemy>().DoDamage(10);
                    
                        if(!linkedEnemies.Contains(activeEnemies[randomEnemy]))
                            linkedEnemies.Add(activeEnemies[randomEnemy]);
                        activeEnemies.Remove(activeEnemies[randomEnemy]);
                        n++;

                        print("Spark 2 --------- Moving");

                        secondInterlink = true;
                    }
                }
                else
                {
                    linkedEnemies.Clear();
                    n = linkedEnemies.Count;
                }
            }
            FilterEnemies();
        }
        else
        {
            linkedEnemies.Clear();
            n = linkedEnemies.Count;
        }

        firstInterlink = false;
        secondInterlink = false;
        
        currentCoroutine = null;
    }

    Vector3 SimpleBezier(Vector3 a, Vector3 b, Vector3 c, float duration)
    {
        Vector3 m1 = Vector3.Lerp( a, b, duration );
        Vector3 m2 = Vector3.Lerp( b, c, duration );
        return  Vector3.Lerp(m1, m2, duration);
    }

    Vector3 CenterPoint(Vector3 a, Vector3 b, float duration)
    {
        Vector3 temp = new Vector3(Mathf.Lerp(a.x, b.x, duration), Mathf.Lerp(a.y, b.y, duration) + 2, 0);
        return temp;
    }
    
}
