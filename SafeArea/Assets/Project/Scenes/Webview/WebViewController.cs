using System.Threading.Tasks;
using Gree.UnityWebView;
using TMPro;
using UnityEngine;

public class WebViewController : MonoBehaviour
{
    [SerializeField] RectTransform _safeArea;

    [SerializeField] RectTransform _topUnsafe;
    [SerializeField] RectTransform _bottomUnsafe;
    [SerializeField] RectTransform _leftUnsafe;
    [SerializeField] RectTransform _rightUnsafe;

    [SerializeField] RectTransform _inputField;
    [SerializeField] TMP_InputField _inputFieldText;


    [SerializeField] RectTransform _header;
    [SerializeField] RectTransform _footer;
    [SerializeField] RectTransform _webview;

    float _width;
    float _height;

    WebViewObject _webView;

    float headerHeight = 150;

    void Start()
    {
        _width = Screen.width;
        _height = Screen.height;
        FixSafeArea();
    }

    public void CloseWebview()
    {
        Destroy(_webView);
        _header.gameObject.SetActive(false);
        _footer.gameObject.SetActive(false);
        _webview.gameObject.SetActive(false);

        _inputField.gameObject.SetActive(true);
    }

    public void OnEnter()
    {
        _inputFieldText.text = "";
        _inputField.gameObject.SetActive(false);

        float safeLeftBorder = Screen.safeArea.x;
        float safeBottomBorder = Screen.safeArea.y;
        float safeRightBorder = _width - safeLeftBorder - Screen.safeArea.width;
        float safeTopBorder = _height - safeBottomBorder - Screen.safeArea.height;

        float leftPercentBorder = safeLeftBorder / _width;
        float bottomPercentBorder = safeBottomBorder / _height;
        float rightPercentBorder = safeRightBorder / _width;
        float topPercentBorder = safeTopBorder / _height;

        float topPercentBorderWebView = (Mathf.RoundToInt(safeTopBorder) + headerHeight) / _height;
        float bottomPercentBorderWebView = (Mathf.RoundToInt(safeBottomBorder) + headerHeight) / _height; ;
        float rightPercentBorderWebView = Mathf.RoundToInt(safeRightBorder) / _width;
        float leftPercentBorderWebView = Mathf.RoundToInt(safeLeftBorder) / _width;

        _header.anchorMax = new Vector2(1 - rightPercentBorderWebView, 1 - topPercentBorder);
        _header.anchorMin = new Vector2(leftPercentBorderWebView, 1 - topPercentBorderWebView);

        _footer.anchorMax = new Vector2(1 - rightPercentBorderWebView, bottomPercentBorderWebView);
        _footer.anchorMin = new Vector2(leftPercentBorderWebView, bottomPercentBorder);

        _webview.anchorMax = new Vector2(1 - rightPercentBorder, 1 - topPercentBorderWebView);
        _webview.anchorMin = new Vector2(leftPercentBorder, bottomPercentBorderWebView);

        _header.gameObject.SetActive(true);
        _footer.gameObject.SetActive(true);
        _webview.gameObject.SetActive(true);

        int top = Mathf.RoundToInt(_height * topPercentBorderWebView);
        int bottom = Mathf.RoundToInt(_height * bottomPercentBorderWebView);
        int left = Mathf.RoundToInt(_width * leftPercentBorderWebView);
        int right = Mathf.RoundToInt(_width * rightPercentBorderWebView);

        CreateWebView(left, top, right, bottom);
    }

