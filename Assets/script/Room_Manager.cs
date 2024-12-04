using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//yep hashtable thành cấu trúc dữ liệu trong Pun2
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class Room_Manager : MonoBehaviourPunCallbacks
{
    public static Room_Manager instance;
     
    public GameObject player;

    [Space]
    public Transform[] spawn_Points;


    [Space]
    public GameObject room_Cam;

    [Space]
    public GameObject name_UI;
    public GameObject Connecting_Ui;


    private string Nickname = "unname";

    [HideInInspector]
    public int kill = 0;
    [HideInInspector]
    public int death = 0;

    public string room_name = "test";

    private void Awake()
    {
        instance = this;

    }

    //đặt nickname
    public void ChangeNickName(string _name)
    {
        // Truncate the nickname if it exceeds 6 characters
        if (_name.Length > 6)
        {
            _name = _name.Substring(0, 6);
        }

        Nickname = _name;
    }

    //Join game 
    public void JoinRoom()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.JoinOrCreateRoom(room_name, new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);

        name_UI.SetActive(false);
        Connecting_Ui.SetActive(true);
    }

  

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("suka blyat");

        //roomcam when we already load in the room
        room_Cam.SetActive(false);
        Respawn_Player();
    }

    public void Respawn_Player()
    {
        Transform spawn_Point = spawn_Points[UnityEngine.Random.Range(0, spawn_Points.Length)];
        //spawn Player

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawn_Point.position, Quaternion.identity);

        //call playersetup

        _player.GetComponent<PlayerSetup>().isLocalPlayer();
        _player.GetComponent<Hearth>().islocal_Player = true;

        _player.GetComponent<PhotonView>().RPC("SetNickName", RpcTarget.AllBuffered, Nickname);

        PhotonNetwork.LocalPlayer.NickName = Nickname;
        
    }

    public void SetHashes()
    {
        try
        {
            //mỗi player sẽ có các thuộc tính là phần tử của hashmap 
            Hashtable hs = PhotonNetwork.LocalPlayer.CustomProperties;

            hs["kill"] = kill;
            hs["death"] = death;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hs);

        }
        catch
        {

        }
    }
}
