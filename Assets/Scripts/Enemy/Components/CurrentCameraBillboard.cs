using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentCameraBillboard : MonoBehaviour {

	void Update () {
        // Shouldn't use main camera. Later on, need to find way to get current camera (DO NOT USE Camera.current)

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
    }
}
