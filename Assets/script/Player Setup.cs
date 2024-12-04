using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    /* lý do phải sinh  ra script này là khi liên kết với 1 player khác thì thao tác di chuyển
       sẽ được thực thi đồng loạt ở cả 2 player cùng 1 lúc nên script này sẽ tách thao tác di chuyển từng player 1 riêng lẻ
     */

    public movemnet Movement;

    public new GameObject camera;


    public string nickname;

    public TextMeshPro nickName_Text;

    
    public void isLocalPlayer()
    {
        Movement.enabled = true;

        camera.SetActive(true);
    }
    [PunRPC]
    public void SetNickName(string _name)
    {
        nickname = _name;

        nickName_Text.text = nickname;
    }

    //hash kill
       
}
