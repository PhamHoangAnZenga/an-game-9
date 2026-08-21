using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class NativeKeyboardController : MonoBehaviour
{
    [SerializeField] RectTransform _safeArea;

    [SerializeField] RectTransform _topUnsafe;
    [SerializeField] RectTransform _bottomUnsafe;
    [SerializeField] RectTransform _leftUnsafe;
    [SerializeField] RectTransform _rightUnsafe;

    [SerializeField] RectTransform _inputField;
    [SerializeField] TMP_InputField _inputFieldText;

    float _width;
    float _height;

    void Start()
    {
        _width = Screen.width;
        _height = Screen.height;
        FixSafeArea();
    }

    public void OnEnter()
    {
        _inputFieldText.text = "";
    }
    
    public async void OnSelectInputField()
    {

        float safeLeft = Screen.safeArea.x;
        float safeBottom = Screen.safeArea.y;
        float safeRight = safeLeft + Screen.safeArea.width;
        float safeTop = safeBottom + Screen.safeArea.height;
                
        if (TouchScreenKeyboard.isSupported)
        {
            while(!TouchScreenKeyboard.visible)
            {
                await Task.Yield();
            }
            while(!(TouchScreenKeyboard.area.height > 0) )
            {
                await Task.Yield();
            }    
            safeBottom = TouchScreenKeyboard.area.height;
        }

        float leftRatio = safeLeft / _width;
        float bottomRatio = safeBottom / _height;
        float rightRatio = (_width - safeRight) / _width;
        float topRatio = (_height - safeTop) / _height;

        _safeArea.anchorMin = new Vector2(leftRatio, bottomRatio);
        _safeArea.anchorMax = new Vector2(1 - rightRatio, 1 - topRatio);

        SetUnsafeArea(leftRatio, bottomRatio, rightRatio, topRatio);

        _inputField.anchoredPosition = new Vector2(_inputField.anchoredPosition.x, 0);
    }

    public async void OnDeselectInputField()
    {

        float safeLeft = Screen.safeArea.x;
        float safeBottom = Screen.safeArea.y;
        float safeRight = safeLeft + Screen.safeArea.width;
        float safeTop = safeBottom + Screen.safeArea.height;
                
        if (TouchScreenKeyboard.isSupported)
        {
            while(TouchScreenKeyboard.visible)
            {
                await Task.Yield();
            }
            while(TouchScreenKeyboard.area.height > 0.01f )
            {
                await Task.Yield();
            }    
        }

        float leftRatio = safeLeft / _width;
        float bottomRatio = safeBottom / _height;
        float rightRatio = (_width - safeRight) / _width;
        float topRatio = (_height - safeTop) / _height;

        _safeArea.anchorMin = new Vector2(leftRatio, bottomRatio);
        _safeArea.anchorMax = new Vector2(1 - rightRatio, 1 - topRatio);

        SetUnsafeArea(leftRatio, bottomRatio, rightRatio, topRatio);
        _inputField.anchoredPosition = new Vector2(_inputField.anchoredPosition.x, 50);
    }

    void FixSafeArea()
    {
        float safeLeft = Screen.safeArea.x;
        float safeBottom = Screen.safeArea.y;
        float safeRight = safeLeft + Screen.safeArea.width;
        float safeTop = safeBottom + Screen.safeArea.height;

        float leftRatio = safeLeft / _width;
        float bottomRatio = safeBottom / _height;
        float rightRatio = (_width - safeRight) / _width;
        float topRatio = (_height - safeTop) / _height;

        _safeArea.anchorMin = new Vector2(leftRatio, bottomRatio);
        _safeArea.anchorMax = new Vector2(1 - rightRatio, 1 - topRatio);

        SetUnsafeArea(leftRatio, bottomRatio, rightRatio, topRatio);

    }

    void SetUnsafeArea(float leftRatio, float bottomRatio, float rightRatio, float topRatio)
    {
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
