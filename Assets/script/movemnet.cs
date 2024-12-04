using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class movemnet : MonoBehaviour
{
    //suka blyat walk]
    [Header("walk")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float  maxVelocityChange = 10f;

    [Header("run mother fucker")]
    [SerializeField] private float sprint_Speed = 14f;

    [Header("jumpping")]
    [SerializeField] private float Jump_Height = 10f;

    [Header("lực cản không khí")]
    [SerializeField] private float aircontrol = 0.5f;

    private Vector2 input;
    private Rigidbody rb;

    [Header("float check trạng thái")]
    private bool springting;
    private bool jumping;
    private bool isgrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // tạo vector input có magnitude = 1 cùng hướng với vector ban đầu ( vdu(0,1,0)
        input.Normalize();

        springting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");
    }

    private void OnTriggerStay(Collider other)
    {
        isgrounded = true;
    }

    private void FixedUpdate()
    {
        if(isgrounded)
        {
            if(jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, Jump_Height, rb.velocity.z);
            }
            else if (input.magnitude > 0.5f)
            {
                rb.AddForce(Caculatate_Movement(springting ? sprint_Speed : speed), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.deltaTime, velocity1.y, velocity1.z * 0.2f * Time.deltaTime);
                rb.velocity = velocity1;
            }
        }
        else
        {
            if (input.magnitude > 0.5f)
            {
                rb.AddForce(Caculatate_Movement(springting ? sprint_Speed * aircontrol : speed * aircontrol), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.deltaTime, velocity1.y, velocity1.z * 0.2f * Time.deltaTime);
                rb.velocity = velocity1;
            }
        }
        isgrounded = false;
    }


    private Vector3 Caculatate_Movement(float _speed)
    {
        // Tạo ra vector vận tốc đích dựa trên đầu vào của người dùng và hướng của đối tượng
        Vector3 target_velocity = new Vector3(input.x, 0, input.y);
        target_velocity = transform.TransformDirection(target_velocity);

        // Nhân vận tốc đích với tốc độ di chuyển
        target_velocity *= _speed;

        // Lấy vận tốc hiện tại của đối tượng
        Vector3 velocity = rb.velocity;

        // Nếu người chơi nhấn phím điều khiển
        if (input.magnitude > 0.5f)
        {
            // Tính toán sự thay đổi vận tốc cần thiết
            Vector3 velocity_change = target_velocity - velocity;

            // Giới hạn sự thay đổi vận tốc theo cả hai hướng (x, z)
            velocity_change.x = Mathf.Clamp(velocity_change.x, -maxVelocityChange, maxVelocityChange);
              
            velocity_change.z = Mathf.Clamp(velocity_change.z, -maxVelocityChange, maxVelocityChange);

            // Giữ vận tốc theo trục y bằng 0 để đối tượng không di chuyển theo trục y
            velocity_change.y = 0;
            // Trả về sự thay đổi vận tốc
            return (velocity_change);
        }
        else
        {
            // Nếu không có đầu vào từ người dùng, không có sự thay đổi vận tốc
            return new Vector3();
        }
    }
}
