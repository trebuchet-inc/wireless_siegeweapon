using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NetworkTrackerComponent))]
 public class NetworkTrackerComponentEditor : Editor
 {
   	public override void OnInspectorGUI()
   	{
		base.OnInspectorGUI();

     	NetworkTrackerComponent myScript = target as NetworkTrackerComponent;
 
     	myScript.objectToTrack = (ObjectToTrack)EditorGUILayout.EnumPopup("Object To Track", myScript.objectToTrack);
		     
    	if(myScript.objectToTrack == ObjectToTrack.Custom)
		{
			myScript.customObject = (GameObject)EditorGUILayout.ObjectField(myScript.customObject, typeof(GameObject), true);
		} 
   	}
}
