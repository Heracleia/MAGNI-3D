using UnityEngine;
using System.Collections;

public class TargetCollision : MonoBehaviour {

	void OnCollisionEnter (Collision collision) {
		if (collision.transform.tag == "HydraObject" && transform.parent != null) {
			if(transform.parent.parent != null) {
				if(transform.parent.parent.GetComponent<TargetCreation>().currentTarget == transform.parent.GetComponent<TargetParent>().targetNum) {
					transform.parent.GetComponent<TargetParent> ().targetCollided ();
				}
			}
		}
	}
}
