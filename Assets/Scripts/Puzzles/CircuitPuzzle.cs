using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzle : MonoBehaviour, ICircuitPiece {


	ICircuitPiece[] input;
	MeshRenderer hub;
	public Material hubActive;
	public Material hubInactive;
	MeshRenderer connector;

	public GateType gateType;

	// Use this for initialization
	void Start () {
		input = GetComponentsInChildren<ICircuitPiece> ();
		Debug.Log ("circuit pieces count: "+input.Length);
		hub = GetComponent<MeshRenderer> ();
		connector = this.gameObject.transform.Find ("Out Connector").GetComponent<MeshRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Output ()) {
			hub.material = connector.material = hubActive;
			Debug.Log ("got here");
		} 
		else {
			hub.material = connector.material = hubInactive;
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
		} 
		else{
			return false;
		}
	
	}

	bool Or(){
				return input[1].Output() || input[2].Output();
		//return true;
	}
	bool And(){
				return input[1].Output() && input[2].Output();
		//return false;
	}

	bool XOr(){
		return input[1].Output() ^ input[2].Output();
	}

	bool NAnd(){
		return !(input[1].Output() && input[2].Output());
	}

	bool XNor(){
		return input[1].Output() == input[2].Output();
	}

	bool NOr(){
		return ! (input[1].Output() || input[2].Output());
		//return true;
	}




}

public enum GateType{
	OR, AND, NOR, XOR, XNOR, NAND

}
