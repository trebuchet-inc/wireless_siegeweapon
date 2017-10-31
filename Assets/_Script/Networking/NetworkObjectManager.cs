using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;
using System;

public class NetworkObjectManager : Photon.MonoBehaviour 
{
	public static NetworkObjectManager Instance;

	public List<NVRInteractable> nvrPool;

	void Awake()
    {
        Instance = this;
    }	

	public void SendBeginInterraction(NVRInteractable item, NVRHand hand)
	{
		SerializableTransform t = new SerializableTransform(item.transform.position - hand.transform.position, item.transform.rotation);
		photonView.RPC("ReceiveBeginInteraction", PhotonTargets.Others, SerializationToolkit.ObjectToByteArray(t), hand.IsRight, GetObjectID(item), NetworkPlayerManager.Instance.personalID);
	}

	public void SendEndInterraction(NVRInteractable item)
	{
		SerializableRigidbody rb = new SerializableRigidbody(item.Rigidbody);
		photonView.RPC("ReceiveEndInteraction", PhotonTargets.Others, SerializationToolkit.ObjectToByteArray(rb), GetObjectID(item));
	}

	int GetObjectID(NVRInteractable obj)
	{
		for(int i =0; i < nvrPool.Count; i++)
		{
			if(nvrPool[i] == obj)
			{
				return i;
			}
		}
		return -1;
	}
	
	[PunRPC]
    void ReceiveBeginInteraction(byte[] data, bool rightHand, int objectId, int playerId)
    {
		if(objectId < 0) Debug.LogError("invalid object ID");

		SerializableTransform t = (SerializableTransform)SerializationToolkit.ByteArrayToObject(data);
		GameObject hand = NetworkPlayerManager.Instance.GetNetworkPlayerHand(playerId, rightHand);
		
        nvrPool[objectId].transform.position = hand.transform.position + t.position.Deserialize();
		nvrPool[objectId].transform.rotation = t.rotation.Deserialize();
		nvrPool[objectId].BeginInteraction(hand.GetComponent<NVRHand>());
    }

	[PunRPC]
    void ReceiveEndInteraction(byte[] data, int objectId)
    {
		if(objectId < 0) Debug.LogError("invalid object ID");

		SerializableRigidbody rb = (SerializableRigidbody)SerializationToolkit.ByteArrayToObject(data);

		nvrPool[objectId].EndInteraction(nvrPool[objectId].AttachedHand);
		nvrPool[objectId].Rigidbody.velocity = rb.velocity.Deserialize();
		nvrPool[objectId].Rigidbody.angularVelocity = rb.angularVelocity.Deserialize();
    }
}
