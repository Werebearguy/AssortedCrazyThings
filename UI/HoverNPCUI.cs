using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.NPCs.Harvester;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace AssortedCrazyThings.UI
{
	class HoverNPCUI : UIState
	{
		internal static bool visible = false;
		internal static string drawString = "";
		internal static Color drawColor = Color.White;

		public static LocalizedText CatchWithNetText { get; private set; }

		public HoverNPCUI()
		{
			string category = $"UI.{nameof(HoverNPCUI)}";
			CatchWithNetText ??= AssUtils.Instance.GetLocalization($"{category}CatchWithNet");
		}

		private string AlmostVanillaBehavior()
		{
			//works on dummies for some reason
			Player player = Main.LocalPlayer;
			string ret = "";
			Rectangle rectangle = new Rectangle((int)((float)Main.mouseX + Main.screenPosition.X), (int)((float)Main.mouseY + Main.screenPosition.Y), 1, 1);
			if (player.gravDir == -1f)
			{
				rectangle.Y = (int)Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
			}

			float num = Main.mouseTextColor / 255f;

			for (int k = 0; k < Main.maxNPCs; k++)
			{
				//LoadNPC(Main.npc[k].type); //idk why
				if (Main.npc[k].active)
				{
					Rectangle npcrect = new Rectangle((int)Main.npc[k].Bottom.X - Main.npc[k].frame.Width / 2, (int)Main.npc[k].Bottom.Y - Main.npc[k].frame.Height, Main.npc[k].frame.Width, Main.npc[k].frame.Height);
					if (Main.npc[k].type >= NPCID.WyvernHead && Main.npc[k].type <= NPCID.WyvernTail) //Wyvern
					{
						npcrect = new Rectangle((int)((double)Main.npc[k].position.X + (double)Main.npc[k].width * 0.5 - 32.0), (int)((double)Main.npc[k].position.Y + (double)Main.npc[k].height * 0.5 - 32.0), 64, 64);
					}
					if (rectangle.Intersects(npcrect)) //mouse cursor inside hitbox
					{
						drawColor = new Color((byte)(255 * num), (byte)(255 * num), (byte)(255 * num), Main.mouseTextColor);
						player.cursorItemIconEnabled = false;
						ret = Main.npc[k].GivenOrTypeName;
						int num2 = k;
						if (Main.npc[k].realLife >= 0)
						{
							num2 = Main.npc[k].realLife;
						}
						if (Main.npc[num2].lifeMax > 1 && !Main.npc[num2].dontTakeDamage)
						{
							ret = ret + ": " + Main.npc[num2].life + "/" + Main.npc[num2].lifeMax;
						}
						break;
					}
				}
			}
			return ret;
		}

		private string Custom()
		{
			Player player = Main.LocalPlayer;
			string ret = "";
			Rectangle rectangle = new Rectangle((int)((float)Main.mouseX + Main.screenPosition.X), (int)((float)Main.mouseY + Main.screenPosition.Y), 1, 1);
			if (player.gravDir == -1f)
			{
				rectangle.Y = (int)Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
			}

			float num = Main.mouseTextColor / 255f;

			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC npc = Main.npc[k];
				if (npc.active)
				{
					Rectangle npcrect = new Rectangle((int)npc.Bottom.X - npc.frame.Width / 2, (int)npc.Bottom.Y - npc.frame.Height, npc.frame.Width, npc.frame.Height);
					if (rectangle.Intersects(npcrect)) //mouse cursor inside hitbox
					{
						drawColor = new Color((byte)(35 * num), (byte)(200f * num), (byte)(254f * num), Main.mouseTextColor);

						if (ContentConfig.Instance.Bosses)
						{
							if (npc.type == ModContent.NPCType<DungeonSoul>() ||
							   npc.type == ModContent.NPCType<DungeonSoulFreed>())
							{
								ret = CatchWithNetText.ToString();
							}
						}

						if (ContentConfig.Instance.HostileNPCs)
						{
							if (npc.type == ModContent.NPCType<ChunkysEye>() || npc.type == ModContent.NPCType<MeatballsEye>())
							{
								drawColor = Color.White * num;
								ret = CatchWithNetText.ToString();
							}
						}

						break;
					}
				}
			}
			return ret;
		}

		//Update
		public override void Update(GameTime gameTime)
		{
			//if (!visible) return;
			if (Main.hoverItemName != "") return;
			base.Update(gameTime);

			drawString = "";

			int lastMouseXbak = Main.lastMouseX;
			int lastMouseYbak = Main.lastMouseY;
			int mouseXbak = Main.mouseX;
			int mouseYbak = Main.mouseY;
			int lastscreenWidthbak = Main.screenWidth;
			int lastscreenHeightbak = Main.screenHeight;

			PlayerInput.SetZoom_Unscaled();
			PlayerInput.SetZoom_MouseInWorld();

			//do stuff
			//drawString = AlmostVanillaBehavior();
			drawString = Custom();

			Main.lastMouseX = lastMouseXbak;
			Main.lastMouseY = lastMouseYbak;
			Main.mouseX = mouseXbak;
			Main.mouseY = mouseYbak;
			Main.screenWidth = lastscreenWidthbak;
			Main.screenHeight = lastscreenHeightbak;
		}

		//Draw
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//if (!visible) return;
			if (Main.hoverItemName != "") return;
			base.DrawSelf(spriteBatch);

			if (drawString != "") Main.LocalPlayer.cursorItemIconEnabled = false;
			Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);
			mousePos.X += 10;
			mousePos.Y += 10;
			if (Main.ThickMouse)
			{
				mousePos.X += 6;
				mousePos.Y += 6;
			}

			DynamicSpriteFont font = FontAssets.MouseText.Value;
			Vector2 vector = font.MeasureString(drawString);

			if (mousePos.X + vector.X + 4f > Main.screenWidth)
			{
				mousePos.X = (int)((float)Main.screenWidth - vector.X - 4f);
			}
			if (mousePos.Y + vector.Y + 4f > Main.screenHeight)
			{
				mousePos.Y = (int)(Main.screenHeight - vector.Y - 4f);
			}

			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, drawString, mousePos + new Vector2(2, 24), drawColor, 0, Vector2.Zero, Vector2.One);
		}
	}
}
