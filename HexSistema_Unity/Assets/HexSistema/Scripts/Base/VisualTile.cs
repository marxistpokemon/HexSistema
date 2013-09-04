using UnityEngine;
using System.Collections;

public abstract class VisualTile : MonoBehaviour {
	
	public Tile logicTile;
	public int cornerNum;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(Config.reg.tileSize,Config.reg.tileThickness, 
			Config.reg.tileSize);
		cornerNum = Utils.GetTileCornerNum(logicTile.tileType);
	}
	
	public static void UpdateAllMaterials(){
		Utils.instance.allVisual.ForEach(visual=>
			visual.transform.renderer.sharedMaterial = 
				Config.reg.terrainMaterials[(int)visual.logicTile.terrain]
			);
		
	}
	
	public void OnMouseDown(){
		Utils.instance.debugMsg.Log(
			logicTile.index + 
			" | Elev: " + logicTile.elevation +
			" | Water: " + logicTile.waterbool +
			" | Coast: " + logicTile.coast +
			" | Border: " + logicTile.border, true);
		foreach (Corner corner in logicTile.corners) {
			Utils.instance.debugMsg.Log(
			corner.index + 
			" | Elev: " + corner.elevation +
			" | Water: " + corner.water +
			" | Coast: " + corner.coast, false);
		}
	}
	
	#region Gizmos
	
	void OnDrawGizmos(){
		
		if(Config.reg.debugMode){
			DrawEdges(Color.yellow);
			DrawCorners(Color.green);
			DrawCenter(Color.red);
		}
		
		//if(makeThing) DrawCrazyPath();
	}
	
	void DrawEdges(Color color){
		Gizmos.color = color;
		for (int i = 0; i < cornerNum; i++) {
			Gizmos.DrawLine(logicTile.borders[i].corner0.wPos, logicTile.borders[i].corner1.wPos);
		}
	}
	
	void DrawCorners(Color color){
		Gizmos.color = color;
		for (int i = 0; i < cornerNum; i++) {
			Gizmos.DrawWireSphere(logicTile.corners[i].wPos,Config.reg.cornerTolerance);
		}
	}
	
	void DrawCenter(Color color){
		Gizmos.color = color;
		Gizmos.DrawWireSphere(logicTile.wPos,Config.reg.cornerTolerance);
	}
	
	#endregion
	
	public void ElevationDeform(){
		
		Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int i = 0;
		int changed = 0;
		
        while (i < vertices.Length) {
			
			Vector3 wVertice = transform.TransformPoint(vertices[i]);
			
			if(wVertice.y > transform.position.y){
				
				Vector2 wVerticeXZ = new Vector2(wVertice.x, wVertice.z);
				
				for (int j = 0; j < cornerNum; j++) {
					Vector2 wCornerXZ = new Vector2(logicTile.corners[j].wPos.x, 
						logicTile.corners[j].wPos.z);
					float distance = Vector2.Distance(wVerticeXZ, wCornerXZ);
					if(distance <= Config.reg.cornerTolerance) {
						changed++;
						vertices[i].y = 
							logicTile.corners[j].elevation + 
							Config.reg.tileThickness/2;
					}
				}
				
				Vector2 wCenterXZ = new Vector2(logicTile.wPos.x, 
					logicTile.wPos.z);
				float distance2 = Vector2.Distance(wVerticeXZ, wCenterXZ);
				if(distance2 <=Config.reg.cornerTolerance) {
					changed++;
					vertices[i].y = logicTile.elevation + Config.reg.tileThickness/2;
				}
			}
			
            i++;
        }
        mesh.vertices = vertices;
		
		MeshCollider mc = transform.GetComponent<MeshCollider>();
		mc.sharedMesh = null;
		mc.sharedMesh = mesh;
		
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.Optimize();
	}
}
