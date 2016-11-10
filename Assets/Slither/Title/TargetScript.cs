using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Target script.
/// </summary>
public class TargetScript : MonoBehaviour {

	private float timeleft;
	private int time = 6;

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter(Collider collider) {
	}

	/// <summary>
	/// Raises the trigger stay event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerStay(Collider collider) {
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

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerExit(Collider collider){
		if (collider.gameObject.tag == "SinglePlay") {
			TextMesh start = collider.gameObject.GetComponent<TextMesh>();
			start.text = "SinglePlay";
			time = 6;
		}
	}
}