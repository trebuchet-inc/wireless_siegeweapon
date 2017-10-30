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

	public NetworkPlayerData (Vector3[] pos, Quaternion[] rot, bool[] begInt, bool[] endInt, string objName)
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

		beginInterraction = begInt;
		endInterraction = endInt;
		objectName = objName;
	}
}

public class NetworkPlayerManager : Photon.MonoBehaviour 
{
	public static NetworkPlayerManager Instance;

	public int personalID;
	public GameObject playerPrefab;
	public List<NetworkPlayerComponent> players;

	[HideInInspector]
	public string objectName;
	[HideInInspector]
	public bool[] beginInterractionTrigger
	{
		get
		{
			return _beginInterractionTrigger;
		}
		set
		{
			_beginInterractionTrigger = value;
		}
	}
	[HideInInspector]
	public bool[] endInterractionTrigger
	{
		get
		{
			return _endInterractionTrigger;
		}
		set
		{
			_endInterractionTrigger = value;
		}
	}

	bool[] _beginInterractionTrigger;
	bool[] _endInterractionTrigger;

	void Awake()
    {
        Instance = this;
    }

	void Start()
	{
		_beginInterractionTrigger = new bool[2];
		_endInterractionTrigger = new bool[2];
	}

	void FixedUpdate()
	{
		if(!PhotonNetwork.connected) return;

		NetworkPlayerData data = new NetworkPlayerData(
			new Vector3[]{NVRPlayer.Instance.Head.transform.position, NVRPlayer.Instance.LeftHand.transform.position, NVRPlayer.Instance.RightHand.transform.position},
			new Quaternion[]{NVRPlayer.Instance.Head.transform.rotation, NVRPlayer.Instance.LeftHand.transform.rotation, NVRPlayer.Instance.RightHand.transform.rotation},
			beginInterractionTrigger,
			endInterractionTrigger,
			objectName);
	
		BinaryFormatter formatter = new BinaryFormatter();
		byte[] serializedData = SerializationToolkit.ObjectToByteArray(data); 

		photonView.RPC("UpdateNetworkPlayer", PhotonTargets.Others, serializedData, personalID);

		for(int i = 0; i < beginInterractionTrigger.Length; i++)
		{
			if(beginInterractionTrigger[i]) print("begin interraction");
			beginInterractionTrigger[i] = false;
			endInterractionTrigger[i] = false;
		}
	}

	void OnDestroy()
	{
		photonView.RPC("DestroyNetworkPlayer", PhotonTargets.Others, personalID);
	}

	[PunRPC]
    void UpdateNetworkPlayer(byte[] data, int id)
    {
        foreach(NetworkPlayerComponent p in players)
		{
			if(p.id == id)
			{
				p.lastDataPackage = (NetworkPlayerData) SerializationToolkit.ByteArrayToObject(data);
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
