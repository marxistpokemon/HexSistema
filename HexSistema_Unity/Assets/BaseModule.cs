using UnityEngine;
using System.Collections;

public abstract class BaseModule : MonoBehaviour {
	
	public int id;
	public bool active = false;
	
	public abstract void Run();
}
