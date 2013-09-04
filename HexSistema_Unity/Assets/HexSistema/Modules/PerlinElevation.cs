using UnityEngine;
using System.Collections;

public class PerlinElevation : BaseModule {
	
	public bool bypassWater = false;
	public bool resetElevation = true;
	public float baseElevation = 0;
	public float maxElevation = 10;
	public int octaves = 4;
	public float frequency = 5f;
	private float amplitude = 1f;
	public bool smooth = true;
	
	public override void Run(){
		
		Debug.Log("RunModule"+id);
		
		PerlinNoise perlin = new PerlinNoise(Mathf.FloorToInt(Random.value*10000000));
		perlin.LoadPermTableIntoTexture();
		
		foreach (var corner in Utils.instance.allCorners) {
			
			// ignore water
			if(bypassWater && corner.water){
				continue;
			}
			
			float startingElevation;
			if(resetElevation){
				startingElevation = baseElevation;
				corner.elevation = Config.reg.SeaLevel;
				corner.touches[0].elevation = Config.reg.SeaLevel;
			}
			else {
				startingElevation = 0;
			}
			
			// calculate values to -1 to 1 domain
			float cornerElevation = perlin.FractalNoise2D(
				corner.wPos.x/Config.reg.tileSize, 
				corner.wPos.z/Config.reg.tileSize,
				octaves, frequency, amplitude)/amplitude;
			
			
			float tileElevation = perlin.FractalNoise2D(
				corner.touches[0].wPos.x/Config.reg.tileSize, 
				corner.touches[0].wPos.z/Config.reg.tileSize,
				octaves, frequency, amplitude)/amplitude;
			
			cornerElevation = Mathf.Clamp(cornerElevation, Config.reg.SeaLevel, maxElevation);
			tileElevation = Mathf.Clamp(tileElevation, Config.reg.SeaLevel, maxElevation);
			corner.elevation += startingElevation + cornerElevation*maxElevation;
			corner.touches[0].elevation	+= startingElevation + tileElevation*maxElevation;
			
			if(!smooth){
				corner.elevation = Mathf.Floor(corner.elevation);
				corner.touches[0].elevation = Mathf.Floor(corner.touches[0].elevation);
			}
		}
		Debug.Log("FinishModule"+id);
	}	
}
