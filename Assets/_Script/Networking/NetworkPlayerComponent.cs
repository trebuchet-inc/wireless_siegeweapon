using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class NetworkPlayerComponent : MonoBehaviour 
{
	public int id;
	public NetworkPlayerData targetValues;

	GameObject[] _playerParts;
	NVRVirtualHand[] _hands;

	void Start()
	{
		_playerParts = new GameObject[3];
		targetValues = new NetworkPlayerData(new Vector3[]{Vector3.zero,Vector3.zero,Vector3.zero}, 
											new Quaternion[]{Quaternion.identity,Quaternion.identity,Quaternion.identity},
											new bool[]{false,false},
											new bool[]{false,false});
		setPlayerPart();
	}

	void FixedUpdate()
	{
		for(int i = 0; i < _playerParts.Length; i++)
		{
			if(_playerParts[i] != null)
			{
				_playerParts[i].transform.position = Vector3.Lerp(_playerParts[i].transform.position, targetValues.positions[i].Deserialize(), Time.deltaTime * 10);
				_playerParts[i].transform.rotation = Quaternion.Lerp(_playerParts[i].transform.rotation, targetValues.rotations[i].Deserialize(), Time.deltaTime * 10);
			}
			else
			{
				setPlayerPart();
			} 
		}

		for(int i = 0; i < _hands.Length; i++)
		{
			if(targetValues.beginInterraction[i])
			{
				_hands[i].Hold();
			} 
			else if(!targetValues.endInterraction[i])
			{
				_hands[i].Release();
			}
		}
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
