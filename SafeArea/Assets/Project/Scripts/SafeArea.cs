using System;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;

    [SerializeField] RectTransform _topUnsafe;
    [SerializeField] RectTransform _bottomUnsafe;

    public void ShowInfo()
    {
        Debug.Log($"witdh: {Screen.width} , height: {Screen.height}");
        Debug.Log(Screen.safeArea);
    }

    public void FixSafeArea()
    {
        float height = Screen.height;
        float width = Screen.width;
        float safeHeight = Screen.safeArea.x;
        float safeWidth = Screen.safeArea.y;

        Debug.Log(safeHeight / height + " " + safeWidth / width);
        float heightRatio = safeHeight / height / 2;
        float widthRatio = safeWidth / width / 2;

        _rectTransform.anchorMax = new Vector2(1 - heightRatio, 1 - widthRatio);
        _rectTransform.anchorMin = new Vector2(heightRatio, widthRatio);

        SetUnsafeArea();
    }

    void SetUnsafeArea()
    {
        _topUnsafe = new Vector2();
    }
}
 