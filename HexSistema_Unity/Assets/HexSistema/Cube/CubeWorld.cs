using UnityEngine;
using System.Collections;

public class CubeWorld : World {
	
	public int worldSizeQ = 1;
	public int worldSizeR = 1;
	
	public override IEnumerator GenerateRegularGrid(int burstSize){
		Utils.instance.debugMsg.Log("GenerateRegularGrid: Start.", true);
		
		int burstCount = 0;
		
		for (int q = 0; q < worldSizeQ; q++) {
			
			for (int r = 0; r < worldSizeR; r++) {
				
				CubeTile cube = new CubeTile(TileType.SQUARE, new Coord(q,r), 
					Config.reg.tileSize , new Vector3(
					q*Config.reg.tileSize, 0, r*Config.reg.tileSize));
				Utils.instance.debugMsg.Log("GenerateRegularGrid: New Logic Tile: " 
				+ Utils.instance.allTiles.Count, true);
				burstCount++;
				if(burstCount >= burstSize){
					burstCount = 0;
					yield return new WaitForSeconds(routineTimer);
				}
					
			}
		}	
		
		Utils.instance.debugMsg.Log("GenerateRegularGrid: Start connections.", false);
		// logical references to other tiles and corners
		Tile.ConnectAllNeighbours();
		Corner.ConnectAllTouches();
		
		// logical references to edges
		Edge.MakeAllEdges();
		Corner.ConnectAllProtrudes();
		Utils.instance.debugMsg.Log("GenerateRegularGrid: End connections.", false);
		
		StartCoroutine(SetupVisualGrid(worldSizeQ*2));
		Debug.Log("Tiles: " + Utils.instance.allTiles.Count);
		Debug.Log("Corners: " + Utils.instance.allCorners.Count);
		Utils.instance.debugMsg.Log("GenerateRegularGrid: Finished.", false);
	}
	
	public override void AddVisualTile (Tile t) {
		
		GameObject obj = (GameObject)GameObject.Instantiate(visualPrefab, 
			t.wPos, Quaternion.identity);
		CubeVisualTile v = obj.GetComponent<CubeVisualTile>();
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
