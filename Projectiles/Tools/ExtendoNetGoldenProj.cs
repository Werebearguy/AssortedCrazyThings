namespace AssortedCrazyThings.Projectiles.Tools
{
    public class ExtendoNetGoldenProj : ExtendoNetBaseProj
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.scale = 1.3f;
            initialSpeed = 13f;
            extendSpeed = 4f;
            retractSpeed = 3.6f;
        }
    }
}
