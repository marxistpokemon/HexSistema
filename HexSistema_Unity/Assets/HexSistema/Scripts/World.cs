using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class World : MonoBehaviour {
	
	public TileType tileType;
	
	public GameObject visualPrefab;
	
	public float baseElevation = 0;
	public int perlinOctaves = 4;
	public float perlinFrq = 5f;
	public float perlinAmp = 3f;
	public bool perlinSmooth = true;
	
	public float routineTimer = 0.0f;
	
	public abstract IEnumerator GenerateRegularGrid(int burstSize);
	
	public abstract void AddVisualTile (Tile t);

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
	
	public IEnumerator SetupVisualGrid(int burstSize){
		// reset registry list
		Utils.instance.allVisual = new List<VisualTile>();
		Utils.instance.debugMsg.Log("Setup Visual Tiles : Start.", false);
		
		int burstCount = 0;
		for (int i = 0; i < Utils.instance.allTiles.Count; i++) {
			AddVisualTile(Utils.instance.allTiles[i]);
			burstCount++;
			if(burstCount > burstSize){
				burstCount = 0;
				yield return new WaitForSeconds(routineTimer);
			}
			
		}
		
		Utils.instance.debugMsg.Log("Setup Visual Tiles : End.", false);
	}
	
	// destroys everything andd clears containers
	public void ResetWorld(){
		if(Utils.instance.allVisual.Count > 0)
			Utils.instance.allVisual.ForEach(v => DestroyImmediate(v.gameObject));
		
		Utils.instance.allVisual.Clear ();
		Utils.instance.allTiles.Clear();
		Utils.instance.allCorners.Clear();
		Utils.instance.allEdges.Clear();
	}
}
