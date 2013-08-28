using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	
	public int worldRadius = 2;
	
	public GameObject visualPrefab;
	
	public float baseElevation = 0;
	public int perlinOctaves = 4;
	public float perlinFrq = 5f;
	public float perlinAmp = 3f;
	public bool perlinSmooth = true;
	
	public float routineTimer = 0.0f;
	
	

	// Use this for initialization
	void Start () {
		
		//GenerateRadialGrid(Config.reg.tileSize , worldRadius, routineTimer);	
	}
	
	// destroys everything andd clears containers
	void ResetWorld(){
		if(Utils.instance.allVisual.Count > 0)
			Utils.instance.allVisual.ForEach(v => DestroyImmediate(v.gameObject));
		
		Utils.instance.allVisual.Clear ();
		Utils.instance.allTiles.Clear();
		Utils.instance.allCorners.Clear();
		Utils.instance.allEdges.Clear();
	}
	
	void Update () {

		if(Input.GetKeyUp(KeyCode.V)){
			GeneratePerlinElevations();
			//Corner.UpdateAllCornersElevation(false);
			Utils.instance.allVisual.ForEach(t => VisualHex.ElevationDeform(t));
		}
		if(Input.GetKeyUp(KeyCode.T)){
			ResetWorld();
		}
		if(Input.GetKeyUp(KeyCode.R)){
			ResetWorld();
			StartCoroutine(GenerateRadialGrid(Config.reg.tileSize , worldRadius));
		}
	}
	
	public IEnumerator GenerateRadialGrid(float tileSize , int gridRadius){
		
		Utils.instance.debugMsg.Log("GenerateRadialGrid: Start.", true);
		
		Tile start = new Tile(new Coord(0,0), tileSize , new Vector3(0,0,0));
		
		Tile H;
		//Coord nextCoord = new Coord();
		
		for (int k = 1; k <= gridRadius; k++) {
			
			if(Tile.FindNeighborAt(start, 4) == null){
				start.neighbors[4] = Tile.AddNewNeighbour(start, 4);
				//Debug.Log ("ADD - WC: " + start.neighbors[4].index);
				//ConnectNeighbours(mundo, start);
			}
			H = start.neighbors[4];
			start = H; // prepara para prox
			
			for (int i = 0; i < 6; i++) {
				for (int j = 0; j < k; j++) {
					
					Tile neighborThere = Tile.FindNeighborAt(H, i);
					
					if(neighborThere == null){
						
						neighborThere = Tile.AddNewNeighbour(H, i);
					}
					H.neighbors[i] = neighborThere;
					H = H.neighbors[i];
				}
			}
			Utils.instance.debugMsg.Log("GenerateRadialGrid: New Logic Tile: " 
				+ Utils.instance.allTiles.Count, true);
			yield return new WaitForSeconds(routineTimer);
		}	
		
		// logical references to other tiles and corners
		Tile.ConnectAllNeighbours();
		Corner.ConnectAllTouches();
		
		// logical references to edges
		Edge.MakeAllEdges();
		Corner.ConnectAllProtrudes();
		
		
		StartCoroutine(SetupVisualGrid());
		Debug.Log("Tiles: " + Utils.instance.allTiles.Count);
		Debug.Log("Corners: " + Utils.instance.allCorners.Count);
		Utils.instance.debugMsg.Log("GenerateRadialGrid: Finished.", false);
	}
	
	public void GeneratePerlinElevations(){
		
		PerlinNoise perlin = new PerlinNoise(Mathf.FloorToInt(Random.value*10000000));
		perlin.LoadPermTableIntoTexture();
		
		Utils.instance.allCorners.ForEach(corner => {
			
			corner.elevation = baseElevation + perlin.FractalNoise2D(
				corner.wPos.x/Config.reg.tileSize , 
				corner.wPos.z/Config.reg.tileSize ,
				perlinOctaves, perlinFrq, perlinAmp);
			
			corner.touches[0].elevation = baseElevation + perlin.FractalNoise2D(
				corner.touches[0].wPos.x/Config.reg.tileSize , 
				corner.touches[0].wPos.z/Config.reg.tileSize ,
				perlinOctaves, perlinFrq, perlinAmp);
		
			if(!perlinSmooth){
				corner.elevation = Mathf.Floor(corner.elevation);
			}
			
			
			corner.elevation = Mathf.Clamp(corner.elevation, Config.reg.SeaLevel, 1000);
			corner.touches[0].elevation = Mathf.Clamp(corner.touches[0].elevation, 
				Config.reg.SeaLevel, 1000);
			corner.touches[0].Update();
			corner.GetWater();
			if(corner.GetWater()) corner.elevation = Config.reg.SeaLevel;
			// corner.touches[0].elevation = corner.touches[0].GetElevationFromCorners();
				
		});
	}	
	
	public IEnumerator SetupVisualGrid(){
		// reset registry list
		Utils.instance.allVisual = new List<VisualHex>();
		Utils.instance.debugMsg.Log("Setup Visual Tiles : Start.", false);
		
		for (int i = 0; i < Utils.instance.allTiles.Count; i++) {
			AddVisualTile(Utils.instance.allTiles[i]);
			yield return new WaitForSeconds(routineTimer);
		}
		
		Utils.instance.debugMsg.Log("Setup Visual Tiles : End.", false);
	}
	
	public void AddVisualTile (Tile t) {
		
		GameObject obj = (GameObject)GameObject.Instantiate(visualPrefab, 
			t.wPos, Quaternion.identity);
		VisualHex v = obj.GetComponent<VisualHex>();
		//mutual references
		v.logicTile = t;
		t.visual = v;
		v.transform.position = v.logicTile.wPos;
		v.transform.parent = this.transform;
		Utils.instance.allVisual.Add(v);
		Utils.instance.debugMsg.Log("New Visual Tile: " 
				+ Utils.instance.allVisual.Count, true);
		
	}
}
