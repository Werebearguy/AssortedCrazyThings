using Terraria.ID;

//Credit to JereTheJuggler for this entire file
namespace AssortedCrazyThings.Base.Data
{
	public enum FurnitureSet
	{
		wooden,
		ebonwood,
		richMahogany,
		pearlwood,
		shadewood,
		palmWood,
		borealWood,
		frozen,
		skyware,
		livingWood,
		slime,
		honey,
		pineWood,
		blueDungeon,
		pinkDungeon,
		greenDungeon,
		gothic,
		steampunk,
		golden,
		martian,
		meteorite,
		granite,
		marble,
		crystal,
		lihzahrd,
		obsidian,
		flesh,
		mushroom,
		pumpkin,
		cactus,
		glass,
		dynasty,
		spooky,
		// for 1.4
		spider,
		lesion,
		solar,
		vortex,
		nebula,
		stardust,
		sandstone,
		bamboo
	}
	public enum FurnitureType
	{
		table,
		chair,
		workbench,
		candle,
		candelabra,
		bed,
		lantern,
		lamp,
		toilet,
		piano,
		clock,
		chandelier,
		bathtub,
		vase,
		door,
		chest,
		bookcase,
		sink,
		sofa,
		dresser
	}

	public static class Furniture
	{
		public static void GetFurnitureVars(FurnitureSet set, FurnitureType type, out ushort tileId, out ushort style)
		{
			if (set == FurnitureSet.wooden && type == FurnitureType.toilet)
			{
				tileId = TileID.Chairs;
				style = Chair.toilet;
			}
			else if (set == FurnitureSet.golden && type == FurnitureType.toilet)
			{
				tileId = TileID.Chairs;
				style = Chair.toilet;
			}
			else
			{
				tileId = GetFurnitureTileID(type);
				style = GetFurnitureStyle(set, type);
			}
		}

		public static ushort GetFurnitureTileID(FurnitureType type)
		{
			return type switch
			{
				FurnitureType.bathtub => TileID.Bathtubs,
				FurnitureType.bed => TileID.Beds,
				FurnitureType.bookcase => TileID.Bookcases,
				FurnitureType.candelabra => TileID.Candelabras,
				FurnitureType.candle => TileID.Candles,
				FurnitureType.chair => TileID.Chairs,
				FurnitureType.chandelier => TileID.Chandeliers,
				FurnitureType.chest => TileID.Containers,
				FurnitureType.clock => TileID.GrandfatherClocks,
				FurnitureType.door => TileID.ClosedDoor,
				FurnitureType.dresser => TileID.Dressers,
				FurnitureType.lamp => TileID.Lamps,
				FurnitureType.lantern => TileID.HangingLanterns,
				FurnitureType.piano => TileID.Pianos,
				FurnitureType.sink => TileID.Sinks,
				FurnitureType.sofa => TileID.Benches,
				FurnitureType.table => TileID.Tables,
				FurnitureType.toilet => TileID.Toilets,
				FurnitureType.vase => TileID.Statues,
				FurnitureType.workbench => TileID.WorkBenches,
				_ => 0,
			};
		}

		public static ushort GetFurnitureStyle(FurnitureSet set, FurnitureType type)
		{
			return type switch
			{
				FurnitureType.bathtub => Bathtub.GetStyle(set),
				FurnitureType.bed => Bed.GetStyle(set),
				FurnitureType.bookcase => Bookcase.GetStyle(set),
				FurnitureType.candelabra => Candelabra.GetStyle(set),
				FurnitureType.candle => Candle.GetStyle(set),
				FurnitureType.chair => Chair.GetStyle(set),
				FurnitureType.chandelier => Chandelier.GetStyle(set),
				FurnitureType.chest => Chests.GetStyle(set),
				FurnitureType.clock => Clock.GetStyle(set),
				FurnitureType.door => Door.GetStyle(set),
				FurnitureType.dresser => Dresser.GetStyle(set),
				FurnitureType.lamp => Lamp.GetStyle(set),
				FurnitureType.lantern => Lantern.GetStyle(set),
				FurnitureType.piano => Piano.GetStyle(set),
				FurnitureType.sink => Sink.GetStyle(set),
				FurnitureType.sofa => Sofa.GetStyle(set),
				FurnitureType.table => Table.GetStyle(set),
				FurnitureType.toilet => Toilet.GetStyle(set),
				FurnitureType.vase => Vase.GetStyle(set),
				FurnitureType.workbench => WorkBench.GetStyle(set),
				_ => 0,
			};
		}
	}

	public static class Trophy
	{
		public const byte eyeOfCthulhu = 0;
		public const byte eaterOfWorlds = 1;
		public const byte brainOfCthulhu = 2;
		public const byte skeletron = 3;
		public const byte queenBee = 4;
		public const byte wallOfFlesh = 5;
		public const byte theDestroyer = 6;
		public const byte skeletronPrime = 7;
		public const byte retinazer = 8;
		public const byte spazmatism = 9;
		public const byte plantera = 10;
		public const byte golem = 11;
		public const byte mourningWood = 36;
		public const byte pumpking = 37;
		public const byte iceQueen = 38;
		public const byte santank1 = 39;
		public const byte everscream = 40;
		public const byte goldfish = 50;
		public const byte bunnyfish = 51;
		public const byte swordfish = 52;
		public const byte sharkteeth = 53;
		public const byte kingSlime = 54;
		public const byte dukeFishron = 55;
		public const byte lunaticCultist = 56;
		public const byte martianSaucer = 57;
		public const byte flyingDutchman = 58;
		public const byte moonLord = 59;
		public const byte darkMage = 60;
		public const byte betsy = 61;
		public const byte ogre = 62;
		public const byte empressOfLight = 72;
		public const byte queenSlime = 73;
	}
	public static class Hanging3x3
	{
		public const byte blacksmithRack = 41;
		public const byte capentryRack = 42;
		public const byte helmetRack = 43;
		public const byte spearRack = 44;
		public const byte swordRack = 45;
		public const byte lifePreserver = 46;
		public const byte shipsWheel = 47;
		public const byte compassRose = 48;
		public const byte wallAnchor = 49;
	}
	public static class Painting3x3
	{
		public const byte bloodMoonRising = 12;
		public const byte theHangedMan = 13;
		public const byte gloryOfTheFire = 14;
		public const byte boneWarp = 15;
		public const byte hangingWallSkeleton = 16;
		public const byte hangingWallSkeletonUpsideDown = 17;
		public const byte skellingtonJSkellingsworth = 18;
		public const byte theCursedMan = 19;
		public const byte sunflowers = 20;
		public const byte terrarianGothic = 21;
		public const byte guidePicasso = 22;
		public const byte theGuardiansGaze = 23;
		public const byte fatherOfSomeone = 24;
		public const byte nurseLisa = 25;
		public const byte discover = 26;
		public const byte handEarth = 27;
		public const byte oldMiner = 28;
		public const byte skelehead = 29;
		public const byte impFace = 30;
		public const byte ominousPresence = 31;
		public const byte shiningMoon = 32;
		public const byte theMerchant = 33;
		public const byte crownoDevoursHisLunch = 34;
		public const byte rareEnchantment = 35;
		public const byte andrewSphinx = 63;
		public const byte watchfulAntlion = 64;
		public const byte burningSpirit = 65;
		public const byte jawsOfDeath = 66;
		public const byte theSandsOfSlime = 67;
		public const byte snakeIHateSnakes = 68;
		public const byte fore = 69;
		public const byte nevermore = 70;
		public const byte reborn = 71;

