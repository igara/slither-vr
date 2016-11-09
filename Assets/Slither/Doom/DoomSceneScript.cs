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
	private float camera_y_offset;

	private float worm_y_offset;

	/// <summary>
	/// The degree.
	/// </summary>
	const float degree = 30.0f;

	/// <summary>
	/// The full degree.
	/// </summary>
	const float full_degree = 360.0f;

	[SerializeField] GameObject m_worm;

	[SerializeField] GameObject m_worm_head;
	[SerializeField] GameObject m_worm_left_eye;
	[SerializeField] GameObject m_worm_right_eye;

	[SerializeField] GameObject[] m_worm_body;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		camera_y_offset = m_camera.transform.position.y;
		worm_y_offset = m_worm_head.transform.position.y;
		// 親子関係を解除
//		m_worm.transform.DetachChildren();
//		// こ　親
//		m_worm_head.transform.parent = m_camera.transform;
//		m_worm_left_eye.transform.parent = m_camera.transform;
//		m_worm_right_eye.transform.parent = m_camera.transform;
//		m_worm_body_1.transform.parent = m_camera.transform;
//		m_worm_body_2.transform.parent = m_camera.transform;
//		m_worm_body_3.transform.parent = m_camera.transform;
//		m_worm_body_4.transform.parent = m_camera.transform;
//		m_worm_body_5.transform.parent = m_camera.transform;
//		m_worm_tail.transform.parent = m_camera.transform;
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
		Vector3 camera_direction = new Vector3 (m_camera.transform.forward.x, 0, m_camera.transform.forward.z).normalized * move_speed * Time.deltaTime;
		Quaternion camera_rotation = Quaternion.Euler (new Vector3 (0, -m_camera.transform.rotation.eulerAngles.y, 0));
		m_camera.transform.Translate (camera_rotation * camera_direction);
		m_camera.transform.position = new Vector3 (m_camera.transform.position.x, camera_y_offset, m_camera.transform.position.z);

		Vector3 tmp_position = new Vector3 (0, 0, 0);
		Quaternion tmp_rotation = new Quaternion(0, 0, 0, 0);
		for (int i = 0; i < m_worm_body.Length; i++) {
			// 頭
			if (i == 0) {
				tmp_position = new Vector3 (
					m_worm_body [i].gameObject.transform.position.x,
					m_worm_body [i].gameObject.transform.position.y,
					m_worm_body [i].gameObject.transform.position.z
				);
				tmp_rotation = Quaternion.Euler (new Vector3 (
					m_worm_body [i].gameObject.transform.rotation.x,
					m_worm_body [i].gameObject.transform.rotation.y,
					m_worm_body [i].gameObject.transform.rotation.z
				));
				m_worm_body [i].gameObject.transform.position = m_camera.transform.position;
				m_worm_body [i].gameObject.transform.rotation = m_camera.transform.rotation;
			} else {
				Vector3 tmp2_position = new Vector3 (
					m_worm_body [i].gameObject.transform.position.x,
					m_worm_body [i].gameObject.transform.position.y,
					m_worm_body [i].gameObject.transform.position.z
				);
				Quaternion tmp2_rotation = Quaternion.Euler (new Vector3 (
					m_worm_body [i].gameObject.transform.rotation.x,
					m_worm_body [i].gameObject.transform.rotation.y,
					m_worm_body [i].gameObject.transform.rotation.z
				));
				m_worm_body [i].gameObject.transform.position = tmp_position;
				m_worm_body [i].gameObject.transform.rotation = tmp_rotation;
				tmp_position = tmp2_position;
				tmp_rotation = tmp2_rotation;
			}
		}
	}
}
