using UnityEngine;
using System.Collections;

public class HexTile : Tile {

	public HexTile (TileType pTileType, Coord pCPos, float pSize, Vector3 pWPos) :
		base (pTileType, pCPos, pSize, pWPos) {
		
		height = size * 2;
		width = Mathf.Sqrt(3f)/2 * height;
		
		corners[0] = new HexCorner(this, 0, new Vector3(wPos.x,	wPos.y,	wPos.z + height/2));
		corners[1] = new HexCorner(this, 1, new Vector3(wPos.x + width/2, wPos.y, wPos.z + height/4));
		corners[2] = new HexCorner(this, 2, new Vector3(wPos.x + width/2, wPos.y, wPos.z - height/4));
		corners[3] = new HexCorner(this, 3, new Vector3(wPos.x, wPos.y, wPos.z - height/2));
		corners[4] = new HexCorner(this, 4, new Vector3(wPos.x - width/2, wPos.y, wPos.z - height/4));
		corners[5] = new HexCorner(this, 5, new Vector3(wPos.x - width/2, wPos.y, wPos.z + height/4));
	}
	
	public override Tile AddNewNeighbour(int direction) {
		
		if(direction < 0 || direction > Utils.GetTileCornerNum(tileType) ||
			neighbors[direction] != null){
			return null;
		}
		
		Coord posNew = new Coord(cPos.q + Utils.instance.hexNeighborsRelCPos[direction].q, 
			cPos.r + Utils.instance.hexNeighborsRelCPos[direction].r);
		
		Vector3 auxPos = new Vector3();
		switch(direction){
			case 0 : auxPos = new Vector3(width/2, wPos.y, height*3/4); break;
			case 1 : auxPos = new Vector3(width, wPos.y, 0); break;
			case 2 : auxPos = new Vector3(width/2, wPos.y, -height*3/4); break;
			case 3 : auxPos = new Vector3(-width/2, wPos.y, -height*3/4); break;
			case 4 : auxPos = new Vector3(-width, wPos.y, 0); break;
			case 5 : auxPos = new Vector3(-width/2, wPos.y, height*3/4); break;
		}
		Vector3 worldPos = wPos + auxPos;
		HexTile temp = new HexTile(tileType, posNew, Config.reg.tileSize, worldPos);
		neighbors[direction] = temp;
		return temp;
	}
	
	public override Tile FindNeighborAt(int direction){
		
		Coord neighborCoord = new Coord(
			cPos.q + Utils.instance.hexNeighborsRelCPos[direction].q,
			cPos.r + Utils.instance.hexNeighborsRelCPos[direction].r);
		
		Tile res = Utils.instance.allTiles.Find(
			tRes => 
			tRes.cPos.q == neighborCoord.q &&
			tRes.cPos.r == neighborCoord.r);
		
		return res;
	}
	
	public override void ConnectNeighbours(){
		for (int i = 0; i < cornerNum; i++) {
			HexTile tile2;
			neighbors[i] = FindNeighborAt(i);
			if(neighbors[i] != null){
				tile2 = neighbors[i] as HexTile;
				switch(i){
					case 0 : tile2.neighbors[3] = this; break;
					case 1 : tile2.neighbors[4] = this; break;
					case 2 : tile2.neighbors[5] = this; break;
					case 3 : tile2.neighbors[0] = this; break;
					case 4 : tile2.neighbors[1] = this; break;
					case 5 : tile2.neighbors[2] = this; break;
				}
			}
		}
	}
}
