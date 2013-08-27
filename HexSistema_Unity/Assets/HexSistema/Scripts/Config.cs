using UnityEngine;
using System.Collections;

[System.Serializable]
public class Config : MonoBehaviour {
	
	public static Config reg;
	
	public bool debugMode = false;
	
	// geometry
	public float tileSize;
	public float tileThickness;
	
	// world model
	public float SeaLevel = 0f;
	public float RockLevel = 3f;
	public float SnowLevel = 7f;

	// HexCorner
	public float cornerTolerance;

	// visuals
	public Material[] terrainMaterials;

	// Use this for initialization
	void Awake(){
		reg = this;
	}
}
