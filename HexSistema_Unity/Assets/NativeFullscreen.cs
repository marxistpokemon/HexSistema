using UnityEngine;
using System.Collections;

public class NativeFullscreen : MonoBehaviour {
	
	void Update(){
		if(Input.GetKeyUp(KeyCode.Space)){
			
			Screen.fullScreen = !Screen.fullScreen;
			
			if(Screen.fullScreen)
				Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, true);
		}
	}
}
