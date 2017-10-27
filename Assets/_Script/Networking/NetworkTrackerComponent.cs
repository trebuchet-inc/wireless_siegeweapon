using UnityEngine;
using Photon;
using NewtonVR;

public enum ObjectToTrack 
{
	NVR_Head,
	NVR_Hand_Right,
	NVR_Hand_Left,
	Custom
}

public class NetworkTrackerComponent : Photon.MonoBehaviour 
{
	[HideInInspector] public ObjectToTrack objectToTrack;
	[HideInInspector] public GameObject customObject;

	GameObject _trackedObj;
	Vector3 _correctPlayerPos;
    Quaternion _correctPlayerRot;
	PhotonView _photonView;

	void Start () {
		switch(objectToTrack)
		{
			case ObjectToTrack.NVR_Head :
			_trackedObj = FindObjectOfType<NVRPlayer>().Head.gameObject;
			break;

			case ObjectToTrack.NVR_Hand_Right :
			_trackedObj = FindObjectOfType<NVRPlayer>().RightHand.gameObject;
			break;

			case ObjectToTrack.NVR_Hand_Left :
			_trackedObj = FindObjectOfType<NVRPlayer>().LeftHand.gameObject;
			break;

			case ObjectToTrack.Custom :
			_trackedObj = customObject;
			break;
		}

		if(photonView == null) _photonView = transform.parent.GetComponent<PhotonView>();
		else _photonView = photonView;
	}
	
	void Update () {
		if (!_photonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, this._correctPlayerPos, Time.deltaTime * 10);
			transform.rotation = Quaternion.Lerp(transform.rotation, this._correctPlayerRot, Time.deltaTime * 10);
		}
		else
		{
			transform.position = _trackedObj.transform.position;
			transform.rotation = _trackedObj.transform.rotation;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(_trackedObj.transform.position);
            stream.SendNext(_trackedObj.transform.rotation);
        }
        else
        {
            this._correctPlayerPos = (Vector3) stream.ReceiveNext();
            this._correctPlayerRot =(Quaternion) stream.ReceiveNext();
        }
    }
}
