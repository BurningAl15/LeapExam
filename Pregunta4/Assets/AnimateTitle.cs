using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AnimateTitle : MonoBehaviour
{
    [SerializeField] private List<Image> images = new List<Image>();

    [SerializeField] private int imagesIndex = 0;

    private bool breakLoop = false;

    private int colorIndex = 0;

    private Color currentColor;
    private Color nextColor;

    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject imageSlide;

    private Vector3 initialPosition, endPosition;

    private void Start()
    {
        ColorAsigner();

        var tempImgSlidePos = imageSlide.GetComponent<RectTransform>().localPosition;

        initialPosition = Vector3.one;
        endPosition = Vector3.right * 1980;

        StartCoroutine(AnimateUI(.2f));
    }

    public void BreakAnimateUILoop()
    {
        breakLoop = true;
        StopCoroutine("AnimateUI");

        currentColor = currentColor != Color.white ? images[imagesIndex].color : Color.white;
        nextColor = ColorUtils._instance.GetCurrentColor_UI(Random.Range(0, ColorUtils._instance.GetColorsCount()));

        StartCoroutine(ButtonPress(.05f));
    }

    
    #region Coroutines

    private IEnumerator AnimateUI(float _time)
    {
        while (!breakLoop)
            if (imagesIndex < images.Count)
            {
                for (float i = 0; i < _time; i += Time.deltaTime)
                {
                    images[imagesIndex].color = Color.Lerp(currentColor, nextColor, i / _time);
                    yield return null;
                }

                images[imagesIndex].color = Color.Lerp(currentColor, nextColor, 1);
                imagesIndex++;
            }
            else if (imagesIndex >= images.Count)
            {
                imagesIndex = 0;
                colorIndex++;
                if (colorIndex >= ColorUtils._instance.GetColorsCount()) colorIndex = 0;
                ColorAsigner();
            }

        images[imagesIndex].color = nextColor;
    }

    private IEnumerator ButtonPress(float _time)
    {
        for (var i = 0; i < images.Count; i++)
        {
            for (float j = 0; j < _time; j += Time.deltaTime)
            {
                images[i].color = Color.Lerp(currentColor, nextColor, j / _time);
                yield return null;
            }

            images[i].color = Color.Lerp(currentColor, nextColor, 1);
        }

        UI.SetActive(false);

        for (float j = 0; j < 1; j += Time.deltaTime)
        {
            imageSlide.GetComponent<RectTransform>().localPosition = Vector3.Lerp(initialPosition, endPosition, j);
            yield return null;
        }

        GridChipChecker._instance.ChangeToPlay();
        StopCoroutine("ButtonPress");
    }

    #endregion

    
    #region Utils

    private void ColorAsigner()
    {
        currentColor = currentColor != Color.white ? images[imagesIndex].color : Color.white;
        nextColor = ColorUtils._instance.GetCurrentColor_UI(colorIndex);
    }

    #endregion
}