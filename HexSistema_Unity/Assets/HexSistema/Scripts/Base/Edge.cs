using UnityEngine;
using System.Collections;

public class Edge {

public int index;
	
	// logical references
	public Tile join0, join1;
	public Corner corner0, corner1;
	
	// geometry
	public Vector3 midpoint;  // halfway between v0,v1
	
	// world model
	public int river;  // volume of water, or 0
	
	Edge(Corner pCorner0, Corner pCorner1, 
		Tile pJoin0, Tile pJoin1){
		
		index = Utils.instance.allEdges.Count;
		Utils.instance.allEdges.Add(this);
		
		join0 = pJoin0;
		join1 = pJoin1;
		corner0 = pCorner0;
		corner1 = pCorner1;
	}
	
	public static void MakeAllEdges(){
		Utils.instance.allTiles.ForEach(tile => {
			MakeEdges(tile);
		});
	}
	
	public static void MakeEdges(Tile tile){
		
		for (int i = 0; i < tile.cornerNum; i++) {
			Edge newEdge;
			
			if(i == tile.cornerNum-1){
				newEdge = new Edge(
					tile.corners[i],
					tile.corners[0],
					tile,
					tile.neighbors[i]);
			}
			else {
				newEdge = new Edge(
					tile.corners[i],
					tile.corners[i+1],
					tile,
					tile.neighbors[i]);
			}
			Utils.instance.allEdges.Add(newEdge);
			
			// connects to tile
			tile.borders[i] = newEdge;
		}
	}
};
