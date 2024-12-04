using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hearth : MonoBehaviour
{
    [SerializeField] public int heath;
    public bool islocal_Player;

    [Header("Ui")]
    [SerializeField] private TextMeshProUGUI hearth_text;

    [Header("hearth bar ui")]
    [SerializeField] private RectTransform hearth_Bar_Size;
    private float original_hearth_bar_size;


    private void Start()
    {
        original_hearth_bar_size = hearth_Bar_Size.sizeDelta.x;
    }

    //private void Update()
    //{
        //hearth_Bar_Size.sizeDelta = new Vector2(original_hearth_bar_size * heath / 100f, hearth_Bar_Size.sizeDelta.y);
    //}





    [PunRPC]
    public void TakeDamage(int _damage)
    {

        heath -= _damage;

        hearth_Bar_Size.sizeDelta = new Vector2(original_hearth_bar_size * heath / 100f, hearth_Bar_Size.sizeDelta.y);
        hearth_text.text = heath.ToString();

       
        if (heath <= 0)
        {
            
            if (islocal_Player)
            {
                Room_Manager.instance.Respawn_Player();

                //add kill
                Room_Manager.instance.death++;
                Room_Manager.instance.SetHashes();
            }

            Destroy(gameObject);
        }
    }    

}
