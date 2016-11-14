using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Title scene script.
/// </summary>
public class TitleSceneScript : MonoBehaviour {

	/// <summary>
	/// The m camera.
	/// </summary>
	[SerializeField] GameObject m_camera;

	/// <summary>
	/// The m title.
	/// </summary>
	[SerializeField] GameObject m_title;

	/// <summary>
	/// The m start.
	/// </summary>
	[SerializeField] GameObject m_start;

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
	/// Start this instance.
	/// </summary>
	void Start () {

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
		if (collider.gameObject.tag == "SinglePlay") {
			TextMesh start = collider.gameObject.GetComponent<TextMesh>();
			start.text = "SinglePlay";
			time = 6;
		}
	}

	/// <summary>
	/// Redirecteds the on trigger stay. 子のゲームオブジェクトのcolliderをキープ検知する
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerStay (Collider collider)
	{
		if (collider.gameObject.tag == "SinglePlay") {
			TextMesh start = collider.gameObject.GetComponent<TextMesh>();
			//だいたい1秒ごとに処理を行う
			timeleft -= Time.deltaTime;
			if (timeleft <= 0.0) {
				timeleft = 1.0f;
				time -= 1;
				start.text = time.ToString();
				if (time == 0) {
					SceneManager.LoadScene("Slither/SingleDoom/SingleDoomScene");
				}
			}
		}
	}
}
