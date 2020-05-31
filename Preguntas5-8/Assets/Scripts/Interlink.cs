using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interlink : MonoBehaviour
{
    public static Interlink _instance;
    
    private bool firstInterlink;
    private bool secondInterlink;

    private Enemy[] tempEnemies;
    [SerializeField] List<Enemy> inactiveEnemies=new List<Enemy>();
    [SerializeField] List<Enemy> activeEnemies=new List<Enemy>();

    [SerializeField] private CharacterController2D player;
    private Coroutine currentCoroutine = null;

    private int n = 0;

    [Range(0,1)]
    [SerializeField] private float percentageChance1;
    [Range(0,1)]
    [SerializeField] private float percentageChance2;
    
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance !=null)
            Destroy(this.gameObject);

        tempEnemies = FindObjectsOfType<Enemy>();

        percentageChance1 = 1 - percentageChance1;
        percentageChance2 = 1 - percentageChance2;
        
        inactiveEnemies = tempEnemies.ToList();
    }

    public void TryInterlink(Enemy _hittedEnemy)
    {
        if (!firstInterlink)
        {
            if (Random.value > percentageChance1)
            {
                firstInterlink = true;
                activeEnemies.Add(_hittedEnemy);
                inactiveEnemies.Remove(_hittedEnemy);
                int randomEnemy = Random.Range(0, inactiveEnemies.Count);
                CallSpark(_hittedEnemy,inactiveEnemies[randomEnemy]);
            }
        }
    }
    void CallSpark(Enemy a,Enemy b)
    {
        print("Call Spark");
        if (currentCoroutine == null)
            currentCoroutine = StartCoroutine(SparkMovement(.5f, a, b));
    }

    IEnumerator SparkMovement(float _duration, Enemy a, Enemy b)
    {
        GameObject spark = ObjectPooler._instance.GetPooledObject("Spark");
        float maxDuration=_duration;
        
        activeEnemies.Add(b);
        inactiveEnemies.Remove(b);

        Vector3 center = CenterPoint(b.transform.position, a.transform.position, .5f);
        
        
        while (_duration>0)
        {
            if (spark != null)
            {
                // spark.transform.position = Vector3.Lerp(a.transform.position,b.transform.position,_duration/maxDuration);
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

        if (!secondInterlink)
        {
            if (Random.value > percentageChance2)
            {
                _duration = maxDuration;

                int randomEnemy = Random.Range(0, inactiveEnemies.Count);
                
                center = CenterPoint(inactiveEnemies[randomEnemy].transform.position, b.transform.position, .5f);
                
                spark = ObjectPooler._instance.GetPooledObject("Spark");
                while (_duration>0)
                {
                    if (spark != null)
                    {
                        spark.transform.position =SimpleBezier(inactiveEnemies[randomEnemy].transform.position, center,b.transform.position,_duration/maxDuration);
                        spark.transform.rotation = Quaternion.identity;
                        spark.SetActive(true);
                    }
            
                    _duration -= Time.fixedDeltaTime;
                    yield return null;
                }
                inactiveEnemies[randomEnemy].GetComponent<Enemy>().DoDamage(10);
                
                activeEnemies.Add(inactiveEnemies[randomEnemy]);
                inactiveEnemies.Remove(inactiveEnemies[randomEnemy]);
                
                spark.SetActive(false);

                print("Spark 2 --------- Moving");

                secondInterlink = true;
            }
        }

        inactiveEnemies = tempEnemies.ToList();

        // activeEnemies.Clear();

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

        // return Vector3.Lerp(a, b, duration);
        return temp;
    }
    
}
