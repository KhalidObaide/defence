using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMPro.TMP_InputField roomCodeInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomCodeInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomCodeInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Sandbox");
    }
}
