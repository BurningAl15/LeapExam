using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipLogic : MonoBehaviour
{
    public enum ChipType
    {
        Single,
        Double
    }

    [Header("Chip Type")]
    [Tooltip("Tipos de chips: Las que tienen una sola salida (Single), las que tienen dos (Double)")]
    public ChipType chipType;

    [Tooltip("Colliders hijos (Single - 1 Collider)/(Double - 2 Colliders)")] [SerializeField]
    private List<ColliderChecker> chipColliders = new List<ColliderChecker>();

    private int connectedElementsCounter = 0;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool hasBeenConnected = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #region Chip Logic Utils

    public void ChangeColor(Color _color)
    {
        spriteRenderer.color = _color;
    }

    public int ConnectedElementsCounter
    {
        get => connectedElementsCounter;
        set => connectedElementsCounter = value;
    }

    public bool HasBeenConnected
    {
        get => hasBeenConnected;
        set => hasBeenConnected = value;
    }

    #endregion
}