		public static readonly byte[] painterPaintings = new byte[] {
			nevermore,
			reborn
		};
		public static readonly byte[] desertPaintings = new byte[] {
			andrewSphinx,
			watchfulAntlion,
			burningSpirit,
			jawsOfDeath,
			theSandsOfSlime,
			snakeIHateSnakes
		};
		public static readonly byte[] undergroundPaintings = new byte[] {
			theMerchant,
			nurseLisa,
			oldMiner,
			rareEnchantment,
			sunflowers,
			terrarianGothic,
			guidePicasso,
			crownoDevoursHisLunch,
			discover,
			fatherOfSomeone
		};
		public static readonly byte[] dungeonPaintings = new byte[] {
			bloodMoonRising,
			boneWarp,
			theCursedMan,
			gloryOfTheFire,
			theGuardiansGaze,
			theHangedMan,
			skellingtonJSkellingsworth
		};
		public static readonly byte[] hellPaintings = new byte[] {
			handEarth,
			impFace,
			skelehead,
			ominousPresence,
			shiningMoon
		};
	}
	public static class Painting3x2
	{
		public const byte demonsEye = 0;
		public const byte findingGold = 1;
		public const byte firstEncounter = 2;
		public const byte goodMorning = 3;
		public const byte undergroundReward = 4;
		public const byte throughTheWindow = 5;
		public const byte placeAboveTheClouds = 6;
		public const byte doNotStepOnTheGrass = 7;
		public const byte coldWatersInTheWhiteLand = 8;
		public const byte lightlessChasms = 9;
		public const byte theLandOfDeceivingLooks = 10;
		public const byte daylight = 11;
		public const byte secretOfTheSands = 12;
		public const byte deadlandComesAlive = 13;
		public const byte evilPresence = 14;
		public const byte skyGuardian = 15;
		public const byte livingGore = 16;
		public const byte flowingMagma = 17;
		public const byte holly = 18;
		public const byte theDuplicityOfReflections = 19;
		public const byte stillLife = 20;

		public static readonly byte[] painterPaintings = new byte[] {
			coldWatersInTheWhiteLand,
			daylight,
			deadlandComesAlive,
			doNotStepOnTheGrass,
			evilPresence,
			firstEncounter,
			goodMorning,
			theLandOfDeceivingLooks,
			lightlessChasms,
			placeAboveTheClouds,
			secretOfTheSands,
			skyGuardian,
			throughTheWindow,
			undergroundReward,
			stillLife
		};
		public static readonly byte[] undergroundPaintings = new byte[] {
			findingGold
		};
		public static readonly byte[] hellPaintings = new byte[] {
			demonsEye,
			flowingMagma,
			livingGore
		};
	}
	public static class Painting6x4
	{
		public const byte theEyeSeesTheEnd = 0;
		public const byte somethingEvilIsWatchingYou = 1;
		public const byte theTwinsHaveAwoken = 2;
		public const byte theScreamer = 3;
		public const byte goblinsPlayingPoker = 4;
		public const byte drayadisque = 5;
		public const byte impact = 6;
		public const byte poweredByBirds = 7;
		public const byte theDestroyer = 8;
		public const byte thePersistencyOfEyes = 9;
		public const byte unicornCrossingTheHallows = 10;
		public const byte greatWave = 11;
		public const byte starryNight = 12;
		public const byte facingTheCerebralMastermind = 13;
		public const byte lakeOfFire = 14;
		public const byte trioSuperHeroes = 15;
		public const byte theCreationOfTheGuide = 16;
		public const byte jackingSkeletron = 17;
		public const byte bitterHarvest = 18;
		public const byte bloodMoonCountess = 19;
		public const byte hallowsEve = 20;
		public const byte morbidCuriosity = 21;
		public const byte tigerSkin = 22;
		public const byte leopardSkin = 23;
		public const byte zebraSkin = 24;
		public const byte treasureMap = 25;
		public const byte pillaginMePixels = 26;
		public const byte castleMarsberg = 27;
		public const byte martiaLisa = 28;
		public const byte theTruthIsUpThere = 29;
		public const byte sparky = 30;
		public const byte acorns = 31;
		public const byte coldSnap = 32;
		public const byte cursedSaint = 33;
		public const byte snowfellas = 34;
		public const byte theSeason = 35;
		public const byte notAKidNorASquid = 36;
		public const byte lifeAboveTheSand = 37;
		public const byte oasis = 38;
		public const byte prehistoryPreserved = 39;
		public const byte ancientTablet = 40;
		public const byte uluru = 41;
		public const byte visitingThePyramids = 42;
		public const byte theRollingGreens = 43;
		public const byte graveyard = 44;

		public static readonly byte[] painterPaintings = new byte[] {
			graveyard
		};
		public static readonly byte[] travellingMerchantPaintings = new byte[] {
			acorns,
			castleMarsberg,
			coldSnap,
			cursedSaint,
			martiaLisa,
			notAKidNorASquid,
			theSeason,
			snowfellas,
			theTruthIsUpThere
		};
		public static readonly byte[] desertPaintings = new byte[] {
			lifeAboveTheSand,
			oasis,
			prehistoryPreserved,
			ancientTablet,
			uluru,
			visitingThePyramids
		};
		public static readonly byte[] dungeonPaintings = new byte[] {
			theCreationOfTheGuide,
			theDestroyer,
			drayadisque,
			theEyeSeesTheEnd,
			facingTheCerebralMastermind,
			goblinsPlayingPoker,
			greatWave,
			impact,
			thePersistencyOfEyes,
			poweredByBirds,
			theScreamer,
			sparky,
			somethingEvilIsWatchingYou,
			starryNight,
			trioSuperHeroes,
			theTwinsHaveAwoken,
			unicornCrossingTheHallows
		};
		public static readonly byte[] hellPaintings = new byte[] {
			lakeOfFire
		};
		public static readonly byte[] goodieBagPaintings = new byte[] {
			bitterHarvest,
			bloodMoonCountess,
			hallowsEve,
			jackingSkeletron,
			morbidCuriosity
		};
	}
	public static class Painting2x3
	{
		public const byte waldo = 0;
		public const byte darkness = 1;
		public const byte darkSoulReaper = 2;
		public const byte land = 3;
		public const byte trappedGhost = 4;
		public const byte americanExplosive = 5;
		public const byte gloriousNight = 6;
		public const byte bandageBoy = 7;
		public const byte divineEye = 8;
		public const byte studyOfABallAtRest = 9;
		public const byte ghostManifestation = 10;
		public const byte wickedUndead = 11;
		public const byte bloodyGoblet = 12;

		public static readonly byte[] painterPaintings = new byte[] {
			ghostManifestation,
			wickedUndead,
			bloodyGoblet
		};
		public static readonly byte[] desertPaintings = new byte[] {
			bandageBoy,
			divineEye
		};
		public static readonly byte[] undergroundPaintings = new byte[] {
			americanExplosive,
			gloriousNight,
			land,
			waldo
		};
		public static readonly byte[] hellPaintings = new byte[] {
			darkSoulReaper,
			darkness,
			trappedGhost
		};
	}

	public static class Platform
	{
		public const byte wooden = 0;
		public const byte ebonwood = 1;
		public const byte richMahogany = 2;
		public const byte pearlwood = 3;
		public const byte bone = 4;
		public const byte shadewood = 5;
		public const byte blueBrick = 6;
		public const byte pinkBrick = 7;
		public const byte greenBrick = 8;
		public const byte metalShelf = 9;
		public const byte brassShelf = 10;
		public const byte gothic = 11;
		public const byte dungeon = 12;
		public const byte obsidian = 13;
		public const byte glass = 14;
		public const byte pumpkin = 15;
		public const byte spooky = 16;
		public const byte palmWood = 17;
		public const byte mushroom = 18;
		public const byte boreal = 19;
		public const byte slime = 20;
		public const byte steampunk = 21;
		public const byte skyware = 22;
		public const byte livingWood = 23;
		public const byte honey = 24;
		public const byte cactus = 25;
		public const byte martian = 26;
		public const byte meteorite = 27;
		public const byte granite = 28;
		public const byte marble = 29;
		public const byte crystal = 30;
		public const byte golden = 31;
		public const byte dynasty = 32;
		public const byte lihzahrd = 33;
		public const byte flesh = 34;
		public const byte frozen = 35;
	}

