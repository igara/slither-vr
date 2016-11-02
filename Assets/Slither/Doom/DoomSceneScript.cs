using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Doom scene script.
/// </summary>
public class DoomSceneScript : MonoBehaviour {

	/// <summary>
	/// The m camera.
	/// </summary>
	[SerializeField] GameObject m_camera;

	/// <summary>
	/// The m target mark.赤い×印
	/// </summary>
	[SerializeField] GameObject m_target_mark;

	/// <summary>
	/// The m target dummy.
	/// </summary>
	[SerializeField] GameObject m_target_dummy;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		offset = m_camera.transform.position - m_target_mark.transform.position;

	}
	private Vector3 offset = Vector3.zero;

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		
	}
}
