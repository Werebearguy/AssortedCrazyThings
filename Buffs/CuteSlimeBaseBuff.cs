using System;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public abstract class CuteSlimeBaseBuff : ModBuff
    {
        protected int projType = -1;
        protected byte color = 255;

        public sealed override void SetDefaults()
        {
            MoreSetDefaults();
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        protected virtual void MoreSetDefaults()
        {

        }

        public sealed override void Update(Player player, ref int buffIndex)
        {
            PetPlayer mPlayer = player.GetModPlayer<PetPlayer>(mod);

            player.buffTime[buffIndex] = 18000;

            MoreUpdate(mPlayer);

            if (projType != -1)
            {
                if (color != 255)
                {

                    bool petProjectileNotSpawned = player.ownedProjectileCounts[projType] <= 0;
                    if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
                    {
                        int i = Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y, 0f, 0f, projType, 0, 0f, player.whoAmI, 0f, 0f);
                        mPlayer.petColor = color;
                        mPlayer.slimePetIndex = i;
                    }
                }
                else
                {
                    throw new Exception("color not specified in " + Name);
                }
            }
            else
            {
                throw new Exception("projType not specified in " + Name);
            }
        }

        protected virtual void MoreUpdate(PetPlayer mPlayer)
        {

        }
    }
}
