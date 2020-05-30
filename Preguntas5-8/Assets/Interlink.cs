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

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance !=null)
            Destroy(this.gameObject);

        tempEnemies = FindObjectsOfType<Enemy>();
        
        inactiveEnemies = tempEnemies.ToList();
    }

    public void TryInterlink(Enemy _hittedEnemy)
    {
        if (!firstInterlink)
        {
            if (Random.value > .75f)
            {
                firstInterlink = true;
                activeEnemies.Add(_hittedEnemy);
                inactiveEnemies.Remove(_hittedEnemy);
                int randomEnemy = Random.Range(0, inactiveEnemies.Count);
                inactiveEnemies[randomEnemy].Damage(10);
                activeEnemies.Add(inactiveEnemies[randomEnemy]);
                inactiveEnemies.Remove(inactiveEnemies[randomEnemy]);
                Ray();

                if (!secondInterlink)
                {
                    if (Random.value > 90)
                    {
                        secondInterlink = true;
                    }
                }
            }
            else
            {
                print("Not Active");
            }
        }
    }


    void Ray()
    {
        if (activeEnemies.Count > 0)
        {
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                if(i+1<activeEnemies.Count)
                    Debug.DrawLine(activeEnemies[i].transform.position,activeEnemies[i+1].transform.position,Color.red);
                
            }
        }
    }

}
