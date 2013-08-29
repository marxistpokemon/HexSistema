using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class World : MonoBehaviour {
	
	public TileType tileType;
	public GameObject visualPrefab;
	public float routineTimer = 0.0f;
	public List<BaseModule> allModules;
	
	public abstract IEnumerator GenerateRegularGrid(int burstSize);
	
	public abstract void AddVisualTile (Tile t);
	
	public void Start(){
		allModules = new List<BaseModule>();
		foreach (var item in GetComponents<BaseModule>()) {
			allModules.Add(item);
			item.id = allModules.IndexOf(item);
		}
	}

	public void Update () {
		
		if(Input.GetKeyUp(KeyCode.Alpha0)){
			RunModule(0);
		}
		
		if(Input.GetKeyUp(KeyCode.Alpha1)){
			RunModule(1);
		}

		if(Input.GetKeyUp(KeyCode.A)){
			RunAllModules();
		}
		
		if(Input.GetKeyUp(KeyCode.A)){
			Utils.instance.allTiles.ForEach(t=>t.UpdateIsBorder());
		}
		
		if(Input.GetKeyUp(KeyCode.Return)){
			Utils.instance.allVisual.ForEach(t => t.ElevationDeform());
			Tile.UpdateAllTerrainByElevation();
		}
		
		if(Input.GetKeyUp(KeyCode.T)){
			ResetWorld();
		}
		if(Input.GetKeyUp(KeyCode.R)){
			//ResetWorld();
			StartCoroutine(GenerateRegularGrid(100));
		}
	}
	
	public void RunAllModules(){
		allModules.ForEach(m=> RunModule(m.id));
	}
	
	public bool RunModule(int id){
		BaseModule temp = null;
		
		temp = allModules.Find(m => m.id == id);
		
		if(!temp || !temp.active)
			return false;
		
		temp.Run();
		return true;
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
