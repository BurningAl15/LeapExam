using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    [SerializeField] private CharacterController2D player;
    [SerializeField] private Vector3 pos;
    [SerializeField] List<Enemy> activeEnemies=new List<Enemy>();
    [SerializeField] private Vector3 forward;

    private int selectedIndex;
    [SerializeField] private float maxDistance=20;
    private Enemy[] tempEnemies;
    private void Awake()
    {
        pos = player.transform.position;
        forward = Vector3.right;
        
        tempEnemies = FindObjectsOfType<Enemy>();

        FilterEnemies();

        ActivateEnemies();
        forward = LookingDirection(activeEnemies[selectedIndex]);
    }

    void FilterEnemies()
    {
        //Sort enemies by distance
        tempEnemies = tempEnemies.OrderBy(c => GetDistance(c)).ToArray();

        activeEnemies.Clear();
        
        //Remove enemies by direction
        for (int i = 0; i < tempEnemies.Length; i++)
        {
            if (IsInRange(tempEnemies[i]))
            {
                if(IsInVisionField(tempEnemies[i],forward,90))
                    activeEnemies.Add(tempEnemies[i]);
            }
            else
            {
                DeactivateByIndex(i);
            }
        }
    }
    
    Vector3 LookingDirection(Enemy _tempEnemy)
    {
        Vector3 _tempCurrentDirection = _tempEnemy.transform.position - pos;

        return _tempCurrentDirection;
    }

    bool IsInRange(Enemy _tempEnemy)
    {
        float _tempDistance = Vector3.Distance(_tempEnemy.transform.position, pos);
        return _tempDistance<maxDistance?true:false;
    }
    
    float GetDistance(Enemy _tempEnemy)
    {
        float _tempDistance = Vector3.Distance(_tempEnemy.transform.position, pos);
        return _tempDistance;
    }

    float GetVisionField(Enemy _tempEnemy,Vector3 _direction)
    {
        return Vector3.Angle(LookingDirection(_tempEnemy), _direction);
    }
    
    private bool IsInVisionField(Enemy _tempEnemy,Vector3 _direction,float _visiondAngle)
    {
        var _angle = GetVisionField(_tempEnemy, _direction);
        
        return _angle >= -_visiondAngle &&
               _angle <= _visiondAngle
            ? true
            : false;
    }
    
    private void Update()
    {
        pos = player.transform.position;
        
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            Last();
        else if(Input.GetKeyDown(KeyCode.RightArrow))
            Next();

        if (Input.GetKeyDown(KeyCode.A))
        {
            forward = Vector3.left * LookingDirection(activeEnemies[selectedIndex]).x;
        }
            
        else if (Input.GetKeyDown(KeyCode.D))
        {
            forward = Vector3.right * LookingDirection(activeEnemies[selectedIndex]).x;
        }

        Debug.DrawRay(pos,Quaternion.Euler(GetVisionField(activeEnemies[selectedIndex],forward), 0, 0)*forward,Color.red );
        Debug.DrawRay(pos,Quaternion.Euler(0,  0,-45)*forward ,Color.green);
        Debug.DrawRay(pos,Quaternion.Euler(0,  0,45)*forward ,Color.yellow);

        
        Debug.DrawRay(pos,LookingDirection(activeEnemies[selectedIndex]));
    }

    void Next()
    {
        FilterEnemies();
        
        selectedIndex++;
        if (selectedIndex > activeEnemies.Count-1)
            selectedIndex = 0;

        ActivateEnemies();
    }

    void Last()
    {
        FilterEnemies();
        
        selectedIndex--;
        if (selectedIndex < 0)
            selectedIndex = activeEnemies.Count -1;

        ActivateEnemies();
    }

    void ActivateEnemies()
    {
        if (activeEnemies.Count > 0)
        {
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                activeEnemies[i].DeactivateEnemy();
            }
            activeEnemies[selectedIndex].ActivateEnemy();
        }
        else
        {
            DeactivateAllEnemies();
        }
    }

    void DeactivateAllEnemies()
    {
        for (int i = 0; i < tempEnemies.Length; i++)
        {
            tempEnemies[i].DeactivateEnemy();
        }
        Debug.Log("Calling");
    }

    void DeactivateByIndex(int _enemyIndex)
    {
        tempEnemies[_enemyIndex].DeactivateEnemy();
    }
}
