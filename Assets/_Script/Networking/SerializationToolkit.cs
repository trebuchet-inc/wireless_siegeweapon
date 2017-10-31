using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class SerializableVector3
{
	public float x;
	public float y;
	public float z;

	public SerializableVector3 (Vector3 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
	}

	public Vector3 Deserialize()
	{
		return new Vector3(x,y,z);
	}
}

[Serializable]
public class SerializableQuaternion
{
	public float w;
	public float x;
	public float y;
	public float z;

	public SerializableQuaternion (Quaternion q)
	{
		w = q.w;
		x = q.x;
		y = q.y;
		z = q.z;
	}

	public Quaternion Deserialize()
	{
		return new Quaternion(x,y,z,w);
	}
}

[Serializable]
public class SerializableTransform
{
	public SerializableVector3 position;
	public SerializableQuaternion rotation;
	public SerializableVector3 scale;

	public SerializableTransform (Transform t) 
	{
		position = new SerializableVector3(t.position);
		rotation = new SerializableQuaternion(t.rotation);
		scale = new SerializableVector3(t.localScale);
	}

	public SerializableTransform (Vector3 pos, Quaternion rot) 
	{
		position = new SerializableVector3(pos);
		rotation = new SerializableQuaternion(rot);
		scale = new SerializableVector3(Vector3.one);
	}

	public SerializableTransform (Vector3 pos, Quaternion rot, Vector3 sca) 
	{
		position = new SerializableVector3(pos);
		rotation = new SerializableQuaternion(rot);
		scale = new SerializableVector3(sca);
	}
}

[Serializable]
public class SerializableRigidbody
{
	public SerializableVector3 velocity;
	public SerializableVector3 angularVelocity;

	public SerializableRigidbody (Rigidbody rb)
	{
		velocity = new SerializableVector3(rb.velocity);
		angularVelocity = new SerializableVector3(rb.angularVelocity);
	} 
}

public class SerializationToolkit 
{
	public static byte[] ObjectToByteArray(object obj)
	{
		if(obj == null)
			return null;

		BinaryFormatter formatter = new BinaryFormatter();
		MemoryStream stream = new MemoryStream();
		formatter.Serialize(stream, obj);

		return stream.ToArray();
	}

	public static object ByteArrayToObject(byte[] bytes)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		MemoryStream stream = new MemoryStream();
		stream.Write(bytes, 0, bytes.Length);
		stream.Seek(0, SeekOrigin.Begin);
		object obj = (object) formatter.Deserialize(stream);

		return obj;
	}
}
