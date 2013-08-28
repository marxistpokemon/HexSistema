using UnityEngine;
using System.Collections;

public abstract class Corner {
	public string index;
  
    public Vector3 wPos;  // location
	public int direction;
	
	// world model
    public bool ocean;  // ocean
    public bool water;  // lake or ocean
    public bool coast;  // touches ocean and land polygons
    public bool border;  // at the edge of the map
    public float elevation;  // 0.0-1.0
    public float moisture;  // 0.0-1.0
	public int river;  // 0 if no river, or volume of water in river
    public Corner downslope;  // pointer to adjacent corner most downhill
	public Corner watershed;  // pointer to coastal corner, or null
    public int watershed_size;
	
	// logical references
	public int touchesNum;
    public Tile[] touches;
	public int protrudesNum;
    public Edge[] protrudes;
	public int adjacentNum;
    public Corner[] adjacent;
	
	public Corner(Tile tile, int pDirection, Vector3 wPoint)
	{
		wPos = wPoint;
		direction = pDirection;
		
		index = "C:[ "+wPos.x.ToString("0.00") + ", " + 
			wPos.y.ToString("0.00") + ", " +  
			wPos.z.ToString("0.00")+" ]";
		Utils.instance.allCorners.Add(this);
		
		elevation = 0;
		
		touchesNum = Utils.GetCornerTouchesNum(tile.tileType);
		touches = new Tile[touchesNum];
		touches[0] = tile;
		
		protrudesNum = Utils.GetCornerProtrudesNum(tile.tileType);
	    protrudes = new Edge[protrudesNum];
		
		adjacentNum = Utils.GetCornerAdjacentNum(tile.tileType);
	    adjacent = new Corner[3];
	}
	
	public bool GetWater(){
		water = touches[0].waterbool;
		return touches[0].waterbool;
	}
	
	public abstract void ConnectProtrudes();
	
	public static void ConnectAllProtrudes(){
		Utils.instance.allCorners.ForEach(c => c.ConnectProtrudes());
	}
	
	public abstract void ConnectAdjacent();
	
	public static void ConnectAllAdjacent(){
		Utils.instance.allCorners.ForEach(c => c.ConnectAdjacent());
	}
	
	public abstract void ConnectTouches ();
	
	public static void ConnectAllTouches(){
		Utils.instance.allCorners.ForEach(c => c.ConnectTouches());
	}

	public Corner FindCornerAt(Vector3 wPoint, float tolerance){
		
		Corner temp = Utils.instance.allCorners.Find(corner =>
			Vector3.Distance(wPos, wPoint) <= tolerance);
		
		return temp;
	}
	
	public float UpdateCornerElevation(bool smooth){
		float old = touches[0].elevation;
		float res = touches[0].elevation;
		for (int i = 0; i < touchesNum; i++) {
			if(touches[i] != null){
				res = Mathf.Max(res, touches[i].elevation);
				if(touches[i].terrain == TileTerrain.WATER){
					elevation = Config.reg.SeaLevel;
					return elevation;
				}
			}
		}
		if(smooth) {
			res = Mathf.Lerp(old, res, 0.5f);
		}
		elevation = res;
		return res;
		
	}
	
	public static void UpdateAllCornersElevation(bool smooth){
		Utils.instance.allCorners.ForEach(c => {
			c.UpdateCornerElevation(smooth);
		});
	}
}
