using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{
    public static NetworkManager Instance;

    [HideInInspector] public PhotonView globalPhotonView;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        globalPhotonView = GetComponent<PhotonView>();
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("RemotePlayer", Vector3.zero, Quaternion.identity, 0);
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
