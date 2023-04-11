using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[Content(ContentType.Bosses)]
	[AutoloadEquip(EquipType.Wings)] //Dummy texture, non-4 frame wings need custom drawing
	public class HarvesterWings : AccessoryBase
	{
		//Mirror of wing type 43, item ID 4754 (Grox The Great's Wings)
		public const int NumFrames = 7;

		public override void SafeSetStaticDefaults()
		{
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(95, 6.5f, 1.35f);
		}

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = 5;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.maxMinions++;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.25f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 0.4f;
			maxAscentMultiplier = 2f;
			constantAscend = 0.12f;
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			if (inUse || player.jump > 0)
			{
				int endFrame = 6 + 1;
				int startFrame = 1;
				int frameSpeed = 3;
				player.wingFrameCounter++;
				if (player.wingFrameCounter > frameSpeed)
				{
					player.wingFrame++;
					player.wingFrameCounter = 0;
					if (player.wingFrame >= endFrame)
					{
						player.wingFrame = startFrame;
					}
				}
			}
			else if (player.velocity.Y != 0f)
			{
				player.wingFrame = 1;
			}
			else
			{
				player.wingFrame = 0;
			}

			if (inUse)
			{
				bool isFlapFrame = player.wingFrame == 4;

				if (isFlapFrame)
				{
					if (!player.flapSound)
					{
						SoundEngine.PlaySound(SoundID.Item32, player.position);
					}

					player.flapSound = true;
				}
				else
				{
					player.flapSound = false;
				}
			}

			if (player.velocity.Y != 0 && player.wingFrame != 0)
			{
				if (Main.rand.NextBool(3))
				{
					int dustOffsetX = -16 - player.direction * 30;
					int dustOffsetY = -16;
					int dustIndex = Dust.NewDust(new Vector2(player.Center.X + dustOffsetX, player.Center.Y + dustOffsetY), 40, 30, 135, 0f, 0f, 0, default(Color), 1.5f);
					Dust dust = Main.dust[dustIndex];
					dust.noGravity = true;
					dust.noLight = true;
					dust.velocity *= 0.3f;
					if (Main.rand.NextBool(5))
					{
						dust.fadeIn = 1f;
					}
					dust.shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bone, 25).AddIngredient(ItemID.SoulofFlight, 10).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 10).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 2).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
