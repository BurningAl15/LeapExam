using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{

    [SerializeField] private float width;
    [SerializeField] private float height;

    [SerializeField] private GameObject elementB;
    [SerializeField] private GameObject elementC;

    private bool isColliding1 = false;
    private bool isColliding2 = false;
    
    void Update()
    {
        Collider(elementB, elementC);
    }

    void Collider(GameObject _otherObject_1,GameObject _otherObject_2)
    {
        if (_otherObject_1.transform.position.x < this.transform.position.x + width &&
            _otherObject_1.transform.position.x > this.transform.position.x - width &&
            _otherObject_1.transform.position.y < this.transform.position.y + height &&
            _otherObject_1.transform.position.y > this.transform.position.y - height)
        {
            isColliding1 = true;
            print("Is Colliding: "+_otherObject_1.name);

            if (_otherObject_2.transform.position.x < this.transform.position.x + width &&
                _otherObject_2.transform.position.x > this.transform.position.x - width &&
                _otherObject_2.transform.position.y < this.transform.position.y + height &&
                _otherObject_2.transform.position.y > this.transform.position.y - height)
            {
                isColliding1 = false;
                isColliding2 = true;
                print("Is Double Colliding! ");
            }
            // return true;
        }
        else if (_otherObject_2.transform.position.x < this.transform.position.x + width &&
                 _otherObject_2.transform.position.x > this.transform.position.x - width &&
                 _otherObject_2.transform.position.y < this.transform.position.y + height &&
                 _otherObject_2.transform.position.y > this.transform.position.y - height)
        {
            isColliding1 = true;
            print("Is Colliding: "+_otherObject_2.name);
            // return true;
        }
        else
        {
            isColliding1 = false;
            isColliding2 = false;
            // return false;
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = !isColliding1? new Color(1, 0, 0, .5f) : new Color(0, 1, 0, .5f);
        Gizmos.color = new Color(1, 0, 0, .5f);
        if (isColliding1)
            Gizmos.color = new Color(0, 1, 0, .5f);
        else if(isColliding2)
            Gizmos.color = new Color(0, 0, 1, .5f);
        else 
            Gizmos.color = new Color(1, 0, 0, .5f);
        
        Gizmos.DrawCube(this.transform.position, new Vector3(width,height,.1f));
    }
}
