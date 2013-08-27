using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour {
	
	public static Utils instance;
	
	public List<Tile> allTiles;
	public List<Corner> allCorners;
	public List<Edge> allEdges;
	public List<VisualHex> allVisual;
	
	public DebugMessage debugMsg;
	
	// relative positions, in coords, of tile neighbours
	public Coord[] neighborsRelCPos = new Coord[]
	{
		new Coord(1, -1), new Coord(1,0), new Coord(0, 1),
		new Coord(-1, 1), new Coord(-1, 0), new Coord(0, -1)
	};
	
	public void Awake(){
		Utils.instance = this;
		this.hideFlags = HideFlags.HideInInspector;
		
		
		debugMsg = GetComponentInChildren<DebugMessage>();
		allTiles = new List<Tile>();
		allCorners = new List<Corner>();
		allEdges = new List<Edge>();
		allVisual = new List<VisualHex>();
	}
	
}
