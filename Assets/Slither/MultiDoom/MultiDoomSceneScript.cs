using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class MultiDoomSceneScript : MonoBehaviour {

	/// <summary>
	/// The m camera.
	/// </summary>
	[SerializeField] GameObject m_camera;
	/// <summary>
	/// The y offset. カメラの位置高さ
	/// </summary>
	private float camera_y_offset;
	/// <summary>
	/// The m gvr viewer.
	/// </summary>
	[SerializeField] GvrViewer m_gvr_viewer;

	/// <summary>
	/// The m target mark.赤い×印
	/// </summary>
	[SerializeField] GameObject m_target_mark;

	/// <summary>
	/// The m target dummy. ダミーがオブジェクト触れた時にイベントを発生させる
	/// </summary>
	[SerializeField] GameObject m_target_dummy;

	/// <summary>
	/// The move speed. 動くスピード
	/// </summary>
	public float move_speed  = 10.0f;

	/// <summary>
	/// The degree. ミミズが動く角度の範囲
	/// </summary>
	const float degree = 30.0f;

	/// <summary>
	/// The zero degree. 角度なし
	/// </summary>
	const float zero_degree = 0.0f;
	/// <summary>
	/// The full degree. 全角度
	/// </summary>
	const float full_degree = 360.0f;

	/// <summary>
	/// The zero position. 0位置
	/// </summary>
	const float zero_position = 0.0f;

	/// <summary>
	/// The size of the screen. 画面のサイズ
	/// </summary>
	const float screen_size = 500.0f;

	/// <summary>
	/// The m worm. ミミズ全体のオブジェクト
	/// </summary>
	[SerializeField] GameObject m_worm;
	/// <summary>
	/// The m worm head.
	/// </summary>
	[SerializeField] GameObject m_worm_head;
	/// <summary>
	/// The m worm left eye.
	/// </summary>
	[SerializeField] GameObject m_worm_left_eye;
	/// <summary>
	/// The m worm right eye.
	/// </summary>
	[SerializeField] GameObject m_worm_right_eye;
	/// <summary>
	/// The m worm body. ミミズの胴体部分
	/// </summary>
	[SerializeField] List<GameObject> m_worm_body;

	/// <summary>
	/// The m item.
	/// </summary>
	[SerializeField] List<GameObject> m_items;

	/// <summary>
	/// The m terrain. 地面のゲームオブジェクト
	/// </summary>
	[SerializeField] GameObject m_terrain;

	/// <summary>
	/// The timeleft. 秒計算
	/// </summary>
	private float timeleft;

	/// <summary>
	/// The gameover flag.
	/// </summary>
	private bool gameover_flag = false;

	/// <summary>
	/// The pose flag.
	/// </summary>
	private bool pose_flag = false;

	/// <summary>
	/// The audio source.
	/// </summary>
	[SerializeField] AudioSource[] audio_source;

	/// <summary>
	/// Sound.
	/// </summary>
	private enum Sound {
		Eat,
        BGM
	}

	/// <summary>
	/// The web socket.
	/// </summary>
	private WebSocket web_socket;

	/// <summary>
	/// The client identifier.
	/// </summary>
	private string client_id;

	/// <summary>
	/// The user data.
	/// </summary>
	private UserData user_data;

	/// <summary>
	/// The user data dic.
	/// </summary>
	private Dictionary<string, UserData> user_data_dic;

	/// <summary>
	/// The m another user. 他のユーザの集まりを管理
	/// </summary>
	[SerializeField] GameObject m_another_user;

	/// <summary>
	/// Inits the start position. スタート位置の初期化
	/// </summary>
	private void InitStartPosition() {

		camera_y_offset = m_camera.transform.position.y;
		// オブジェクトの座標
		float position_x = UnityEngine.Random.Range(zero_position, screen_size);
		float position_z = UnityEngine.Random.Range(zero_position, screen_size);

		Renderer renderer = m_worm.transform.FindChild("EarthwormBody0").gameObject.GetComponent<Renderer>();
		renderer.material.color = GameSetting.select_worm_color;
		renderer = m_worm.transform.FindChild("EarthwormBody1").gameObject.GetComponent<Renderer>();
		renderer.material.color = GameSetting.select_worm_color;
		renderer = m_worm.transform.FindChild("EarthwormBody2").gameObject.GetComponent<Renderer>();
		renderer.material.color = GameSetting.select_worm_color;
		renderer = m_worm.transform.FindChild("EarthwormBody3").gameObject.GetComponent<Renderer>();
		renderer.material.color = GameSetting.select_worm_color;
		renderer = m_worm.transform.FindChild("EarthwormBody4").gameObject.GetComponent<Renderer>();
		renderer.material.color = GameSetting.select_worm_color;
		renderer = m_worm.transform.FindChild("EarthwormBody5").gameObject.GetComponent<Renderer>();
		renderer.material.color = GameSetting.select_worm_color;
		// 虫の開始位置をランダムで
		m_worm.transform.position = new Vector3 (
			position_x,
			m_worm.transform.position.y,
			position_z
		);
		m_camera.transform.position = new Vector3 (
			position_x,
			m_camera.transform.position.y,
			position_z + 5
		);
	}

	/// <summary>
	/// Inits the web socket.
	/// </summary>
	private void InitWebSocket() {
		user_data_dic = new Dictionary<string, UserData>();
		web_socket = new WebSocket(GameSetting.websoket_server);

		web_socket.Connect();
		web_socket.OnMessage += (object sender, MessageEventArgs e) =>
		{
			UserData another_user_data = new UserData();
			try {
				// client_idを取得
				another_user_data = JsonUtility.FromJson<UserData>(e.Data);
			} catch {
				if (client_id == null) {
					client_id = e.Data;
				}
				return;
			}
			if (another_user_data.client_id != client_id) {
				// 違うユーザ
				if (user_data_dic.ContainsKey(another_user_data.client_id)) {
					// すでにユーザが存在するとき
					user_data_dic[another_user_data.client_id] = another_user_data;
//					print("存在する時");
				} else {
					// ユーザが存在しないとき
					user_data_dic.Add(another_user_data.client_id, another_user_data);
//					print("存在しない時");
				}
			} else if(another_user_data.client_id == client_id) {
				// 同じユーザ
			}
		};
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
        audio_source[(int)Sound.BGM].Play();
		m_gvr_viewer.VRModeEnabled = GameSetting.vr_mode_flag;
		InitStartPosition ();
		InitWebSocket ();
		FetchWebSocket ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		if (!gameover_flag) {
			FetchWebSocket ();
			FetchUser();
			// 現在の角度を取得する
			float x = m_camera.transform.eulerAngles.x;

			// 角度以内なら前進する
			if ((zero_degree <= x && x <= degree) || (full_degree - degree <= x && x <= full_degree)) {
				MoveForward ();
			}
			// アイテムを作成する
			CreateItem ();
		}
	}

	/// <summary>
	/// Fetch the web socket.
	/// </summary>
	private void FetchWebSocket() {
		if (client_id != null) {
			user_data = new UserData ();
			user_data.client_id = client_id;
			user_data.count = m_worm_body.Count;
			user_data.position = m_worm_body [0].gameObject.transform.position;
			user_data.rotation = m_worm_body [0].gameObject.transform.rotation;
			user_data.color = GameSetting.select_worm_color;

			string json = JsonUtility.ToJson(user_data);
			web_socket.Send (json);
		}
	}

	/// <summary>
	/// Fetch the user.
	/// </summary>
	private void FetchUser() {
		if (user_data_dic != null) {
			Dictionary<string, UserData> user_data_tmp = user_data_dic;
			List<string> client_ids = new List<string>(user_data_tmp.Keys);
			foreach (string client_id in client_ids) {
				// DicからUserDataを取り出す
				UserData user_data = user_data_tmp[client_id];
				if (GameObject.Find (client_id) == null) {
					// 他のユーザのゲームオブジェクトがない時
					GameObject user = m_worm;
					// オブジェクトを生産
					GameObject item = (GameObject)Instantiate(
							user,
							user_data.position,
							user_data.rotation
					);
					item.name = client_id;

					foreach (Transform body_transform in item.transform) {
                        Renderer renderer = body_transform.GetComponent<Renderer>();
                        renderer.material.color = user_data.color;
                    }
					// 地面のオブジェクトの子になる様にアイテムを配置を行う
					item.transform.parent = m_another_user.transform;
				} else {
					// 他のユーザのゲームオブジェクトがある時
					GameObject item = GameObject.Find(client_id);
                    Vector3 tmp_position = new Vector3 (
                            zero_position,
                            zero_position,
                            zero_position
                    );
                    Quaternion tmp_rotation = Quaternion.Euler (new Vector3 (
                            zero_degree,
                            zero_degree,
                            zero_degree
                    ));
                    foreach (Transform body_transform in item.transform) {
                        if (body_transform.name == "EarthwormBody0") {
                            tmp_position = new Vector3 (
                                    body_transform.gameObject.transform.position.x,
                                    body_transform.gameObject.transform.position.y,
                                    body_transform.gameObject.transform.position.z
                            );
                            tmp_rotation = Quaternion.Euler (new Vector3 (
                                    body_transform.gameObject.transform.rotation.x,
                                    body_transform.gameObject.transform.rotation.y,
                                    body_transform.gameObject.transform.rotation.z
                            ));
							body_transform.transform.position = new Vector3(
									user_data.position.x,
									0,
									user_data.position.z
							);
						} else {
                            Vector3 tmp2_position = new Vector3 (
                                    body_transform.gameObject.transform.position.x,
                                    body_transform.gameObject.transform.position.y,
                                    body_transform.gameObject.transform.position.z
                            );
                            Quaternion tmp2_rotation = Quaternion.Euler (new Vector3 (
                                    body_transform.gameObject.transform.rotation.x,
                                    body_transform.gameObject.transform.rotation.y,
                                    body_transform.gameObject.transform.rotation.z
                            ));
                            body_transform.gameObject.transform.position = tmp_position;
                            body_transform.gameObject.transform.rotation = tmp_rotation;
                            tmp_position = tmp2_position;
                            tmp_rotation = tmp2_rotation;
                        }
                    }
					item.transform.rotation = user_data.rotation;
				}
			}
		}
	}

	/// <summary>
	/// Moves the forward.
	/// 前進する処理
	/// </summary>
	private void MoveForward() {
		Vector3 camera_direction = new Vector3 (
			m_camera.transform.forward.x,
			zero_position,
			m_camera.transform.forward.z).normalized * move_speed * Time.deltaTime;
		Quaternion camera_rotation = Quaternion.Euler (new Vector3 (
			zero_position,
			-m_camera.transform.rotation.eulerAngles.y,
			zero_position
		));
		m_camera.transform.Translate (camera_rotation * camera_direction);
		m_camera.transform.position = new Vector3 (
			m_camera.transform.position.x,
			camera_y_offset,
			m_camera.transform.position.z
		);

		Vector3 tmp_position = new Vector3 (
			zero_position,
			zero_position,
			zero_position
		);
		Quaternion tmp_rotation = Quaternion.Euler (new Vector3 (
			zero_degree,
			zero_degree,
			zero_degree
		));
		for (int i = 0; i < m_worm_body.Count; i++) {
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
				m_worm_body [i].gameObject.transform.position = new Vector3(
					m_camera.transform.position.x,
					zero_position,
					m_camera.transform.position.z
				);
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

	/// <summary>
	/// Creates the item.
	/// アイテムを生成する処理
	/// </summary>
	private void CreateItem() {
		// 追加するアイテムをランダムで選択する
		int randam_item_index = UnityEngine.Random.Range (0, m_items.Count);

		// だいたい1秒ごとに処理を行う
		timeleft -= Time.deltaTime;
		if (timeleft <= 0.0) {
			timeleft = 1.0f;

			if (randam_item_index == 0) {
				// バナナを生成
				CreatedBanana(m_items[randam_item_index]);
			} else if (randam_item_index == 1) {
				// パンを作成
				CreatedBread(m_items[randam_item_index]);
			} else if (randam_item_index == 2) {
				// ドーナツを作成
				CreatedDonut(m_items[randam_item_index]);
			}
		}

	}

	/// <summary>
	/// Gets the banana. バナナを作成する
	/// </summary>
	/// <param name="create_item">Create item.</param>
	private void CreatedBanana(GameObject create_item) {
		// オブジェクトの座標
		float position_x = UnityEngine.Random.Range(zero_position, screen_size);
		float position_y = 1.5f;
		float position_z = UnityEngine.Random.Range(zero_position, screen_size);
		Vector3 tmp_position = new Vector3 (
			position_x,
			position_y,
			position_z
		);
		// オブジェクトの角度
		float rotation_x = zero_degree;
		float rotation_y = UnityEngine.Random.Range(zero_degree, full_degree);
		float rotation_z = 90;
		Quaternion tmp_rotation = Quaternion.Euler (new Vector3 (
			rotation_x,
			rotation_y,
			rotation_z
		));

		// オブジェクトを生産
		GameObject item = (GameObject)Instantiate(
			create_item,
			tmp_position,
			tmp_rotation
		);
		// 地面のオブジェクトの子になる様にアイテムを配置を行う
		item.transform.parent = m_terrain.transform;
	}

	/// <summary>
	/// Createds the bread. パンを作成する
	/// </summary>
	/// <param name="create_item">Create item.</param>
	private void CreatedBread(GameObject create_item) {
		// オブジェクトの座標
		float position_x = UnityEngine.Random.Range(zero_position, screen_size);
		float position_y = zero_position;
		float position_z = UnityEngine.Random.Range(zero_position, screen_size);
		Vector3 tmp_position = new Vector3 (
			position_x,
			position_y,
			position_z
		);
		// オブジェクトの角度
		float rotation_x = zero_degree;
		float rotation_y = UnityEngine.Random.Range(zero_degree, full_degree);
		float rotation_z = zero_degree;
		Quaternion tmp_rotation = Quaternion.Euler (new Vector3 (
			rotation_x,
			rotation_y,
			rotation_z
		));

		// オブジェクトを生産
		GameObject item = (GameObject)Instantiate(
			create_item,
			tmp_position,
			tmp_rotation
		);
		// 地面のオブジェクトの子になる様にアイテムを配置を行う
		item.transform.parent = m_terrain.transform;
	}

	/// <summary>
	/// Createds the donut. ドーナツを作成する
	/// </summary>
	/// <param name="create_item">Create item.</param>
	private void CreatedDonut(GameObject create_item) {
		// オブジェクトの座標
		float position_x = UnityEngine.Random.Range(zero_position, screen_size);
		float position_y = zero_position;
		float position_z = UnityEngine.Random.Range(zero_position, screen_size);
		Vector3 tmp_position = new Vector3 (
			position_x,
			position_y,
			position_z
		);
		// オブジェクトの角度
		float rotation_x = 90.0f;
		float rotation_y = zero_degree;
		float rotation_z = zero_degree;
		Quaternion tmp_rotation = Quaternion.Euler (new Vector3 (
			rotation_x,
			rotation_y,
			rotation_z
		));

		// オブジェクトを生産
		GameObject item = (GameObject)Instantiate(
			create_item,
			tmp_position,
			tmp_rotation
		);
		// 地面のオブジェクトの子になる様にアイテムを配置を行う
		item.transform.parent = m_terrain.transform;
	}

	/// <summary>
	/// Redirecteds the on trigger enter. 子のゲームオブジェクトのcolliderを開始検知する
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerEnter (Collider collider)
	{
		string gameobject_name = collider.gameObject.name;
		if (gameobject_name == "banana(Clone)") {
			// バナナと衝突
			EatItem(collider.gameObject);
		} else if (gameobject_name == "bread(Clone)") {
			// パンと衝突
			EatItem(collider.gameObject);
		} else if (gameobject_name == "donut(Clone)") {
			// ドーナツと衝突
			EatItem(collider.gameObject);
		}
		if (gameobject_name == "TargetDummy") {
			SwichPose ();
		}
		if (gameobject_name == "Wall1") {
			SwichGameOver ();
		} else if (gameobject_name == "Wall2") {
			SwichGameOver ();
		} else if (gameobject_name == "Wall3") {
			SwichGameOver ();
		} else if (gameobject_name == "Wall4") {
			SwichGameOver ();
		}
	}

	/// <summary>
	/// Createds the Another User. 他のユーザを作成する
	/// </summary>
	/// <param name="user_data">User data.</param>
	private void CreatedAnotherUser(UserData user_data) {
		try {
			GameObject.Find (user_data.client_id);
			// すでにこのユーザが作成されていたとき

		} catch {
			// ユーザが作成されていないとき
			// オブジェクトを生産
			GameObject user = (GameObject)Instantiate(
					m_worm_head,
				// オブジェクトの座標
					user_data.position,
				// オブジェクトの角度
					user_data.rotation
			);
			// 地面のオブジェクトの子になる様にアイテムを配置を行う
			user.transform.parent = m_terrain.transform;
		}
	}

	/// <summary>
	/// Redirecteds the on trigger exit.
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerExit (Collider collider)
	{
		string gameobject_name = collider.gameObject.name;
		if (gameobject_name == "TargetDummy") {
			SwichPose ();
		}
	}

	/// <summary>
	/// Redirecteds the on trigger stay. 子のゲームオブジェクトのcolliderをキープ検知する
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerStay (Collider collider)
	{
		string gameobject_name = collider.gameObject.name;
	}

	/// <summary>
	/// Eats the item.
	/// </summary>
	private void EatItem(GameObject item) {
		Destroy (item);
		audio_source[(int)Sound.Eat].Play();
		// オブジェクトを生産
		GameObject new_body = (GameObject)Instantiate(
			m_worm_body [m_worm_body.Count - 1],
			m_worm_body [m_worm_body.Count - 1].transform.position,
			m_worm_body [m_worm_body.Count - 1].transform.rotation
		);
		// 芋虫のオブジェクトの子になる様にアイテムを配置を行う
		new_body.transform.parent = m_worm.transform;
        new_body.name = "EarthwormBody" + m_worm_body.Count;
		// 子のオブジェクトにした時にスケールは変更させない
		new_body.transform.localScale = Vector3.one;
		m_worm_body.Add (new_body);
	}

	/// <summary>
	/// Swichs the game over.
	/// </summary>
	private void SwichGameOver() {
		gameover_flag = !gameover_flag;
		SceneManager.LoadScene("Slither/GameOver/GameOverScene");
	}

	/// <summary>
	/// Swichs the pose.
	/// </summary>
	private void SwichPose() {
		pose_flag = !pose_flag;
	}

	/// <summary>
	/// メモリの限界が来たとき
	/// </summary>
	/// <param name="message"></param>
	public void DidReceiveMemoryWarning (string message) {
		System.GC.Collect ();
		Resources.UnloadUnusedAssets ();
	}

	/// <summary>
	///
	/// </summary>
	void OnDisable() {

		web_socket.Close(CloseStatusCode.Normal, client_id);
	}
}
