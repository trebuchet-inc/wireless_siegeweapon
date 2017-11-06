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
        //PhotonNetwork.ConnectToMaster("192.168.1.100", 5056, "64d0546d-f744-41eb-8817-1db17103b312", "0.1");
        PhotonNetwork.ConnectToMaster("127.0.0.1", 5055, "64d0546d-f744-41eb-8817-1db17103b312", "0.1");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Fail To Connect To Room");
        PhotonNetwork.CreateRoom(null);
        _roomCreator = true;
        Debug.Log("Room Created");
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
