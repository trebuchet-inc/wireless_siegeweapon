using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectToTrack 
{
	NVR_Head,
	NVR_Hand,
	Custom
}

public class NetworkTrackerComponent : MonoBehaviour 
{
	public ObjectToTrack objectToTrack;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
