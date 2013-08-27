using UnityEngine;
using System.Collections;

public class Tile {
	
	public string index;
	
	public float height;
	public float width;
	public float radius;
  
	public Coord cPos;  // coordinate location
    public Vector3 wPos;  // world location
	
	
    public bool waterbool;  // lake or ocean
    public bool oceanbool;  // ocean
    public bool coast;  // land polygon touching an ocean
    public bool border;  // at the edge of the map
    //public EBiome biome;  // biome type (see article)
    public float elevation;  // 0.0-1.0
    public float moisture;  // 0.0-1.0

    public Tile[] neighbors;
    public Edge[] borders;
    public Corner[] corners;
	
	public VisualHex visual;
	
	public TileTerrain terrain;

	
	public Tile (Coord pCPos, float pRadius, Vector3 pWPos){
		index = "H:[ "+pCPos.q.ToString() + ", " + pCPos.r.ToString()+" ]";
		
		// add to registry
		Utils.instance.allTiles.Add(this);
		
		// positions
		cPos = pCPos;
		wPos = pWPos;
		
		// geometry
		radius = pRadius;
		height = radius * 2;
		width = Mathf.Sqrt(3f)/2 * height;
		
		// terrain
		terrain = TileTerrain.WATER;
		
		// refs
		neighbors = new Tile[6];
		corners = new Corner[6];
		borders = new Edge[6];
		//ConnectNeighbours(this);
		
		corners[0] = new Corner(this, 0, new Vector3(wPos.x,	wPos.y,	wPos.z + height/2));
		corners[1] = new Corner(this, 1, new Vector3(wPos.x + width/2, wPos.y, wPos.z + height/4));
		corners[2] = new Corner(this, 2, new Vector3(wPos.x + width/2, wPos.y, wPos.z - height/4));
		corners[3] = new Corner(this, 3, new Vector3(wPos.x, wPos.y, wPos.z - height/2));
		corners[4] = new Corner(this, 4, new Vector3(wPos.x - width/2, wPos.y, wPos.z - height/4));
		corners[5] = new Corner(this, 5, new Vector3(wPos.x - width/2, wPos.y, wPos.z + height/4));
		
	}
	
	// CAUTION - called once per frame by this.visual
	public void Update(){
		
		// changes terrain based on elevation
		terrain = TileTerrain.LAND;
		waterbool = false;
		if(elevation <= Config.reg.SeaLevel){
			terrain = TileTerrain.WATER;
			waterbool = true;
		}
		if(elevation >= Config.reg.RockLevel ) {
			terrain = TileTerrain.ROCK;
		}
		if(elevation >= Config.reg.SnowLevel ) {
			terrain = TileTerrain.SNOW;
		}
		
		// changes the border status based on amount of neighbours
		if(GetNeighbourCount() < 6){
			border = true;	
		}
		else {
			border = false;
		}
		
		// updates corners
		for (int i = 0; i < 6; i++) {
			corners[i].border = border;
		}
		
	}
	
	public float GetElevationFromCorners(){
		float newElev = 0;
		for (int i = 0; i < 6; i++) {
			if(corners[i] != null){
				newElev += corners[i].elevation/6;
			}
		}
		return newElev;
	}
	
	#region Neighbours
	
	public int GetNeighbourCount(){
		int res = 0;
		for (int i = 0; i < 6; i++) {
			if(neighbors[i] != null) res++;
		}
		return res;
	}
	
	public static Tile AddNewNeighbour(Tile tile, int direction){
		
		if(tile == null) return null;
		
		if(direction < 0 || direction > 5 ||
			tile.neighbors[direction] != null){
			return null;
		}
		
		Coord posNew = new Coord(tile.cPos.q + Utils.instance.neighborsRelCPos[direction].q, 
			tile.cPos.r + Utils.instance.neighborsRelCPos[direction].r);
		
		Vector3 auxPos = new Vector3();
		switch(direction){
			case 0 : auxPos = new Vector3(tile.width/2, tile.wPos.y, tile.height*3/4); break;
			case 1 : auxPos = new Vector3(tile.width, tile.wPos.y, 0); break;
			case 2 : auxPos = new Vector3(tile.width/2, tile.wPos.y, -tile.height*3/4); break;
			case 3 : auxPos = new Vector3(-tile.width/2, tile.wPos.y, -tile.height*3/4); break;
			case 4 : auxPos = new Vector3(-tile.width, tile.wPos.y, 0); break;
			case 5 : auxPos = new Vector3(-tile.width/2, tile.wPos.y, tile.height*3/4); break;
		}
		Vector3 worldPos = tile.wPos + auxPos;
		Tile temp = new Tile(posNew, Config.reg.tileSize, worldPos);
		tile.neighbors[direction] = temp;
		return temp;
	}
	
	public static Tile FindNeighborAt(Tile tile, int direction){
		
		if(tile == null) return null;
		
		Coord neighborCoord = new Coord(
			tile.cPos.q + Utils.instance.neighborsRelCPos[direction].q,
			tile.cPos.r + Utils.instance.neighborsRelCPos[direction].r);
		
		Tile res = Utils.instance.allTiles.Find(
			tRes => 
			tRes.cPos.q == neighborCoord.q &&
			tRes.cPos.r == neighborCoord.r);
		
		return res;
	}
	
	public static void ConnectAllNeighbours(){
		Utils.instance.allTiles.ForEach(t => {
			ConnectNeighbours(t);	
		});
	}
	
	public static void ConnectNeighbours(Tile tile1){
		for (int i = 0; i < 6; i++) {
			Tile tile2;
			tile1.neighbors[i] = Tile.FindNeighborAt(tile1, i);
			if(tile1.neighbors[i] != null){
				tile2 = tile1.neighbors[i];
				switch(i){
					case 0 : tile2.neighbors[3] = tile1; break;
					case 1 : tile2.neighbors[4] = tile1; break;
					case 2 : tile2.neighbors[5] = tile1; break;
					case 3 : tile2.neighbors[0] = tile1; break;
					case 4 : tile2.neighbors[1] = tile1; break;
					case 5 : tile2.neighbors[2] = tile1; break;
				}
			}
		}
	}
	
	public static int GetNeighbourCountByTerrain(Tile tile, TileTerrain query){
		
		int res = 0;
		for (int i = 0; i < 6; i++) {
			if(tile.neighbors[i] != null){
				if(tile.neighbors[i].terrain == query) res++;
			}
		}
		return res;
	}
	
	#endregion
}

