using UnityEngine;
using System.Collections;

public class ChildColliderTrigger : MonoBehaviour {

	/// <summary>
	/// The m parent.
	/// </summary>
	[SerializeField] GameObject m_parent;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter(Collider collider)
	{
		m_parent.SendMessage("RedirectedOnTriggerEnter", collider);
	}

	/// <summary>
	/// Raises the trigger stay event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerStay(Collider collider)
	{
		m_parent.SendMessage("RedirectedOnTriggerStay", collider);
	}
}
