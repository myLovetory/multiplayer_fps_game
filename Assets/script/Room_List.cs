using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Room_List : MonoBehaviourPunCallbacks
{
    public static Room_List Instance;

    public GameObject room_manager_Gameobject;
    public GameObject Looby_gameobject;

    public Room_Manager room_Manager;


    [Header("UI")]
    public Transform room_List_Parent;

    public GameObject room_Item_Prefab;

    public List<RoomInfo> cached_Room_List = new List<RoomInfo>();

    //change room to create name
    public void Change_room_to_create_name(string _room_name)
    {
        room_Manager.room_name = _room_name;
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        yield return new WaitUntil(() => PhotonNetwork.IsConnected);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> room_list)
    {
        foreach (var room in room_list)
        {
            int index = cached_Room_List.FindIndex(r => r.Name == room.Name);
            if (index != -1)
            {
                if (room.RemovedFromList)
                {
                    cached_Room_List.RemoveAt(index);
                }
                else
                {
                    cached_Room_List[index] = room;
                }
            }
            else if (!room.RemovedFromList)
            {
                cached_Room_List.Add(room);
            }
        }

        // Cập nhật UI sau khi đã cập nhật danh sách phòng
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform roomItem in room_List_Parent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cached_Room_List)
        {
            GameObject room_item = Instantiate(room_Item_Prefab, room_List_Parent);
            room_item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            room_item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/10";

            room_item.GetComponent<room_Join_button>().roomName = room.Name;
        }
    }

    //chức năng như lồn
    public void Join_Room_By_Name(string _name)
    {
        room_Manager.room_name = _name;
        room_manager_Gameobject.SetActive(true);
        //Looby_gameobject.SetActive(false);
    }
}


