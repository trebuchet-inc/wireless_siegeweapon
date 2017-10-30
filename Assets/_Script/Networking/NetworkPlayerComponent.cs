using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class NetworkPlayerComponent : MonoBehaviour 
{
	public int id;
	public List<NetworkPlayerData> dataBuffer;

	GameObject[] _playerParts;
	NVRVirtualHand[] _hands;

	void Start()
	{
		_playerParts = new GameObject[3];
		_hands = new NVRVirtualHand[2];
		dataBuffer = new List<NetworkPlayerData>();
		setPlayerPart();
	}

	void FixedUpdate()
	{
		readData();
	}

	void readData()
	{
		if(dataBuffer.Count <= 0) return;

		for(int i = 0; i < _playerParts.Length; i++)
		{
			if(_playerParts[i] != null)
			{
				NetworkPlayerData lastDataPackage = dataBuffer[dataBuffer.Count - 1];
				_playerParts[i].transform.position = Vector3.Lerp(_playerParts[i].transform.position, lastDataPackage.positions[i].Deserialize(), Time.deltaTime * 10);
				_playerParts[i].transform.rotation = Quaternion.Lerp(_playerParts[i].transform.rotation, lastDataPackage.rotations[i].Deserialize(), Time.deltaTime * 10);
			}
			else
			{
				setPlayerPart();
			} 
		}

		foreach(NetworkPlayerData data in dataBuffer)
		{
			for(int i = 0; i < _hands.Length; i++)
			{
				if(data.beginInterraction[i])
				{
					print("begin interraction from network player");
					_hands[i].ForceInteraction(data.objectName);
				} 
				else if(data.endInterraction[i])
				{
					print("end interraction from network player");
					_hands[i].Release();
				}
			}
		}

		dataBuffer.Clear();
	}

	void setPlayerPart()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			switch(transform.GetChild(i).name)
			{
				case "head" :
				_playerParts[0] = transform.GetChild(i).gameObject;
				break;

				case "rightHand" :
				_playerParts[1] = transform.GetChild(i).gameObject;
				_hands[0] = _playerParts[1].GetComponent<NVRVirtualHand>();
				break;

				case "leftHand" :
				_playerParts[2] = transform.GetChild(i).gameObject;
				_hands[1] = _playerParts[2].GetComponent<NVRVirtualHand>();
				break;
			}
		}
		print("setPlayerPart");
	}
}
