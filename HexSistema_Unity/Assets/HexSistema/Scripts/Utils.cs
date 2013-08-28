using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour {
	
	public static Utils instance;
	
	public List<Tile> allTiles;
	public List<Corner> allCorners;
	public List<Edge> allEdges;
	public List<VisualTile> allVisual;
	
	public DebugMessage debugMsg;
	
	// relative positions, in coords, of hex tile neighbours
	public Coord[] hexNeighborsRelCPos = new Coord[]
	{
		new Coord(1, -1), new Coord(1,0), new Coord(0, 1),
		new Coord(-1, 1), new Coord(-1, 0), new Coord(0, -1)
	};
	
	// relative positions, in coords, of hex tile neighbours
	public Coord[] cubeNeighborsRelCPos = new Coord[]
	{
		new Coord(-1, 0), new Coord(0, 1),
		new Coord(1, 0), new Coord(0, -1) 
	};
	
	public static int GetTileCornerNum(TileType tileType){
		switch(tileType){
			case TileType.HEX : return 6;
			case TileType.SQUARE : return 4;
		}
		return 0;
	}
	
	public static int GetCornerTouchesNum(TileType tileType){
		switch(tileType){
			case TileType.HEX : return 3;
			case TileType.SQUARE : return 4;
		}
		return 0;
	}
	
	public static int GetCornerAdjacentNum(TileType tileType){
		switch(tileType){
			case TileType.HEX : return 3;
			case TileType.SQUARE : return 4;
		}
		return 0;
	}
	
	public static int GetCornerProtrudesNum(TileType tileType){
		switch(tileType){
			case TileType.HEX : return 3;
			case TileType.SQUARE : return 4;
		}
		return 0;
	}
	
	public void Awake(){
		Utils.instance = this;
		this.hideFlags = HideFlags.HideInInspector;
		
		
		debugMsg = GetComponentInChildren<DebugMessage>();
		allTiles = new List<Tile>();
		allCorners = new List<Corner>();
		allEdges = new List<Edge>();
		allVisual = new List<VisualTile>();
	}
	
}
