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

    [SerializeField] TextMeshProUGUI debugText;

    float _width;
    float _height;

    void Start()
    {
        debugText.text = IsKeyboardAreaSupported();
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
            debugText.text = Time.time.ToString() + '\n';
            while (!TouchScreenKeyboard.visible)
            {
                await Task.Yield();
            }
            debugText.text = debugText.text + Time.time.ToString() + '\n';
            while (!(GetKeyboardHeight() > 0))
            {
                await Task.Yield();
            }    
            
            debugText.text = debugText.text + Time.time.ToString() + '\n';
            safeBottom = GetKeyboardHeight();
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

    string IsKeyboardAreaSupported()
    {
        // Kiểm tra xem có đang chạy trên Android không
        if (Application.platform != RuntimePlatform.Android)
            return "FALSE";

        try
        {
            // Truy cập vào class android.os.Build.VERSION của Android
            using (AndroidJavaClass buildVersion = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                // Lấy giá trị biến tĩnh SDK_INT
                int apiLevel = buildVersion.GetStatic<int>("SDK_INT");

                // Trả về true nếu API >= 30 (Android 11 trở lên)
                return apiLevel.ToString();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Lỗi khi lấy API Level: " + e.Message);
            return "FALSE";
        }
    }
    
    public int GetKeyboardHeight()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            debugText.text = "NOT ANDROID";
            return 0;
        }
        
        using(AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");
          
            using(AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
            {
                View.Call("getWindowVisibleDisplayFrame", Rct);
              
                return Screen.height - Rct.Call<int>("height");
            }
        }
        // Nếu không chạy trên Android, trả về 0
        // if (Application.platform != RuntimePlatform.Android) 
        //     return 0;

        // try
        // {
        //     using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        //     {
        //         // Lấy Activity hiện tại
        //         AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                
        //         // Lấy Window và DecorView (View gốc của toàn bộ màn hình Android)
        //         AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");
        //         AndroidJavaObject decorView = window.Call<AndroidJavaObject>("getDecorView");

        //         // Tạo một biến Rect của Android (không phải Rect của Unity)
        //         AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect");
                
        //         // Đổ dữ liệu khung hình thực tế đang hiển thị vào rect
        //         decorView.Call("getWindowVisibleDisplayFrame", rect);

        //         // Lấy tổng chiều cao của màn hình gốc
        //         int decorHeight = decorView.Call<int>("getHeight");
                
        //         // Lấy chiều cao phần màn hình còn nhìn thấy được (chưa bị bàn phím che)
        //         int visibleHeight = rect.Call<int>("height");

        //         // Tính ra chiều cao bàn phím
        //         int keyboardHeight = decorHeight - visibleHeight;

        //         // Lưu ý: Ngay cả khi không có bàn phím, chênh lệch có thể là vài chục pixel
        //         // do thanh điều hướng (Navigation Bar) hoặc thanh trạng thái. 
        //         // Nên ta chỉ tính là có bàn phím khi chiều cao đủ lớn (ví dụ > 200 pixel).
        //         if (keyboardHeight < 200) 
        //         {
        //             return 0;
        //         }

        //         return keyboardHeight;
        //     }
        // }
        // catch (System.Exception e)
        // {
        //     Debug.LogError("Lỗi khi tính chiều cao bàn phím: " + e.Message);
        //     return 0;
        // }
    }
}
