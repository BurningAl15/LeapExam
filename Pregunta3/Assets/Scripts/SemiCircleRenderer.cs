using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SemiCircleRenderer : MonoBehaviour
{
    [Header("Radio")] 
    [Tooltip("Radio (r)")] 
    [SerializeField] private float radius;

    [Header("Número de puntos")]
    [Tooltip("Número de puntos (n)")]
    [SerializeField] private int pointsNumber;

    [SerializeField] private bool useGameobjectAsCenter;

    [Header("Centro")]
    [Tooltip("Centro (P)")] 
    [SerializeField] private Vector3 centerPosition;

    [Header("Extras")]
    [Tooltip("1) Eje en el que se muestra la semicircunferencia " + "2) Hay 3 opciones xy, xz, yz")]
    [SerializeField] private string axis;

    [FormerlySerializedAs("circles")] [SerializeField]
    private GameObject circlesPrefab;

    [FormerlySerializedAs("resetPos")] [SerializeField]
    private bool resetPosition;

    [SerializeField] private List<GameObject> points = new List<GameObject>();

    private void Start()
    {
        SetCenterAndPoints();
    }
    
    private void Update()
    {
        if (!resetPosition)
        {
            ResetPoints();
            SetCenterAndPoints();
            resetPosition = true;
        }
    }

    private void PlacePoints(float _radius, Vector3 _centerPosition, int _pointsNumber, string _axis)
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
        
        Instante_Point("CenterPoint",_centerPosition,true);
        
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
            
            Instante_Point("Point-" + i,_centerPosition + position,false);
        }
    }

    #region Utils
    private void SetCenterAndPoints()
    {
        var center = useGameobjectAsCenter ? transform.position : centerPosition;
        PlacePoints(radius, center, pointsNumber, axis);
    }
    
    private void Instante_Point(string _name,Vector3 _centerPosition, bool _changeColor=false)
    {
        var point = Instantiate(circlesPrefab, _centerPosition, Quaternion.identity);
        point.name = _name;
        if(_changeColor)
            point.GetComponent<SpriteRenderer>().color=Color.Lerp(Color.red,Color.white,.5f);
        point.transform.parent = transform;
        points.Add(point);
    }    
    
    private void ResetPoints()
    {
        for (var i = 0; i < points.Count; i++) Destroy(points[i]);
        points.Clear();
    }
    #endregion
}