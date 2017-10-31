using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;
using NewtonVR;

[Serializable]
public class NetworkPlayerData
{
	public SerializableVector3[] positions;
	public SerializableQuaternion[] rotations;
	public bool[] beginInterraction;
	public bool[] endInterraction;
	public string objectName;

	public NetworkPlayerData (Vector3[] pos, Quaternion[] rot)
	{
		positions = new SerializableVector3[pos.Length];
		for(int i = 0; i < positions.Length; i++)
		{
			positions[i] = new SerializableVector3(pos[i]);
		}

		rotations = new SerializableQuaternion[rot.Length];
		for(int i = 0; i < rotations.Length; i++)
		{
			rotations[i] = new SerializableQuaternion(rot[i]);
		}
	}
}

public class NetworkPlayerManager : Photon.MonoBehaviour 
{
	public static NetworkPlayerManager Instance;

	public int personalID;
	public GameObject playerPrefab;
	public List<NetworkPlayerComponent> players;

	void Awake()
    {
        Instance = this;
    }

	void FixedUpdate()
	{
		if(!PhotonNetwork.connected) return;

		NetworkPlayerData data = new NetworkPlayerData(
			new Vector3[]{NVRPlayer.Instance.Head.transform.position, NVRPlayer.Instance.RightHand.transform.position, NVRPlayer.Instance.LeftHand.transform.position},
			new Quaternion[]{NVRPlayer.Instance.Head.transform.rotation, NVRPlayer.Instance.RightHand.transform.rotation, NVRPlayer.Instance.LeftHand.transform.rotation});
	
		BinaryFormatter formatter = new BinaryFormatter();
		byte[] serializedData = SerializationToolkit.ObjectToByteArray(data); 

		photonView.RPC("UpdateNetworkPlayer", PhotonTargets.Others, serializedData, personalID);
	}

	public GameObject GetNetworkPlayerHand(int id, bool rightHand)
	{
		foreach(NetworkPlayerComponent p in players)
		{
			if(p.id == id)
			{
				if(rightHand) return p.playerParts[1];
				else return p.playerParts[2];
			}
		}
		return null;
	}

	[PunRPC]
    void UpdateNetworkPlayer(byte[] data, int id)
    {
        foreach(NetworkPlayerComponent p in players)
		{
			if(p.id == id)
			{
				p.ReceiveData((NetworkPlayerData) SerializationToolkit.ByteArrayToObject(data));
				return;
			}
		}
    }

	[PunRPC]
    void SpawnNetworkPlayer(Vector3 pos, Quaternion rot, int id)
    {
        GameObject _newPlayer = Instantiate(playerPrefab, pos, rot) ;
        NetworkPlayerComponent _networkPlayer = _newPlayer.GetComponent<NetworkPlayerComponent>();

		_networkPlayer.id = id;
		players.Add(_networkPlayer);
    }

	[PunRPC]
    void DestroyNetworkPlayer(int id)
    {
        foreach(NetworkPlayerComponent p in players)
		{
			if(p.id == id)
			{
				players.Remove(p);
				Destroy(p.gameObject);
				return;
			}
		}
    }
}
