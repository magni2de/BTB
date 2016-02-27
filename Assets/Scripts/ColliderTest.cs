using UnityEngine;
using System.Collections;

public class ColliderTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

		BoxCollider2D[] myColliders = gameObject.GetComponents<BoxCollider2D>();

		if (myColliders.Length > 0) {

			Debug.Log (string.Format ("ОК. Нашли {0} BoxCollider2D", myColliders.Length));

			foreach (BoxCollider2D bc in myColliders) {
				Debug.Log ("---------------------------------------------------------------------------");
				Debug.Log (string.Format ("Size: {0},       Offset: {1}", bc.size, bc.offset));
			}

		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
