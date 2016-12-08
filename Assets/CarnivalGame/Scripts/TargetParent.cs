using UnityEngine;
using System.Collections;

public class TargetParent : MonoBehaviour {

	public int targetNum;
	public int subpiecesEraseTime = 30;

	public void targetCollided () {
		for(int i = 0; i < 3; i ++) {
			transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		}
		Destroy (transform.GetChild (0).gameObject, subpiecesEraseTime);
		Destroy (transform.GetChild (1).gameObject, subpiecesEraseTime);
		Destroy (transform.GetChild (2).gameObject, subpiecesEraseTime);
		Destroy (transform.GetChild (3).gameObject);
		transform.DetachChildren();
		Destroy(gameObject);
	}
}
