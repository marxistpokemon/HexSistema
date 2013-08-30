using UnityEngine;
using System.Collections;

public class HexWorld : World {
	
	public int gridRadius;
	
	public override IEnumerator GenerateRegularGrid(int burstSize){
		
		Utils.instance.debugMsg.Log("GenerateRegularGrid: Start.", true);
		
		HexTile start = new HexTile(TileType.HEX, new Coord(0,0), 
			Config.reg.tileSize , new Vector3(0,0,0));
		
		HexTile H;
		//Coord nextCoord = new Coord();
		
		for (int k = 1; k <= gridRadius; k++) {
			
			if(start.FindNeighborAt(4) == null){
				start.neighbors[4] = start.AddNewNeighbour(4);
			}
			H = start.neighbors[4] as HexTile;
			start = H; // prepara para prox
			
			for (int i = 0; i < 6; i++) {
				for (int j = 0; j < k; j++) {
					
					HexTile neighborThere = H.FindNeighborAt(i) as HexTile;
					
					if(neighborThere == null){
						
						neighborThere = H.AddNewNeighbour(i) as HexTile;
					}
					H.neighbors[i] = neighborThere as HexTile;
					H = H.neighbors[i] as HexTile;
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
		
		
		StartCoroutine(SetupVisualGrid(100));
		Debug.Log("Tiles: " + Utils.instance.allTiles.Count);
		Debug.Log("Corners: " + Utils.instance.allCorners.Count);
		Utils.instance.debugMsg.Log("GenerateRadialGrid: Finished.", false);
	}
	
	public override void AddVisualTile (Tile t) {
		
		GameObject obj = (GameObject)GameObject.Instantiate(visualPrefab, 
			t.wPos, Quaternion.identity);
		HexVisualTile v = obj.GetComponent<HexVisualTile>();
		//mutual references
		v.logicTile = t;
		t.visual = v;
		v.transform.position = v.logicTile.wPos;
		//v.transform.parent = this.transform;
		Utils.instance.allVisual.Add(v);
		Utils.instance.debugMsg.Log("New Visual Tile: " 
				+ Utils.instance.allVisual.Count, true);
		
	}
	
	
}
