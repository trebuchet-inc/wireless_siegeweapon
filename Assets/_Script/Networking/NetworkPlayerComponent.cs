using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class NetworkPlayerComponent : MonoBehaviour 
{
	public int id;

	[HideInInspector] public GameObject[] playerParts;
	List<NetworkPlayerData> _dataBuffer;
	NetworkPlayerData _lastData;
	NVRVirtualHand[] _hands;

	void Start()
	{
		playerParts = new GameObject[3];
		_hands = new NVRVirtualHand[2];
		_dataBuffer = new List<NetworkPlayerData>();
		setPlayerPart();
	}

	void FixedUpdate()
	{
		readData();
	}

	void readData()
	{
		if(_lastData == null) return;

		for(int i = 0; i < playerParts.Length; i++)
		{
			if(playerParts[i] != null)
			{
				playerParts[i].transform.position = Vector3.Lerp(playerParts[i].transform.position, _lastData.positions[i].Deserialize(), Time.deltaTime * 10);
				playerParts[i].transform.rotation = Quaternion.Lerp(playerParts[i].transform.rotation, _lastData.rotations[i].Deserialize(), Time.deltaTime * 10);
			}
			else
			{
				setPlayerPart();
			} 
		}

		if(_dataBuffer.Count <= 0) return;

		_dataBuffer.Clear();
	}

	public void ReceiveData(NetworkPlayerData data)
	{
		_lastData = data;
		_dataBuffer.Add(data);
	}

	void setPlayerPart()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			switch(transform.GetChild(i).name)
			{
				case "head" :
				playerParts[0] = transform.GetChild(i).gameObject;
				break;

				case "rightHand" :
				playerParts[1] = transform.GetChild(i).gameObject;
				_hands[1] = playerParts[1].GetComponent<NVRVirtualHand>();
				break;

				case "leftHand" :
				playerParts[2] = transform.GetChild(i).gameObject;
				_hands[0] = playerParts[2].GetComponent<NVRVirtualHand>();
				break;
			}
		}
		print("setPlayerPart");
	}
}
