using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float fireRate;
    private float nextfire;
    [SerializeField] private Camera cam;

    [Header("the effect")]
    public GameObject hitVfx;

    [Header("Ammo")]
    [SerializeField] private int mag = 5;
    [SerializeField] private int ammo = 30;
    [SerializeField] private int magAmmo = 30;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI magtext;
    [SerializeField] private TextMeshProUGUI ammoText;


    [Header("Animation")]
    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip reload;

    [Header("Recoil Setting")]
    //[Range(0f, 1f)] 
    //[SerializeField] private float RecoilPercent = 0.3f;
    [Range(0, 2)]
    [SerializeField] private float RecoverPercent = 0.7f;

    [Space]
    [SerializeField] private float Recoil_Up = 1f;
    [SerializeField] private float Recoil_Back = 0f;

    private Vector3 original_pos;
    private Vector3 recoil_velocity = Vector3.zero;

    private float recoil_Length;
    private float recover_Length;

    public bool recoiling;
    public bool recovering;

   
    // Update is called once per frame
    private void Start()
    {
        magtext.text = mag.ToString();
        ammoText.text = ammo + " / " + magAmmo;

        original_pos = transform.localPosition;

        recoil_Length = 0;
        recover_Length = 1 / fireRate * RecoverPercent;
    }

    void Update()
    {
        if(nextfire > 0)
        {
            nextfire -= Time.deltaTime;
        }

        if(Input.GetButton("Fire1") && nextfire <=0 && ammo > 0 && anim.isPlaying == false)
        {
            nextfire = 1 / fireRate;
            ammo--;

            Fire();

            magtext.text = mag.ToString();
            ammoText.text = ammo + " / " + magAmmo;

        }

        if (Input.GetKeyDown(KeyCode.R) && mag > 0)
        {
            Reload();
        }
        if(ammo <= 0 && mag > 0)
        {
            Reload();
        }

        if(recoiling)
        {
            recoil();
        }

        if(recovering)
        {
            Recovering();
        }    
    }

    void Reload ()
    {

        anim.Play(reload.name);


        if(mag > 0)
        {
            mag--;
            ammo = magAmmo;
        }
        magtext.text = mag.ToString();
        ammoText.text = ammo + " / " + magAmmo; 
    }

    void Fire()
    {
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit;

        if(Physics.Raycast(ray.origin, ray.direction ,out hit, 50f))
        {
            PhotonNetwork.Instantiate(hitVfx.name, hit.point, Quaternion.identity);

            if(hit.transform.gameObject.GetComponent<Hearth>())
            {

                //PhotonNetwork.LocalPlayer.AddScore(damage); add theo lượng damage gây nên
                
                //add theo kill
                if(damage >= hit.transform.GetComponent<Hearth>().heath)
                {
                    //add kill
                    Room_Manager.instance.kill++;
                    Room_Manager.instance.SetHashes();
                    PhotonNetwork.LocalPlayer.AddScore(100);
                }

                // Giả sử 'photonView' là PhotonView của người bắn
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);

            }
        }
    }

    void recoil()
    {
        Vector3 final_Pos = new Vector3(original_pos.x, original_pos.y + Recoil_Up, original_pos.z - Recoil_Back);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, final_Pos, ref recoil_velocity,recoil_Length );

        if(transform.localPosition == final_Pos)
        {
            recoiling = false;
            recovering = true;

        }
    }

    void Recovering()
    {
        Vector3 final_Pos = original_pos;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, final_Pos, ref recoil_velocity, recover_Length);

        if (transform.localPosition == final_Pos)
        {
            recoiling = false;
            recovering = false;

        }
    }
}
