using AssortedCrazyThings.Buffs.Mounts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Mounts
{
	[Content(ContentType.Bosses)]
	public class SigilOfTheWingMount : AssMount
	{
		public const int FrameCount = 6;

		public override void Load()
		{
			On.Terraria.Mount.DoesHoverIgnoresFatigue += Mount_DoesHoverIgnoresFatigue;
		}

		private static bool Mount_DoesHoverIgnoresFatigue(On.Terraria.Mount.orig_DoesHoverIgnoresFatigue orig, Mount self)
		{
			bool ret = orig(self);

			if (self.Type >= MountID.Count && self.Type == ModContent.MountType<SigilOfTheWingMount>())
			{
				//Otherwise it falls down after fatigueMax runs out
				return true;
			}

			return ret;
		}

		public override void SetStaticDefaults()
		{
			//Stats mostly copied from UFO
			MountData.spawnDust = 135;
			MountData.buff = ModContent.BuffType<SigilOfTheWingBuff>();
			MountData.heightBoost = -16;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 6f;
			MountData.dashSpeed = 6f;
			MountData.flightTimeMax = 320;
			MountData.fatigueMax = 320;
			MountData.fallDamage = 0f;
			MountData.usesHover = true;
			MountData.runSpeed = 8f;
			MountData.dashSpeed = 8f;
			MountData.acceleration = 0.16f;
			MountData.jumpHeight = 10;
			MountData.jumpSpeed = 4f;
			MountData.blockExtraJumps = true;
			MountData.totalFrames = FrameCount;
			int[] array = new int[MountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 0;
			}
			MountData.playerYOffsets = array;
			MountData.xOffset = 0;
			MountData.bodyFrame = 0;
			MountData.yOffset = 4;
			MountData.playerHeadOffset = 0;
			MountData.standingFrameCount = FrameCount;
			MountData.standingFrameDelay = 4;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = FrameCount;
			MountData.runningFrameDelay = 4;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = FrameCount;
			MountData.flyingFrameDelay = 4;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = FrameCount;
			MountData.inAirFrameDelay = 4;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = FrameCount;
			MountData.idleFrameDelay = 4;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			MountData.emitsLight = true;
			MountData.lightColor = new Vector3(0.5f, 0.5f, 1f);
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;
			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			//UFO code
			var mount = mountedPlayer.mount;
			if (mount._frameState != state)
			{
				mount._frameState = state;
				if (state != 1 && state != 2)
				{
					mount._frameCounter = 0f;
				}
			}

			if (state != 0)
			{
				mount._idleTime = 0;
			}

			var data = mount._data;
			if (data.emitsLight)
			{
				Lighting.AddLight(mountedPlayer.Center, data.lightColor);
			}

			//state = 2 code:
			mount._frameCounter += 1f;
			if (mount._frameCounter > data.inAirFrameDelay)
			{
				mount._frameCounter -= data.inAirFrameDelay;
				mount._frame++;
			}
			if (mount._frame < data.inAirFrameStart || mount._frame >= data.inAirFrameStart + data.inAirFrameCount)
			{
				mount._frame = data.inAirFrameStart;
			}

			//New visuals:
			if (Main.rand.NextFloat() < (mountedPlayer.velocity.LengthSquared() > 2 * 2 ? 0.6f : 0.2f))
			{
				Vector2 position = mountedPlayer.Bottom + new Vector2(0, -16 + mountedPlayer.gfxOffY);
				position.X += Main.rand.NextFloatDirection() * Main.rand.Next(mountedPlayer.width / 2);
				Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-1.5f, -1f)), 200, Color.LightGray, 1f);
				dust.noGravity = false;
				dust.noLight = true;
				dust.fadeIn = Main.rand.NextFloat(0.8f, 1.1f);
			}

			return false;
		}

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
			spriteEffects = SpriteEffects.None;
			drawColor = Color.White * drawPlayer.stealth * (1f - shadow);
			return true;
		}
	}
}
