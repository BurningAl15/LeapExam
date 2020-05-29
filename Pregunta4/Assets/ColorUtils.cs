using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtils : MonoBehaviour
{
    public static ColorUtils _instance;

    [SerializeField] private List<Color> colors = new List<Color>();

    private Color currentColor;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance!=null)
            Destroy(this.gameObject);
    }

    public Color GetCurrentColor_UI(int colorIndex)
    {
        return colors[colorIndex];
    }

    public void RandomizeColor()
    {
        currentColor = colors[Random.Range(0, colors.Count)];
    }
    
    public Color GetCurrentColor_InGame()
    {
        return currentColor;
    }

    public int GetColorsCount()
    {
        return colors.Count;
    }
}
