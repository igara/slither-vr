using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Doom scene script.
/// </summary>
public class SingleDoomSceneScript : MonoBehaviour {

	/// <summary>
	/// The m camera.
	/// </summary>
	[SerializeField] GameObject m_camera;
	/// <summary>
	/// The y offset. カメラの位置高さ
	/// </summary>
	private float camera_y_offset;


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

	private bool gameover_flag = false;

	/// <summary>
	/// Inits the start position. スタート位置の初期化
	/// </summary>
	private void InitStartPosition() {
		camera_y_offset = m_camera.transform.position.y;
		// オブジェクトの座標
		float position_x = Random.Range(zero_position, screen_size);
		float position_z = Random.Range(zero_position, screen_size);
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
	/// Start this instance.
	/// </summary>
	void Start () {
		InitStartPosition ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {

		if (!gameover_flag) {
			// 現在の角度を取得する
			float x = m_camera.transform.eulerAngles.x;

			// 角度以内なら前進する
			if ((zero_degree <= x && x <= degree) || (full_degree - degree <= x && x <= full_degree)) {
				MoveFoward ();
			}
			// アイテムを作成する
			CreateItem ();
		}
	}

	/// <summary>
	/// Moves the foward.
	/// 前進する処理
	/// </summary>
	private void MoveFoward() {
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
		int ramdam_item_index = Random.Range (0, m_items.Count);

		// だいたい1秒ごとに処理を行う
		timeleft -= Time.deltaTime;
		if (timeleft <= 0.0) {
			timeleft = 1.0f;

			if (ramdam_item_index == 0) {
				// バナナを生成
				CreatedBanana(m_items[ramdam_item_index]);
			} else if (ramdam_item_index == 1) {
				// パンを作成
				CreatedBread(m_items[ramdam_item_index]);
			} else if (ramdam_item_index == 2) {
				// ドーナツを作成
				CreatedDonut(m_items[ramdam_item_index]);
			}
		}

	}

	/// <summary>
	/// Gets the banana. バナナを作成する
	/// </summary>
	/// <param name="create_item">Create item.</param>
	private void CreatedBanana(GameObject create_item) {
		// オブジェクトの座標
		float position_x = Random.Range(zero_position, screen_size);
		float position_y = 1.5f;
		float position_z = Random.Range(zero_position, screen_size);
		Vector3 tmp_position = new Vector3 (
			position_x,
			position_y,
			position_z
		);
		// オブジェクトの角度
		float rotation_x = zero_degree;
		float rotation_y = Random.Range(zero_degree, full_degree);
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
		float position_x = Random.Range(zero_position, screen_size);
		float position_y = zero_position;
		float position_z = Random.Range(zero_position, screen_size);
		Vector3 tmp_position = new Vector3 (
			position_x,
			position_y,
			position_z
		);
		// オブジェクトの角度
		float rotation_x = zero_degree;
		float rotation_y = Random.Range(zero_degree, full_degree);
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
		float position_x = Random.Range(zero_position, screen_size);
		float position_y = zero_position;
		float position_z = Random.Range(zero_position, screen_size);
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
	/// Redirecteds the on trigger stay. 子のゲームオブジェクトのcolliderを終了検知する
	/// </summary>
	/// <param name="collider">Collider.</param>
	public void RedirectedOnTriggerStay (Collider collider)
	{
		
	}

	/// <summary>
	/// Eats the item.
	/// </summary>
	private void EatItem(GameObject item) {
		Destroy (item);
		// オブジェクトを生産
		GameObject new_body = (GameObject)Instantiate(
			m_worm_body [m_worm_body.Count - 1],
			m_worm_body [m_worm_body.Count - 1].transform.position,
			m_worm_body [m_worm_body.Count - 1].transform.rotation
		);
		// 芋虫のオブジェクトの子になる様にアイテムを配置を行う
		new_body.transform.parent = m_worm.transform;
		// 子のオブジェクトにした時にスケールは変更させない
		new_body.transform.localScale = Vector3.one;
		m_worm_body.Add (new_body);
	}

	/// <summary>
	/// Swichs the game over.
	/// </summary>
	private void SwichGameOver() {
		gameover_flag = true;
	}
}
