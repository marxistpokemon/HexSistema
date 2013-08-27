using UnityEngine;
using System.Collections;

public class DebugMessage : MonoBehaviour {
	
	private GUIText myText;
	
	void Start(){
		myText = this.guiText;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Config.reg.debugMode){
			this.gameObject.SetActive(true);
		}
		else {
			this.gameObject.SetActive(false);
		}
	}
	
	public void Log(string msg, bool clear){
		if(!clear){
			myText.text += " \n" + msg;
		}
		else {
			myText.text = msg;
		}	
	}
}
