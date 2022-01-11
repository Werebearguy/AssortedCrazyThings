using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    //Soul that is spawned during the revive stage of Harvester
    [Content(ContentType.Bosses)]
    public class DungeonSoulRevive : AssNPC
    {
        protected int frameSpeed;
        private const int LifeMax = 50;
        private const int Defense = 0;

        public Color overlayColor = Color.White;

        public static readonly short sinYPeriod = 120;

        public const int State_FlyingOut = 0;
        public const int State_FlyingBack = 1;

        public ref float AI_State => ref NPC.ai[0];

        public const int FlyingBackTime = 300; //5 seconds * 60

        //TODO might not be needed
        public Vector2 StopFlyingPosition
        {
            get => new Vector2(NPC.ai[1], NPC.ai[2]);
            set
            {
                NPC.ai[1] = value.X;
                NPC.ai[2] = value.Y;
            }
        }

        public int ParentWhoAmI
        {
            get => (int)NPC.ai[3] - 1;
            set => NPC.ai[3] = value + 1;
        }

        public bool HasParent => ParentWhoAmI >= 0 && ParentWhoAmI < Main.maxNPCs;

        //Acts as init aswell
        public ref float AI_SinTimer => ref NPC.localAI[0];

        //Synced
        public ref float AI_Timer => ref NPC.localAI[1];

        public ref float AI_OverlayTimer => ref NPC.localAI[2];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Soul");
            Main.npcFrameCount[NPC.type] = 6;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true //Hides this NPC from the Bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 40;
            NPC.npcSlots = 0f;
            NPC.dontCountMe = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.dontTakeDamage = true; //Controlled by AI later
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.damage = 0;
            NPC.defense = Defense;
            NPC.lifeMax = LifeMax;
            NPC.scale = 1f;
            NPC.HitSound = SoundID.NPCHit1?.WithVolume(0.5f);
            NPC.DeathSound = new LegacySoundStyle(SoundID.Zombie, 53);
            NPC.value = 0f;
            NPC.aiStyle = -1;
            NPC.direction = 1;
            NPC.alpha = 255; //Fade in

            frameSpeed = 4;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.defense = Defense;
            NPC.lifeMax = LifeMax;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((float)AI_Timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AI_Timer = reader.ReadSingle();
        }

        public static void KillInstantly(NPC npc)
        {
            npc.HitEffect(0);
            npc.life = 0;
            npc.active = false;

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.LoopAnimation(frameHeight, frameSpeed);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            drawColor.R = Math.Max(drawColor.R, (byte)200); //100 for dark
            drawColor.G = Math.Max(drawColor.G, (byte)200);
            drawColor.B = Math.Max(drawColor.B, (byte)200);
            return drawColor.MultiplyRGBA(overlayColor) * NPC.Opacity;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            drawColor = NPC.GetAlpha(drawColor);

            //Vanilla draw code
            float someNumModdedNPCsArentUsing = 0f;
            float drawOffsetY = Main.NPCAddHeight(NPC);
            Asset<Texture2D> asset = TextureAssets.Npc[NPC.type];
            Texture2D texture = asset.Value;

            Vector2 halfSize = new Vector2(asset.Width() / 1 / 2, asset.Height() / Main.npcFrameCount[NPC.type] / 2);

            SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 halfSizeOff = halfSize * NPC.scale;
            Vector2 textureOff = new Vector2(asset.Width() * NPC.scale / 1 / 2f, asset.Height() * NPC.scale / (float)Main.npcFrameCount[NPC.type]);
            float sinY = (float)((Math.Sin(AI_SinTimer / sinYPeriod * MathHelper.TwoPi) - 1) * 8);
            Vector2 drawPosition = new Vector2(NPC.Center.X, NPC.Bottom.Y + drawOffsetY + someNumModdedNPCsArentUsing + NPC.gfxOffY + 4f + sinY);
            spriteBatch.Draw(texture, drawPosition + halfSizeOff - textureOff - screenPos, NPC.frame, drawColor, NPC.rotation, halfSize, NPC.scale, effects, 0f);

            spriteBatch.Draw(texture, drawPosition + halfSizeOff - textureOff - screenPos, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, halfSize, NPC.scale, effects, 0f);
        }

        public override void AI()
        {
            if (!HasParent)
            {
                KillInstantly(NPC);
                return;
            }

            NPC parent = Main.npc[ParentWhoAmI];
            if (!parent.active || parent.type != AssortedCrazyThings.harvesterTypes[2])
            {
                KillInstantly(NPC);
                return;
            }

            Harvester harvester = parent.ModNPC as Harvester;

            if (AI_SinTimer == 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(NPC.Center + new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0.1f, 0.4f);
                    }
                }

                AI_SinTimer = Main.rand.Next(sinYPeriod);
            }

            AI_SinTimer++;
            if (AI_SinTimer > sinYPeriod)
            {
                AI_SinTimer = 1;
            }

            NPC.scale = 1f;

            if (AI_State == State_FlyingOut)
            {
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 10;
                    if (NPC.alpha < 0)
                    {
                        NPC.alpha = 0;
                    }
                }

                NPC.dontTakeDamage = true;
                NPC.chaseable = false;
                StopFlyingPosition = Vector2.Zero;
                //Initial velocity given by whatever is spawning them
                NPC.velocity *= 0.94f;

                float lenSQ = NPC.velocity.LengthSquared();

                if (lenSQ < 0.02f)
                {
                    NPC.velocity = Vector2.Zero;
                }

                if (lenSQ < 0.5f)
                {
                    AI_Timer++;
                    if (AI_Timer < 20)
                    {
                        //Slight wait time
                        return;
                    }
                    AI_State = State_FlyingBack;
                    AI_Timer = 0;
                    StopFlyingPosition = NPC.Center;
                    NPC.netUpdate = true;
                }
            }
            else if (AI_State == State_FlyingBack)
            {
                NPC.dontTakeDamage = false;
                NPC.chaseable = true;
                Vector2 fromTo = parent.Center - NPC.Center;
                float fromToDistSQ = fromTo.LengthSquared();

                //Make sure it can reach from current location to parent in FlyingBackTime - AI_Timer ticks
                NPC.velocity = fromTo / (FlyingBackTime - AI_Timer);
                ModifyOverlayColor(parent, fromToDistSQ);

                AI_Timer++;
                if (AI_Timer > FlyingBackTime)
                {
                    AI_Timer = FlyingBackTime;
                }

                if (fromToDistSQ < 10)
                {
                    KillInstantly(NPC);

                    harvester.IncrementReviveSoulsProgress();
                    return;
                }
            }
        }

        private void ModifyOverlayColor(NPC parent, float fromToDistSQ)
        {
            //Scale overlay pulse frequency by the distance to boss (total: stopFlyingToBossDistSQ, current: fromToDistSQ)
            Vector2 stopFlyingToBoss = parent.Center - StopFlyingPosition;
            float stopFlyingToBossDistSQ = stopFlyingToBoss.LengthSquared();

            //Starts at 1f at max dist
            float remainingRatio = fromToDistSQ / stopFlyingToBossDistSQ;

            int maxOverlayPeriod = (int)(30 + remainingRatio * 180); //Higher == slower

            float overlayIntensity = 0.75f * (0.6f + 0.25f * (1f - remainingRatio)); //Higher = intense

            AI_OverlayTimer++;
            if (AI_OverlayTimer > maxOverlayPeriod)
            {
                AI_OverlayTimer = 0;
            }

            float overlaySin = (float)((Math.Sin(AI_OverlayTimer / maxOverlayPeriod * MathHelper.TwoPi) + 1) * 0.5f);

            //Main.NewText("remainingRatio:" + remainingRatio);
            //Main.NewText("maxOverlayPeriod:" + maxOverlayPeriod);
            //Main.NewText("overlayIntensity:" + overlayIntensity);
            //Main.NewText("overlaySin:" + overlaySin);

            //0f == white
            //1f == blue
            overlayColor = Color.Lerp(Color.White, new Color(0, 73, 193), overlaySin * overlayIntensity);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int number = NPC.life <= 0 ? 15 : 7;
            for (int i = 0; i < number; i++)
            {
                Dust dust = Dust.NewDustPerfect(NPC.Center + new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), 59, new Vector2(hitDirection + Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
                dust.noLight = true;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
            }
        }
    }
}
