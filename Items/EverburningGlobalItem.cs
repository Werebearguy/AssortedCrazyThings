using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.Accessories)]
	public class EverburningGlobalItem : AssGlobalItem
	{
		public override bool InstancePerEntity => false;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			EverburningGlobalItem myClone = (EverburningGlobalItem)base.Clone(item, itemClone);
			return myClone;
		}

		public override void UseAnimation(Item item, Player player)
		{
			if (Main.myPlayer != player.whoAmI)
			{
				return;
			}

			//Spawn dust only clientside
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

			if (mPlayer.AnyCandleBuff)
			{
				if (item.damage > 0 && item.noMelee)
				{
					ShootCandleDust(item, mPlayer);
				}
			}
		}

		private void SpawnMeleeDust(int type, Color color, Rectangle hitbox, Player player)
		{
			//6 is the default fire particle type
			Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, type, player.velocity.X * 0.2f + (player.direction * 3), player.velocity.Y * 0.2f, 100, color, 2f);
			dust.noGravity = true;
			dust.velocity.X *= 2f;
			dust.velocity.Y *= 2f;
		}

		private void ShootCandleDust(Item item, AssPlayer mPlayer)
		{
			Player player = mPlayer.Player;
			Vector2 cm = Main.MouseWorld - player.Center;
			float rand = Main.rand.NextFloat(0.7f, 1.3f);
			float len = cm.Length();
			Vector2 velo = cm * item.shootSpeed * rand / len;
			Vector2 pos = new Vector2(player.Center.X, player.Center.Y + 8f);

			//reduce but not prevent spam from rapid fire
			if (player.ownedProjectileCounts[ModContent.ProjectileType<CandleDustDummy>()] < 2)
			{
				Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, velo + player.velocity, ModContent.ProjectileType<CandleDustDummy>(), 0, 0f, player.whoAmI);
			}
		}

		public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
		{
			if (item.damage > 0 && !item.noMelee)
			{
				AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
				if (mPlayer.everburningCandleBuff)
				{
					Color color = new Color(255, 255, 255);
					SpawnMeleeDust(6, color, hitbox, player);
				}
				if (mPlayer.everburningCursedCandleBuff)
				{
					Color color = new Color(255, 255, 255); //(196, 255, 0);
					SpawnMeleeDust(75, color, hitbox, player);
				}
				if (mPlayer.everfrozenCandleBuff)
				{
					Color color = new Color(255, 255, 255);
					SpawnMeleeDust(59, color, hitbox, player);
				}
				if (mPlayer.everburningShadowflameCandleBuff)
				{
					Color color = new Color(196, 0, 255);
					SpawnMeleeDust(62, color, hitbox, player);
				}
				//type 64 is ichor
			}
		}
	}
}
