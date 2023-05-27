using UnityEngine;
using Photon.Pun;

public class SpwanPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(), Quaternion.identity);
    }
}
