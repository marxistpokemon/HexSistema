using UnityEngine;
using System.Collections;

[System.Serializable]
public class Coord {
	
	public int q;
	public int r;
	public char annotation;
	
	public Coord (){
		q = -1;
		r = -1;
	}
	
	public Coord (int pQ, int pR){
		q = pQ;
		r = pR;
	}
	
	public Coord (int pQ, int pR, char pAnnotation){
		q = pQ;
		r = pR;
		annotation = pAnnotation;
	}
};