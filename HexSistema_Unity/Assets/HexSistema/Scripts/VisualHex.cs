using UnityEngine;
using System.Collections;

public class VisualHex : MonoBehaviour {
	
	public Tile logicTile;
	
	public bool makeThing = false;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(Config.reg.tileSize,Config.reg.tileThickness, 
			Config.reg.tileSize);
	}
	
	// Update is called once per frame
	void Update () {
		
		logicTile.Update();
		transform.renderer.sharedMaterial =Config.reg.terrainMaterials[(int)logicTile.terrain];
		
//		if(logic.border){
//			transform.renderer.material.color = Color.magenta;
//		}
//		else {
//			transform.renderer.material.color = Color.white;
//		}
	}
	
	#region Gizmos
	
	void OnDrawGizmos(){
		
		if(Config.reg.debugMode){
			DrawEdges(Color.yellow);
			DrawCorners(Color.green);
			DrawCenter(Color.red);
		}
		
		if(makeThing) DrawCrazyPath();
	}
	
	void DrawEdges(Color color){
		Gizmos.color = color;
		for (int i = 0; i < 6; i++) {
			Gizmos.DrawLine(logicTile.borders[i].corner0.wPos, logicTile.borders[i].corner1.wPos);
		}
	}
	
	void DrawCorners(Color color){
		Gizmos.color = color;
		for (int i = 0; i < 6; i++) {
			Gizmos.DrawWireSphere(logicTile.corners[i].wPos,Config.reg.cornerTolerance);
		}
	}
	
	void DrawCenter(Color color){
		Gizmos.color = color;
		Gizmos.DrawWireSphere(logicTile.wPos,Config.reg.cornerTolerance);
	}
	
	void DrawCrazyPath(){
		Gizmos.color = Color.magenta;
		
		for (int i = 0; i < 4; i++) {
			Gizmos.DrawLine(
				logicTile.wPos, 
				logicTile.neighbors[i].borders[0].join0.wPos);
		}
	}
	
	#endregion
	
	public static void ElevationDeform(VisualHex tile){
		
		Mesh mesh = tile.transform.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int i = 0;
		int changed = 0;
		
        while (i < vertices.Length) {
			
			Vector3 wVertice = tile.transform.TransformPoint(vertices[i]);
			
			if(wVertice.y > tile.transform.position.y){
				
				Vector2 wVerticeXZ = new Vector2(wVertice.x, wVertice.z);
				
				for (int j = 0; j < 6; j++) {
					Vector2 wCornerXZ = new Vector2(tile.logicTile.corners[j].wPos.x, 
						tile.logicTile.corners[j].wPos.z);
					float distance = Vector2.Distance(wVerticeXZ, wCornerXZ);
					if(distance <=Config.reg.cornerTolerance) {
						changed++;
						vertices[i].y = 
							tile.logicTile.corners[j].elevation + 
							Config.reg.tileThickness/2;
					}
				}
				
				Vector2 wCenterXZ = new Vector2(tile.logicTile.wPos.x, 
					tile.logicTile.wPos.z);
				float distance2 = Vector2.Distance(wVerticeXZ, wCenterXZ);
				if(distance2 <=Config.reg.cornerTolerance) {
					changed++;
					vertices[i].y = tile.logicTile.elevation +Config.reg.tileThickness/2;
				}
			}
			
            i++;
        }
        mesh.vertices = vertices;
		
		MeshCollider mc = tile.transform.GetComponentInChildren<MeshCollider>();
		mc.sharedMesh = null;
		mc.sharedMesh = mesh;
		
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.Optimize();
	}
}
