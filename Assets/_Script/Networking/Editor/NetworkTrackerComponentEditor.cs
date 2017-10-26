using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NetworkTrackerComponent))]
 public class MyScriptEditor : Editor
 {
   	void OnInspectorGUI()
   	{
     	NetworkTrackerComponent myScript = target as NetworkTrackerComponent;
 
     	myScript.objectToTrack = EditorGUILayout.EnumPopup("Object To Track", ObjectToTrack, null);
     
    	if(myScript.objectToTrack == ObjectToTrack.Custom){}
 
   	}
}
