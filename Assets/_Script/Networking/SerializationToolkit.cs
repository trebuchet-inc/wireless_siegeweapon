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
