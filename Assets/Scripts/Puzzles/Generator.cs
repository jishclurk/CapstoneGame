using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour,ICircuitPiece {

	private ICircuitPiece[] input;
	MeshRenderer connector;
	MeshRenderer genMesh;
	Material broken1;
	Material broken2;
	Material fixed1;
	Light genSpot;
	Material genActive;
	Material genInactive;
	ParticleSystem ps1;
	bool turnedOn;
	Gen_Collide trigger;
	private bool solved;
	float t = 1.0f;
	float minimum = -0.75f;
	float maximum = 0.75f;
	float flickerTime = 0.2f;
	float cTime;
	bool flick;


	// Use this for initialization
	void Start () {
		input = GetComponentsInChildren<ICircuitPiece> ();
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();
		genMesh = GetComponent<MeshRenderer> ();
		broken1 = Resources.Load ("Materials/Gen_Broken2") as Material;
		broken2 = Resources.Load ("Materials/Gen_Broken1") as Material;
		fixed1 = Resources.Load ("Materials/Eq_2") as Material;
		genSpot = this.gameObject.transform.Find ("Spot Light").GetComponent<Light> ();
		turnedOn = false;
		trigger = this.gameObject.transform.Find ("ColliderEq_2").GetComponent<Gen_Collide> ();
		solved = false;
		genActive = Resources.Load ("Materials/Green_Beam") as Material;
		genInactive = Resources.Load("Materials/White_Beam") as Material;
		cTime = 0;
		flick = false;
		ps1 = this.transform.FindChild ("Lightning").GetComponent<ParticleSystem> ();
		ps1.Play ();
	}

	// Update is called once per frame
	void Update () {
		

		if (!solved) {
			turnedOn = trigger.triggered;
			if (turnedOn) {
				if (this.Output ()) {
					connector.sharedMaterial = genActive;
					genMesh.sharedMaterial = fixed1;
					if (ps1.isPlaying) {
						ps1.Stop ();
						//ps2.Stop ();
					}
					input [1].Lock ();
					LockMyself ();

				} else {
					connector.sharedMaterial = genInactive;
				}
				genSpot.intensity = 100;
			} else {
				genSpot.intensity = 0;
				flicker ();
				if (ps1.isStopped) {
					ps1.Play ();
					//ps2.Play ();
				}
			}
		} 

		connector.sharedMaterial.mainTextureOffset = new Vector2(0.0f, Mathf.Lerp(minimum,maximum,t));
		// .. and increate the t interpolater
		t += 0.75f * Time.deltaTime;
		if (t > 1.0f){
			t = 0.0f;
		}
		//} else {


		//}
	}

	public bool Output(){
		return input[1].Output ();
	}

	public bool isSending(){
		return Output () && turnedOn;
	}

	public void Lock(){
		//solved = true;
	}

	void flicker(){
			if (cTime < flickerTime) {
				if (flick) {
				genMesh.sharedMaterial = broken1;
				} else {
				genMesh.sharedMaterial = broken2;
				}

			} else {
				flick = !flick;
				cTime = 0;

			}
		cTime += Time.deltaTime;
//		float lerp = Mathf.PingPong(Time.time, 1.0f) / 1.0f;
//		genMesh.material.Lerp(broken1, broken2, t);

	}

	public void LockMyself(){
		solved = true;
	}

	public Transform GetTransform(){
		return this.transform;
	}

	public void Solve(){
		connector.sharedMaterial = genActive;
		genMesh.sharedMaterial = fixed1;
		if (ps1.isPlaying) {
			ps1.Stop ();
			//ps2.Stop ();
		}
		turnedOn = true;
		LockMyself ();
		input [1].Solve ();
	}


}
