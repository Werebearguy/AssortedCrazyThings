using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    [Content(ContentType.DroppedPets)]
    public class PetQueenSlimeAirProj : SimplePetProjBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Sibling");
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);
            AIType = ProjectileID.ZephyrFish;
            Projectile.width = 20;
            Projectile.height = 20;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.zephyrfish = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.PetQueenSlime = false;
            }
            if (modPlayer.PetQueenSlime)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
        }
    }

    [Content(ContentType.DroppedPets)]
    public abstract class PetQueenSlimeGroundProj : BabySlimeBase
    {
        const int period = 70;
        protected bool front;

        protected int timer = 0;

        public override bool UseJumpingFrame => false;

        public PetQueenSlimeGroundProj(bool front)
        {
            this.front = front;
        }

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Sibling");
        }

        public override void SafeSetDefaults()
        {
            Projectile.minion = false;

            Projectile.width = 20;
            Projectile.height = 18;

            DrawOriginOffsetY = 0;
            DrawOffsetX = -20;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.PetQueenSlime = false;
            }
            if (modPlayer.PetQueenSlime)
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }

        public override void PostAI()
        {
            timer = (timer + 1) % period;

            alignFront = (timer - period / 2) > 0;
            if (!front)
            {
                alignFront = !alignFront;
            }
        }
    }

    public class PetQueenSlimeGround1Proj : PetQueenSlimeGroundProj
    {
        public PetQueenSlimeGround1Proj() : base(true)
        {

        }
    }

    public class PetQueenSlimeGround2Proj : PetQueenSlimeGroundProj
    {

        public PetQueenSlimeGround2Proj() : base(false)
        {

        }
    }
}
