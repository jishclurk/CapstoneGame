using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzle : MonoBehaviour, ICircuitPiece {


	List<ICircuitPiece> result;
	MeshRenderer hub;
	public Material hubActive;
	public Material hubInactive;
	MeshRenderer connector;
	ParticleSystem ps;
	bool solved;

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

				Debug.Log ("circuit piece 1: "+cp.ToString()+"\n");
				result.Add (cp);
				Debug.Log ("circuit piece 1: "+result[0].ToString()+"\n");
			}

		}
		//input = GetComponentsInChildren<ICircuitPiece> ();
		//Debug.Log ("circuit piece 1: "+result..ToString()+"\n" + "circuit piece 2:"+result[1].ToString());
		hub = GetComponent<MeshRenderer> ();
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();
		solved = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (!solved) {
			if (this.Output ()) {
				hub.material = hubActive;
				connector.material = hubActive;
				if (ps.isStopped) {
					ps.Play ();
				}
				//Debug.Log ("got here");
			} else {
				hub.material = hubInactive;
				connector.material = hubInactive;
				if (ps.isPlaying) {
					ps.Stop ();
				}
			}
		}

	}

	public void Lock(){
		solved = true;
		result [0].Lock ();
		result [1].Lock ();
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
		} 
		else{
			return false;
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




}

public enum GateType{
	OR, AND, NOR, XOR, XNOR, NAND

}
