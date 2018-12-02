using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
    public class MyPlayer : ModPlayer
    {
		public bool OrigamiCrane = false;
		public bool Machan = false;
		public bool MiniMegalodon = false;
		public bool SmallMegalodon = false;
		public bool CuteSlimeXmas = false;
		public bool YoungHarpy = false;
		public bool CuteGastropod = false;
		public bool YoungWyvern = false;
		public bool BabyIchorSticker = false;
		public bool LifelikeMechanicalFrog = false;
		public bool CuteSlimeBlue = false;
		public bool CuteSlimeGreen = false;
		public bool CuteSlimePink = false;
		public bool CuteSlimeBlack = false;
		public bool CuteSlimePurple = false;
		public bool CuteSlimeRed = false;
		public bool CuteSlimeYellow = false;
		public bool CuteSlimeRainbow = false;
		public bool ChunkyandMeatball = false;
		public bool DemonHeart = false;
		public bool BrainofConfusion = false;
		public bool AlienHornet = false;
		public bool DocileDemonEyeRed = false;
		public bool DocileFracturedEyeRed = false;
		public bool DocileDemonEyeGreen = false;
		public bool DocileFracturedEyeGreen = false;
		public bool DocileDemonEyePurple = false;
		public bool DocileFracturedEyePurple = false;
		public bool DocileMechanicalEyeRed = false;
		public bool DocileMechanicalEyeGreen = false;
		public bool DocileMechanicalEyePurple = false;
		public bool DetachedHungry = false;
		public bool BabyOcram = false;
		public bool CursedSkull = false;
		public bool BabyCrimera = false;
		public bool VampireBat = false;
		public bool TorturedSoul = false;
		public bool EnchantedSword = false;
		public bool summ_test_01 = false;
		
		public override void ResetEffects()
		{
			OrigamiCrane = false;
			Machan = false;
			MiniMegalodon = false;
			SmallMegalodon = false;
			CuteSlimeXmas = false;
			YoungHarpy = false;
			CuteGastropod = false;
			YoungWyvern = false;
			BabyIchorSticker = false;
			LifelikeMechanicalFrog = false;
			CuteSlimeBlue = false;
			CuteSlimeGreen = false;
			CuteSlimePink = false;
			CuteSlimeBlack = false;
			CuteSlimePurple = false;
			CuteSlimeRed = false;
			CuteSlimeYellow = false;
			CuteSlimeRainbow = false;
			ChunkyandMeatball = false;
			DemonHeart = false;
			BrainofConfusion = false;
			AlienHornet = false;
			DocileDemonEyeRed = false;
			DocileFracturedEyeRed = false;
			DocileDemonEyeGreen = false;
			DocileFracturedEyeGreen = false;
			DocileDemonEyePurple = false;
			DocileFracturedEyePurple = false;
			DocileMechanicalEyeRed = false;
			DocileMechanicalEyeGreen = false;
			DocileMechanicalEyePurple = false;
			DetachedHungry = false;
			BabyOcram = false;
			CursedSkull = false;
			BabyCrimera = false;
			VampireBat = false;
			TorturedSoul = false;
			EnchantedSword = false;
			summ_test_01 = false;
			
		}	
	}
}