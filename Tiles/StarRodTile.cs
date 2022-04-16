using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Items.Placeable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AssortedCrazyThings.Tiles
{
	//Classes handling player nearby state below
	[Content(ContentType.PlaceablesFunctional)]
	public class StarRodTile : DroppableTile<StarRodItem>
	{
		public const int Height = 4;
		public const int TotalHeight = 18 * Height;
		public const int Width = 2;
		public const int TotalWidth = 18 * Width;

		public static Asset<Texture2D> glowmaskAsset;
		public static Asset<Texture2D> pulseAsset;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				glowmaskAsset = ModContent.Request<Texture2D>(Texture + "_Glowmask");
				pulseAsset = ModContent.Request<Texture2D>(Texture + "_Pulse");
			}
		}

		public override void Unload()
		{
			glowmaskAsset = null;
			pulseAsset = null;
		}

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = Height;
			TileObjectData.newTile.Origin = new Point16(Width - 1, Height - 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Star Rod");
			AddMapEntry(new Color(75, 80, 75), name);
			DustType = 1;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, TotalWidth, TotalHeight, ItemType);
		}

		//you need these four things for the outline to work:
		//_Highlight.png
		//TileID.Sets.HasOutlines[Type] = true;
		//TileID.Sets.DisableSmartCursor[Type] = true;
		//and this hook
		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			Tile tile = Main.tile[i, j];
			tileFrameY = (short)(tile.TileFrameY % TotalHeight);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Texture2D texture = Main.instance.TilesRenderer.GetTileDrawTexture(tile, i, j);
			Vector2 zero = new Vector2(Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			int width = 16;
			int height = 16;
			int offsetY = 2;
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;

			TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);

			Color color = Lighting.GetColor(i, j);
			Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
			pos.Y += offsetY;
			Rectangle frame = new Rectangle(frameX, frameY, width, height);
			spriteBatch.Draw(texture, pos, frame, color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

			//Draw glowmask
			bool isEnabled = IsEnabled(i, j);
			if (isEnabled)
			{
				spriteBatch.Draw(glowmaskAsset.Value, pos, frame, Color.White, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}

			//Draw pulse
			if (CanBeActive(i, j))
			{
				//Main.GlobalTimeWrappedHourly basically counts in full seconds. 1f = 1 second
				Color pulseColor = Color.White;
				float colorMult = (float)Math.Sin(Main.GlobalTimeWrappedHourly / 2.5f * MathHelper.TwoPi) * 0.5f + 0.5f;
				colorMult *= 0.5f; //by default from 0 to 0.5f

				if (isEnabled)
				{
					colorMult *= 1.4f;
					colorMult += 0.3f;
				}
				pulseColor *= colorMult;
				pulseColor.A = Math.Max((byte)100, pulseColor.A); //So its pulsating visibly even when fully lighted

				spriteBatch.Draw(pulseAsset.Value, pos, frame, pulseColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}

			AssUtils.DrawTileHighlight(spriteBatch, i, j, Type, color, pos, frame);

			return false;
		}

		public override bool RightClick(int i, int j)
		{
			SoundEngine.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
			return true;
		}

		public override void HitWire(int i, int j)
		{
			int x = i - Main.tile[i, j].TileFrameX / 18 % Width;
			int y = j - Main.tile[i, j].TileFrameY / 18 % Height;
			int change = TotalHeight; //frameY stores the state
			if (!IsEnabled(x, y))
			{
				change = -change;
			}

			Tile tile;
			for (int l = x; l < x + Width; l++)
			{
				for (int m = y; m < y + Height; m++)
				{
					tile = Framing.GetTileSafely(l, m);
					if (tile.HasTile && tile.TileType == Type)
					{
						tile.TileFrameY = (short)(tile.TileFrameY + change);
					}
				}
			}

			if (Wiring.running)
			{
				Wiring.SkipWire(x, y + 0);
				Wiring.SkipWire(x, y + 1);
				Wiring.SkipWire(x, y + 2);
				Wiring.SkipWire(x, y + 3);
				Wiring.SkipWire(x + 1, y + 0);
				Wiring.SkipWire(x + 1, y + 1);
				Wiring.SkipWire(x + 1, y + 2);
				Wiring.SkipWire(x + 1, y + 3);
			}
			NetMessage.SendTileSquare(-1, x, y + 2, 4);
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.mouseInterface = true;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ItemType;
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (IsEnabled(i, j) && CanBeActive(i, j))
			{
				Main.LocalPlayer.GetModPlayer<StarRodModPlayer>().starRodTileNearby = true;
			}
		}

		/// <summary>
		/// If the tile is able to do its thing (regardless of <see cref="IsEnabled"/> state)
		/// </summary>
		private static bool CanBeActive(int i, int j)
		{
			return !Main.dayTime && j < Main.worldSurface;
		}

		/// <summary>
		/// If the tile is enabled (by default or by the player on right click)
		/// </summary>
		private static bool IsEnabled(int i, int j)
		{
			return Main.tile[i, j].TileFrameY < TotalHeight;
		}
	}

	[Content(ContentType.PlaceablesFunctional)]
	public class StarRodModSystem : AssSystem
	{
		public override void ResetNearbyTileEffects()
		{
			Main.LocalPlayer.GetModPlayer<StarRodModPlayer>().starRodTileNearby = false;
		}
	}

	//Responsible for x-axis repositioning spawned falling star spawners towards a suitable location
	[Content(ContentType.PlaceablesFunctional)]
	public class StarRodGlobalProjectile : AssGlobalProjectile
	{
		public const float DetectionDistX = 1920 * 5; //5 screens in either direction

		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return entity.type == ProjectileID.FallingStarSpawner;
		}

		public override bool InstancePerEntity => true;

		private bool firstTick = false;

		public override bool PreAI(Projectile projectile)
		{
			//Modified event rate messes with the timer increment, need safer detection (at the start of AI)
			if (firstTick)
			{
				return true;
			}
			firstTick = true;

			if (projectile.owner != Main.myPlayer)
			{
				return true;
			}

			if (!TryGetRedirectX(projectile, DetectionDistX, out float redirectX, out _))
			{
				return true;
			}

			projectile.position.X = redirectX;
			if (Math.Abs(projectile.velocity.X) > 2.5f)
			{
				//Anything above this would land outside the screen, so reduce it
				projectile.velocity.X *= 0.5f;
			}

			return true;
		}

		private static bool TryGetRedirectX(Projectile projectile, float detectionDistX, out float redirectX, out float distX)
		{
			redirectX = 0;

			//Find nearest player in range of a star rod
			float maxDistX = float.MaxValue;
			distX = float.MaxValue;
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];
				if (!player.active || player.dead)
				{
					continue;
				}

				if (!player.GetModPlayer<StarRodModPlayer>().starRodTileWorking)
				{
					continue;
				}

				distX = Math.Abs(projectile.Center.X - player.Center.X);
				if (distX < maxDistX && distX < detectionDistX)
				{
					maxDistX = distX;
					redirectX = player.Center.X;
				}
			}

			return redirectX != 0;
		}

		//For reference. Spawned by the server
		//private void VanillaFallingStarSpawnerAI(Projectile projectile)
		//{
		//    if (Main.dayTime)
		//    {
		//        projectile.Kill();
		//        return;
		//    }

		//    projectile.ai[0] += (float)Main.desiredWorldEventsUpdateRate;
		//    if (projectile.localAI[0] == 0f && Main.netMode != 2)
		//    {
		//        projectile.localAI[0] = 1f;
		//        if ((double)Main.LocalPlayer.position.Y < Main.worldSurface * 16.0)
		//            Star.StarFall(projectile.position.X);
		//    }

		//    if (projectile.owner != Main.myPlayer || !(projectile.ai[0] >= 180f))
		//        return;

		//    if (projectile.ai[1] > -1f)
		//    {
		//        projectile.velocity.X *= 0.35f;
		//        if (projectile.Center.X < Main.player[(int)projectile.ai[1]].Center.X)
		//            projectile.velocity.X = Math.Abs(projectile.velocity.X);
		//        else
		//            projectile.velocity.X = 0f - Math.Abs(projectile.velocity.X);
		//    }

		//    Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y, 12, 1000, 10f, Main.myPlayer);
		//    projectile.Kill();
		//}
	}

	[Content(ContentType.PlaceablesFunctional)]
	public class StarRodModPlayer : AssPlayerBase
	{
		public bool starRodTileNearby = false; //Reset in ModSystem, clientside

		public bool starRodTileWorking = false; //All-side, set by the buff

		public override void ResetEffects()
		{
			starRodTileWorking = false;
		}

		public override void PreUpdateBuffs()
		{
			if (starRodTileNearby)
			{
				Player.AddBuff(ModContent.BuffType<StarRodBuff>(), 2, quiet: false);
			}
		}
	}
}
