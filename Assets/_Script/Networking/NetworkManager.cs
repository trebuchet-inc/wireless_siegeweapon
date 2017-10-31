using UnityEngine;
using NewtonVR;

public class NetworkManager : Photon.PunBehaviour
{
    public static NetworkManager Instance;

    bool _roomCreator = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
        _roomCreator = true;
    }

    public override void OnJoinedRoom()
    {
        int id = PhotonNetwork.AllocateViewID();
        NetworkPlayerManager.Instance.personalID = id;
        NetworkPlayerManager.Instance.photonView.RPC("SpawnNetworkPlayer", PhotonTargets.OthersBuffered, Vector3.zero, Quaternion.identity, id);
    }
    
    public override void OnLeftRoom()
    {
        NetworkPlayerManager.Instance.photonView.RPC("DestroyNetworkPlayer", PhotonTargets.Others, NetworkPlayerManager.Instance.personalID);
        print("DestroyNetworkPlayer");
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
