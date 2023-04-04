using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[Content(ContentType.Bosses)]
	public class BoneCleavingFang : WeaponItemBase
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bone-Cleaving Fang");
			Tooltip.SetDefault("Striking enemies will charge the sword, causing it to glow with power." +
				"\nRelease the built up power by right-clicking. The brighter the sword, the greater the power." +
				"\nThe sword's power fades when not used.");
		}

		public override void SetDefaults()
		{
			//Stats stonger than Muramasa
			Item.CloneDefaults(ItemID.Muramasa);
			Item.damage = 28;
			Item.knockBack = 3;
			Item.width = 40;
			Item.height = 56;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.rare = 2;
			Item.useTurn = false;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 0, 57, 50); //5 silver for souls, 50 for leather, 2.5 silver for bone
			Item.shoot = ModContent.ProjectileType<BoneCleavingFangProj>();
			Item.shootSpeed = 12f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.altFunctionUse != 2 || player.GetModPlayer<BoneClearingFangPlayer>().AbilityAvailable();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				BoneClearingFangPlayer modPlayer = player.GetModPlayer<BoneClearingFangPlayer>();
				if (modPlayer.TryGetPower(out float power))
				{
					Projectile.NewProjectile(source, player.Center + Vector2.Normalize(velocity) * 7f, velocity, type, (int)(damage * power), knockback, Main.myPlayer, modPlayer.TimerRatio);
					modPlayer.ResetTimer();
				}
			}
			return false;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (target.CanBeChasedBy())
			{
				player.GetModPlayer<BoneClearingFangPlayer>().AddTimer();
			}
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (player.altFunctionUse == 2)
			{
				return;
			}

			BoneClearingFangPlayer modPlayer = player.GetModPlayer<BoneClearingFangPlayer>();
			if (modPlayer.AbilityAvailable() && modPlayer.PlayedAbilityMaxedFX)
			{
				if (modPlayer.TimerRatio >= 0.9f)
				{
					Vector2 dir = Vector2.UnitX * player.direction;
					for (int i = 0; i < 2; i++)
					{
						Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, 135, 0f, 0f, 150);
						dust.noGravity = true;
						dust.velocity *= 1.2f;
						dust.velocity += 2.5f * dir;
						dust.scale = 1.3f;
					}
				}
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bone, 25).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 10).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddTile(TileID.Anvils).Register();
		}
	}

	[Content(ContentType.Bosses)]
	public class BoneClearingFangPlayer : AssPlayerBase
	{
		public const int TimerMax = 16 * 60;

		//no sync so other players wont see it
		public float Timer { get; private set; } = 0;

		public bool PlayedAbilityAvailableFX { get; private set; }

		public bool PlayedAbilityMaxedFX { get; private set; }

		public float TimerRatio => Timer / TimerMax;

		public static bool HoldingItem(Item item) => item.type == ModContent.ItemType<BoneCleavingFang>();

		public void AddTimer()
		{
			Timer += 60; //Each hit adds 1 second worth of cooldown
			if (Timer >= TimerMax)
			{
				Timer = TimerMax;
			}
		}

		public void ResetTimer()
		{
			Timer = 0;
			ResetFX();
		}

		public void ResetFX()
		{
			PlayedAbilityAvailableFX = false;
			PlayedAbilityMaxedFX = false;
		}

		/// <summary>
		/// When the ability can be used/harvested
		/// </summary>
		public bool AbilityAvailable()
		{
			return Timer >= 4 * 60;
		}

		public bool TryGetPower(out float power)
		{
			power = 0f;

			if (AbilityAvailable())
			{
				float maxPower = 8f;
				float floor = 0.5f;
				power = 1f + floor + (maxPower - floor) * TimerRatio;
			}

			return power > 0f;
		}

		public override void PreUpdate()
		{
			if (Main.myPlayer != Player.whoAmI)
			{
				return;
			}

			if (HoldingItem(Player.HeldItem) && Timer > 0)
			{
				if (!PlayedAbilityAvailableFX && AbilityAvailable())
				{
					PlayedAbilityAvailableFX = true;
					SoundEngine.PlaySound(SoundID.MaxMana.WithVolumeScale(0.8f), Player.Center);
					for (int i = 0; i < 20; i++)
					{
						Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 135, 0f, 0f, 150);
						dust.noGravity = true;
						dust.velocity *= 1.5f;
						dust.scale = 1.1f;
					}
				}

				if (!PlayedAbilityMaxedFX && TimerRatio >= 1f)
				{
					PlayedAbilityMaxedFX = true;
					SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
					for (int i = 0; i < 20; i++)
					{
						Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 135, 0f, 0f, 150);
						dust.noGravity = true;
						dust.velocity *= 1.5f;
						dust.scale = 1.5f;
					}
				}
			}

			if (Timer > 0)
			{
				Timer -= 0.33f; //Decrease only 33% speed
				if (Timer <= 0)
				{
					ResetTimer();
				}
			}
		}
	}
}
