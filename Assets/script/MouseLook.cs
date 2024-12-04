using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook instance;

    [Header("Settings")]
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor = true;
    [Space]
    private Vector2 sensitivity = new Vector2(2, 2);
    [Space]
    public Vector2 smoothing = new Vector2(3, 3);

    [Header("First Person")]
    public GameObject characterBody;

    //Lưu trữ hướng ban đầu của camera và cơ thể nhân vật khi script bắt đầu chạy.
    private Vector2 targetDirection;
    private Vector2 targetCharacterDirection;

    //độ mượt của chuột
    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;

    //lưu trữ sự thay đổi chuột theo khung hình htai
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool scoped;

    void Start()
    {
        instance = this;

        // Set target direction to the camera's initial orientation.
        targetDirection = transform.localRotation.eulerAngles;

        // Set target direction for the character body to its inital state.
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
        
        if (lockCursor)
            LockCursor();

    }

    //khóa chuột khi khởi chạy
    public void LockCursor()
    {
        // make the cursor hidden and locked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Cho phép script giới hạn dựa trên giá trị mục tiêu mong muốn.
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Lấy đầu vào chuột thô để có đọc chính xác hơn trên các chuột nhạy cảm hơn.
        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Điều chỉnh đầu vào theo cài đặt độ nhạy và nhân với giá trị làm mượt.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Nội suy chuyển động chuột theo thời gian để áp dụng độ làm mượt delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Tính toán giá trị dịch chuyển chuột tuyệt đối từ điểm gốc.
        _mouseAbsolute += _smoothMouse;

        // Giới hạn và áp dụng giá trị x cục bộ trước, để không bị ảnh hưởng bởi các biến đổi thế giới.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // Sau đó, giới hạn và áp dụng giá trị y toàn cục.
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        // Nếu có một cơ thể nhân vật đóng vai trò là cha của camera
        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }

}
