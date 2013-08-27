using UnityEngine;
using System.Collections;

public class Corner {
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
    public Tile[] touches;
    public Edge[] protrudes;
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
		
		touches = new Tile[3];
		touches[0] = tile;
		
	    protrudes = new Edge[3];
	    adjacent = new Corner[3];
	}
	
	public bool GetWater(){
		water = touches[0].waterbool;
		return touches[0].waterbool;
	}
	
	public static void ConnectProtrudes(Corner corner){
		switch(corner.direction){
		case 0 : 
			if(corner.touches[1] != null) corner.protrudes[0] = corner.touches[1].borders[1];
			corner.protrudes[1] = corner.touches[0].borders[1];
			corner.protrudes[2] = corner.touches[0].borders[5];
			break;
		case 1 : 
			if(corner.touches[1] != null) corner.protrudes[0] = corner.touches[1].borders[2];	
			corner.protrudes[1] = corner.touches[0].borders[2];
			corner.protrudes[2] = corner.touches[0].borders[0];
			break;
		case 2 : 
			if(corner.touches[1] != null) corner.protrudes[0] = corner.touches[1].borders[3]; 
			corner.protrudes[1] = corner.touches[0].borders[3];
			corner.protrudes[2] = corner.touches[0].borders[1];
			break;
		case 3 : 
			if(corner.touches[1] != null) corner.protrudes[0] = corner.touches[1].borders[4];	
			corner.protrudes[1] = corner.touches[0].borders[4];
			corner.protrudes[2] = corner.touches[0].borders[2];
			break;
		case 4 : 
			if(corner.touches[1] != null) corner.protrudes[0] = corner.touches[1].borders[5];	
			corner.protrudes[1] = corner.touches[0].borders[5];
			corner.protrudes[2] = corner.touches[0].borders[3];
			break;
		case 5 : 
			if(corner.touches[1] != null) corner.protrudes[0] = corner.touches[1].borders[0]; 
			corner.protrudes[1] = corner.touches[0].borders[0];
			corner.protrudes[2] = corner.touches[0].borders[4];
			break;
		}
	}
	
	public static void ConnectAllProtrudes(){
		Utils.instance.allCorners.ForEach(c => ConnectProtrudes(c));
	}
	
	public static void ConnectAdjacent(Corner corner){
		switch(corner.direction){
		case 0 : 
			if(corner.touches[1] != null) corner.adjacent[0] = corner.touches[1].corners[1];
			corner.adjacent[1] = corner.touches[0].corners[1];
			corner.adjacent[2] = corner.touches[0].corners[5];
			break;
		case 1 : 
			if(corner.touches[1] != null) corner.adjacent[0] = corner.touches[1].corners[2];	
			corner.adjacent[1] = corner.touches[0].corners[2];
			corner.adjacent[2] = corner.touches[0].corners[0];
			break;
		case 2 : 
			if(corner.touches[1] != null) corner.adjacent[0] = corner.touches[1].corners[3]; 
			corner.adjacent[1] = corner.touches[0].corners[3];
			corner.adjacent[2] = corner.touches[0].corners[1];
			break;
		case 3 : 
			if(corner.touches[1] != null) corner.adjacent[0] = corner.touches[1].corners[4];	
			corner.adjacent[1] = corner.touches[0].corners[4];
			corner.adjacent[2] = corner.touches[0].corners[2];
			break;
		case 4 : 
			if(corner.touches[1] != null) corner.adjacent[0] = corner.touches[1].corners[5];	
			corner.adjacent[1] = corner.touches[0].corners[5];
			corner.adjacent[2] = corner.touches[0].corners[3];
			break;
		case 5 : 
			if(corner.touches[1] != null) corner.adjacent[0] = corner.touches[1].corners[0]; 
			corner.adjacent[1] = corner.touches[0].corners[0];
			corner.adjacent[2] = corner.touches[0].corners[4];
			break;
		}
	}
	
	public static void ConnectAllAdjacent(){
		Utils.instance.allCorners.ForEach(c => ConnectAdjacent(c));
	}
	
	public static void ConnectTouches(Corner corner){
		int j = corner.direction;
		switch(j){
			case 0 : {						
				corner.touches[1] = corner.touches[0].neighbors[5];
				corner.touches[2] = corner.touches[0].neighbors[0];
			}
			break;
			case 1 : 
			case 2 : 
			case 3 : 
			case 4 : 
			case 5 : {
				corner.touches[1] = corner.touches[0].neighbors[j-1];
				corner.touches[2] = corner.touches[0].neighbors[j];
			}
			break;
		}
	}
	
	public static void ConnectAllTouches(){
		Utils.instance.allCorners.ForEach(c => ConnectTouches(c));
	}

	public static Corner FindCornerAt(Vector3 wPoint, float tolerance){
		
		Corner temp = Utils.instance.allCorners.Find(corner =>
			Vector3.Distance(corner.wPos, wPoint) <= tolerance);
		
		return temp;
	}
	
	public static float UpdateCornerElevation(Corner corner, bool smooth){
		float old = corner.touches[0].elevation;
		float res = corner.touches[0].elevation;
		for (int i = 0; i < 3; i++) {
			if(corner.touches[i] != null){
				res = Mathf.Max(res, corner.touches[i].elevation);
				if(corner.touches[i].terrain == TileTerrain.WATER){
					corner.elevation = Config.reg.SeaLevel;
					return corner.elevation;
				}
			}
		}
		if(smooth) {
			res = Mathf.Lerp(old, res, 0.5f);
		}
		corner.elevation = res;
		return res;
		
	}
	
	public static void UpdateAllCornersElevation(bool smooth){
		Utils.instance.allCorners.ForEach(c => {
			UpdateCornerElevation(c, smooth);
		});
	}
}
