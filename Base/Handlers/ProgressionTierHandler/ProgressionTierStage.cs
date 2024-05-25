namespace AssortedCrazyThings.Base.Handlers.ProgressionTierHandler
{
	//flinx: 22 dps (dummy) -> matches preboss tier
	//frog: 68 dps (dummy). skeleton: 35. skeleton archer: 24 -> matches skeletron tier
	//blade: 30 dps (dummy). skeleton archer: 30
	//optic: 80 dps (dummy), skeleton archer: 55
	//xeno: 90 dps (dummy), armored skeleton: 55
	public enum ProgressionTierStage : int
	{
		//Value important, texture index, ordered by progression
		PreBoss = 0,
		EoC = 1,
		Evil = 2,
		Skeletron = 3,
		WoF = 4,
		Mech = 5,
		Plantera = 6,
		Cultist = 7
	}

}
