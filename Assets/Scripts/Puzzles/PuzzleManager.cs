using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzleManager : MonoBehaviour {

	[System.Serializable]
	public class Puzzle
	{

		public int winValue;
		public int curValue;


		public int width;
		public int height;
		public LoopPiece[,] pieces;


	}


	public Puzzle puzzle;
	private bool solved;

	// Use this for initialization
	void Start () {
	 
		solved = false;

		Vector2 dimensions = CheckDimensions ();

		puzzle.width = (int)dimensions.x;
		puzzle.height = (int)dimensions.y;

		puzzle.pieces = new LoopPiece[puzzle.width, puzzle.height];



		foreach (LoopPiece piece in gameObject.GetComponentsInChildren<LoopPiece>()) {

			puzzle.pieces [(int)piece.transform.localPosition.x, (int)piece.transform.localPosition.y] = piece;

		}


		foreach (LoopPiece item in puzzle.pieces) {

			Debug.Log(item.gameObject);
		}



		puzzle.winValue = GetWinValue ();

		Shuffle ();

		puzzle.curValue=Sweep ();


	}

	public int Sweep()
	{
		int value = 0;

		for (int h = 0; h < puzzle.height; h++) {
			for (int w = 0; w < puzzle.width; w++) {


				//compares top
				if(h!=puzzle.height-1)
				if (puzzle.pieces [w, h].values [0] == 1 && puzzle.pieces [w, h + 1].values [2] == 1)
					value++;


				//compare right
				if(w!=puzzle.width-1)
				if (puzzle.pieces [w, h].values [1] == 1 && puzzle.pieces [w + 1, h].values [3] == 1)
					value++;


			}
		}

		return value;

	}

	public int QuickSweep(int w,int h)
	{
		int value = 0;

		//compares top
		if(h!=puzzle.height-1)
		if (puzzle.pieces [w, h].values [0] == 1 && puzzle.pieces [w, h + 1].values [2] == 1)
			value++;


		//compare right
		if(w!=puzzle.width-1)
		if (puzzle.pieces [w, h].values [1] == 1 && puzzle.pieces [w + 1, h].values [3] == 1)
			value++;


		//compare left
		if (w != 0)
		if (puzzle.pieces [w, h].values [3] == 1 && puzzle.pieces [w - 1, h].values [1] == 1)
			value++;

		//compare bottom
		if (h != 0)
		if (puzzle.pieces [w, h].values [2] == 1 && puzzle.pieces [w, h-1].values [0] == 1)
			value++;


		return value;

	}

	int GetWinValue()
	{

		int winValue = 0;
		foreach (var piece in puzzle.pieces) {


			foreach (var j in piece.values) {
				winValue += j;
			}


		}

		winValue /= 2;

		return winValue;



	}

	void Shuffle()
	{
		foreach (var piece in puzzle.pieces) {

			int k = Random.Range (0, 4);

			for (int i = 0; i < k; i++) {
				piece.RotatePiece ();
			}


		}
	}

	Vector2 CheckDimensions()
	{
		Vector2 aux = Vector2.zero;

		LoopPiece[] pieces = gameObject.GetComponentsInChildren<LoopPiece>();
		//GameObject[] pieces = gameObject.FindGameObjectsWithTag ("Piece");

		foreach (LoopPiece p in pieces) {
			if (p.transform.localPosition.x > aux.x)
				aux.x = p.transform.localPosition.x;

			if (p.transform.localPosition.y > aux.y)
				aux.y= p.transform.localPosition.y;
		}

		aux.x++;
		aux.y++;

		return aux;
	}

	public void Win(){

		solved = true;
	}

	public bool Output(){
		return solved;
	}
		

	// Update is called once per frame
	void Update () {



	}
}