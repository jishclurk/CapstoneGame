using UnityEngine;
using System.Collections;

public class Zombie_ControlScript : MonoBehaviour {
	
	private Animator myAnimator;
	public bool moving;
	public bool sprinting;
	public bool turningLeft;
	public bool turningRight;
	public int action = 0;
	public int death = 0;
	public bool eating = false;

	void Start () {
		//Locate the animator on the current character.
		myAnimator = GetComponent<Animator>();
	}
	
	void Update () {
		//Listen for Key Presses
		GetInput ();
		UpdateAnimator();
	}
	
	//This method will listen for player input and control the variables used by the animator
	void GetInput(){
		if(Input.GetKey ("w")){
			moving = true;
		} else {
			moving = false;
		}
		if(Input.GetKey ("left shift")){
			sprinting = true;
		} else {
			sprinting = false;
		}
		
		//If the player is standing still, use the turningLeft/turningRight animator Booleans to
		//Set animation-driven root motion turns.  If the player is moving/sprinting then
		//Use procedural animation by updating the transform rotation for turning while running
		if(Input.GetKey ("q")){
			turningLeft = true;
			if(moving){
				this.transform.Rotate(Vector3.up * (Time.deltaTime + -0.6f), Space.World);
			}
			if(sprinting){
				this.transform.Rotate(Vector3.up * (Time.deltaTime + -2.0f), Space.World);
			}
		} else {
			turningLeft = false;
		}
		if(Input.GetKey("e")){
			turningRight = true;
			if(moving){
				this.transform.Rotate(Vector3.up * (Time.deltaTime + 0.6f), Space.World);
			}
			if(sprinting){
				this.transform.Rotate(Vector3.up * (Time.deltaTime + 2.0f), Space.World);
			}
		} else {
			turningRight = false;
		}
		
		//listen for action buttons and set action code
		if(Input.GetKeyDown ("1")){action = 1;}
		if(Input.GetKeyUp ("1")){action = 0;}
		if(Input.GetKeyDown ("3")){action = 3;}
		if(Input.GetKeyUp ("3")){action = 0;}
		if(Input.GetKeyDown ("4")){action = 4;}
		if(Input.GetKeyUp ("4")){action = 0;}
		
		//Toggles Action Code 2
		if(Input.GetKeyDown ("2")){
			if(action == 0){
				action = 2;
			} else if (action == 2){
				action = 0;
			}
		}
	
		//Toggles Action Code 5
		if(Input.GetKeyDown ("5")){
			if(death == 0){
				death = 1;
			} else if (death == 1){
				death = 0;
			}
		}
		
		//Toggles Action Code 6
		if(Input.GetKeyDown ("6")){
			if(death == 0){
				death = 2;
			} else if (death == 2){
				death = 0;
			}
		}
	}
	
	//After the input and speeds are calculated, send the information to the animator
	void UpdateAnimator(){
		myAnimator.SetBool ("Moving", moving);
		myAnimator.SetBool ("Sprinting", sprinting);
		myAnimator.SetBool ("TurningLeft", turningLeft);
		myAnimator.SetBool ("TurningRight", turningRight);
		myAnimator.SetInteger("Action", action);
		myAnimator.SetInteger("Death", death);
	}
	
	void OnGUI(){
		GUI.Label (new Rect(0, 0, 150, 20), "W: Walk");
		GUI.Label (new Rect(0, 20, 150, 20), "Shfit: Toggle Sprint");
		GUI.Label (new Rect(0, 40, 150, 50), "Q:  Rotate Left");
		GUI.Label (new Rect(0, 60, 150, 50), "E:  Rotate Right");
		GUI.Label (new Rect(0, 80, 150, 50), "1:  Attack");
		GUI.Label (new Rect(0, 100, 150, 20), "2:  Toggle Eating");
		GUI.Label (new Rect(0, 120, 150, 20), "3:  Take Damage");
		GUI.Label (new Rect(0, 140, 150, 20), "4:  Bite Neck");
		GUI.Label (new Rect(0, 160, 150, 20), "5:  Toggle Death Type A");
		GUI.Label (new Rect(0, 180, 150, 20), "6:  Toggle Death Type B");
	}
}
