using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sway : MonoBehaviour
{
    //cho weapon đỡ cứng
    [Header("Setting")]
    [SerializeField] private float sway_Clamp = 0.9f;

    [Space]
    [SerializeField] private float smoothing = 3f;

    private Vector3 origin;

    private void Start()
    {
        origin = transform.localPosition;
    }

    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        input.x = Mathf.Clamp(input.x, -sway_Clamp, sway_Clamp);
        input.y = Mathf.Clamp(input.y, -sway_Clamp, sway_Clamp);

        Vector3 target = new Vector3(-input.x, -input.y, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, target + origin, Time.deltaTime * smoothing);

    }
}
