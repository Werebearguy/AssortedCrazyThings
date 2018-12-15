using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace AssortedCrazyThings.Items
{
	public class AssGlobalItem : GlobalItem
	{
		public AssGlobalItem()
		{

		}

		public override bool InstancePerEntity
		{
			get
			{
				return false;
			}
		}

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			AssGlobalItem myClone = (AssGlobalItem)base.Clone(item, itemClone);
			return myClone;
		}

        public override bool CanUseItem(Item item, Player player)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
            if(mPlayer.everburningCandleBuff || mPlayer.everfrozenCandleBuff || mPlayer.everburningShadowflameCandleBuff || mPlayer.everburningCursedCandleBuff )
            {
                if (base.CanUseItem(item, player) && player.HeldItem.active && player.HeldItem.damage >= 0)
                {
                    if (player.HeldItem.melee)
                    {
                        //TODO do something with auto-fire boomerangs
                        if (item.shoot > 0 && item.shootSpeed > 0)
                        {
                            ShootCandleDust(item, mPlayer);
                        }
                    }

                    else if (player.HeldItem.ranged)
                    {
                        if (player.HasAmmo(item, true))
                        {
                            ShootCandleDust(item, mPlayer);
                        }
                    }

                    else if (player.HeldItem.magic && player.HeldItem.mana <= player.statMana)
                    {
                        ShootCandleDust(item, mPlayer);
                    }

                    else if (player.HeldItem.thrown)
                    {
                        ShootCandleDust(item, mPlayer);
                    }
                }
            }
            //Main.NewText("" + player.selectedItem + " " + player.HeldItem.active + " " + player.HeldItem.damage + " " + player.HeldItem.melee + " " + player.HeldItem.ranged + " " + player.HeldItem.magic + " " + player.HeldItem.thrown);
            return base.CanUseItem(item, player);
        }

        private void SpawnMeleeDust(int type, Color color, Rectangle hitbox, Player player)
        {
            //6 is the default fire particle type
            int dustid = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, type, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, color, 2f);
            Main.dust[dustid].noGravity = true;
            Dust dust2 = Main.dust[dustid];
            dust2.velocity.X = dust2.velocity.X * 2f;
            Dust dust3 = Main.dust[dustid];
            dust3.velocity.Y = dust3.velocity.Y * 2f;
        }

        private void SpawnRangedDust(int type, Color color, float speed, Player player)
        {
            Vector2 cm = new Vector2(Main.MouseWorld.X - player.Center.X, Main.MouseWorld.Y - player.Center.Y);
            for (int k = 0; k < 10; k++)
            {
                if (Main.rand.NextFloat() < 0.8f)
                {
                    float randx = Main.rand.NextFloat(0.7f, 1.3f);
                    float randx2 = Main.rand.NextFloat(-1.5f, 1.5f);
                    float randy = Main.rand.NextFloat(0.7f, 1.3f);
                    float bobandy = Main.rand.NextFloat(-1.5f, 1.5f);
                    float velox = ((cm.X * speed * randx) / cm.Length()) + randx2; //first rand makes it so it has different velocity factor (how far it flies)
                    float veloy = ((cm.Y * speed * randy) / cm.Length()) + bobandy; //second rand is a kinda offset used mainly for when shooting vertically or horizontally
                    Vector2 velo = new Vector2(velox, veloy);
                    Vector2 pos = new Vector2(player.Center.X + velox * 1.2f, player.Center.Y + veloy * 1.2f);
                    Dust dust = Dust.NewDustPerfect(pos, type, velo, 0, color, 2.368421f);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
        }

        private void ShootCandleDust(Item item, AssPlayer mPlayer)
        {
            if (mPlayer.everburningCandleBuff)
            {
                Color color = new Color(255, 255, 255);
                SpawnRangedDust(6, color, item.shootSpeed, mPlayer.player);
            }
            if (mPlayer.everburningCursedCandleBuff)
            {
                Color color = new Color(196, 255, 0); //so it's light green and not dark green
                SpawnRangedDust(61, color, item.shootSpeed, mPlayer.player);
            }
            if (mPlayer.everfrozenCandleBuff)
            {
                Color color = new Color(255, 255, 255);
                SpawnRangedDust(59, color, item.shootSpeed, mPlayer.player);
            }
            if (mPlayer.everburningShadowflameCandleBuff)
            {
                Color color = new Color(196, 0, 255);
                SpawnRangedDust(62, color, item.shootSpeed, mPlayer.player);
            }
        }

        public  override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (mPlayer.everburningCandleBuff)
            {
                Color color = new Color(255, 255, 255);
                SpawnMeleeDust(6, color, hitbox, player);
            }
            if (mPlayer.everburningCursedCandleBuff)
            {
                Color color = new Color(196, 255, 0);
                SpawnMeleeDust(61, color, hitbox, player);
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