	public static class Door
	{
		public const byte wooden = 0;
		public const byte ebonwood = 1;
		public const byte richMahogany = 2;
		public const byte pearlwood = 3;
		public const byte cactus = 4;
		public const byte flesh = 5;
		public const byte mushroom = 6;
		public const byte livingWood = 7;
		public const byte bone = 8;
		public const byte skyware = 9;
		public const byte shadewood = 10;
		public const byte lihzahrdLocked = 11;
		public const byte lihzahrd = 12;
		public const byte dungeon = 13;
		public const byte lead = 14;
		public const byte iron = 15;
		public const byte blueDungeon = 16;
		public const byte greenDungeon = 17;
		public const byte pinkDungeon = 18;
		public const byte obsidian = 19;
		public const byte glass = 20;
		public const byte golden = 21;
		public const byte honey = 22;
		public const byte steampunk = 23;
		public const byte pumpkin = 24;
		public const byte spooky = 25;
		public const byte pine = 26;
		public const byte frozen = 27;
		public const byte dynasty = 28;
		public const byte palmWood = 29;
		public const byte boreal = 30;
		public const byte slime = 31;
		public const byte martian = 32;
		public const byte meteorite = 33;
		public const byte granite = 34;
		public const byte marble = 35;
		public const byte crystal = 36;
		public const byte spider = 37;
		public const byte lesion = 38;
		public const byte solar = 39;
		public const byte vortex = 40;
		public const byte nebula = 41;
		public const byte stardust = 42;
		public const byte sandstone = 43;
		public const byte stone = 44;
		public const byte bamboo = 45;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => dungeon,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => pine,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Chair
	{
		public const byte wooden = 0;
		public const byte toilet = 1;
		public const byte ebonwood = 2;
		public const byte richMahogany = 3;
		public const byte pearlwood = 4;
		public const byte livingWood = 5;
		public const byte cactus = 6;
		public const byte bone = 7;
		public const byte flesh = 8;
		public const byte mushroom = 9;
		public const byte skyware = 10;
		public const byte shadewood = 11;
		public const byte lihzahrd = 12;
		public const byte blueDungeon = 13;
		public const byte greenDungeon = 14;
		public const byte pinkDungeon = 15;
		public const byte obsidian = 16;
		public const byte gothic = 17;
		public const byte glass = 18;
		public const byte golden = 19;
		public const byte goldenToilet = 20;
		public const byte barStool = 21;
		public const byte honey = 22;
		public const byte steampunk = 23;
		public const byte pumpkin = 24;
		public const byte spooky = 25;
		public const byte pine = 26;
		public const byte dynasty = 27;
		public const byte frozen = 28;
		public const byte palmWood = 29;
		public const byte boreal = 30;
		public const byte slime = 31;
		public const byte martian = 32;
		public const byte meteorite = 33;
		public const byte granite = 34;
		public const byte marble = 35;
		public const byte crystal = 36;
		public const byte spider = 37;
		public const byte lesion = 38;
		public const byte solar = 39;
		public const byte vortex = 40;
		public const byte nebula = 41;
		public const byte stardust = 42;
		public const byte sandstone = 43;
		public const byte bamboo = 44;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => gothic,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => pine,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class WorkBench
	{
		public const byte wooden = 0;
		public const byte ebonwood = 1;
		public const byte richMahogany = 2;
		public const byte pearlwood = 3;
		public const byte bone = 4;
		public const byte cactus = 5;
		public const byte flesh = 6;
		public const byte mushroom = 7;
		public const byte slime = 8;
		public const byte shadewood = 9;
		public const byte lihzahrd = 10;
		public const byte blueDungeon = 11;
		public const byte greenDungeon = 12;
		public const byte pinkDungeon = 13;
		public const byte obsidian = 14;
		public const byte gothic = 15;
		public const byte pumpkin = 16;
		public const byte spooky = 17;
		public const byte dynasty = 18;
		public const byte honey = 19;
		public const byte frozen = 20;
		public const byte steampunk = 21;
		public const byte palmWood = 22;
		public const byte boreal = 23;
		public const byte skyware = 24;
		public const byte glass = 25;
		public const byte livingWood = 26;
		public const byte martian = 27;
		public const byte meteorite = 28;
		public const byte granite = 29;
		public const byte marble = 30;
		public const byte crystal = 31;
		public const byte golden = 32;
		public const byte spider = 33;
		public const byte lesion = 34;
		public const byte solar = 35;
		public const byte vortex = 36;
		public const byte nebula = 37;
		public const byte stardust = 38;
		public const byte sandstone = 39;
		public const byte bamboo = 40;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => gothic,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Table
	{
		public const byte wooden = 0;
		public const byte ebonwood = 1;
		public const byte richMahogany = 2;
		public const byte pearlwood = 3;
		public const byte bone = 4;
		public const byte flesh = 5;
		public const byte livingWood = 6;
		public const byte skyware = 7;
		public const byte shadewood = 8;
		public const byte lihzahrd = 9;
		public const byte blueDungeon = 10;
		public const byte greenDungeon = 11;
		public const byte pinkDungeon = 12;
		public const byte obsidian = 13;
		public const byte gothic = 14;
		public const byte glass = 15;
		public const byte banquetTable = 16;
		public const byte bar = 17;
		public const byte golden = 18;
		public const byte honey = 19;
		public const byte steampunk = 20;
		public const byte pumpkin = 21;
		public const byte spooky = 22;
		public const byte pine = 23;
		public const byte frozen = 24;
		public const byte dynasty = 25;
		public const byte palmWood = 26;
		public const byte mushroom = 27;
		public const byte boreal = 28;
		public const byte slime = 29;
		public const byte cactus = 30;
		public const byte martian = 31;
		public const byte meteorite = 32;
		public const byte granite = 33;
		public const byte marble = 34;
		public const byte crystal = 35;
		public const byte spider = 36;
		public const byte lesion = 37;
		public const byte solar = 38;
		public const byte vortex = 39;
		public const byte nebula = 40;
		public const byte stardust = 41;
		public const byte sandstone = 42;
		public const byte bamboo = 43;
		public const byte picnicTable = 44;
		public const byte fancyPicnicTable = 45;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => gothic,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => pine,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Candle
	{
		public const byte gold = 0;
		public const byte blueDungeon = 1;
		public const byte greenDungeon = 2;
		public const byte pinkDungeon = 3;
		public const byte cactus = 4;
		public const byte ebonwood = 5;
		public const byte flesh = 6;
		public const byte glass = 7;
		public const byte frozen = 8;
		public const byte richMahogany = 9;
		public const byte pearlwood = 10;
		public const byte lihzahrd = 11;
		public const byte skyware = 12;
		public const byte pumpkin = 13;
		public const byte livingWood = 14;
		public const byte shadewood = 15;
		public const byte golden = 16;
		public const byte dynasty = 17;
		public const byte palmWood = 18;
		public const byte mushroom = 19;
		public const byte boreal = 20;
		public const byte slime = 21;
		public const byte honey = 22;
		public const byte steampunk = 23;
		public const byte spooky = 24;
		public const byte obsidian = 25;
		public const byte martian = 26;
		public const byte meteorite = 27;
		public const byte granite = 28;
		public const byte marble = 29;
		public const byte crystal = 30;
		public const byte spider = 31;
		public const byte lesion = 32;
		public const byte solar = 33;
		public const byte vortex = 34;
		public const byte nebula = 35;
		public const byte stardust = 36;
		public const byte sandstone = 37;
		public const byte bamboo = 38;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => 0,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Lamp
	{
		public const byte tikiTorch = 0;
		public const byte cactus = 1;
		public const byte ebonwood = 2;
		public const byte flesh = 3;
		public const byte glass = 4;
		public const byte frozen = 5;
		public const byte richMahogany = 6;
		public const byte pearlwood = 7;
		public const byte lihzahrd = 8;
		public const byte skyware = 9;
		public const byte spooky = 10;
		public const byte honey = 11;
		public const byte steampunk = 12;
		public const byte livingWood = 13;
		public const byte shadewood = 14;
		public const byte golden = 15;
		public const byte bone = 16;
		public const byte dynasty = 17;
		public const byte palmWood = 18;
		public const byte mushroom = 19;
		public const byte boreal = 20;
		public const byte slime = 21;
		public const byte pumpkin = 22;
		public const byte obsidian = 23;
		public const byte blueDungeon = 24;
		public const byte greenDungeon = 25;
		public const byte pinkDungeon = 26;
		public const byte martian = 27;
		public const byte meteorite = 28;
		public const byte granite = 29;
		public const byte marble = 30;
		public const byte crystal = 31;
		public const byte spider = 32;
		public const byte lesion = 33;
		public const byte solar = 34;
		public const byte vortex = 35;
		public const byte nebula = 36;
		public const byte stardust = 37;
		public const byte sandstone = 38;
		public const byte bamboo = 39;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => 0,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Torch
	{
		public const byte normal = 0;
		public const byte blue = 1;
		public const byte red = 2;
		public const byte green = 3;
		public const byte purple = 4;
		public const byte white = 5;
		public const byte yellow = 6;
		public const byte demon = 7;
		public const byte cursed = 8;
		public const byte ice = 9;
		public const byte orange = 10;
		public const byte ichor = 11;
		public const byte ultrabright = 12;
		public const byte bone = 13;
		public const byte rainbow = 14;
		public const byte pink = 15;
		public const byte desert = 16;
		public const byte coral = 17;
		public const byte corrupt = 18;
		public const byte crimson = 19;
		public const byte hallowed = 20;
		public const byte jungle = 21;
	}

	public static class Chests
	{
		public const byte wooden = 0;
		public const byte gold = 1;
		public const byte goldLocked = 2;
		public const byte shadow = 3;
		public const byte shadowLocked = 4;
		public const byte barrel = 5;
		public const byte trashCan = 6;
		public const byte ebonwood = 7;
		public const byte richMahogany = 8;
		public const byte pearlwood = 9;
		public const byte ivy = 10;
		public const byte ice = 11;
		public const byte livingWood = 12;
		public const byte skyware = 13;
		public const byte shadewood = 14;
		public const byte webCovered = 15;
		public const byte lihzahrd = 16;
		public const byte ocean = 17;
		public const byte jungle = 18;
		public const byte corruption = 19;
		public const byte crimson = 20;
		public const byte hallowed = 21;
		public const byte frozen = 22;
		public const byte jungleLocked = 23;
		public const byte corruptionLocked = 24;
		public const byte crimsonLocked = 25;
		public const byte hallowedLocked = 26;
		public const byte frozenLocked = 27;
		public const byte dynasty = 28;
		public const byte honey = 29;
		public const byte steampunk = 30;
		public const byte palmWood = 31;
		public const byte mushroom = 32;
		public const byte boreal = 33;
		public const byte slime = 34;
		public const byte greenDungeon = 35;
		public const byte greenDungeonLocked = 36;
		public const byte pinkDungeon = 37;
		public const byte pinkDungeonLocked = 38;
		public const byte blueDungeon = 39;
		public const byte blueDungeonLocked = 40;
		public const byte bone = 41;
		public const byte cactus = 42;
		public const byte flesh = 43;
		public const byte obsidian = 44;
		public const byte pumpkin = 45;
		public const byte spooky = 46;
		public const byte glass = 47;
		public const byte martian = 48;
		public const byte meteorite = 49;
		public const byte granite = 50;
		public const byte marble = 51;
		public const byte crystal = 52;
		public const byte golden = 53;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				//for 1.4
				FurnitureSet.spider => 0,
				FurnitureSet.lesion => 0,
				FurnitureSet.solar => 0,
				FurnitureSet.vortex => 0,
				FurnitureSet.nebula => 0,
				FurnitureSet.stardust => 0,
				FurnitureSet.sandstone => 0,
				FurnitureSet.bamboo => 0,
				_ => 0,
			};
		}
	}

	public static class Chandelier
	{
		public const byte copper = 0;
		public const byte silver = 1;
		public const byte gold = 2;
		public const byte tin = 3;
		public const byte tungsten = 4;
		public const byte platinum = 5;
		public const byte jackelier = 6;
		public const byte cactus = 7;
		public const byte ebonwood = 8;
		public const byte flesh = 9;
		public const byte honey = 10;
		public const byte frozen = 11;
		public const byte richMahogany = 12;
		public const byte pearlwood = 13;
		public const byte lihzahrd = 14;
		public const byte skyware = 15;
		public const byte spooky = 16;
		public const byte glass = 17;
		public const byte livingWood = 18;
		public const byte shadewood = 19;
		public const byte golden = 20;
		public const byte bone = 21;
		public const byte dynasty = 22;
		public const byte palmWood = 23;
		public const byte mushroom = 24;
		public const byte boreal = 25;
		public const byte slime = 26;
		public const byte blueDungeon = 27;
		public const byte greenDungeon = 28;
		public const byte pinkDungeon = 29;
		public const byte steampunk = 30;
		public const byte pumpkin = 31;
		public const byte obsidian = 32;
		public const byte martian = 33;
		public const byte meteorite = 34;
		public const byte granite = 35;
		public const byte marble = 36;
		public const byte crystal = 37;
		public const byte spider = 38;
		public const byte lesion = 39;
		public const byte solar = 40;
		public const byte vortex = 41;
		public const byte nebula = 42;
		public const byte stardust = 43;
		public const byte sandstone = 44;
		public const byte bamboo = 45;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => 0,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Bed
	{
		public const byte normal = 0;
		public const byte wooden = 0;
		public const byte ebonwood = 1;
		public const byte richMahogany = 2;
		public const byte pearlwood = 3;
		public const byte shadewood = 4;
		public const byte blueDungeon = 5;
		public const byte greenDungeon = 6;
		public const byte pinkDungeon = 7;
		public const byte obsidian = 8;
		public const byte glass = 9;
		public const byte golden = 10;
		public const byte honey = 11;
		public const byte steampunk = 12;
		public const byte cactus = 13;
		public const byte flesh = 14;
		public const byte frozen = 15;
		public const byte lihzahrd = 16;
		public const byte skyware = 17;
		public const byte spooky = 18;
		public const byte livingWood = 19;
		public const byte bone = 20;
		public const byte dynasty = 21;
		public const byte palmWood = 22;
		public const byte mushroom = 23;
		public const byte boreal = 24;
		public const byte slime = 25;
		public const byte pumpkin = 26;
		public const byte martian = 27;
		public const byte meteorite = 28;
		public const byte granite = 29;
		public const byte marble = 30;
		public const byte crystal = 31;
		public const byte spider = 32;
		public const byte lesion = 33;
		public const byte solar = 34;
		public const byte vortex = 35;
		public const byte nebula = 36;
		public const byte stardust = 37;
		public const byte sandstone = 38;
		public const byte bamboo = 39;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Bathtub
	{
		public const byte normal = 0;
		public const byte cactus = 1;
		public const byte ebonwood = 2;
		public const byte flesh = 3;
		public const byte glass = 4;
		public const byte frozen = 5;
		public const byte richMahogany = 6;
		public const byte pearlwood = 7;
		public const byte lihzahrd = 8;
		public const byte skyware = 9;
		public const byte spooky = 10;
		public const byte honey = 11;
		public const byte steampunk = 12;
		public const byte livingWood = 13;
		public const byte shadewood = 14;
		public const byte bone = 15;
		public const byte dynasty = 16;
		public const byte palmWood = 17;
		public const byte mushroom = 18;
		public const byte boreal = 19;
		public const byte slime = 20;
		public const byte blueDungeon = 21;
		public const byte greenDungeon = 22;
		public const byte pinkDungeon = 23;
		public const byte pumpkin = 24;
		public const byte obsidian = 25;
		public const byte golden = 26;
		public const byte martian = 27;
		public const byte meteorite = 28;
		public const byte granite = 29;
		public const byte marble = 30;
		public const byte crystal = 31;
		public const byte spider = 32;
		public const byte lesion = 33;
		public const byte solar = 34;
		public const byte vortex = 35;
		public const byte nebula = 36;
		public const byte stardust = 37;
		public const byte sandstone = 38;
		public const byte bamboo = 39;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => normal,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Lantern
	{
		public const byte chain = 0;
		public const byte brass = 1;
		public const byte caged = 2;
		public const byte carriage = 3;
		public const byte alchemy = 4;
		public const byte diabolist = 5;
		public const byte oilRagSconce = 6;
		public const byte starInABottle = 7;
		public const byte hangingJackOLantern = 8;
		public const byte heart = 9;
		public const byte cactus = 10;
		public const byte ebonwood = 11;
		public const byte flesh = 12;
		public const byte honey = 13;
		public const byte steampunk = 14;
		public const byte glass = 15;
		public const byte richMahogany = 16;
		public const byte pearlwood = 17;
		public const byte frozen = 18;
		public const byte lihzahrd = 19;
		public const byte skyware = 20;
		public const byte spooky = 21;
		public const byte livingWood = 22;
		public const byte shadewood = 23;
		public const byte golden = 24;
		public const byte bone = 25;
		public const byte dynasty = 26;
		public const byte palmWood = 27;
		public const byte mushroom = 28;
		public const byte boreal = 29;
		public const byte slime = 30;
		public const byte pumpkin = 31;
		public const byte obsidian = 32;
		public const byte martian = 33;
		public const byte meteorite = 34;
		public const byte granite = 35;
		public const byte marble = 36;
		public const byte crystal = 37;
		public const byte spider = 38;
		public const byte lesion = 39;
		public const byte solar = 40;
		public const byte vortex = 41;
		public const byte nebula = 42;
		public const byte stardust = 43;
		public const byte sandstone = 44;
		public const byte bamboo = 45;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => 0,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => 0,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => 0,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => 0,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Piano
	{
		public const byte normal = 0;
		public const byte wooden = 0;
		public const byte ebonwood = 1;
		public const byte richMahogany = 2;
		public const byte pearlwood = 3;
		public const byte shadewood = 4;
		public const byte livingWood = 5;
		public const byte flesh = 6;
		public const byte frozen = 7;
		public const byte glass = 8;
		public const byte honey = 9;
		public const byte steampunk = 10;
		public const byte blueDungeon = 11;
		public const byte greenDungeon = 12;
		public const byte pinkDungeon = 13;
		public const byte golden = 14;
		public const byte obsidian = 15;
		public const byte bone = 16;
		public const byte cactus = 17;
		public const byte spooky = 18;
		public const byte skyware = 19;
		public const byte lihzahrd = 20;
		public const byte palmWood = 21;
		public const byte mushroom = 22;
		public const byte boreal = 23;
		public const byte slime = 24;
		public const byte pumpkin = 25;
		public const byte martian = 26;
		public const byte meteorite = 27;
		public const byte granite = 28;
		public const byte marble = 29;
		public const byte crystal = 30;
		public const byte dynasty = 31;
		public const byte spider = 32;
		public const byte lesion = 33;
		public const byte solar = 34;
		public const byte vortex = 35;
		public const byte nebula = 36;
		public const byte stardust = 37;
		public const byte sandstone = 38;
		public const byte bamboo = 39;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => normal,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Bookcase
	{
		public const byte normal = 0;
		public const byte wooden = 0;
		public const byte blueDungeon = 1;
		public const byte greenDungeon = 2;
		public const byte pinkDungeon = 3;
		public const byte obsidian = 4;
		public const byte gothic = 5;
		public const byte cactus = 6;
		public const byte ebonwood = 7;
		public const byte flesh = 8;
		public const byte honey = 9;
		public const byte steampunk = 10;
		public const byte glass = 11;
		public const byte richMahogany = 12;
		public const byte pearlwood = 13;
		public const byte spooky = 14;
		public const byte skyware = 15;
		public const byte lihzahrd = 16;
		public const byte frozen = 17;
		public const byte livingWood = 18;
		public const byte shadewood = 19;
		public const byte golden = 20;
		public const byte bone = 21;
		public const byte dynasty = 22;
		public const byte palmWood = 23;
		public const byte mushroom = 24;
		public const byte boreal = 25;
		public const byte slime = 26;
		public const byte pumpkin = 27;
		public const byte martian = 28;
		public const byte meteorite = 29;
		public const byte granite = 30;
		public const byte marble = 31;
		public const byte crystal = 32;
		public const byte spider = 33;
		public const byte lesion = 34;
		public const byte solar = 35;
		public const byte vortex = 36;
		public const byte nebula = 37;
		public const byte stardust = 38;
		public const byte sandstone = 39;
		public const byte bamboo = 40;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Clock
	{
		public const byte normal = 0;
		public const byte wooden = 0;
		public const byte dynasty = 1;
		public const byte golden = 2;
		public const byte glass = 3;
		public const byte honey = 4;
		public const byte steampunk = 5;
		public const byte boreal = 6;
		public const byte slime = 7;
		public const byte bone = 8;
		public const byte cactus = 9;
		public const byte ebonwood = 10;
		public const byte frozen = 11;
		public const byte lihzahrd = 12;
		public const byte livingWood = 13;
		public const byte richMahogany = 14;
		public const byte flesh = 15;
		public const byte mushroom = 16;
		public const byte obsidian = 17;
		public const byte palmWood = 18;
		public const byte pearlwood = 19;
		public const byte pumpkin = 20;
		public const byte shadewood = 21;
		public const byte spooky = 22;
		public const byte skyware = 23;
		public const byte martian = 24;
		public const byte meteorite = 25;
		public const byte granite = 26;
		public const byte marble = 27;
		public const byte crystal = 28;
		public const byte sunplate = 29;
		public const byte blueDungeon = 30;
		public const byte greenDungeon = 31;
		public const byte pinkDungeon = 32;
		public const byte spider = 33;
		public const byte lesion = 34;
		public const byte solar = 35;
		public const byte vortex = 36;
		public const byte nebula = 37;
		public const byte stardust = 38;
		public const byte sandstone = 39;
		public const byte bamboo = 40;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Sink
	{
		public const byte wooden = 0;
		public const byte ebonwood = 1;
		public const byte richMahogany = 2;
		public const byte pearlwood = 3;
		public const byte bone = 4;
		public const byte flesh = 5;
		public const byte livingWood = 6;
		public const byte skyware = 7;
		public const byte shadewood = 8;
		public const byte lihzahrd = 9;
		public const byte blueDungeon = 10;
		public const byte greenDungeon = 11;
		public const byte pinkDungeon = 12;
		public const byte obsidian = 13;
		public const byte metal = 14;
		public const byte glass = 15;
		public const byte golden = 16;
		public const byte honey = 17;
		public const byte steampunk = 18;
		public const byte pumpkin = 19;
		public const byte spooky = 20;
		public const byte frozen = 21;
		public const byte dynasty = 22;
		public const byte palmWood = 23;
		public const byte mushroom = 24;
		public const byte boreal = 25;
		public const byte slime = 26;
		public const byte cactus = 27;
		public const byte martian = 28;
		public const byte meteorite = 29;
		public const byte granite = 30;
		public const byte marble = 31;
		public const byte crystal = 32;
		public const byte spider = 33;
		public const byte lesion = 34;
		public const byte solar = 35;
		public const byte vortex = 36;
		public const byte nebula = 37;
		public const byte stardust = 38;
		public const byte sandstone = 39;
		public const byte bamboo = 40;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Sofa
	{
		public const byte bench = 0;
		public const byte wooden = 1;
		public const byte ebonwood = 2;
		public const byte richMahogany = 3;
		public const byte pearlwood = 4;
		public const byte shadewood = 5;
		public const byte blueDungeon = 6;
		public const byte greenDungeon = 7;
		public const byte pinkDungeon = 8;
		public const byte golden = 9;
		public const byte obsidian = 10;
		public const byte bone = 11;
		public const byte cactus = 12;
		public const byte spooky = 13;
		public const byte skyware = 14;
		public const byte honey = 15;
		public const byte steampunk = 16;
		public const byte mushroom = 17;
		public const byte glass = 18;
		public const byte pumpkin = 19;
		public const byte lihzahrd = 20;
		public const byte palmWood = 21;
		public const byte boreal = 22;
		public const byte slime = 23;
		public const byte flesh = 24;
		public const byte frozen = 25;
		public const byte livingWood = 26;
		public const byte martian = 27;
		public const byte meteorite = 28;
		public const byte granite = 29;
		public const byte marble = 30;
		public const byte crystal = 31;
		public const byte dynasty = 32;
		public const byte spider = 33;
		public const byte lesion = 34;
		public const byte solar = 35;
		public const byte vortex = 36;
		public const byte nebula = 37;
		public const byte stardust = 38;
		public const byte sandstone = 39;
		public const byte bamboo = 40;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Toilet
	{
		public const byte ebonwood = 0;
		public const byte richMahogany = 1;
		public const byte pearlwood = 2;
		public const byte livingWood = 3;
		public const byte cactus = 4;
		public const byte bone = 5;
		public const byte flesh = 6;
		public const byte mushroom = 7;
		public const byte skyware = 8;
		public const byte shadewood = 9;
		public const byte lihzahrd = 10;
		public const byte blueDungeon = 11;
		public const byte greenDungeon = 12;
		public const byte pinkDungeon = 13;
		public const byte obsidian = 14;
		public const byte frozen = 15;
		public const byte glass = 16;
		public const byte honey = 17;
		public const byte steampunk = 18;
		public const byte pumpkin = 19;
		public const byte spooky = 20;
		public const byte dynasty = 21;
		public const byte palmWood = 22;
		public const byte boreal = 23;
		public const byte slime = 24;
		public const byte martian = 25;
		public const byte granite = 26;
		public const byte marble = 27;
		public const byte crystal = 28;
		public const byte spider = 29;
		public const byte lesion = 30;
		public const byte diamond = 31;
		public const byte meteorite = 32;
		public const byte solar = 33;
		public const byte vortex = 34;
		public const byte nebula = 35;
		public const byte stardust = 36;
		public const byte sandstone = 37;
		public const byte bamboo = 38;
		public const byte terra = 39;

		public static ushort getTileId(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.golden or FurnitureSet.wooden => TileID.Chairs,
				_ => 497,
			};
		}
		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => Chair.goldenToilet,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => Chair.toilet,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Dresser
	{
		public const byte normal = 0;
		public const byte wooden = 0;
		public const byte ebonwood = 1;
		public const byte richMahogany = 2;
		public const byte pearlwood = 3;
		public const byte shadewood = 4;
		public const byte blueDungeon = 5;
		public const byte greenDungeon = 6;
		public const byte pinkDungeon = 7;
		public const byte golden = 8;
		public const byte obsidian = 9;
		public const byte bone = 10;
		public const byte cactus = 11;
		public const byte spooky = 12;
		public const byte skyware = 13;
		public const byte honey = 14;
		public const byte lihzahrd = 15;
		public const byte palmWood = 16;
		public const byte mushroom = 17;
		public const byte boreal = 18;
		public const byte slime = 19;
		public const byte pumpkin = 20;
		public const byte steampunk = 21;
		public const byte glass = 22;
		public const byte flesh = 23;
		public const byte martian = 24;
		public const byte meteorite = 25;
		public const byte granite = 26;
		public const byte marble = 27;
		public const byte crystal = 28;
		public const byte dynasty = 29;
		public const byte frozen = 30;
		public const byte livingWood = 31;
		public const byte spider = 32;
		public const byte lesion = 33;
		public const byte solar = 34;
		public const byte vortex = 35;
		public const byte nebula = 36;
		public const byte stardust = 37;
		public const byte sandstone = 38;
		public const byte bamboo = 39;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => wooden,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Candelabra
	{
		public const byte gold = 0;
		public const ushort platinumCandelabraId = TileID.PlatinumCandelabra;
		public const byte cactus = 1;
		public const byte ebonwood = 2;
		public const byte flesh = 3;
		public const byte honey = 4;
		public const byte steampunk = 5;
		public const byte glass = 6;
		public const byte richMahogany = 7;
		public const byte pearlwood = 8;
		public const byte frozen = 9;
		public const byte lihzahrd = 10;
		public const byte skyware = 11;
		public const byte spooky = 12;
		public const byte livingWood = 13;
		public const byte shadewood = 14;
		public const byte golden = 15;
		public const byte bone = 16;
		public const byte dynasty = 17;
		public const byte palmWood = 18;
		public const byte mushroom = 19;
		public const byte boreal = 20;
		public const byte slime = 21;
		public const byte blueDungeon = 22;
		public const byte greenDungeon = 23;
		public const byte pinkDungeon = 24;
		public const byte obsidian = 25;
		public const byte pumpkin = 26;
		public const byte martian = 27;
		public const byte meteorite = 28;
		public const byte granite = 29;
		public const byte marble = 30;
		public const byte crystal = 31;
		public const byte spider = 32;
		public const byte lesion = 33;
		public const byte solar = 34;
		public const byte vortex = 35;
		public const byte nebula = 36;
		public const byte stardust = 37;
		public const byte sandstone = 38;
		public const byte bamboo = 39;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.borealWood => boreal,
				FurnitureSet.cactus => cactus,
				FurnitureSet.crystal => crystal,
				FurnitureSet.dynasty => dynasty,
				FurnitureSet.ebonwood => ebonwood,
				FurnitureSet.flesh => flesh,
				FurnitureSet.frozen => frozen,
				FurnitureSet.glass => glass,
				FurnitureSet.golden => golden,
				FurnitureSet.gothic => 0,
				FurnitureSet.granite => granite,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.honey => honey,
				FurnitureSet.lihzahrd => lihzahrd,
				FurnitureSet.livingWood => livingWood,
				FurnitureSet.marble => marble,
				FurnitureSet.martian => martian,
				FurnitureSet.meteorite => meteorite,
				FurnitureSet.mushroom => mushroom,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.palmWood => palmWood,
				FurnitureSet.pearlwood => pearlwood,
				FurnitureSet.pineWood => 0,
				FurnitureSet.pinkDungeon => pinkDungeon,
				FurnitureSet.pumpkin => pumpkin,
				FurnitureSet.richMahogany => richMahogany,
				FurnitureSet.shadewood => shadewood,
				FurnitureSet.skyware => skyware,
				FurnitureSet.slime => slime,
				FurnitureSet.spooky => spooky,
				FurnitureSet.steampunk => steampunk,
				FurnitureSet.wooden => 0,
				FurnitureSet.spider => spider,
				FurnitureSet.lesion => lesion,
				FurnitureSet.solar => solar,
				FurnitureSet.vortex => vortex,
				FurnitureSet.nebula => nebula,
				FurnitureSet.stardust => stardust,
				FurnitureSet.sandstone => sandstone,
				FurnitureSet.bamboo => bamboo,
				_ => 0,
			};
		}
	}

	public static class Banner
	{
		public const byte red = 0;
		public const byte green = 1;
		public const byte blue = 2;
		public const byte yellow = 3;
		public const byte ankh = 4;
		public const byte snake = 5;
		public const byte omega = 6;
		public const byte world = 7;
		public const byte sun = 8;
		public const byte gravity = 9;
		public const byte marchingBones = 10;
		public const byte necromanticSign = 11;
		public const byte rustedCompany = 12;
		public const byte raggedBrotherhood = 13;
		public const byte moltenLegion = 14;
		public const byte diabolicSigil = 15;
		public const byte hellbound = 16;
		public const byte hellHammer = 17;
		public const byte helltower = 18;
		public const byte lostHopesOfMan = 19;
		public const byte obsidianWatcher = 20;
		public const byte lavaErupts = 21;
		public const byte anglerFish = 22;
		public const byte angryNimbus = 23;
		public const byte anomuraFungus = 24;
		public const byte antlion = 25;
		public const byte arapaima = 26;
		public const byte armoredSkeleton = 27;
		public const byte caveBat = 28;
		public const byte bird = 29;
		public const byte blackRecluse = 30;
		public const byte bloodFeeder = 31;
		public const byte bloodJelly = 32;
		public const byte bloodCrawler = 33;
		public const byte boneSerpent = 34;
		public const byte bunny = 35;
		public const byte chaosElemental = 36;
		public const byte mimic = 37;
		public const byte clown = 38;
		public const byte corruptBunny = 39;
		public const byte corruptGoldfish = 40;
		public const byte crab = 41;
		public const byte crimera = 42;
		public const byte crimsonAxe = 43;
		public const byte corruptHammer = 44;
		public const byte demon = 45;
		public const byte demonEye = 46;
		public const byte derpling = 47;
		public const byte eaterOfSouls = 48;
		public const byte enchantedSword = 49;
		public const byte zombieEskimo = 50;
		public const byte faceMonster = 51;
		public const byte floatyGross = 52;
		public const byte flyingFish = 53;
		public const byte flyingSnake = 54;
		public const byte frankenstein = 55;
		public const byte fungiBulb = 56;
		public const byte fungoFish = 57;
		public const byte gastropod = 58;
		public const byte goblinThief = 59;
		public const byte goblinSorcerer = 60;
		public const byte goblinPeon = 61;
		public const byte goblinScout = 62;
		public const byte goblinWarrior = 63;
		public const byte goldfish = 64;
		public const byte harpy = 65;
		public const byte hellbat = 66;
		public const byte herpling = 67;
		public const byte hornet = 68;
		public const byte iceElemental = 69;
		public const byte icyMerman = 70;
		public const byte fireImp = 71;
		public const byte blueJellyfish = 72;
		public const byte jungleCreeper = 73;
		public const byte lihzahrd = 74;
		public const byte manEater = 75;
		public const byte meteorHead = 76;
		public const byte moth = 77;
		public const byte mummy = 78;
		public const byte mushiLadybug = 79;
		public const byte parrot = 80;
		public const byte pigron = 81;
		public const byte piranha = 82;
		public const byte pirateDeckhand = 83;
		public const byte pixie = 84;
		public const byte raincoatZombie = 85;
		public const byte reaper = 86;
		public const byte shark = 87;
		public const byte skeleton = 88;
		public const byte darkCaster = 89;
		public const byte blueSlime = 90;
		public const byte flinx = 91;
		public const byte wallCreeper = 92;
		public const byte sporeZombie = 93;
		public const byte swampThing = 94;
		public const byte giantTortoise = 95;
		public const byte toxicSludge = 96;
		public const byte umbrellaSlime = 97;
		public const byte unicorn = 98;
		public const byte vampire = 99;
		public const byte vulture = 100;
		public const byte nymph = 101;
		public const byte werewolf = 102;
		public const byte wolf = 103;
		public const byte worldFeeder = 104;
		public const byte worm = 105;
		public const byte wraith = 106;
		public const byte wyvern = 107;
		public const byte zombie = 108;
		public const byte angryTrapper = 109;
		public const byte armoredViking = 110;
		public const byte blackSlime = 111;
		public const byte blueArmoredBones = 112;
		public const byte blueCultistArcher = 113;
		public const byte blueCultistCaster = 114;
		public const byte blueCultistFighter = 115;
		public const byte boneLee = 116;
		public const byte clinger = 117;
		public const byte cochinealBeetle = 118;
		public const byte corruptPenguin = 119;
		public const byte corruptSlime = 120;
		public const byte corruptor = 121;
		public const byte crimslime = 122;
		public const byte cursedSkull = 123;
		public const byte cyanBeetle = 124;
		public const byte devourer = 125;
		public const byte diabolist = 126;
		public const byte doctorBones = 127;
		public const byte dungeonSlime = 128;
		public const byte dungeonSpirit = 129;
		public const byte elfArcher = 130;
		public const byte elfCopter = 131;
		public const byte eyezor = 132;
		public const byte flocko = 133;
		public const byte ghost = 134;
		public const byte giantBat = 135;
		public const byte giantCursedSkull = 136;
		public const byte giantFlyingFox = 137;
		public const byte gingerbreadMan = 138;
		public const byte goblinArcher = 139;
		public const byte greenSlime = 140;
		public const byte headlessHorseman = 141;
		public const byte hellArmoredBones = 142;
		public const byte hellhound = 143;
		public const byte hoppinJack = 144;
		public const byte iceBat = 145;
		public const byte iceGolem = 146;
		public const byte iceSlime = 147;
		public const byte ichorSticker = 148;
		public const byte illuminantBat = 149;
		public const byte illuminantSlime = 150;
		public const byte jungleBat = 151;
		public const byte jungleSlime = 152;
		public const byte krampus = 153;
		public const byte lacBeetle = 154;
		public const byte lavaBat = 155;
		public const byte lavaSlime = 156;
		public const byte martianBrainscrambler = 157;
		public const byte martianDrone = 158;
		public const byte martianEngineer = 159;
		public const byte martianGigazapper = 160;
		public const byte martianGrayGrunt = 161;
		public const byte martianOfficer = 162;
		public const byte martianRayGunner = 163;
		public const byte martianScultixGunner = 164;
		public const byte martianTeslaTurret = 165;
		public const byte misterStabby = 166;
		public const byte motherSlime = 167;
		public const byte necromancer = 168;
		public const byte nutcracker = 169;
		public const byte paladin = 170;
		public const byte penguin = 171;
		public const byte pinky = 172;
		public const byte poltergeist = 173;
		public const byte possessedArmor = 174;
		public const byte presentMimic = 175;
		public const byte purpleSlime = 176;
		public const byte raggedCaster = 177;
		public const byte rainbowSlime = 178;
		public const byte raven = 179;
		public const byte redSlime = 180;
		public const byte runeWizard = 181;
		public const byte rustyArmoredBones = 182;
		public const byte scarecrow = 183;
		public const byte scultix = 184;
		public const byte skeletonArcher = 185;
		public const byte skeletonCommando = 186;
		public const byte skeletonSniper = 187;
		public const byte slimer = 188;
		public const byte snatcher = 189;
		public const byte snowBalla = 190;
		public const byte snowmanGangsta = 191;
		public const byte spikedIceSlime = 192;
		public const byte spikedJungleSlime = 193;
		public const byte splinterling = 194;
		public const byte squid = 195;
		public const byte tacticalSkeleton = 196;
		public const byte theGroom = 197;
		public const byte tim = 198;
		public const byte undeadMiner = 199;
		public const byte undeadViking = 200;
		public const byte whiteCultistArcher = 201;
		public const byte whiteCultistCaster = 202;
		public const byte whiteCultistFighter = 203;
		public const byte yellowSlime = 204;
		public const byte yeti = 205;
		public const byte zombieElf = 206;
		public const byte goblinSummoner = 207;
		public const byte salamander = 208;
		public const byte giantShelly = 209;
		public const byte crawdad = 210;
		public const byte fritz = 211;
		public const byte creatureFromTheDeep = 212;
		public const byte drManFly = 213;
		public const byte mothron = 214;
		public const byte severedHand = 215;
		public const byte thePossessed = 216;
		public const byte butcher = 217;
		public const byte psycho = 218;
		public const byte deadlySphere = 219;
		public const byte nailhead = 220;
		public const byte poisonousSpore = 221;
		public const byte medusa = 222;
		public const byte hoplite = 223;
		public const byte graniteElemental = 224;
		public const byte graniteGolem = 225;
		public const byte bloodZombie = 226;
		public const byte drippler = 227;
		public const byte tombCrawler = 228;
		public const byte duneSplicer = 229;
		public const byte antlionSwarmer = 230;
		public const byte antlionCharger = 231;
		public const byte ghoul = 232;
		public const byte lamia = 233;
		public const byte desertSpirit = 234;
		public const byte basilisk = 235;
		public const byte sandPoacher = 236;
		public const byte stargazer = 237;
		public const byte milkywayWeaver = 238;
		public const byte flowInvader = 239;
		public const byte twinklePopper = 240;
		public const byte smallStarCell = 241;
		public const byte starCell = 242;
		public const byte corite = 243;
		public const byte sroller = 244;
		public const byte crawltipede = 245;
		public const byte drakomireRider = 246;
		public const byte drakomire = 247;
		public const byte selenian = 248;
		public const byte predictor = 249;
		public const byte brainSuckler = 250;
		public const byte nebulaFloater = 251;
		public const byte evolutionBeast = 252;
		public const byte alienLarva = 253;
		public const byte alienQueen = 254;
		public const byte alienHornet = 255;
		public const ushort vortexian = 256;
		public const ushort stormDiver = 257;
		public const ushort pirateCaptain = 258;
		public const ushort pirateDeadeye = 259;
		public const ushort pirateCorsair = 260;
		public const ushort pirateCrossbower = 261;
		public const ushort martianWalker = 262;
		public const ushort redDevil = 263;
		public const ushort pinkJellyfish = 264;
		public const ushort greenJellyfish = 265;
		public const ushort darkMummy = 266;
		public const ushort lightMummy = 267;
		public const ushort angryBones = 268;
		public const ushort iceTortoise = 269;
		public const ushort sandSlime = 270;
		public const ushort seaSnail = 271;
		public const ushort sandElemental = 272;
		public const ushort sandShark = 273;
		public const ushort boneBiter = 274;
		public const ushort fleshReaver = 275;
		public const ushort crystalThresher = 276;
		public const ushort angryTumbler = 277;
		public const ushort etherianGoblinBomber = 278;
		public const ushort etherianGoblin = 279;
		public const ushort oldOnesSkeleton = 280;
		public const ushort drakin = 281;
		public const ushort koboldGlider = 282;
		public const ushort kobold = 283;
		public const ushort witherBeast = 284;
		public const ushort etherianWyvern = 285;
		public const ushort etherianJavelinThrower = 286;
		public const ushort etherianLightningBug = 287;
		public const ushort theBride = 288;
		public const ushort zombieMerman = 289;
		public const ushort wanderingEyeFish = 290;
		public const ushort bloodSquid = 291;
		public const ushort bloodEel = 292;
		public const ushort hemogoblinShark = 293;
		public const ushort dreadnautilus = 294;
		public const ushort angryDandelion = 295;
		public const ushort gnome = 296;
		public const ushort rockGolem = 297;
		public const ushort bloodMummy = 298;
		public const ushort sporeSkeleton = 299;
		public const ushort sporeBat = 300;
		public const ushort antlionLarva = 301;
		public const ushort visciousBunny = 302;
		public const ushort visciousGoldfish = 303;
		public const ushort visciousPenguin = 304;
		public const ushort corruptMimic = 305;
		public const ushort crimsonMimic = 306;
		public const ushort hallowedMimic = 307;
		public const ushort mossHornet = 308;
		public const ushort wanderingEye = 309;

		public static readonly ushort[] craftableBanners = new ushort[] {
			red,
			green,
			blue,
			yellow
		};
		public static readonly ushort[] floatingIslandBanners = new ushort[] {
			gravity,
			world,
			sun
		};
		public static readonly ushort[] hellBanners = new ushort[] {
			hellbound,
			hellHammer,
			helltower,
			lostHopesOfMan,
			obsidianWatcher,
			lavaErupts
		};
		public static readonly ushort[] pyramidBanners = new ushort[] {
			ankh,
			snake,
			omega
		};
		public static readonly ushort[] dungeonBanners = new ushort[] {
			marchingBones,
			necromanticSign,
			rustedCompany,
			raggedBrotherhood,
			moltenLegion,
			diabolicSigil
		};
	}

	public static class Vase
	{
		public const byte blueDungeon = 46;
		public const byte greenDungeon = 47;
		public const byte pinkDungeon = 48;
		public const byte obsidian = 49;

		public static byte GetStyle(FurnitureSet set)
		{
			return set switch
			{
				FurnitureSet.blueDungeon => blueDungeon,
				FurnitureSet.greenDungeon => greenDungeon,
				FurnitureSet.obsidian => obsidian,
				FurnitureSet.pinkDungeon => pinkDungeon,
				_ => 0,
			};
		}
	}

	public static class Statue
	{
		public const byte armor = 0;
		public const byte angel = 1;
		public const byte star = 2;
		public const byte sword = 3;
		public const byte slime = 4;
		public const byte goblin = 5;
		public const byte shield = 6;
		public const byte bat = 7;
		public const byte fish = 8;
		public const byte bunny = 9;
		public const byte skeleton = 10;
		public const byte reaper = 11;
		public const byte woman = 12;
		public const byte imp = 13;
		public const byte gargoyle = 14;
		public const byte gloom = 15;
		public const byte hornet = 16;
		public const byte bomb = 17;
		public const byte crab = 18;
		public const byte hammer = 19;
		public const byte potion = 20;
		public const byte spear = 21;
		public const byte cross = 22;
		public const byte jellyfish = 23;
		public const byte bow = 24;
		public const byte boomerang = 25;
		public const byte boot = 26;
		public const byte chest = 27;
		public const byte bird = 28;
		public const byte axe = 29;
		public const byte corrupt = 30;
		public const byte tree = 31;
		public const byte anvil = 32;
		public const byte pickaxe = 33;
		public const byte mushroom = 34;
		public const byte eyeball = 35;
		public const byte pillar = 36;
		public const byte heart = 37;
		public const byte pot = 38;
		public const byte sunflower = 39;
		public const byte king = 40;
		public const byte queen = 41;
		public const byte piranha = 42;
		public const byte lihzahrd = 43;
		public const byte lihzahrdWatcher = 44;
		public const byte lihzardGuardian = 45;
		public const byte shark = 50;
		public const byte squirrel = 51;
		public const byte butterfly = 52;
		public const byte worm = 53;
		public const byte firefly = 54;
		public const byte scorpion = 55;
		public const byte snail = 56;
		public const byte grasshopper = 57;
		public const byte mouse = 58;
		public const byte duck = 59;
		public const byte penguin = 60;
		public const byte frog = 61;
		public const byte buggy = 62;
		public const byte spider = 63;
		public const byte unicorn = 64;
		public const byte drippler = 65;
		public const byte wraith = 66;
		public const byte boneSkeleton = 67;
		public const byte undeadViking = 68;
		public const byte medusa = 69;
		public const byte harpy = 70;
		public const byte pigron = 71;
		public const byte hoplite = 72;
		public const byte graniteGolem = 73;
		public const byte armedZombie = 74;
		public const byte bloodZombie = 75;
		public const byte owl = 76;
		public const byte seagull = 77;
		public const byte dragonfly = 78;
		public const byte turtle = 79;
	}

	public static class Pot
	{
		public const byte minUndergroundPot = 0;
		public const byte maxUndergroundPot = 11;
		public const byte minIcePot = 12;
		public const byte maxIcePot = 20;
		public const byte minJunglePot = 21;
		public const byte maxJunglePot = 29;
		public const byte minDungeonPot = 30;
		public const byte maxDungeonPot = 38;
		public const byte minHellPot = 39;
		public const byte maxHellPot = 47;
		public const byte minCorruptionPot = 48;
		public const byte maxCorruptionPot = 56;
		public const byte minWebbedPot = 57;
		public const byte maxWebbedPot = 65;
		public const byte minCrimsonPot = 66;
		public const byte maxCrimsonPot = 74;
		public const byte minPyramidPot = 75;
		public const byte maxPyramidPot = 83;
		public const byte minTemplePot = 84;
		public const byte maxTemplePot = 92;
		public const byte minMarblePot = 93;
		public const byte maxMarblePot = 101;
		public const byte minSandstonePot = 102;
		public const byte maxSandstonePot = 110;
	}
}
