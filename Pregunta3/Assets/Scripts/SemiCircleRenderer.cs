using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiCircleRenderer : MonoBehaviour
{
    [SerializeField] private float radius;

    [SerializeField] private int pointsNumber;

    [SerializeField] private string axis;
    [SerializeField] private GameObject circles;

    private void Start()
    {
        PlacePoints(radius, transform.position, pointsNumber, axis);
    }

    public void PlacePoints(float _radius, Vector3 _centerPosition, int _pointsNumber, string _axis)
    {
        var lookDirection = Vector3.zero;
        switch (_axis)
        {
            case "xy":
                lookDirection = Vector3.forward;
                break;
            case "yz":
                lookDirection = Vector3.right;
                break;
            case "xz":
                lookDirection = Vector3.up;
                break;
        }

        var centerDirection = Quaternion.LookRotation(lookDirection * _radius);

        for (var i = 0; i < _pointsNumber; i++)
        {
            var angle = Mathf.PI * (i + 1) / (_pointsNumber + 1f);

            var axis1 = Mathf.Sin(angle) * _radius;
            var axis2 = Mathf.Cos(angle) * _radius;

            var position = Vector3.zero;

            switch (_axis)
            {
                case "xy":
                    position = new Vector3(axis1, axis2, _centerPosition.z);
                    break;
                case "xz":
                    position = new Vector3(axis1, _centerPosition.y, axis2);
                    break;
                case "yz":
                    position = new Vector3(_centerPosition.x, axis1, axis2);
                    break;
            }

            position = centerDirection * position;
            var circle = Instantiate(circles, _centerPosition + position, Quaternion.identity);
            circle.transform.localScale = Vector3.one * .25f;
        }
    }
}
