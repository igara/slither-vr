using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharactorSelectSceneScript : MonoBehaviour {

	/// <summary>
	/// The n camera.
	/// </summary>
	[SerializeField] GameObject m_camera;

	/// <summary>
	/// The m gvr viewer.
	/// </summary>
	[SerializeField] GvrViewer m_gvr_viewer;

	/// <summary>
	/// The m target mark.赤い×印
	/// </summary>
	[SerializeField] GameObject m_target_mark;

	/// <summary>
	/// The m count.
	/// </summary>
	[SerializeField] GameObject m_count;

	/// <summary>
	/// The timeleft.
	/// </summary>
	private float timeleft;
	/// <summary>
	/// The time.
	/// </summary>
	private int time = 6;

	/// <summary>
	/// The m light.
	/// </summary>
	[SerializeField] GameObject m_light;

	/// <summary>
	/// The m target dummy. ダミーがオブジェクト触れた時にイベントを発生させる
	/// </summary>
	[SerializeField] GameObject m_target_dummy;

	/// <summary>
	/// The m worm.
	/// </summary>
	[SerializeField] List<GameObject> m_worms;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		m_gvr_viewer.VRModeEnabled = GameSetting.vr_mode_flag;
		InitWormSetting ();
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		UpdateLightRotation ();
	}

	/// <summary>
	/// Inits the worm setting.
	/// </summary>
	private void InitWormSetting() {
		foreach (GameObject worm in m_worms) {
		}
	}

	/// <summary>
	/// Updates the light rotation.
	/// </summary>
	private void UpdateLightRotation() {
		m_light.transform.eulerAngles = new Vector3(
			45,
			m_camera.transform.localEulerAngles.y,
			0
		);
	}

	/// <summary>
	/// Redirecteds the on trigger enter. 子のゲームオブジェクトのcolliderを開始検知する
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerEnter (Collider collider)
	{
		string gameobject_name = collider.gameObject.name;
	}

	/// <summary>
	/// Redirecteds the on trigger exit.
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerExit (Collider collider)
	{
		string gameobject_tag = collider.gameObject.tag;
		if (gameobject_tag == "Worm") {
			TextMesh count = m_count.gameObject.GetComponent<TextMesh>();
			count.text = "";
			time = 6;
		}
	}

	/// <summary>
	/// Redirecteds the on trigger stay. 子のゲームオブジェクトのcolliderをキープ検知する
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerStay (Collider collider)
	{
		string gameobject_tag = collider.gameObject.tag;
		if (gameobject_tag == "Worm") {
			TextMesh count = m_count.gameObject.GetComponent<TextMesh>();
			//だいたい1秒ごとに処理を行う
			timeleft -= Time.deltaTime;
			if (timeleft <= 0.0) {
				timeleft = 1.0f;
				time -= 1;
				count.text = time.ToString();
				if (time == 0) {
					SceneManager.LoadScene("Slither/SingleDoom/SingleDoomScene");
					time = 6;
				}
			}
		}
	}

}
