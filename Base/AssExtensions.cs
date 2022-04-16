using Microsoft.Xna.Framework;
using Terraria;

namespace AssortedCrazyThings.Base
{
	/// <summary>
	/// contains Extensions for some basic tasks
	/// </summary>
	public static class AssExtensions
	{
		/// <summary>
		/// Returns the average value between the RGB (ignoring A)
		/// </summary>
		public static float GetAverage(this Color color)
		{
			return (color.R + color.G + color.B) / 3f;
		}

		/// <summary>
		/// Returns the Player that owns the given projectile. Only use if you are certain an owner exists and it is a player
		/// </summary>
		public static Player GetOwner(this Projectile proj)
		{
			return Main.player[proj.owner];
		}

		/// <summary>
		/// Copy of vanilla code for spawning a single pet and setting buffTime. Gives random velocity at spawn
		/// </summary>
		/// <param name="player"></param>
		/// <param name="buffIndex"></param>
		/// <param name="petBool"></param>
		/// <param name="petProjID"></param>
		/// <param name="buffTimeToGive"></param>
		public static void AssSpawnPetIfNeededAndSetTime(this Player player, int buffIndex, ref bool petBool, int petProjID, int buffTimeToGive = 18000)
		{
			player.buffTime[buffIndex] = buffTimeToGive;
			player.AssSpawnPetIfNeeded(ref petBool, petProjID, buffIndex);
		}

		/// <summary>
		/// Player.HasItem + checks all banks
		/// </summary>
		/// <param name="player"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool HasItemWithBanks(this Player player, int type)
		{
			if (player.HasItem(type)) return true;

			Item[][] inventoryArray = { player.bank.item, player.bank2.item, player.bank3.item, player.bank4.item };
			for (int i = 0; i < inventoryArray.Length; i++)
			{
				Item[] inventory = inventoryArray[i];
				for (int j = 0; j < inventory.Length; j++)
				{
					Item item = inventory[j];
					if (type == item.type && item.stack > 0) return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if given item is present in the players inventory or equip slots
		/// </summary>
		public static bool ItemInInventoryOrEquipped(this Player player, Item item, bool ignoreVanity = false)
		{
			if (player.HasItem(item.type)) return true;
			if (item.accessory || item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0)
			{
				int maxLength = ignoreVanity ? 10 : player.armor.Length;
				for (int i = 0; i < maxLength; i++)
				{
					if (player.armor[i].type == item.type) return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Copy of vanilla code for spawning a single pet. Gives random velocity at spawn
		/// </summary>
		/// <param name="player"></param>
		/// <param name="petBool"></param>
		/// <param name="petProjID"></param>
		/// <param name="buffIndex"></param>
		public static void AssSpawnPetIfNeeded(this Player player, ref bool petBool, int petProjID, int buffIndex)
		{
			petBool = true;
			if (player.ownedProjectileCounts[petProjID] > 0)
			{
				return;
			}

			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.Center, -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2), petProjID, 0, 0f, player.whoAmI);
			}
		}

		public static void LoopAnimation(ref int frame, ref double frameCounter, int speed, int startFrame, int endFrame)
		{
			if (startFrame < 0)
			{
				startFrame = 0;
			}

			if (frame < startFrame)
			{
				frame = startFrame;
			}
			else if (frame > endFrame)
			{
				frame = endFrame;
			}

			frameCounter++;
			if (frameCounter >= speed)
			{
				frameCounter = 0;
				frame++;
				if (frame > endFrame)
				{
					frame = startFrame;
				}
			}
		}

		public static void LoopAnimationInt(ref int frame, ref int frameCounter, int speed, int startFrame, int endFrame)
		{
			if (startFrame < 0)
			{
				startFrame = 0;
			}

			if (frame < startFrame)
			{
				frame = startFrame;
			}
			else if (frame > endFrame)
			{
				frame = endFrame;
			}

			frameCounter++;
			if (frameCounter >= speed)
			{
				frameCounter = 0;
				frame++;
				if (frame > endFrame)
				{
					frame = startFrame;
				}
			}
		}

		/// <summary>
		/// Loops through all frames in a set speed from top to bottom and repeats
		/// </summary>
		public static void LoopAnimation(this NPC npc, int frameHeight, int speed, int startFrame = 0, int endFrame = -1)
		{
			if (endFrame == -1)
			{
				endFrame = Main.npcFrameCount[npc.type] - 1;
			}

			int frame = npc.frame.Y / frameHeight;
			LoopAnimation(ref frame, ref npc.frameCounter, speed, startFrame, endFrame);
			npc.frame.Y = frame * frameHeight;
		}

		/// <summary>
		/// Loops through all frames in a set speed from top to bottom and repeats
		/// </summary>
		public static void LoopAnimation(this Projectile proj, int speed, int startFrame = 0, int endFrame = -1)
		{
			if (endFrame == -1)
			{
				endFrame = Main.projFrames[proj.type] - 1;
			}

			LoopAnimationInt(ref proj.frame, ref proj.frameCounter, speed, startFrame, endFrame);
		}

		/// <summary>
		/// Same as LoopAnimation, but stops at the last frame. Returns true if still animating
		/// </summary>
		public static bool WaterfallAnimation(this NPC npc, int frameHeight, int speed, int startFrame = 0, int endFrame = -1)
		{
			//If no endFrame specified: take last frame on the sheet, otherwise, endFrame
			bool lastFrame = (endFrame == -1 && npc.frame.Y * frameHeight >= Main.npcFrameCount[npc.type] - 1) || (endFrame != -1);

			bool stillAnimating = !lastFrame;
			if (stillAnimating) npc.LoopAnimation(frameHeight, speed, startFrame, endFrame);
			return stillAnimating;
		}

		/// <summary>
		/// Same as LoopAnimation, but stops at the last frame. Returns true if still animating
		/// </summary>
		public static bool WaterfallAnimation(this Projectile proj, int speed, int startFrame = 0, int endFrame = -1)
		{
			//If no endFrame specified: take last frame on the sheet, otherwise, endFrame
			bool lastFrame = (endFrame == -1 && proj.frame >= Main.projFrames[proj.type] - 1) || (endFrame != -1);

			bool stillAnimating = !lastFrame;
			if (stillAnimating) proj.LoopAnimation(speed, startFrame, endFrame);
			return stillAnimating;
		}
	}
}
