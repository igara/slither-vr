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
	/// The move speed.
	/// </summary>
	public float move_speed  = 15.0f;

	/// <summary>
	/// The y offset.
	/// </summary>
	private float y_offset;

	/// <summary>
	/// The degree.
	/// </summary>
	const float degree = 30.0f;

	/// <summary>
	/// The full degree.
	/// </summary>
	const float full_degree = 360.0f;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		y_offset = m_camera.transform.position.y;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		// 現在の角度を取得する
		float x = m_camera.transform.eulerAngles.x;

		// 角度以内なら前進する
		if ((0.0f <= x && x <= degree) || (full_degree - degree <= x && x <= full_degree)) {
			moveFoward ();
		}
	}

	/// <summary>
	/// Moves the foward.
	/// </summary>
	private void moveFoward() {
		Vector3 direction = new Vector3 (m_camera.transform.forward.x, 0, m_camera.transform.forward.z).normalized * move_speed * Time.deltaTime;
		Quaternion rotation = Quaternion.Euler (new Vector3 (0, -m_camera.transform.rotation.eulerAngles.y, 0));
		m_camera.transform.Translate (rotation * direction);
		m_camera.transform.position = new Vector3 (m_camera.transform.position.x, y_offset, m_camera.transform.position.z);
	}
}
