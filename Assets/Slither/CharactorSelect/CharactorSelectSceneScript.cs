﻿using UnityEngine;
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
			string game_object_name = worm.name;
			if (game_object_name == "Earthworm1") {
				Renderer renderer = worm.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody1").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody2").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody3").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody4").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody5").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 0, 0, 1));
			} else if(game_object_name == "Earthworm2") {
				Renderer renderer = worm.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody1").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody2").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody3").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody4").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody5").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(1, 1, 0, 1));
			} else if(game_object_name == "Earthworm3") {
				Renderer renderer = worm.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody1").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody2").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody3").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody4").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody5").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 0, 1));
			} else if(game_object_name == "Earthworm4") {
				Renderer renderer = worm.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody1").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody2").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody3").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody4").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody5").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 1, 1, 1));
			} else if(game_object_name == "Earthworm5") {
				Renderer renderer = worm.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody1").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody2").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody3").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody4").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 0, 1));
				renderer = worm.transform.FindChild("EarthwormBody5").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 0, 1));
			} else if(game_object_name == "Earthworm6") {
				Renderer renderer = worm.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody1").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody2").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody3").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody4").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 1, 1));
				renderer = worm.transform.FindChild("EarthwormBody5").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0, 0, 1, 1));
			} else if(game_object_name == "Earthworm7") {
				Renderer renderer = worm.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1));
				renderer = worm.transform.FindChild("EarthwormBody1").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1));
				renderer = worm.transform.FindChild("EarthwormBody2").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1));
				renderer = worm.transform.FindChild("EarthwormBody3").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1));
				renderer = worm.transform.FindChild("EarthwormBody4").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1));
				renderer = worm.transform.FindChild("EarthwormBody5").gameObject.GetComponent<Renderer>();
				renderer.material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1));
			}
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
					Renderer renderer = collider.gameObject.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
					GameSetting.select_worm_color = renderer.material.GetColor("_Color");
					if ((GameSetting.game_mode_status ?? "Single") == "Single") {
						SceneManager.LoadScene ("Slither/SingleDoom/SingleDoomScene");
					} else if (GameSetting.game_mode_status == "Multi") {
						SceneManager.LoadScene ("Slither/MultiDoom/MultiDoomScene");
					}
					time = 6;
				}
			}
		}
	}

}
