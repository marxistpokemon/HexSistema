using UnityEngine;
using System.Collections;

[System.Serializable]
public class CubeTile : Tile {

	public CubeTile (TileType pTileType, Coord pCPos, float pSize, Vector3 pWPos) :
		base (pTileType, pCPos, pSize, pWPos) {
		
		height = size;
		width = size;
		
		corners[0] = new CubeCorner(this, 0, new Vector3(wPos.x - size/2, wPos.y, wPos.z + size/2));
		corners[1] = new CubeCorner(this, 1, new Vector3(wPos.x + size/2, wPos.y, wPos.z + size/2));
		corners[2] = new CubeCorner(this, 2, new Vector3(wPos.x + size/2, wPos.y, wPos.z - size/2));
		corners[3] = new CubeCorner(this, 3, new Vector3(wPos.x - size/2, wPos.y, wPos.z - size/2));
	}
	
	public override Tile AddNewNeighbour(int direction) {
		
		if(direction < 0 || direction > Utils.GetTileCornerNum(tileType) ||
			neighbors[direction] != null){
			return null;
		}
		
		Coord posNew = new Coord(cPos.q + Utils.instance.cubeNeighborsRelCPos[direction].q, 
			cPos.r + Utils.instance.cubeNeighborsRelCPos[direction].r);
		
		Vector3 auxPos = new Vector3();
		
		switch(direction){
			case 0 : auxPos = new Vector3(-size, wPos.y, 0); break;
			case 1 : auxPos = new Vector3(0, wPos.y, size); break;
			case 2 : auxPos = new Vector3(size, wPos.y, 0); break;
			case 3 : auxPos = new Vector3(0, wPos.y, -size); break;
		}
		Vector3 worldPos = wPos + auxPos;
		CubeTile temp = new CubeTile(tileType, posNew, Config.reg.tileSize, worldPos);
		neighbors[direction] = temp;
		return temp;
	}
	
	public override Tile FindNeighborAt(int direction){
		
		Coord neighborCoord = new Coord(
			cPos.q + Utils.instance.cubeNeighborsRelCPos[direction].q,
			cPos.r + Utils.instance.cubeNeighborsRelCPos[direction].r);
		
		Tile res = Utils.instance.allTiles.Find(
			tRes => 
			tRes.cPos.q == neighborCoord.q &&
			tRes.cPos.r == neighborCoord.r);
		
		return res;
	}
	
	public override void ConnectNeighbours(){
		for (int i = 0; i < cornerNum; i++) {
			CubeTile tile2;
			neighbors[i] = FindNeighborAt(i);
			if(neighbors[i] != null){
				tile2 = neighbors[i] as CubeTile;
				switch(i){
					case 0 : tile2.neighbors[2] = this; break;
					case 1 : tile2.neighbors[3] = this; break;
					case 2 : tile2.neighbors[0] = this; break;
					case 3 : tile2.neighbors[1] = this; break;
				}
			}
		}
	}
}
