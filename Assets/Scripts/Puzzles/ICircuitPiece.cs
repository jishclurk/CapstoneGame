using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICircuitPiece{

	bool Output ();
	void Lock ();
	Transform GetTransform();
}
