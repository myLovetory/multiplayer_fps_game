using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room_Join_button : MonoBehaviour
{
    public string roomName;
   
    public void onButtonPress()
    {
        Room_List.Instance.Join_Room_By_Name(roomName);
        
    }    
}
