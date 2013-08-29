using UnityEngine;
using System.Collections;

public class PerlinElevation : BaseModule {
	
	public bool bypassWater = false;
	public bool resetElevation = true;
	public float baseElevation = 0;
	public int perlinOctaves = 4;
	public float perlinFrq = 5f;
	public float perlinAmp = 3f;
	public bool perlinSmooth = true;
	
	
	public override void Run(){
		
		PerlinNoise perlin = new PerlinNoise(Mathf.FloorToInt(Random.value*10000000));
		perlin.LoadPermTableIntoTexture();
		
		Utils.instance.allCorners.ForEach(corner => {
			
			if(bypassWater && corner.water){
				
			}
			else {
				float startingElevation;
				
				if(resetElevation){
					startingElevation = baseElevation;
					corner.elevation = 0;
					corner.touches[0].elevation = 0;
				}
				else {
					startingElevation = 0;
				}
				
				corner.elevation += startingElevation + perlin.FractalNoise2D(
					corner.wPos.x/Config.reg.tileSize , 
					corner.wPos.z/Config.reg.tileSize ,
					perlinOctaves, perlinFrq, perlinAmp);
				
				corner.touches[0].elevation += startingElevation + perlin.FractalNoise2D(
					corner.touches[0].wPos.x/Config.reg.tileSize , 
					corner.touches[0].wPos.z/Config.reg.tileSize ,
					perlinOctaves, perlinFrq, perlinAmp);
			
				if(!perlinSmooth){
					corner.elevation = Mathf.Floor(corner.elevation);
				}
				
				
				corner.elevation = Mathf.Clamp(corner.elevation, Config.reg.SeaLevel, 1000);
				corner.touches[0].elevation = Mathf.Clamp(corner.touches[0].elevation, 
					Config.reg.SeaLevel, 1000);
				corner.touches[0].Update();
				corner.GetWater();
				if(corner.GetWater()) corner.elevation = Config.reg.SeaLevel;
				
			}
			
			
			// corner.touches[0].elevation = corner.touches[0].GetElevationFromCorners();
				
		});
	}	
}
