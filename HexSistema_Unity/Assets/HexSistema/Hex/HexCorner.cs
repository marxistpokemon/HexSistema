using UnityEngine;
using System.Collections;

public class HexCorner : Corner {

	public HexCorner(HexTile pTile, int pDirection, Vector3 wPoint ) : base (pTile, pDirection, wPoint){
		
	}
	
	public override void ConnectTouches(){
		int j = direction;
		switch(j){
			case 0 : {						
				touches[1] = touches[0].neighbors[5];
				touches[2] = touches[0].neighbors[0];
			}
			break;
			case 1 : 
			case 2 : 
			case 3 : 
			case 4 : 
			case 5 : {
				touches[1] = touches[0].neighbors[j-1];
				touches[2] = touches[0].neighbors[j];
			}
			break;
		}
	}
	
	public override void ConnectAdjacent(){
		switch(direction){
		case 0 : 
			if(touches[1] != null) adjacent[0] = touches[1].corners[1];
			adjacent[1] = touches[0].corners[1];
			adjacent[2] = touches[0].corners[5];
			break;
		case 1 : 
			if(touches[1] != null) adjacent[0] = touches[1].corners[2];	
			adjacent[1] = touches[0].corners[2];
			adjacent[2] = touches[0].corners[0];
			break;
		case 2 : 
			if(touches[1] != null) adjacent[0] = touches[1].corners[3]; 
			adjacent[1] = touches[0].corners[3];
			adjacent[2] = touches[0].corners[1];
			break;
		case 3 : 
			if(touches[1] != null) adjacent[0] = touches[1].corners[4];	
			adjacent[1] = touches[0].corners[4];
			adjacent[2] = touches[0].corners[2];
			break;
		case 4 : 
			if(touches[1] != null) adjacent[0] = touches[1].corners[5];	
			adjacent[1] = touches[0].corners[5];
			adjacent[2] = touches[0].corners[3];
			break;
		case 5 : 
			if(touches[1] != null) adjacent[0] = touches[1].corners[0]; 
			adjacent[1] = touches[0].corners[0];
			adjacent[2] = touches[0].corners[4];
			break;
		}
	}
	
	public override void ConnectProtrudes(){
		switch(direction){
		case 0 : 
			if(touches[1] != null) protrudes[0] = touches[1].borders[1];
			protrudes[1] = touches[0].borders[1];
			protrudes[2] = touches[0].borders[5];
			break;
		case 1 : 
			if(touches[1] != null) protrudes[0] = touches[1].borders[2];	
			protrudes[1] = touches[0].borders[2];
			protrudes[2] = touches[0].borders[0];
			break;
		case 2 : 
			if(touches[1] != null) protrudes[0] = touches[1].borders[3]; 
			protrudes[1] = touches[0].borders[3];
			protrudes[2] = touches[0].borders[1];
			break;
		case 3 : 
			if(touches[1] != null) protrudes[0] = touches[1].borders[4];	
			protrudes[1] = touches[0].borders[4];
			protrudes[2] = touches[0].borders[2];
			break;
		case 4 : 
			if(touches[1] != null) protrudes[0] = touches[1].borders[5];	
			protrudes[1] = touches[0].borders[5];
			protrudes[2] = touches[0].borders[3];
			break;
		case 5 : 
			if(touches[1] != null) protrudes[0] = touches[1].borders[0]; 
			protrudes[1] = touches[0].borders[0];
			protrudes[2] = touches[0].borders[4];
			break;
		}
	}
}
