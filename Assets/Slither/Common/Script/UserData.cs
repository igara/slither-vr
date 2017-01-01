using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class UserData {

	/// <summary>
	/// The client identifier.
	/// </summary>
	public string client_id;

	/// <summary>
	/// The count.
	/// </summary>
	public int count;

	/// <summary>
	/// The position.
	/// </summary>
	public Vector3 position;

	/// <summary>
	/// The rotation.
	/// </summary>
	public Quaternion rotation;

	/// <summary>
	/// The color.
	/// </summary>
	public Color color;
}
