using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Switch_Weapon : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip _switch;
    private int select_weapon = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        Sellect_Weapon();
    }

    // Update is called once per frame
    void Update()
    {
        int per_weapon = select_weapon;

        //key input (fuck this)

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            select_weapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            select_weapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            select_weapon = 2;
        }

        //đổi súng bằng nút chuột giữa
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(select_weapon >= transform.childCount - 1) 
            {
                select_weapon = 0;
            }
            else
            {
                select_weapon += 1;
            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(select_weapon <= 0) 
            {
                select_weapon = transform.childCount - 1;
            }
            else
            {
                select_weapon -= 1;
            }
        }

        if(per_weapon != select_weapon)
        {
            Sellect_Weapon();
        }

    }

    void Sellect_Weapon()
    {

        if(select_weapon >= transform.childCount)
        {
            select_weapon = transform.childCount - 1;
        }
        //stop anim 
        anim.Stop();

        anim.Play(_switch.name);

        int i = 0;
        // gọi đến các object con
        foreach(Transform _weapon in transform)
        {
            if(i == select_weapon)
            {
                _weapon.gameObject.SetActive(true);
            }else
            {
                _weapon.gameObject.SetActive(false);
            }
            i++;
        }
        
    }
}
