using UnityEngine;
using System.Collections;

public class BombCreater : MonoBehaviour {
	public GameObject bombType;
	public AudioClip audioClip;
	private float INTERVAL = 5.0f;
	public float timer = 0.0f;
	

	// Use this for initialization
	void Start () {
		//timer = 0.0f;
		INTERVAL = timer;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer < 0.0f){
			Instantiate(bombType, this.transform.position, this.transform.rotation);
			AudioSource.PlayClipAtPoint(audioClip, new Vector3());
			timer = INTERVAL;
		}

	}
	void BlastOnce () {
		Instantiate(bombType, this.transform.position, this.transform.rotation);
		AudioSource.PlayClipAtPoint(audioClip, new Vector3());
	}
}
