using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerManager : Photon.MonoBehaviour 
{
	public static NetworkPlayerManager Instance;

	public GameObject playerPrefab;
	public List<PhotonView> players;

	void Awake()
    {
        Instance = this;
    }

	[PunRPC]
    void SpawnPlayer(Vector3 pos, Quaternion rot, int id)
    {
        GameObject _newPlayer = Instantiate(playerPrefab, pos, rot) ;
        PhotonView _photonViews = _newPlayer.GetComponent<PhotonView>();

		_photonViews.viewID = id;
		players.Add(_photonViews);
    }
}
