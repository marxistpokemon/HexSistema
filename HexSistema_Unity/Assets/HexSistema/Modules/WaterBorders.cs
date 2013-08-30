using UnityEngine;
using System.Collections;

public class WaterBorders : BaseModule {

	public override void Run ()
	{
		Utils.instance.allTiles.ForEach(tile => {
			if(tile.border) {
				tile.elevation = -1;
			} 
		});
	}
}