    public async void OnSelectInputField()
    {

        float safeLeftBorder = Screen.safeArea.x;
        float safeBottomBorder = Screen.safeArea.y;
        float safeRightBorder = safeLeftBorder + Screen.safeArea.width;
        float safeTopBorder = safeBottomBorder + Screen.safeArea.height;

        if (TouchScreenKeyboard.isSupported)
        {
            while (!TouchScreenKeyboard.visible)
            {
                await Task.Yield();
            }
            while (!(TouchScreenKeyboard.area.height > 0))
            {
                await Task.Yield();
            }
            safeBottomBorder = TouchScreenKeyboard.area.height;
        }

        float leftPercentBorder = safeLeftBorder / _width;
        float bottomPercentBorder = safeBottomBorder / _height;
        float rightPercentBorder = (_width - safeRightBorder) / _width;
        float topPercentBorder = (_height - safeTopBorder) / _height;

        _safeArea.anchorMin = new Vector2(leftPercentBorder, bottomPercentBorder);
        _safeArea.anchorMax = new Vector2(1 - rightPercentBorder, 1 - topPercentBorder);

        SetUnsafeArea(leftPercentBorder, bottomPercentBorder, rightPercentBorder, topPercentBorder);

        _inputField.anchoredPosition = new Vector2(_inputField.anchoredPosition.x, 0);
    }

    public async void OnDeselectInputField()
    {

        float safeLeftBorder = Screen.safeArea.x;
        float safeBottomBorder = Screen.safeArea.y;
        float safeRightBorder = safeLeftBorder + Screen.safeArea.width;
        float safeTopBorder = safeBottomBorder + Screen.safeArea.height;

        if (TouchScreenKeyboard.isSupported)
        {
            while (TouchScreenKeyboard.visible)
            {
                await Task.Yield();
            }
            while (TouchScreenKeyboard.area.height > 0.01f)
            {
                await Task.Yield();
            }
        }

        float leftPercentBorder = safeLeftBorder / _width;
        float bottomPercentBorder = safeBottomBorder / _height;
        float rightPercentBorder = (_width - safeRightBorder) / _width;
        float topPercentBorder = (_height - safeTopBorder) / _height;

        _safeArea.anchorMin = new Vector2(leftPercentBorder, bottomPercentBorder);
        _safeArea.anchorMax = new Vector2(1 - rightPercentBorder, 1 - topPercentBorder);

        SetUnsafeArea(leftPercentBorder, bottomPercentBorder, rightPercentBorder, topPercentBorder);
        _inputField.anchoredPosition = new Vector2(_inputField.anchoredPosition.x, 50);
    }

    void FixSafeArea()
    {
        float safeLeftBorder = Screen.safeArea.x;
        float safeBottomBorder = Screen.safeArea.y;
        float safeRightBorder = safeLeftBorder + Screen.safeArea.width;
        float safeTopBorder = safeBottomBorder + Screen.safeArea.height;

        float leftPercentBorder = safeLeftBorder / _width;
        float bottomPercentBorder = safeBottomBorder / _height;
        float rightPercentBorder = (_width - safeRightBorder) / _width;
        float topPercentBorder = (_height - safeTopBorder) / _height;

        _safeArea.anchorMin = new Vector2(leftPercentBorder, bottomPercentBorder);
        _safeArea.anchorMax = new Vector2(1 - rightPercentBorder, 1 - topPercentBorder);

        SetUnsafeArea(leftPercentBorder, bottomPercentBorder, rightPercentBorder, topPercentBorder);

    }

    void SetUnsafeArea(float leftPercentBorder, float bottomPercentBorder, float rightPercentBorder, float topPercentBorder)
    {
        _leftUnsafe.anchorMax = new Vector2(leftPercentBorder, 1);
        _leftUnsafe.anchorMin = new Vector2(0, 0);

        _bottomUnsafe.anchorMax = new Vector2(1, bottomPercentBorder);
        _bottomUnsafe.anchorMin = new Vector2(0, 0);

        _rightUnsafe.anchorMax = new Vector2(1, 1);
        _rightUnsafe.anchorMin = new Vector2(1 - rightPercentBorder, 0);

        _topUnsafe.anchorMax = new Vector2(1, 1);
        _topUnsafe.anchorMin = new Vector2(0, 1 - topPercentBorder);

    }

    void CreateWebView(int left, int top, int right, int bottom)
    {
        _width = Screen.width;
        _height = Screen.height;

        _webView = new GameObject("WebView").AddComponent<WebViewObject>();

        _webView.Init();

        _webView.LoadURL("https://www.google.com");

        _webView.SetMargins(left, top, right, bottom);

        _webView.SetVisibility(true);

    }
}