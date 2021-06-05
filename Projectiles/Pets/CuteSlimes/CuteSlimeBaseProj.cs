using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public abstract class CuteSlimeBaseProj : ModProjectile
    {
        private const string PetAccessoryFolder = "AssortedCrazyThings/Items/PetAccessories/";
        public const string Sheet = "_Sheet";
        public const string NoHair = "NoHair";
        public const string Addition = "Addition";
        public const string AccSheet = "_Draw";

        public const int Projwidth = 28;
        public const int Projheight = 32;
        private const short clonedAIType = ProjectileID.PetLizard;

        public const int SheetCountX = 7;
        public const int SheetCountY = 7;

        public const int DefaultX = 0;
        public const int DefaultYIdleStart = 0;
        public const int DefaultYIdleEnd = 1;
        public const int DefaultYWalkStart = DefaultYIdleStart;
        public const int DefaultYWalkEnd = DefaultYIdleEnd;
        public const int DefaultYAir = 2;
        public const int DefaultYFlyStart = 3;
        public const int DefaultYFlyEnd = 6;

        public const int MeleeX = 1;
        public const int MeleeYSwingStart = 0;
        public const int MeleeYSwingEnd = 2;

        public const int BowX = 2;
        public const int BowYDrawStart = 0;
        public const int BowYDrawEnd = 3;

        public const int StaffX = 3;
        public const int StaffYCast = 0;
        public const int StaffYFlash = 1;
        public const int StaffYStaff = 2;

        public const int SpikeX = 4;
        public const int SpikeYStart = 0;
        public const int SpikeYEnd = 1;
        public const int SpikeYHug = 2;

        public const int TransformX = 5;
        public const int TransformYStart = 0;
        public const int TransformYEnd = 1;

        public const int SlimeX = 6;
        public const int SlimeYIdleFirst = DefaultYIdleStart;
        public const int SlimeYIdleLast = DefaultYIdleEnd;
        public const int SlimeYWalkFirst = DefaultYIdleStart;
        public const int SlimeYWalkLast = DefaultYIdleEnd;
        public const int SlimeYAir = DefaultYAir;
        public const int SlimeYFlyFirst = DefaultYFlyStart;
        public const int SlimeYFlyLast = DefaultYFlyEnd;

        public static readonly int[] SheetCounts = new int[] { 7, 3, 4, 3, 3, 2, 7 };

        protected int frameCounter = 0;
        protected int frameX = 0;
        protected int frameY = 0;

        public ushort MiscTimer = 0;

        public static Dictionary<int, Asset<Texture2D>> SheetAssets { get; private set; }
        public static Dictionary<int, Asset<Texture2D>> SheetNoHairAssets { get; private set; }

        /// <summary>
        /// Values can be null if it does not exist
        /// </summary>
        public static Dictionary<int, Asset<Texture2D>> SheetAdditionAssets { get; private set; }
        /// <summary>
        /// Values can be null if it does not exist
        /// </summary>
        public static Dictionary<int, Asset<Texture2D>> SheetAdditionNoHairAssets { get; private set; }

        public override void Load()
        {
            if (SheetAssets == null)
                SheetAssets = new Dictionary<int, Asset<Texture2D>>();

            if (SheetNoHairAssets == null)
                SheetNoHairAssets = new Dictionary<int, Asset<Texture2D>>();

            if (SheetAdditionAssets == null)
                SheetAdditionAssets = new Dictionary<int, Asset<Texture2D>>();

            if (SheetAdditionNoHairAssets == null)
                SheetAdditionNoHairAssets = new Dictionary<int, Asset<Texture2D>>();
        }

        public override void Unload()
        {
            DisposeContentsInDict(SheetAssets);
            SheetAssets = null;

            DisposeContentsInDict(SheetNoHairAssets);
            SheetNoHairAssets = null;

            DisposeContentsInDict(SheetAdditionAssets);
            SheetAdditionAssets = null;

            DisposeContentsInDict(SheetAdditionNoHairAssets);
            SheetAdditionNoHairAssets = null;
        }

        public sealed override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1; //Use dummy texture
            Main.projPet[Projectile.type] = true;

            if (!Main.dedServ)
            {
                string sheetName = Texture + Sheet;

                //All of them have these
                SheetAssets[Projectile.type] = ModContent.GetTexture(sheetName);

                SheetNoHairAssets[Projectile.type] = ModContent.GetTexture(sheetName + NoHair);

                //Only some have these
                SheetAdditionAssets[Projectile.type] = GetTextureMaybeNull(sheetName + Addition);

                SheetAdditionNoHairAssets[Projectile.type] = GetTextureMaybeNull(sheetName + Addition + NoHair);
            }

            SafeSetStaticDefaults();
        }

        private static void DisposeContentsInDict<T>(Dictionary<int, T> dict) where T: IDisposable
        {
            if (dict == null)
                return;

            foreach (var item in dict)
            {
                item.Value?.Dispose();
            }
        }

        private static Asset<Texture2D> GetTextureMaybeNull(string name)
        {
            if (ModContent.TextureExists(name))
                return ModContent.GetTexture(name);

            return null;
        }

        //Clamps frames so they never point to invalid/blank frames
        private void ClampFrames()
        {
            if (frameX < 0 || frameX >= SheetCountX)
            {
                frameX = 0;
            }

            if (frameY < 0 || frameY >= SheetCounts[frameX])
            {
                frameY = 0;
            }
        }

        public abstract ref bool PetBool(Player player);

        public virtual void SafeSetStaticDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            Projectile.CloneDefaults(clonedAIType);
            Projectile.width = Projwidth;
            Projectile.height = Projheight;
            AIType = clonedAIType;
            //TODO revisit offsets
            DrawOffsetX = -18;
            DrawOriginOffsetY = -16;

            SafeSetDefaults();
        }

        public virtual void SafeSetDefaults()
        {

        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((ushort)MiscTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MiscTimer = reader.ReadUInt16();
        }

        public sealed override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.lizard = false;

            ref bool petBool = ref PetBool(player);
            if (player.dead)
            {
                petBool = false;
            }
            if (petBool)
            {
                Projectile.timeLeft = 2;
            }
            PetPlayer pPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();

            if (SlimePets.TryGetPetFromProj(Projectile.type, out _))
            {
                pPlayer.slimePetIndex = Projectile.whoAmI;
            }

            return SafePreAI();
        }

        public virtual bool SafePreAI()
        {
            return true;
        }

        public bool InAir => Projectile.ai[0] != 0f;

        //0.1f because vanilla AI sets it to 0.1f when on the ground
        public bool OnGround => Projectile.velocity.Y == 0.1f;

        public override void PostAI()
        {
            frameX = DefaultX;

            //readjusting the animation
            if (InAir)
            {
                AssExtensions.LoopAnimationInt(ref frameY, ref frameCounter, 3, DefaultYFlyStart, DefaultYFlyEnd);
            }
            else
            {
                if (OnGround)
                {
                    if (Projectile.velocity.X == 0f)
                    {
                        //Idle
                        AssExtensions.LoopAnimationInt(ref frameY, ref frameCounter, 16, DefaultYIdleStart, DefaultYIdleEnd);
                    }
                    else if (Math.Abs(Projectile.velocity.X) > 0.1)
                    {
                        //Moving
                        frameCounter += (int)(Math.Abs(Projectile.velocity.X) * 0.25f);
                        AssExtensions.LoopAnimationInt(ref frameY, ref frameCounter, 6, DefaultYWalkStart, DefaultYWalkEnd);
                    }
                    else
                    {
                        frameY = DefaultYIdleEnd;
                        frameCounter = 0;
                    }
                }
                else //jumping/falling
                {
                    frameCounter = 0;
                    frameY = DefaultYAir;
                }
            }

            if (!OnGround) Projectile.rotation = Projectile.velocity.X * 0.01f;

            ClampFrames();
        }

        public override bool PreDraw(ref Color drawColor)
        {
            DrawAccessories(drawColor, preDraw: true);

            DrawBaseSprite(drawColor);

            DrawAccessories(drawColor);
            return false;
        }

        /// <summary>
        /// Draws the base sprite. Picks the NoHair variant if needed
        /// </summary>
        private void DrawBaseSprite(Color drawColor)
        {
            PetPlayer pPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            //check if it wears a "useNoHair" hat, then if it does, change the texture to that,
            //otherwise use default one
            bool useNoHair = false;
            PetAccessory petAccessoryHat = pPlayer.GetAccessoryInSlot((byte)SlotType.Hat);

            if (petAccessoryHat != null &&
                petAccessoryHat.UseNoHair) //if it has a NoHair tex
            {
                useNoHair = true;
            }

            bool drawPreAddition = true;
            bool drawPostAddition = true;

            if (SlimePets.TryGetPetFromProj(Projectile.type, out SlimePet sPet))
            {
                PetAccessory petAccessory;
                //handle if pre/post additions are drawn based on the slimePet(Pre/Post)AdditionSlot
                for (byte slotNumber = 1; slotNumber < 5; slotNumber++)
                {
                    petAccessory = pPlayer.GetAccessoryInSlot(slotNumber);

                    if (petAccessory != null)
                    {
                        if (sPet.PreAdditionSlot == slotNumber)
                            drawPreAddition = false;

                        if (sPet.PostAdditionSlot == slotNumber)
                            drawPostAddition = false;
                    }
                }
            }
            else
            {
                //Can't receive any accessories, reset flag
                useNoHair = false;
            }

            bool drawnPreDraw = !drawPreAddition || SafePreDrawBaseSprite(drawColor, useNoHair); //do a pre-draw for rainbow and dungeon

            if (drawnPreDraw)
            {
                //Draw NoHair if necessary, otherwise regular sprite
                Texture2D texture = SheetAssets[Projectile.type].Value;
                if (useNoHair) //only if not legacy
                {
                    texture = SheetNoHairAssets[Projectile.type].Value;
                }

                if (texture == null)
                {
                    return;
                }

                Rectangle frameLocal = texture.Frame(SheetCountX, SheetCountY, frameX, frameY);
                SpriteEffects effect = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Projwidth * 0.5f, (texture.Height / SheetCountY) * 0.5f);
                Vector2 stupidOffset = new Vector2(Projectile.type == ModContent.ProjectileType<CuteSlimePinkProj>() ? -8f : 0f, Projectile.gfxOffY + DrawOriginOffsetY);
                Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                Color color = Projectile.GetAlpha(drawColor);
                //color = drawColor * ((255f - projectile.alpha) / 255f);

                Main.spriteBatch.Draw(texture, drawPos, frameLocal, color, Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effect, 0f);
            }

            if (drawPostAddition) SafePostDrawBaseSprite(drawColor, useNoHair); //used for xmas bow, lava horn, princess crown and illuminant afterimage
        }

        /// <summary>
        /// Draw the pet specific PreDraw behind the base sprite
        /// </summary>
        public virtual bool SafePreDrawBaseSprite(Color drawColor, bool useNoHair)
        {
            return true;
        }

        /// <summary>
        /// Draw the pet specific PostDraw infront of the base sprite
        /// </summary>
        public virtual void SafePostDrawBaseSprite(Color drawColor, bool useNoHair)
        {

        }

        /// <summary>
        /// Draws the pet vanity accessories (behind or infront of the base sprite)
        /// </summary>
        private void DrawAccessories(Color drawColor, bool preDraw = false)
        {
            PetPlayer pPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            bool isPet = SlimePets.TryGetPetFromProj(Projectile.type, out SlimePet sPet);
            PetAccessory petAccessory;

            string textureString;
            string colorString;
            Texture2D texture;
            Rectangle frameLocal;
            SpriteEffects effect;
            Vector2 drawOrigin;
            Vector2 stupidOffset;
            Color color;
            Vector2 originOffset;
            Vector2 drawPos;

            for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
            {
                petAccessory = pPlayer.GetAccessoryInSlot(slotNumber);

                if (petAccessory != null && isPet &&
                    (preDraw || !petAccessory.PreDraw) &&
                    !sPet.IsSlotTypeBlacklisted[slotNumber])
                {
                    textureString = PetAccessoryFolder + petAccessory.Name;
                    colorString = petAccessory.HasAlts ? petAccessory.AltTextureSuffixes[petAccessory.Color] : "";

                    texture = ModContent.GetTexture(textureString + colorString + AccSheet).Value;

                    frameLocal = texture.Frame(SheetCountX, SheetCountY, frameX, frameY);

                    //get necessary properties and parameters for draw
                    effect = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    drawOrigin = new Vector2(Projwidth * 0.5f, (texture.Height / SheetCountY) * 0.5f);
                    stupidOffset = new Vector2(Projectile.type == ModContent.ProjectileType<CuteSlimePinkProj>() ? -8f : 0f, DrawOriginOffsetY + Projectile.gfxOffY);
                    color = drawColor * ((255 - petAccessory.Alpha) / 255f);

                    originOffset = -petAccessory.Offset;
                    originOffset.X *= Math.Sign(Projectile.spriteDirection);

                    drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                    Main.spriteBatch.Draw(texture, drawPos, frameLocal, color, Projectile.rotation, frameLocal.Size() / 2 + originOffset, Projectile.scale, effect, 0f);
                }
            }
        }
    }
}
