using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    public Transform player1Camera;
    public Transform player2Camera;

    // Start is called before the first frame update
    void Start()
    {
        bool isPlayer1 = PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.PlayerList[0].ActorNumber;
        gameObject.transform.position = isPlayer1 ? player1Camera.position : player2Camera.position;
        gameObject.transform.rotation = isPlayer1 ? player1Camera.rotation : player2Camera.rotation;
    }
}
