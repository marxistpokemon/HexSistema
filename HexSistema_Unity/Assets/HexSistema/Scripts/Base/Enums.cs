public enum TileType {
	SQUARE,
	HEX
}


#region Direction-related Enums

public enum NeighbourDirection {
	NE 	= 0,
	E 	= 1,
	SE 	= 2,
	SW 	= 3,
	W 	= 4,
	NW 	= 5
}

public enum CornerDirection {
	N 	= 0,
	NE 	= 1,
	SE 	= 2,
	S 	= 3,
	SW 	= 4,
	NW 	= 5
}

public enum EdgeDirection {
	NE 	= 0,
	E 	= 1,
	SE 	= 2,
	SW 	= 3,
	W 	= 4,
	NW 	= 5
}

public enum TouchesDirection {
	S 	= 0,
	NW 	= 1,
	NE 	= 2
}

#endregion

#region World-Model Enums

// World Model-related Enums
public enum TileTerrain {
	WATER = 0,
	LAND = 1,
	ROCK = 2,
	SNOW = 3
}

#endregion