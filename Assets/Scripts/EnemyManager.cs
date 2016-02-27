using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	public int levelWidth = 100;
	public int levelHeight = 100;

	public GameObject enemy;

	// Use this for initialization
	void Start () {

		Random.seed = System.Environment.TickCount;


		for (int i = 0; i < 25; i++) {

			float xx = Random.Range (0f, levelWidth);
			float yy = Random.Range (0f, levelHeight);

			Instantiate (enemy, new Vector3 (xx, yy, 0f), Quaternion.identity);
			
		}

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
