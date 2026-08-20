using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;

    [SerializeField] RectTransform _topUnsafe;
    [SerializeField] RectTransform _bottomUnsafe;
    [SerializeField] RectTransform _leftUnsafe;
    [SerializeField] RectTransform _rightUnsafe;

    void Start()
    {
        FixSafeArea();
    }

    void FixSafeArea()
    {
        float width = Screen.width;
        float height = Screen.height;

        float safeLeft = Screen.safeArea.x;
        float safeBottom = Screen.safeArea.y;
        float safeRight = safeLeft + Screen.safeArea.width;
        float safeTop = safeBottom + Screen.safeArea.height;

        float leftRatio = safeLeft / width;
        float bottomRatio = safeBottom / height;
        float rightRatio = (width - safeRight) / width;
        float topRatio = (height - safeTop) / height;

        _rectTransform.anchorMin = new Vector2(leftRatio, bottomRatio);
        _rectTransform.anchorMax = new Vector2(1 - rightRatio, 1 - topRatio);

        SetUnsafeArea(leftRatio, bottomRatio, rightRatio, topRatio);

    }

    void SetUnsafeArea(float leftRatio, float bottomRatio, float rightRatio, float topRatio)
    {
        Debug.Log(leftRatio + " " + bottomRatio + " " + rightRatio + " " + topRatio);
        _leftUnsafe.anchorMax = new Vector2(leftRatio, 1);
        _leftUnsafe.anchorMin = new Vector2(0, 0);

        _bottomUnsafe.anchorMax = new Vector2(1, bottomRatio);
        _bottomUnsafe.anchorMin = new Vector2(0, 0);

        _rightUnsafe.anchorMax = new Vector2(1, 1);
        _rightUnsafe.anchorMin = new Vector2(1 - rightRatio, 0);

        _topUnsafe.anchorMax = new Vector2(1, 1);
        _topUnsafe.anchorMin = new Vector2(0, 1 - topRatio);

    }

}
