using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CollidableObject : MonoBehaviour
{
    [Header("Collision Bounds")] [SerializeField]
    private float width;

    [SerializeField] private float height;

    [Header("Collisionable Objects")] [FormerlySerializedAs("elementB")] [SerializeField]
    private GameObject rectangle_B;

    [FormerlySerializedAs("elementC")] [SerializeField]
    private GameObject rectangle_C;

    private bool isColliding1 = false;
    private bool isColliding2 = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        CheckCollision(rectangle_B, rectangle_C);
        ChangeColor();
    }

    #region Check Collisions

    private void CheckCollision(GameObject _otherObject_1, GameObject _otherObject_2)
    {
        if (_otherObject_1.transform.position.x < transform.position.x + width &&
            _otherObject_1.transform.position.x > transform.position.x - width &&
            _otherObject_1.transform.position.y < transform.position.y + height &&
            _otherObject_1.transform.position.y > transform.position.y - height)
        {
            isColliding1 = true;
            print("Is Colliding: " + _otherObject_1.name);

            if (_otherObject_2.transform.position.x < transform.position.x + width &&
                _otherObject_2.transform.position.x > transform.position.x - width &&
                _otherObject_2.transform.position.y < transform.position.y + height &&
                _otherObject_2.transform.position.y > transform.position.y - height)
            {
                isColliding1 = false;
                isColliding2 = true;
                print("Is Double Colliding! ");
            }
        }
        else if (_otherObject_2.transform.position.x < transform.position.x + width &&
                 _otherObject_2.transform.position.x > transform.position.x - width &&
                 _otherObject_2.transform.position.y < transform.position.y + height &&
                 _otherObject_2.transform.position.y > transform.position.y - height)
        {
            isColliding1 = true;
            print("Is Colliding: " + _otherObject_2.name);

            if (_otherObject_1.transform.position.x < transform.position.x + width &&
                _otherObject_1.transform.position.x > transform.position.x - width &&
                _otherObject_1.transform.position.y < transform.position.y + height &&
                _otherObject_1.transform.position.y > transform.position.y - height)
            {
                isColliding1 = false;
                isColliding2 = true;
                print("Is Double Colliding! ");
            }
        }
        else
        {
            isColliding1 = false;
            isColliding2 = false;
        }
    }

    #endregion
    

    #region Color and Rendering Utils

    public void ChangeColor()
    {
        if (isColliding1)
            spriteRenderer.color = new Color(0, 1, 0, .5f);
        else if (isColliding2)
            spriteRenderer.color = new Color(0, 0, 1, .5f);
        else
            spriteRenderer.color = new Color(1, 0, 0, .5f);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        if (isColliding1)
            Gizmos.color = new Color(0, 1, 0, .5f);
        else if (isColliding2)
            Gizmos.color = new Color(0, 0, 1, .5f);
        else
            Gizmos.color = new Color(1, 0, 0, .5f);

        Gizmos.DrawCube(transform.position, new Vector3(width, height, .1f));
    }    

    #endregion
}