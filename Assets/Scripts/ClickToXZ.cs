using UnityEngine;
using System.Collections;

public class ClickToXZ : MonoBehaviour {
	public GameObject character;
	Vector3 worldPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) == true) {
			Vector3 screenPoint = Input.mousePosition;
			screenPoint.z = 10.0f;
			worldPoint = GetComponent<Camera>().ScreenToWorldPoint (screenPoint);
			Debug.Log (worldPoint);

			if (character != null) {
				character.transform.position = worldPoint;
			}
		}
	}
}
