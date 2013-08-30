using UnityEngine;
using System.Collections;

//[System.Serializable]
public abstract class Tile {
	
	public TileType tileType;
	public string index;
	
	public float height;
	public float width;
	public float size;
  
	public Coord cPos;  // coordinate location
    public Vector3 wPos;  // world location
	
    public bool waterbool;  // lake or ocean
    public bool oceanbool;  // ocean
    public bool coast;  // land polygon touching an ocean
    public bool border;  // at the edge of the map
    public float elevation;  // 0.0-1.0
    public float moisture;  // 0.0-1.
    public Tile[] neighbors;
    public Edge[] borders;
    public Corner[] corners;
	public int cornerNum;
	public VisualTile visual;	
	public TileTerrain terrain;

	public Tile (TileType pTileType, Coord pCPos, float pSize, Vector3 pWPos){
		index = "T:[ "+pCPos.q.ToString() + ", " + pCPos.r.ToString()+" ]";
		
		tileType = pTileType;
		cornerNum = Utils.GetTileCornerNum(tileType);
		
		// add to registry
		Utils.instance.allTiles.Add(this);
		
		// positions
		cPos = pCPos;
		wPos = pWPos;
		
		// refs
		neighbors = new Tile[Utils.GetTileCornerNum(tileType)];
		corners = new Corner[Utils.GetTileCornerNum(tileType)];
		borders = new Edge[Utils.GetTileCornerNum(tileType)];
		
		// geometry
		size = pSize;
		
		waterbool = true;
		oceanbool = true;
		coast = false;
	}
	
	#region Updates
	public static void UpdateAllStatus(){
		UpdateAllTerrainByElevation();
		Corner.UpdateAllCoasts();
		UpdateAllCoasts();
		UpdateAllBorders();
		VisualTile.UpdateAllMaterials();
	}
	
	public static void UpdateAllBorders(){
		Utils.instance.allTiles.ForEach(t=>t.UpdateIsBorder());
	}
	
	public static void UpdateAllCoasts(){
		Utils.instance.allTiles.ForEach(t=>t.UpdateIsCoast());
	}
	
	public static void UpdateAllTerrainByElevation(){
		Utils.instance.allTiles.ForEach(t=>t.UpdateTerrainByElevation());
	}
	
	public void UpdateTerrainByElevation(){
		// changes terrain based on elevation
		
		if(elevation <= Config.reg.SeaLevel){
			waterbool = true;
			terrain = TileTerrain.WATER;
			coast = false;
			foreach(Corner corner in corners){
				corner.water = true;
				corner.coast = false;
				corner.elevation = Config.reg.SeaLevel;
			}
		}
		else {
			waterbool = false;
			terrain = TileTerrain.LAND;
			foreach(Corner corner in corners){
				corner.water = false;
			}
			
			if(elevation >= Config.reg.RockLevel ) {
				terrain = TileTerrain.ROCK;
			}
			if(elevation >= Config.reg.SnowLevel ) {
				terrain = TileTerrain.SNOW;
			}
		}
	}
	
	public void UpdateIsCoast(){
		
		int waterN = GetNeighbourCountByTerrain(TileTerrain.WATER);
		
		if(!waterbool &&
			waterN > 0){
			coast = true;
		}
		else {
			coast = false;
		}
	}
	
	public void UpdateIsBorder(){
		// changes the border status based on amount of neighbours
		if(GetNeighbourCount() < cornerNum){
			border = true;	
		}
		else {
			border = false;
		}
		// updates corners
		for (int i = 0; i < cornerNum; i++) {
			corners[i].border = border;
		}
	
	}
	
	#endregion
	
	#region Neighbours
	
	public static Tile FindTileAt(int Q, int R){
		return Utils.instance.allTiles.Find( t =>
			t.cPos.q == Q &&
			t.cPos.r == R);
	}
	
	public int GetNeighbourCount(){
		int res = 0;
		for (int i = 0; i < cornerNum; i++) {
			if(neighbors[i] != null) res++;
		}
		return res;
	}
	
	public abstract Tile AddNewNeighbour(int direction); 
	
	public abstract Tile FindNeighborAt(int direction);
	
	public static void ConnectAllNeighbours(){
		Utils.instance.allTiles.ForEach(t => {
			t.ConnectNeighbours();	
		});
	}
	
	public abstract void ConnectNeighbours();
	
	public int GetNeighbourCountByTerrain(TileTerrain query){
		
		int res = 0;
		for (int i = 0; i < cornerNum; i++) {
			if(neighbors[i] != null){
				if(neighbors[i].terrain == query) res++;
			}
		}
		return res;
	}
	
	#endregion
}

