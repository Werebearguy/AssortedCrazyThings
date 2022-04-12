using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs.NPCs.Bosses.DungeonBird;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.DungeonBird
{
    [Content(ContentType.Bosses)]
    public class BabyHarvesterProj : AssProjectile
    {
        public class TierData
        {
            public int FrameSpeed { get; init; } = 4;
            public int SoulsToNextTier { get; init; } = 1;
            public int TransformationFrameCount { get; init; } = 1;

            public TierData()
            {

            }
        }

        public int PlayerOwner { get; private set; } = Main.maxPlayers;

        public bool HasValidPlayerOwner => PlayerOwner >= 0 && PlayerOwner < Main.maxPlayers;

        public Player Player => Main.player[PlayerOwner];

        public int SoulsEaten
        {
            get => (int)Projectile.ai[0];
            private set 
            {
                if (TierToSoulsEaten.TryGetValue(MaxTier, out var v))
                {
                    if (value <= v)
                    {
                        Projectile.ai[0] = value;
                    }
                }
            }
        }

        public int PreviousTier
        {
            get => (int)Projectile.ai[1] + 1; //Since ai is initialized as 0, tier should be 1
            private set => Projectile.ai[1] = value - 1;
        }

        public const int MaxTier = 3;
        public int Tier
        {
            get
            {
                foreach (var item in SoulsEatenToTier)
                {
                    if (SoulsEaten < item.Key)
                    {
                        return item.Value;
                    }
                }

                return MaxTier;
            }
        }

        public int CurrentTrafoTier
        {
            get => (int)Projectile.localAI[0];
            private set => Projectile.localAI[0] = value;
        }

        public bool TrafoInProgress => CurrentTrafoTier > 0;

        public int TrafoFrameY
        {
            get => (int)Projectile.localAI[1];
            private set => Projectile.localAI[1] = value;
        }

        public int TrafoTimer { get; private set; }

        public static Dictionary<int, Asset<Texture2D>> SheetAssets;
        public static Dictionary<int, Asset<Texture2D>> WingAssets;
        public static Dictionary<int, Asset<Texture2D>> TransformationAssets;
        public static Dictionary<int, Asset<Texture2D>> TransformationWingAssets;

        //Transformations are indexed by the "from" tier in "from -> to"
        public static Dictionary<int, TierData> TierDatas;
        private static Dictionary<int, int> TierToSoulsEaten;
        private static Dictionary<int, int> SoulsEatenToTier;

        public static int Spawn(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return Main.maxProjectiles;
            }

            Vector2 position = player.Center;
            position.X += Main.rand.NextFloat(-1980, 1980) / 2;
            position.Y += 200; //TODO 1000

            return AssUtils.NewProjectile(new EntitySource_WorldEvent(), position, Vector2.Zero, ModContent.ProjectileType<BabyHarvesterProj>(), 0, 0,
                preSync: (Projectile proj) => (proj.ModProjectile as BabyHarvesterProj).AssignPlayerOwner(player.whoAmI));
        }

        public void AssignPlayerOwner(int newPlayer)
        {
            if (HasValidPlayerOwner)
            {
                Player.ClearBuff(ModContent.BuffType<BabyHarvesterBuff>());
            }

            PlayerOwner = newPlayer;

            Player.AddBuff(ModContent.BuffType<BabyHarvesterBuff>(), 1600, false);
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                SheetAssets = new Dictionary<int, Asset<Texture2D>>();
                WingAssets = new Dictionary<int, Asset<Texture2D>>();
                TransformationAssets = new Dictionary<int, Asset<Texture2D>>();
                TransformationWingAssets = new Dictionary<int, Asset<Texture2D>>();

                for (int i = 1; i < 4; i++)
                {
                    SheetAssets.Add(i, ModContent.Request<Texture2D>(Texture + i + "_Sheet"));
                }

                //1 has no wing glowmask
                for (int i = 2; i < 4; i++)
                {
                    WingAssets.Add(i, ModContent.Request<Texture2D>(Texture + i + "_Sheet_Wings"));
                }

                for (int i = 1; i < 4; i++)
                {
                    TransformationAssets.Add(i, ModContent.Request<Texture2D>(Texture + "_Transformation" + i));
                    TransformationWingAssets.Add(i, ModContent.Request<Texture2D>(Texture + "_Transformation" + i + "_Glowmask"));
                }
            }

            TierDatas = new Dictionary<int, TierData>()
            {
                [1] = new TierData
                {
                    FrameSpeed = 3,
                    SoulsToNextTier = 8, //TODO 1
                    TransformationFrameCount = 7,
                },
                [2] = new TierData
                {
                    FrameSpeed = 4,
                    SoulsToNextTier = 8, //TODO 5
                    TransformationFrameCount = 9,
                },
                [3] = new TierData
                {
                    FrameSpeed = 4,
                    SoulsToNextTier = 8, //TODO 5
                    TransformationFrameCount = 10,
                },
            };

            SoulsEatenToTier = new Dictionary<int, int>();
            TierToSoulsEaten = new Dictionary<int, int>();
            int soulsAccumulated = 0;
            foreach (var data in TierDatas)
            {
                soulsAccumulated += data.Value.SoulsToNextTier;
                SoulsEatenToTier.Add(soulsAccumulated, data.Key);
                TierToSoulsEaten.Add(data.Key, soulsAccumulated);
            }
        }

        public override void Unload()
        {
            SheetAssets = null;
            WingAssets = null;
            TransformationAssets = null;
            TransformationWingAssets = null;

            TierDatas = null;
            SoulsEatenToTier = null;
            TierToSoulsEaten = null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Bird");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);
            Projectile.aiStyle = -1;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.netImportant = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)PlayerOwner);
            writer.Write((byte)SoulsEaten);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            PlayerOwner = reader.ReadByte();
            SoulsEaten = reader.ReadByte();
        }

        public override void AI()
        {
            if (!HasValidPlayerOwner)
            {
                Projectile.Kill();
                return;
            }
            //Player is now valid here

            //Main.NewText("p: " + PreviousTier);
            //Main.NewText("t: " + Tier);
            //Main.NewText("c: " + CurrentTrafoTier);

            if (!TrafoInProgress)
            {
                RegularAI();
            }

            //Split as RegularAI sets TrafoInProgress
            if (TrafoInProgress)
            {
                TransformationAI();
            }

            PreviousTier = Tier;
        }

        private void RegularAI()
        {
            if (Main.GameUpdateCount % 30 == 0)
            {
                SoulsEaten++;
            }

            if (PreviousTier < Tier)
            {
                //Initiate transformation
                CurrentTrafoTier = PreviousTier;
            }

            AssAI.ZephyrfishAI(Projectile, Player);
            Projectile.spriteDirection = -Projectile.spriteDirection;

            int frameSpeed = 4;
            if (TierDatas.TryGetValue(Tier, out var data))
            {
                frameSpeed = data.FrameSpeed;
            }
            AssAI.ZephyrfishDraw(Projectile, frameSpeed);
        }

        private void TransformationAI()
        {
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
            Projectile.velocity *= 0.4f;

            if (TierDatas.TryGetValue(CurrentTrafoTier, out var data))
            {
                TrafoTimer++;
                if (TrafoTimer > 3)
                {
                    TrafoFrameY++;
                    TrafoTimer = 0;
                }
                if (TrafoFrameY >= data.TransformationFrameCount)
                {
                    TrafoFrameY = 0;

                    Vector2 dustOffset = new Vector2(0, 30);
                    Vector2 center = Projectile.Center;
                    for (int i = 0; i < 10 + CurrentTrafoTier * 10; i++)
                    {
                        Vector2 dustOffset2 = dustOffset.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.1f, 1f);
                        Dust dust = Dust.NewDustPerfect(center + dustOffset2, 59, newColor: Color.White, Scale: 2.1f);
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0.2f, 0.8f);
                        dust.velocity = Utils.SafeNormalize(dust.position - center, Vector2.Zero) * 3;
                    }

                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);

                    CurrentTrafoTier = 0; //Reset trafo
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.4f) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Custom draw to just center on the hitbox, as all birds share the same frame size (for seamless transformation)
            Texture2D texture = null;
            int frameCount = 1;
            int frameY = 0;
            int tier = TrafoInProgress ? CurrentTrafoTier : Tier;
            if (TrafoInProgress && TransformationAssets.TryGetValue(tier, out var trafoAsset))
            {
                texture = trafoAsset.Value;

                if (TierDatas.TryGetValue(tier, out var data))
                {
                    frameCount = data.TransformationFrameCount;
                }
                frameY = TrafoFrameY;
            }
            else if (SheetAssets.TryGetValue(tier, out var asset))
            {
                texture = asset.Value;
                frameCount = Main.projFrames[Projectile.type];
                frameY = Projectile.frame;
            }

            if (texture == null)
            {
                return false;
            }

            Rectangle sourceRect = texture.Frame(1, frameCount, frameY: frameY);
            Vector2 drawOrigin = sourceRect.Size() / 2f;

            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Vector2 drawPos = Projectile.position + Projectile.Size / 2f + new Vector2(0, Projectile.gfxOffY + 4f - 1f) - Main.screenPosition;
            Color color = Projectile.GetAlpha(lightColor);

            float rotation = Projectile.rotation;
            float scale = Projectile.scale;
            Main.EntitySpriteDraw(texture, drawPos, sourceRect, color, rotation, drawOrigin, scale, spriteEffects, 0);

            Texture2D wingTexture = null;
            if (TrafoInProgress && TransformationWingAssets.TryGetValue(tier, out var trafoWingAsset))
            {
                wingTexture = trafoWingAsset.Value;
            }
            else if (WingAssets.TryGetValue(tier, out var wingAsset))
            {
                wingTexture = wingAsset.Value;
            }

            if (wingTexture == null)
            {
                return false;
            }

            Main.EntitySpriteDraw(wingTexture, drawPos, sourceRect, Projectile.GetAlpha(Color.White), rotation, drawOrigin, scale, spriteEffects, 0);

            return false;
        }
    }
}
