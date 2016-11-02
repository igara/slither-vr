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
	public float moveSpeed  = 10.0f;

	float yOffset;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		yOffset = m_camera.transform.position.y;


	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		// 1.カメラの傾きを取得
		float x = m_camera.transform.eulerAngles.x;
		Debug.Log (x);

		// 2.ある角度以内であれば前進させる
		//-20
		if ((0.0f <= x && x <= 30.0f) || (330.0f <= x && x <= 360.0f)) {
			moveFoward ();
		}
	}
	private void moveFoward() {
		Vector3 direction = new Vector3 (m_camera.transform.forward.x, 0, m_camera.transform.forward.z).normalized * moveSpeed * Time.deltaTime;
		Quaternion rotation = Quaternion.Euler (new Vector3 (0, -m_camera.transform.rotation.eulerAngles.y, 0));
		m_camera.transform.Translate (rotation * direction);
		m_camera.transform.position = new Vector3 (m_camera.transform.position.x, yOffset, m_camera.transform.position.z);
	}
}
