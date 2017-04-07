using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzle : MonoBehaviour, ICircuitPiece {


	List<ICircuitPiece> result;
	MeshRenderer hub;
	private Material hubActive;
	private Material hubInactive;
	MeshRenderer connector;
	ParticleSystem ps;
	bool loadSolved;
	bool solved;
	float t = 1.0f;
	float minimum = 0.75f;
	float maximum = -0.75f;

	public GateType gateType;

	// Use this for initialization
	void Start () {
		ICircuitPiece[] input = this.transform.GetComponentsInChildren<ICircuitPiece>();
		ps = this.transform.FindChild ("GreenPortal").GetComponent<ParticleSystem> ();
		ps.Stop ();
		result = new List<ICircuitPiece>();
		foreach (ICircuitPiece cp in input)
		{
			if (cp.GetTransform ().parent == this.transform) {

				//Debug.Log ("circuit piece 1: "+cp.ToString()+"\n");
				result.Add (cp);
				//Debug.Log ("circuit piece 1: "+result[0].ToString()+"\n");
			}

		}
		//input = GetComponentsInChildren<ICircuitPiece> ();
		//Debug.Log ("circuit piece 1: "+result..ToString()+"\n" + "circuit piece 2:"+result[1].ToString());
		hub = GetComponent<MeshRenderer> ();
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();
		solved = false;

		hubActive = Resources.Load ("Materials/Green_Beam") as Material;
		AssignColor ();
		loadSolved = false;
		//hubInactive = Resources.Load ("Materials/White_Beam") as Material;

	}
	
	// Update is called once per frame
	void Update () {
		if (!solved) {
			if (this.Output ()) {
				hub.sharedMaterial = hubActive;
				connector.sharedMaterial = hubActive;
				if (ps.isStopped) {
					ps.Play ();
				}
				//Debug.Log ("got here");
			} else {
				hub.sharedMaterial = hubInactive;
				connector.sharedMaterial = hubInactive;
				if (ps.isPlaying) {
					ps.Stop ();
				}
			}

			lerpConnector ();


		} else {
			hub.sharedMaterial = hubActive;
			connector.sharedMaterial = hubActive;
			lerpConnector ();
		}

	}


	void lerpConnector(){
		connector.sharedMaterial.mainTextureOffset = new Vector2 (0.0f, Mathf.Lerp (minimum, maximum, t));
		// .. and increate the t interpolater
		t += 0.75f * Time.deltaTime;
		if (t > 1.0f) {
			t = 0.0f;
		}
		if (ps.isStopped) {
			ps.Play ();
		}
	}
	public void Lock(){
		solved = true;
		result [0].Lock ();
		result [1].Lock ();
	}

	public void Solve(){
		if (gateType == GateType.OR) {
			SolveOr ();
		} else if (gateType == GateType.AND) {
			SolveAnd ();
		} else if (gateType == GateType.XOR) {
			SolveXOr ();
		} else if (gateType == GateType.NAND) {
			SolveNAnd ();
		} else if (gateType == GateType.NOR) {
			SolveOrNOr ();
		} else if (gateType == GateType.XNOR) {
			SolveOrXNor ();
		} 

	}

	public bool Output(){
			if (gateType == GateType.OR) {
				return Or ();
			} else if (gateType == GateType.AND) {
				return And ();
			} else if (gateType == GateType.XOR) {
				return XOr ();
			} else if (gateType == GateType.NAND) {
				return NAnd ();
			} else if (gateType == GateType.NOR) {
				return NOr ();
			} else if (gateType == GateType.XNOR) {
				return XNor ();
			} else {
				return false;
			}
		
	
	}

	void AssignColor(){
		if (gateType == GateType.OR) {
			hubInactive= Resources.Load ("Materials/LightBlue_Beam") as Material;
		} else if (gateType == GateType.AND) {
			hubInactive= Resources.Load ("Materials/Yellow_Beam") as Material;
		} else if (gateType == GateType.XOR) {
			hubInactive= Resources.Load ("Materials/Orange_Beam") as Material;
		} else if (gateType == GateType.NAND) {
			hubInactive= Resources.Load ("Materials/Blue_Beam") as Material;
		} else if (gateType == GateType.NOR) {
			hubInactive= Resources.Load ("Materials/Red_Beam") as Material;
		} else if (gateType == GateType.XNOR) {
			hubInactive= Resources.Load ("Materials/Magenta_Beam") as Material;
		} 
		else{
			hubInactive= Resources.Load ("Materials/Black_Beam") as Material;
		}

	}

	public Transform GetTransform(){
		return this.transform;
	}

	bool Or(){
		return result[0].Output() || result[1].Output();
		//return true;
	}
	bool And(){
				return result[0].Output() && result[1].Output();
		//return false;
	}

	bool XOr(){
		return result[0].Output() != result[1].Output();
	}

	bool NAnd(){
		return !(result[0].Output() && result[1].Output());
	}

	bool XNor(){
		return result[0].Output() == result[1].Output();
	}

	bool NOr(){
		return ! (result[0].Output() || result[1].Output());
		//return true;
	}

	void SolveOr(){
		if (!(result [0].Output () || result [1].Output ())) {
			result [0].Output ();
		}
		result [0].Lock ();
		result [1].Lock ();
		//return true;
	}
	void SolveAnd(){
		if (!(result [0].Output () && result[1].Output())) {
			result [0].Solve ();
			result [1].Solve ();
		}
		result [0].Lock ();
		result [1].Lock ();
		//return result[0].Output() && result[1].Output();
		//return false;
	}

	void SolveXOr(){
		if (result [0].Output () == result[1].Output()) {
			result [0].Solve ();
		}


		result [0].Lock ();
		result [1].Lock ();
		//return result[0].Output() != result[1].Output();
	}

	void SolveNAnd(){
		//return !(result[0].Output() && result[1].Output());
	}

	void SolveOrXNor(){
		//return result[0].Output() == result[1].Output();
	}

	void SolveOrNOr(){
		//return ! (result[0].Output() || result[1].Output());
		//return true;
	}




}

public enum GateType{
	OR, AND, NOR, XOR, XNOR, NAND

}
