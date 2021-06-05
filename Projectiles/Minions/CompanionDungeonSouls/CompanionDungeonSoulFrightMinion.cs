using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls
{
    public class CompanionDungeonSoulFrightMinion : CompanionDungeonSoulMinionBase
    {
        public override void SafeSetDefaults()
        {
            Projectile.minionSlots = 1f;
            defdistanceFromTarget = 700f;
            defdistancePlayerFarAway = 800f;
            defdistancePlayerFarAwayWhenHasTarget = 1200f;
            defdistanceToEnemyBeforeCanDash = 30f; //20f
            defplayerFloatHeight = -60f; //-60f
            defplayerCatchUpIdle = 300f; //300f
            defbackToIdleFromNoclipping = 150f; //150f
            defdashDelay = 40f; //time it stays in the "dashing" state after a dash, he dashes when he is in state 0 aswell
            defdistanceAttackNoclip = defdashDelay * 5f;
            defstartDashRange = defdistanceToEnemyBeforeCanDash + 10f; //30f
            defdashIntensity = 4f; //4f

            veloFactorToEnemy = 7f; //8f
            accFactorToEnemy = 16f; //41f

            veloFactorAfterDash = 7f; //4f
            accFactorAfterDash = 16f; //41f

            defveloIdle = 2f;
            defveloCatchUpIdle = 8f;
            defveloNoclip = 12f;

            dustColor = ItemID.BrightOrangeDye;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int rand = Main.rand.Next(10);
            if (rand == 0)
            {
                target.AddBuff(BuffID.Ichor, 300);
            }
            else if (rand == 1)
            {
                target.AddBuff(BuffID.Poisoned, 300);
            }
        }
    }
}
