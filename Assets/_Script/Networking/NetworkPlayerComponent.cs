using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class NetworkPlayerComponent : MonoBehaviour 
{
	public int id;
	public NetworkPlayerData targetValues;

	GameObject[] _playerpart;

	void Start()
	{
		_playerpart = new GameObject[3];
		targetValues = new NetworkPlayerData(new Vector3[]{Vector3.zero,Vector3.zero,Vector3.zero}, new Quaternion[]{Quaternion.identity,Quaternion.identity,Quaternion.identity});
		setPlayerPart();
	}

	void FixedUpdate()
	{
		for(int i = 0; i < _playerpart.Length; i++)
		{
			if(_playerpart != null)
			{
				_playerpart[i].transform.position = Vector3.Lerp(_playerpart[i].transform.position, targetValues.positions[i].Deserialize(), Time.deltaTime * 10);
				_playerpart[i].transform.rotation = Quaternion.Lerp(_playerpart[i].transform.rotation, targetValues.rotations[i].Deserialize(), Time.deltaTime * 10);
			}
			else
			{
				setPlayerPart();
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
				_playerpart[0] = transform.GetChild(i).gameObject;
				break;

				case "rightHand" :
				_playerpart[1] = transform.GetChild(i).gameObject;
				break;

				case "leftHand" :
				_playerpart[2] = transform.GetChild(i).gameObject;
				break;
			}
		}
		print("setPlayerPart");
	}
}
