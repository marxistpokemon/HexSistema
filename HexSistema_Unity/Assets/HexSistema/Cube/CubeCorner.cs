using UnityEngine;
using System.Collections;

//[System.Serializable]
public class CubeCorner : Corner {

	public CubeCorner(CubeTile pTile, int pDirection, Vector3 wPoint ) 
		: base (pTile, pDirection, wPoint){
	}
	
	public override void ConnectTouches(){
		int j = direction;
		
		switch(j){
		case 0 : 					
			touches[1] = Tile.FindTileAt(touches[0].cPos.q-1, touches[0].cPos.r);
			touches[2] = Tile.FindTileAt(touches[0].cPos.q-1, touches[0].cPos.r+1);
			touches[3] = Tile.FindTileAt(touches[0].cPos.q, touches[0].cPos.r+1);
			break;
		case 1 : 
			touches[1] = Tile.FindTileAt(touches[0].cPos.q, touches[0].cPos.r+1);
			touches[2] = Tile.FindTileAt(touches[0].cPos.q+1, touches[0].cPos.r+1);
			touches[3] = Tile.FindTileAt(touches[0].cPos.q+1, touches[0].cPos.r);
			break;
		case 2 : 
			touches[1] = Tile.FindTileAt(touches[0].cPos.q+1, touches[0].cPos.r);
			touches[2] = Tile.FindTileAt(touches[0].cPos.q+1, touches[0].cPos.r-1);
			touches[3] = Tile.FindTileAt(touches[0].cPos.q, touches[0].cPos.r-1);
			break;
		case 3 :  
			touches[1] = Tile.FindTileAt(touches[0].cPos.q, touches[0].cPos.r-1);
			touches[2] = Tile.FindTileAt(touches[0].cPos.q-1, touches[0].cPos.r-1);
			touches[3] = Tile.FindTileAt(touches[0].cPos.q-1, touches[0].cPos.r);
			break;
		}
	}
	
	public override void ConnectAdjacent(){
		try {
			adjacent[0] = touches[1].corners[0];
			adjacent[1] = touches[2].corners[1];
			adjacent[2] = touches[3].corners[2];
			adjacent[3] = touches[0].corners[3];	
		} catch (System.Exception ex) {
			border = true;
			touches[0].border = true;
		}
		
	}
	
	public override void ConnectProtrudes(){
		try {
			protrudes[0] = touches[1].borders[1];
			protrudes[1] = touches[2].borders[2];
			protrudes[2] = touches[3].borders[3];
			protrudes[3] = touches[0].borders[0];
		} catch (System.Exception ex) {
			border = true;
			touches[0].border = true;
		}
		
	}
}
