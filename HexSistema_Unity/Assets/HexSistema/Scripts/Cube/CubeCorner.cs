﻿using UnityEngine;
using System.Collections;

public class CubeCorner : Corner {

	public CubeCorner(CubeTile pTile, int pDirection, Vector3 wPoint ) : base (pTile, pDirection, wPoint){
		
	}
	
	public override void ConnectTouches(){
		int j = direction;
		
		try {
			switch(j){
			case 0 : 					
				touches[1] = touches[0].neighbors[0];
				touches[2] = touches[1].neighbors[1];
				touches[3] = touches[2].neighbors[2];
				break;
			case 1 : 
				touches[1] = touches[0].neighbors[1];
				touches[2] = touches[1].neighbors[2];
				touches[3] = touches[2].neighbors[3];
				break;
			case 2 : 
				touches[1] = touches[0].neighbors[2];
				touches[2] = touches[1].neighbors[3];
				touches[3] = touches[2].neighbors[0];
				break;
			case 3 :  
				touches[1] = touches[0].neighbors[3];
				touches[2] = touches[1].neighbors[0];
				touches[3] = touches[2].neighbors[1];
				break;
			}
			
		} catch (System.Exception ex) {
			
		}
		
		
	}
	
	public override void ConnectAdjacent(){
		try {
			adjacent[0] = touches[1].corners[0];
			adjacent[1] = touches[2].corners[1];
			adjacent[2] = touches[3].corners[2];
			adjacent[3] = touches[0].corners[3];	
		} catch (System.Exception ex) {
			
		}
		
	}
	
	public override void ConnectProtrudes(){
		try {
			protrudes[0] = touches[1].borders[1];
			protrudes[1] = touches[2].borders[2];
			protrudes[2] = touches[3].borders[3];
			protrudes[3] = touches[0].borders[0];
		} catch (System.Exception ex) {
			
		}
		
	}
}