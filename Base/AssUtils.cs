﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base
{
	public static class AssUtils
	{
		/// <summary>
		/// The instance of the mod
		/// </summary>
		public static AssortedCrazyThings Instance => ModContent.GetInstance<AssortedCrazyThings>(); //just shorter writing AssUtils.Instance than AssortedCrazyThings.Instance

		/// <summary>
		/// Types of modded NPCs which names are ending with Body or Tail
		/// </summary>
		public static int[] isModdedWormBodyOrTail;

		public static void Print(object o)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				Console.WriteLine(o.ToString());
			}

			if (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(o.ToString());
			}
		}

		public static void UIText(string str, Color color)
		{
			CombatText.NewText(Main.LocalPlayer.getRect(), color, str);
		}

		public static Dust QuickDust(int dustType, Vector2 pos, Color color, Vector2 dustVelo = default(Vector2), int alpha = 0, float scale = 1f)
		{
			Dust dust = Dust.NewDustPerfect(pos, dustType, dustVelo, alpha, color, scale);
			dust.position = pos;
			dust.velocity = dustVelo;
			dust.fadeIn = 1f;
			dust.noLight = false;
			dust.noGravity = true;
			return dust;
		}

		public static void QuickDustLine(int dustType, Vector2 start, Vector2 end, float splits, Color color = default(Color), Vector2 dustVelo = default(Vector2), int alpha = 0, float scale = 1f)
		{
			QuickDust(dustType, start, color, dustVelo);
			float num = 1f / splits;
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				QuickDust(dustType, Vector2.Lerp(start, end, num2), color, dustVelo, alpha, scale);
			}
		}

		/// <summary>
		/// Something similar to Dust.QuickDust
		/// </summary>
		public static Dust DrawDustAtPos(Vector2 pos, int dustType = 169)
		{
			//used for showing a position as a dust for debugging
			Dust dust = QuickDust(dustType, pos, Color.White);
			dust.noGravity = true;
			dust.noLight = true;
			return dust;
		}

		public record struct FlamethrowerColors(Color FadeIn, Color Bright, Color Main, Color FadeOut);

		/// <summary>
		/// Draws the flamethrower projectile. Uses hardcoded values. Copied straight from vanilla DrawProj_Flamethrower
		/// </summary>
		public static void DrawFlamethrowerProjectile(Projectile proj, FlamethrowerColors colors)
		{
			bool unusedFlag = false && proj.ai[0] == 1f;
			float timer = proj.localAI[0];
			float startSlowdown = 60f;
			float afterSlowdown = 12f;
			float lifetime = startSlowdown + afterSlowdown;
			Texture2D value = TextureAssets.Projectile[proj.type].Value;
			//Original values, dividing G and B by 2 afterwards
			//Color firstColor = new Color(255, 80, 20, 200);
			//Color secondColor = new Color(255, 255, 20, 70);
			//Color thirdColor = Color.Lerp(new Color(255, 80, 20, 100), color2, 0.25f);
			//Color finalColor = new Color(80, 80, 80, 100);
			Color firstColor = colors.FadeIn;
			Color secondColor = colors.Bright;
			Color thirdColor = Color.Lerp(colors.Main, secondColor, 0.25f);
			Color finalColor = colors.FadeOut;
			float firstToSecondSection = 0.35f;
			float secondToThirdSection = 0.7f;
			float thirdToFinalSection = 0.85f;
			float step = (timer > startSlowdown - 10f) ? 0.175f : 0.2f;
			if (unusedFlag)
			{
				firstColor = new Color(95, 120, 255, 200);
				secondColor = new Color(50, 180, 255, 70);
				thirdColor = new Color(95, 160, 255, 100);
				finalColor = new Color(33, 125, 202, 100);
			}

			int verticalFrames = 7;
			float afterSlowdownLerp = Utils.Remap(timer, startSlowdown, lifetime, 1f, 0f);
			float velocityMult = Math.Min(timer, 20f);
			float timerLerp = Utils.Remap(timer, 0f, lifetime, 0f, 1f);
			float scaleLerp = Utils.Remap(timerLerp, 0.2f, 0.5f, 0.25f, 1f);
			Rectangle rectangle = (!unusedFlag) ? value.Frame(1, verticalFrames, 0, 3) : value.Frame(1, verticalFrames, 0, (int)Utils.Remap(timerLerp, 0.5f, 1f, 3f, 5f));
			if (timerLerp >= 1f)
			{
				return;
			}

			for (int i = 0; i < 2; i++)
			{
				for (float j = 1f; j >= 0f; j -= step)
				{
					Color transparent = (timerLerp < 0.1f) ? Color.Lerp(Color.Transparent, firstColor, Utils.GetLerpValue(0f, 0.1f, timerLerp, clamped: true)) :
						((timerLerp < 0.2f) ? Color.Lerp(firstColor, secondColor, Utils.GetLerpValue(0.1f, 0.2f, timerLerp, clamped: true)) :
						((timerLerp < firstToSecondSection) ? secondColor :
						((timerLerp < secondToThirdSection) ? Color.Lerp(secondColor, thirdColor, Utils.GetLerpValue(firstToSecondSection, secondToThirdSection, timerLerp, clamped: true)) :
						((timerLerp < thirdToFinalSection) ? Color.Lerp(thirdColor, finalColor, Utils.GetLerpValue(secondToThirdSection, thirdToFinalSection, timerLerp, clamped: true)) :
						((!(timerLerp < 1f)) ? Color.Transparent :
						Color.Lerp(finalColor, Color.Transparent, Utils.GetLerpValue(thirdToFinalSection, 1f, timerLerp, clamped: true)))))));
					float num14 = (1f - j) * Utils.Remap(timerLerp, 0f, 0.2f, 0f, 1f);
					Vector2 vector = proj.Center - Main.screenPosition + proj.velocity * -velocityMult * j;
					Color color5 = transparent * num14;
					Color color6 = color5;
					if (!unusedFlag)
					{
						//Colors now supplied raw, instead of specific to red flames
						//color6.G /= 2;
						//color6.B /= 2;
						color6.A = (byte)Math.Min(color5.A + 80f * num14, 255f);
						//Utils.Remap(timer, 20f, fromMax, 0f, 1f);
					}

					float num15 = 1f / step * (j + 1f);
					float num16 = proj.rotation + j * MathHelper.PiOver2 + Main.GlobalTimeWrappedHourly * num15 * 2f;
					float num17 = proj.rotation - j * MathHelper.PiOver2 - Main.GlobalTimeWrappedHourly * num15 * 2f;
					switch (i)
					{
						case 0:
							Main.EntitySpriteDraw(value, vector + proj.velocity * -velocityMult * step * 0.5f, rectangle, color6 * afterSlowdownLerp * 0.25f, num16 + MathHelper.PiOver4, rectangle.Size() / 2f, scaleLerp, SpriteEffects.None);
							Main.EntitySpriteDraw(value, vector, rectangle, color6 * afterSlowdownLerp, num17, rectangle.Size() / 2f, scaleLerp, SpriteEffects.None);
							break;
						case 1:
							if (!unusedFlag)
							{
								Main.EntitySpriteDraw(value, vector + proj.velocity * -velocityMult * step * 0.2f, rectangle, color5 * afterSlowdownLerp * 0.25f, num16 + MathHelper.PiOver2, rectangle.Size() / 2f, scaleLerp * 0.75f, SpriteEffects.None);
								Main.EntitySpriteDraw(value, vector, rectangle, color5 * afterSlowdownLerp, num17 + MathHelper.PiOver2, rectangle.Size() / 2f, scaleLerp * 0.75f, SpriteEffects.None);
							}
							break;
					}
				}
			}
		}

		public static void DrawSkeletronLikeArms(string texString, Vector2 selfPos, Vector2 centerPos, float selfPad = 0f, float centerPad = 0f, float direction = 0f)
		{
			DrawSkeletronLikeArms(ModContent.Request<Texture2D>(texString).Value, selfPos, centerPos, selfPad, centerPad, direction);
		}

		/// <summary>
		/// Draws two "arms" originating from selfPos, "attached" at centerPos
		/// </summary>
		public static void DrawSkeletronLikeArms(Texture2D tex, Vector2 selfPos, Vector2 centerPos, float selfPad = 0f, float centerPad = 0f, float direction = 0f)
		{
			//with all float params = 0f, the arm will originate below the selfPos
			//Pos parameters should be Entity.Center
			//Pad parameters are actually just y offsets
			//direction determines in what direction the elbow bends and by how much (-1 to 1 are preferred)
			//if (tex == null) tex = Main.boneArmTexture;
			Vector2 drawPos = selfPos;
			drawPos += new Vector2(-5f * direction, selfPad);
			centerPos.Y += -tex.Height / 2 + centerPad;
			for (int i = 0; i < 2; i++)
			{
				float x = centerPos.X - drawPos.X;
				float y = centerPos.Y - drawPos.Y;
				float magnitude;
				if (i == 0) //first arm piece starting at selfPos
				{
					x += -(100 + tex.Height) * direction;
					y += 100 + tex.Width;
					magnitude = (float)Math.Sqrt(x * x + y * y);
					magnitude = tex.Height / 2 / magnitude;
					drawPos.X += x * magnitude;
					drawPos.Y += y * magnitude;
				}
				else //second arm piece
				{
					x += -(30 + tex.Width / 2) * direction;
					y += 30 + tex.Height / 2;
					magnitude = (float)Math.Sqrt(x * x + y * y);
					magnitude = tex.Height / 2 / magnitude;
					drawPos.X += x * magnitude;
					drawPos.Y += y * magnitude;
				}
				float rotation = (float)Math.Atan2(y, x) - 1.57f;
				Color color = Lighting.GetColor((int)drawPos.X / 16, (int)(drawPos.Y / 16f));
				Main.spriteBatch.Draw(tex, new Vector2(drawPos.X - Main.screenPosition.X, drawPos.Y - Main.screenPosition.Y), tex.Bounds, color, rotation, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
				if (i == 0)
				{
					//padding for the second arm piece
					drawPos.X += x * magnitude * 1.1f;
					drawPos.Y += y * magnitude * 1.1f;
				}
				else if (Main.instance.IsActive) //not sure what this part does
				{
					drawPos.X += x * magnitude - 16f;
					drawPos.Y += y * magnitude - 6f;
				}
			}
		}

		public static void DrawTether(string texString, Vector2 start, Vector2 end)
		{
			DrawTether(ModContent.Request<Texture2D>(texString).Value, start, end);
		}

		//Credit to IDGCaptainRussia
		/// <summary>
		/// Draws a "connection" between two points
		/// </summary>
		public static void DrawTether(Texture2D tex, Vector2 start, Vector2 end)
		{
			Vector2 position = start;
			Vector2 mountedCenter = end;
			float num1 = tex.Height;
			Vector2 vector2_4 = mountedCenter - position;
			Vector2 vector2_4tt = mountedCenter - position;
			float keepgoing = vector2_4tt.Length();
			Vector2 vector2t = vector2_4;
			vector2t.Normalize();
			position -= vector2t * (num1 * 0.5f);

			float rotation = vector2_4.ToRotation() - 1.57f;
			bool flag = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
				flag = false;
			if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
				flag = false;
			while (flag)
			{
				if (keepgoing <= -1)
				{
					flag = false;
				}
				else
				{
					Vector2 vector2_1 = vector2_4;
					vector2_1.Normalize();
					position += vector2_1 * num1;
					keepgoing -= num1;
					vector2_4 = mountedCenter - position;
					Color color2 = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
					color2 = new Color(color2.R, color2.G, color2.B, 255);
					Main.spriteBatch.Draw(tex, position - Main.screenPosition, new Rectangle(0, 0, tex.Width, (int)Math.Min(num1, num1 + keepgoing)), color2, rotation, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0.0f);
				}
			}
		}

		/// <summary>
		/// Useful when utilizing Projectile.scale
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="lightColor"></param>
		public static void DrawAroundOrigin(Projectile projectile, Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
			Rectangle bounds = Utils.Frame(texture, 1, Main.projFrames[projectile.type], 0, projectile.frame);
			Vector2 origin = new Vector2(bounds.Width * 0.5f, bounds.Height * 0.5f);
			SpriteEffects effect = projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, bounds, projectile.GetAlpha(lightColor), projectile.rotation, origin, projectile.scale, effect, 0);
		}

		/// <summary>
		/// Draws a projectile like god (red) intended. Mimics default drawing from the base game.
		/// <br/>While this works for most projectiles, things using <see cref="ProjAIStyleID.Drill"/> or <see cref="ProjAIStyleID.Spear"/> draw differently.
		/// </summary>
		public static void DrawLikeVanilla(Projectile projectile, Color color, Texture2D texture = null, Vector2 offset = default, Rectangle? frame = null, Vector2 originOffset = default, float rotationOffset = 0f, float scaleOffset = 0f)
		{
			Texture2D realTexture = texture ?? TextureAssets.Projectile[projectile.type].Value;

			SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			int offsetX = 0;
			int offsetY = 0;
			float originX = (realTexture.Width - projectile.width) * 0.5f + projectile.width * 0.5f;
			ProjectileLoader.DrawOffset(projectile, ref offsetX, ref offsetY, ref originX);
			float x = projectile.position.X - Main.screenPosition.X + originX + offsetX;
			float y = projectile.position.Y - Main.screenPosition.Y + projectile.height / 2 + projectile.gfxOffY;
			Rectangle realFrame = frame ?? realTexture.Frame(1, Main.projFrames[projectile.type], frameY: projectile.frame);
			float rotation = projectile.rotation + rotationOffset;
			Vector2 origin = new Vector2(originX, projectile.height / 2 + offsetY);
			float scale = projectile.scale + scaleOffset;
			Main.EntitySpriteDraw(realTexture, new Vector2(x, y) + offset, realFrame, color, rotation, origin + originOffset, scale, effects, 0);
		}

		/// <summary>
		/// Combines two arrays (first + second in order)
		/// </summary>
		public static T[] ConcatArray<T>(T[] first, T[] second)
		{
			T[] combined = new T[first.Length + second.Length];
			Array.Copy(first, combined, first.Length);
			Array.Copy(second, 0, combined, first.Length, second.Length);
			return combined;
		}

		/// <summary>
		/// Fills an array with a default value.
		/// If array is null, creates one with the length specified.
		/// Else, overrides each element with default value
		/// </summary>
		public static void FillWithDefault<T>(ref T[] array, T def, int length = -1)
		{
			if (array == null)
			{
				if (length == -1)
					throw new ArgumentOutOfRangeException("Array is null but length isn't specified");
				array = new T[length];
			}
			else
			{
				length = array.Length;
			}

			for (int i = 0; i < length; i++)
			{
				array[i] = def;
			}
		}

		/// <summary>
		/// Fills a list with a default value.
		/// If array is null, creates one with the length specified.
		/// Else, overrides each element with default value
		/// </summary>
		public static void FillWithDefault<T>(ref List<T> list, T def, int length = -1)
		{
			if (list == null)
			{
				if (length == -1)
					throw new ArgumentOutOfRangeException("List is null but length isn't specified");
				list = new List<T>(length);
			}
			else
			{
				length = list.Count;
			}

			for (int i = 0; i < length; i++)
			{
				list.Add(def);
			}
		}

		/// <summary>
		/// Like NPC.AnyNPC, but checks for each type in the passed array.
		/// If one exists, returns true
		/// </summary>
		public static bool AnyNPCs(int[] types)
		{
			//Like AnyNPCs but checks for an array
			for (int i = 0; i < types.Length; i++)
			{
				if (NPC.AnyNPCs(types[i])) return true;
			}
			return false;
		}

		/// <summary>
		/// Like NPC.AnyNPC, but checks for each type in the passed list.
		/// If one exists, returns true
		/// </summary>
		public static bool AnyNPCs(List<int> types)
		{
			return AnyNPCs(types.ToArray());
		}

		/// <summary>
		/// Like NPC.AnyNPC, but checks for custom condition (active already true).
		/// If one exists, returns true
		/// </summary>
		public static bool AnyNPCs(Func<NPC, bool> condition)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && condition(Main.npc[i])) return true;
			}
			return false;
		}

		/// <summary>
		/// Counts all NPCs in the passed array
		/// </summary>
		public static int CountAllNPCs(int[] types)
		{
			int count = 0;
			for (int i = 0; i < types.Length; i++)
			{
				count += NPC.CountNPCS(types[i]);
			}
			return count;
		}

		/// <summary>
		/// Counts all active projectiles of the given type, and of a given owner if specified
		/// </summary>
		public static int CountProjs(int type, int owner = -1)
		{
			int count = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.type == type &&
					(owner < 0 || proj.owner == owner))
				{
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// Checks if given NPC is a worm body or tail
		/// </summary>
		public static bool IsWormBodyOrTail(NPC npc)
		{
			return npc.dontCountMe || Array.BinarySearch(isModdedWormBodyOrTail, npc.type) >= 0 || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsBody/* || npc.realLife != -1*/;
		}

		/// <summary>
		/// Checks if player is in an evil biome (any of three)
		/// </summary>
		public static bool EvilBiome(Player player)
		{
			return player.ZoneCorrupt || player.ZoneCrimson || player.ZoneHallow;
		}

		/// <summary>
		/// Formats Main.time into a string representation with AM/PM
		/// </summary>
		public static string GetTimeAsString(bool accurate = true)
		{
			string suffix = "AM";
			double doubletime = Main.time;
			if (!Main.dayTime)
			{
				doubletime += 54000.0;
			}
			doubletime = doubletime / 86400.0 * 24.0;
			double wtf = 7.5;
			doubletime = doubletime - wtf - 12.0;
			if (doubletime < 0.0)
			{
				doubletime += 24.0;
			}
			if (doubletime >= 12.0)
			{
				suffix = "PM";
			}
			int hours = (int)doubletime;
			double doubleminutes = doubletime - hours;
			doubleminutes = (int)(doubleminutes * 60.0);
			string minutes = string.Concat(doubleminutes);
			if (doubleminutes < 10.0)
			{
				minutes = "0" + minutes;
			}
			if (hours > 12)
			{
				hours -= 12;
			}
			if (hours == 0)
			{
				hours = 12;
			}
			if (!accurate) minutes = (!(doubleminutes < 30.0)) ? "30" : "00";
			return Language.GetTextValue("Game.Time", hours + ":" + minutes + " " + suffix);
		}

		public static string GetMoonPhaseAsString(bool showNumber = false)
		{
			string suffix = "";
			if (showNumber) suffix = " (" + (Main.moonPhase + 1) + ")";
			string prefix = Lang.inter[102].Value + AssUISystem.GetColon(); //can't seem to find "Moon Phase" in the lang files for GameUI
			string value = "";
			string check = "";
			switch (Main.moonPhase)
			{
				case 0:
					check = "FullMoon";
					break;
				case 1:
					check = "WaningGibbous";
					break;
				case 2:
					check = "ThirdQuarter";
					break;
				case 3:
					check = "WaningCrescent";
					break;
				case 4:
					check = "NewMoon";
					break;
				case 5:
					check = "WaxingCrescent";
					break;
				case 6:
					check = "FirstQuarter";
					break;
				case 7:
					check = "WaxingGibbous";
					break;
				default:
					break;
			}
			value = Language.GetTextValue("GameUI." + check);
			if (value != "") return prefix + value + suffix;
			return "";
		}

		/// <summary>
		/// If you need immediate sync, usually necessary outside of Projectile.AI (as netUpdate is set to false before it's invoked)
		/// </summary>
		/// <param name="projectile"></param>
		public static void NetSync(this Projectile projectile)
		{
			if (Main.myPlayer == projectile.owner)
			{
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI);
				projectile.netSpam = 0;
				projectile.netUpdate = false;
				projectile.netUpdate2 = false;
			}
		}

		/// <summary>
		/// Gets a projectile given its owner ID, its identity ID, and its type ID.
		/// </summary>
		/// <param name="owner">The owner to check</param>
		/// <param name="identity">The identity to check</param>
		/// <param name="type">The type to check</param>
		/// <param name="index">The index ("whoAmI") of the found projectile. Main.maxProjectiles if returning null.</param>
		/// <returns>Returns null if not found.</returns>
		public static Projectile NetGetProjectile(int owner, int identity, int type, out int index)
		{
			for (short i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.owner == owner && proj.identity == identity && proj.type == type)
				{
					index = i;
					return proj;
				}
			}
			index = Main.maxProjectiles;
			return null;
		}

		/// <inheritdoc cref="NetGetProjectile(int, int, int, out int)"/>
		/// <param name="types">The types to check</param>
		public static Projectile NetGetProjectile(int owner, int identity, int[] types, out int index)
		{
			for (short i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.owner == owner && proj.identity == identity && Array.IndexOf(types, proj.type) > -1)
				{
					index = i;
					return proj;
				}
			}
			index = Main.maxProjectiles;
			return null;
		}

		/// <summary>
		/// Alternative version of <see cref="CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0"/>. Checks the condition delegate PER-PLAYER before syncing/spawning the item.
		/// </summary>
		public static void DropItemInstanced(DropAttemptInfo info, int itemType, int itemStack = 1, Func<DropAttemptInfo, bool> condition = null, bool interactionRequired = true)
		{
			if (itemType <= 0)
			{
				return;
			}

			NPC npc = info.npc;
			if (Main.netMode == NetmodeID.Server)
			{
				var origInfo = info;
				int item = Item.NewItem(npc.GetSource_Loot(), npc.getRect(), itemType, itemStack, true);
				Main.timeItemSlotCannotBeReusedFor[item] = 54000;
				for (int p = 0; p < Main.maxPlayers; p++)
				{
					if (Main.player[p].active && (npc.playerInteraction[p] || !interactionRequired))
					{
						//Manually switch the player instance
						info = origInfo;
						info.player = Main.player[p];

						if (condition?.Invoke(info) ?? true)
						{
							NetMessage.SendData(MessageID.InstancedItem, p, -1, null, item);
						}
					}
				}
				Main.item[item].active = false;
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
			{
				if (condition?.Invoke(info) ?? true)
				{
					CommonCode.DropItem(info, itemType, itemStack);
				}
			}
		}

		public static LocalizedText GetDropConditionDescription(string name)
		{
			return Instance.GetLocalization($"DropConditions.{name}.Description");
		}

		/// <summary>
		/// Draws the _Highlight texture of this tile if it exists and can be drawn
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="type"></param>
		/// <param name="color">Color the tile is drawn with</param>
		/// <param name="pos"></param>
		/// <param name="frame"></param>
		public static void DrawTileHighlight(SpriteBatch spriteBatch, int i, int j, int type, Color color, Vector2 pos, Rectangle frame)
		{
			if (TileID.Sets.HasOutlines[type] && Collision.InTileBounds(i, j, Main.TileInteractionLX, Main.TileInteractionLY, Main.TileInteractionHX, Main.TileInteractionHY) && Main.SmartInteractTileCoords.Contains(new Point(i, j)))
			{
				int average = (int)color.GetAverage();
				bool selected = false;
				if (Main.SmartInteractTileCoordsSelected.Contains(new Point(i, j)))
				{
					selected = true;
				}
				if (average > 10)
				{
					Texture2D outlineTexture = TextureAssets.HighlightMask[type].Value;
					Color outlineColor;
					if (selected)
					{
						outlineColor = new Color(average, average, average / 3, average);
					}
					else
					{
						outlineColor = new Color(average / 2, average / 2, average / 2, average);
					}

					if (outlineTexture != null)
					{
						spriteBatch.Draw(outlineTexture, pos, frame, outlineColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		/// <summary>
		/// Modify the velocity (that has the direction from position to targetPos) of something that is affected by gravity in a way that it will still reach targetPos
		/// <br>Note: This might increase the length of velocity depending on how much correction was needed, use offsetCap to limit it.</br>
		/// </summary>
		/// <param name="position">Starting location</param>
		/// <param name="targetPos">Target location</param>
		/// <param name="gravity">Gravity applied to velocity.Y</param>
		/// <param name="velocity">Starting velocity</param>
		/// <param name="ticksWithoutGravity">Amount of initial ticks that velocity should NOT be updated by gravity</param>
		/// <param name="terminalCap">Terminal velocity.Y</param>
		/// <param name="factor">Multiplier for final correction to velocity. 1f == perfect, 0f == none</param>
		public static void ModifyVelocityForGravity(Vector2 position, Vector2 targetPos, in float gravity, ref Vector2 velocity, int ticksWithoutGravity = 0, float terminalCap = 16f, float factor = 1f, float offsetCap = 2.5f)
		{
			//Need to make the velocity + gravity hit targetPos.Y
			//Keep horizontal velocity, correct vertical velocity to account for gravity

			Vector2 toTarget = targetPos - position;
			int ticksToReachX = (int)(toTarget.X / velocity.X); //"Simulated time" it takes to reach target

			float traversedDistanceY = 0;
			float traversedDistanceYNoGravity = 0;
			float velocityYWithGravity = velocity.Y;
			for (int i = 0; i < ticksToReachX; i++)
			{
				if (i >= ticksWithoutGravity)
				{
					velocityYWithGravity += gravity;
					if (velocityYWithGravity > terminalCap)
					{
						velocityYWithGravity = terminalCap;
					}
				}
				traversedDistanceY += velocityYWithGravity;
				traversedDistanceYNoGravity += velocity.Y;
			}

			float offsetY = traversedDistanceY - traversedDistanceYNoGravity;

			float velocityYOffset = offsetY / ticksToReachX;

			velocityYOffset = Math.Min(velocityYOffset, offsetCap);

			velocity.Y -= factor * velocityYOffset;
		}

		/// <summary>
		/// Helper method that returns true if the given parameters allow for a shot from a burst to happen
		/// </summary>
		/// <param name="timer">The timer incrementing by 1</param>
		/// <param name="interval">The total shot interval, including break</param>
		/// <param name="burstDurationRatio">The ratio of the "burst duration" from the interval</param>
		/// <param name="shotsPerBurst">Amount of shots per burst</param>
		/// <returns>Returns true if can shoot</returns>
		public static bool CanShootBurst(int timer, int interval, float burstDurationRatio, int shotsPerBurst)
		{
			//Examples:
			//interval = 60
			//burstDurationRatio = 0.5
			//shotsPerBurst = 3
			//burstDuration = 30
			//timeBetweenShots = 10
			//shots at 10, 20, 30

			//interval = 40
			//burstDurationRatio = 0.5
			//shotsPerBurst = 3
			//burstDuration = 20
			//timeBetweenShots = 6
			//shots at 6, 12, 18 //Due to rounding

			int burstDuration = (int)(burstDurationRatio * interval);
			int timeBetweenShots = burstDuration / shotsPerBurst;

			return timer <= burstDuration && timer >= timeBetweenShots && timer % timeBetweenShots == 0;
		}

		public static bool DontDigUpStartingIsland(NPCSpawnInfo spawnInfo)
		{
			return Main.remixWorld && DontDigUpStartingIslandPosition(spawnInfo.SpawnTileX);
		}

		public static bool DontDigUpStartingIslandPosition(int x)
		{
			return x > Main.maxTilesX * 0.38f + 50 && x < Main.maxTilesX * 0.62f;
		}

		public static bool AboveSurface(NPCSpawnInfo spawnInfo)
		{
			int y = spawnInfo.SpawnTileY;
			bool aboveSurface = y < Main.worldSurface;
			if (Main.remixWorld && y > Main.rockLayer - 20)
			{
				if (y <= Main.maxTilesY - 190 && !Main.rand.NextBool(3))
				{
					aboveSurface = true;
					//This doesnt work because NPC.ResetRemixHax is called before modded chances are evaluated
					//Main.dayTime = false;
					//if (Main.rand.NextBool())
					//{
					//	Main.dayTime = true;
					//}
				}
				else if ((Main.bloodMoon || (Main.eclipse && Main.dayTime)) && DontDigUpStartingIsland(spawnInfo))
				{
					aboveSurface = true;
				}
			}

			return aboveSurface;
		}

		public static bool DontDigUpDaytime(NPCSpawnInfo spawnInfo)
		{
			int y = spawnInfo.SpawnTileY;
			bool day = Main.dayTime;
			if (Main.remixWorld && y > Main.rockLayer - 20)
			{
				if (y <= Main.maxTilesY - 190 && !Main.rand.NextBool(3))
				{
					day = Main.rand.NextBool();
				}
			}

			return day;
		}

		public static bool Underground(NPCSpawnInfo spawnInfo)
		{
			int y = spawnInfo.SpawnTileY;
			bool underground = y > Main.worldSurface && y <= Main.rockLayer;
			if (Main.remixWorld)
				underground = y > Main.rockLayer && y <= Main.maxTilesY - 190;

			return underground;
		}
	}
}
