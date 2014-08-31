using UnityEngine;
using System.Collections;

public class BombBlaster : MonoBehaviour {
	public GameObject bombType;
	public AudioClip audioClip;

	void BlastOnce () {
		Instantiate(bombType, this.transform.position, this.transform.rotation);
		AudioSource.PlayClipAtPoint(audioClip, new Vector3());
	}
}
