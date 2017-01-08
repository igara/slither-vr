using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverSceneScript : MonoBehaviour {

	/// <summary>
	/// The m gvr viewer.
	/// </summary>
	[SerializeField] GvrViewer m_gvr_viewer;

	/// <summary>
	/// The m target mark.赤い×印
	/// </summary>
	[SerializeField] GameObject m_target_mark;

	/// <summary>
	/// The m target dummy.
	/// </summary>
	[SerializeField] GameObject m_target_dummy;

	/// <summary>
	/// The timeleft.
	/// </summary>
	private float timeleft;
	/// <summary>
	/// The time.
	/// </summary>
	private int time = 6;

	/// <summary>
	/// The audio source.
	/// </summary>
	[SerializeField] AudioSource[] audio_source;

	/// <summary>
	/// Sound.
	/// </summary>
	enum Sound {
		Explosion
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		m_gvr_viewer.VRModeEnabled = GameSetting.vr_mode_flag;
		audio_source[(int)Sound.Explosion].Play();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {

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
		if (collider.gameObject.tag == "ReturnTitle") {
			TextMesh title = collider.gameObject.GetComponent<TextMesh>();
			title.text = "ReturnTitle";
			time = 6;
		}
	}

	/// <summary>
	/// Redirecteds the on trigger stay. 子のゲームオブジェクトのcolliderをキープ検知する
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerStay (Collider collider)
	{
		if (collider.gameObject.tag == "ReturnTitle") {
			TextMesh title = collider.gameObject.GetComponent<TextMesh>();
			//だいたい1秒ごとに処理を行う
			timeleft -= Time.deltaTime;
			if (timeleft <= 0.0) {
				timeleft = 1.0f;
				time -= 1;
				title.text = time.ToString();
				if (time == 0) {
					SceneManager.LoadScene("Slither/Title/TitleScene");
					time = 6;
				}
			}
		}
	}
}
