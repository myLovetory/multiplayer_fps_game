using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//thư viện linq giúp truy vẫn dữ liệu từ nhiều nguồn
using System.Linq;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;

public class Player_List : MonoBehaviour
{
    public GameObject Player_Holer;

    [Header("Options")]
    public float Refresh_Rate = 1f;

    [Header("UI")]
    [SerializeField] private GameObject[] slots;

    //keos reference đến chết
    [Space]
    [SerializeField] private TextMeshProUGUI[] name_Text;
    [SerializeField] private TextMeshProUGUI[] Score_Text;
    [SerializeField] private TextMeshProUGUI[] KDA_Text;

    private void Start()
    {
        
        InvokeRepeating(nameof(Refresh), 1f,Refresh_Rate);
    }

    private void Refresh()
    {
        foreach( var slot in slots )
        {
            slot.SetActive(false);
        }

        //yeb 
        var slot_player_list = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player ).ToList();

        int i = 0;
        foreach(var player in slot_player_list)
        {
            slots[i].SetActive(true);

            if(player.NickName =="")
            {
                player.NickName = "unnamed";
            }

            name_Text[i].text = player.NickName;
            Score_Text[i].text = player.GetScore().ToString();

            //update kda 
            if(player.CustomProperties["kill"] != null)
            {
                KDA_Text[i].text = player.CustomProperties["kill"] + "/" + player.CustomProperties["death"];
            }else
            {
                KDA_Text[i].text = "0/0";
            }


            i++;
        }
    }

    private void Update()
    {
        Player_Holer.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
