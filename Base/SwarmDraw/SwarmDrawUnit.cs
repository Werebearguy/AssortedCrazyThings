using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.DataStructures;

namespace AssortedCrazyThings.Base.SwarmDraw
{
    public abstract class SwarmDrawUnit : ICloneable
    {
        public Asset<Texture2D> Asset { get; protected set; }

        public Asset<Texture2D> TrailAsset { get; protected set; }

        public int FrameCount { get; protected set; }
        public int FrameSpeed { get; protected set; }

        protected int frameCounter;
        protected int frame;

        public bool Front { get; protected set; }

        public bool HasTrail => OldPos.Length > 0;

        //Trail parts can also be front/back when switching, don't want to switch the entire trail all at once
        public bool[] OldFront { get; protected set; }

        public int[] OldFrame { get; protected set; }

        protected Vector2 pos;

        public Vector2[] OldPos { get; protected set; }

        protected int dir = 1;

        public int[] OldDir { get; protected set; }

        protected float rot;

        public float[] OldRot { get; protected set; }

        protected Vector2 oldVel;

        protected Vector2 vel;

        /// <summary>
        /// If direction should be automatically set based on velocity. Occurs after AI
        /// </summary>
        public virtual bool AutoDirection => true;

        /// <summary>
        /// If layer should be automatically changed based on X-velocity change. Occurs after AI
        /// </summary>
        public virtual bool AutoLayerX => true;

        public SwarmDrawUnit(Asset<Texture2D> asset, int frameCount, int frameSpeed, int trailLength = 0, Asset<Texture2D> trailAsset = null)
        {
            Asset = asset;
            FrameCount = frameCount;
            FrameSpeed = frameSpeed;

            OldFront = new bool[trailLength];
            OldFrame = new int[trailLength];
            OldPos = new Vector2[trailLength];
            OldDir = new int[trailLength];
            OldRot = new float[trailLength];

            TrailAsset = trailAsset ?? Asset;
        }

        public object Clone()
        {
            var clone = (SwarmDrawUnit)MemberwiseClone();
            int trailLength = OldFront.Length;

            //Need to reinitialize the arrays
            clone.OldFront = new bool[trailLength];
            clone.OldFrame = new int[trailLength];
            clone.OldPos = new Vector2[trailLength];
            clone.OldDir = new int[trailLength];
            clone.OldRot = new float[trailLength];
            Array.Copy(OldFront, clone.OldFront, trailLength);
            Array.Copy(OldFrame, clone.OldFrame, trailLength);
            Array.Copy(OldPos, clone.OldPos, trailLength);
            Array.Copy(OldDir, clone.OldDir, trailLength);
            Array.Copy(OldRot, clone.OldRot, trailLength);
            return clone;
        }

        public virtual int GetShader(PlayerDrawSet drawInfo)
        {
            return 0;
        }

        public virtual Color GetColor(PlayerDrawSet drawInfo)
        {
            return Color.White;
        }

        public virtual float GetScale(PlayerDrawSet drawInfo)
        {
            return 1f;
        }

        public virtual void Animate()
        {
            AssExtensions.LoopAnimationInt(ref frame, ref frameCounter, FrameSpeed, 0, FrameCount - 1);
        }

        public virtual void AI(Vector2 center)
        {

        }

        /// <summary>
        /// Runs whenever the swarm is enabled. Acts as a Reset after initial spawn too
        /// </summary>
        public virtual void OnSpawn()
        {
            if (!AutoLayerX)
            {
                Front = Main.rand.NextBool();
            }

            pos = Vector2.Zero;

            vel = Main.rand.NextVector2Unit();
            if (Math.Abs(vel.X) < 0.1f || Math.Abs(vel.Y) < 0.1f)
            {
                //Prevent too horizontal/vertical movement
                vel.RotatedBy(MathHelper.TwoPi / 12);
            }

            frame = Main.rand.Next(FrameCount);
            frameCounter = Main.rand.Next(FrameSpeed);

            if (!AutoDirection)
            {
                dir = Main.rand.NextBool().ToDirectionInt();
            }

            for (int i = 0; i < OldPos.Length; i++)
            {
                OldFront[i] = Front;
                OldFrame[i] = frame;
                OldPos[i] = pos;
                OldDir[i] = dir;
                OldRot[i] = rot;
            }
        }

        public void Update(Vector2 center)
        {
            Animate();

            if (AutoDirection)
            {
                dir = (vel.X > 0).ToDirectionInt();
            }

            AI(center);

            pos += vel;

            if (AutoLayerX)
            {
                if (Math.Sign(oldVel.X) != Math.Sign(vel.X))
                {
                    Front = !Front;
                }
            }

            if (HasTrail)
            {
                //Shift elements one to the right
                for (int i = OldPos.Length - 1; i > 0; i--)
                {
                    OldFront[i] = OldFront[i - 1];
                    OldFrame[i] = OldFrame[i - 1];
                    OldPos[i] = OldPos[i - 1];
                    OldDir[i] = OldDir[i - 1];
                    OldRot[i] = OldRot[i - 1];
                }

                //Fill leftmost element with current value
                OldFront[0] = Front;
                OldFrame[0] = frame;
                OldPos[0] = pos;
                OldDir[0] = dir;
                OldRot[0] = rot;
            }

            oldVel = vel;
        }

        public List<DrawData> ToDrawDatas(PlayerDrawSet drawInfo, bool front)
        {
            var datas = new List<DrawData>();
            if (front != Front)
            {
                return datas;
            }

            GetDrawDataParams(drawInfo, out Texture2D texture, out Vector2 position, out Rectangle bounds, out Color color, out float rotation, out float scale, out SpriteEffects effects);

            datas.Add(ToDrawData(drawInfo, texture, position, bounds, color, rotation, bounds.Size() / 2, scale, effects));
            return datas;
        }

        public List<DrawData> TrailToDrawDatas(PlayerDrawSet drawInfo, bool front)
        {
            List<DrawData> datas = new List<DrawData>();
            if (!HasTrail)
            {
                return datas;
            }

            for (int i = 0; i < OldPos.Length; i++)
            {
                if (front != OldFront[i])
                {
                    //If a front data is needed but this trail part is not front,
                    //Of if a back data is needed but this trail part is front, continue
                    continue;
                }

                GetDrawDataParams(drawInfo, out Texture2D texture, out Vector2 position, out Rectangle bounds, out Color color, out float rotation, out float scale, out SpriteEffects effects, i);

                var data = ToDrawData(drawInfo, texture, position, bounds, color, rotation, bounds.Size() / 2, scale, effects);
                datas.Add(data);
            }

            return datas;
        }

        private void GetDrawDataParams(PlayerDrawSet drawInfo, out Texture2D texture, out Vector2 position, out Rectangle bounds, out Color color, out float rotation, out float scale, out SpriteEffects effects, int trailIndex = -1)
        {
            bool trail = trailIndex > -1;

            int realFrame = trail ? OldFrame[trailIndex] : frame;
            Vector2 realPos = trail ? OldPos[trailIndex] : pos;
            int realDir = trail ? OldDir[trailIndex] : dir;
            float realRot = trail ? OldRot[trailIndex] : rot;

            texture = (trail ? TrailAsset : Asset).Value;
            bounds = texture.Frame(1, FrameCount, frameY: realFrame);

            effects = realDir == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            const SpriteEffects flipVertically = SpriteEffects.FlipVertically;
            if (drawInfo.playerEffect.HasFlag(flipVertically))
            {
                effects |= flipVertically;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            position = realPos + drawInfo.Position - Main.screenPosition + drawPlayer.bodyPosition + drawPlayer.Size / 2;

            float factor = 1f;
            if (trail)
            {
                int trailCount = OldPos.Length;
                factor = (float)(trailCount - trailIndex) / trailCount;
            }

            color = GetColor(drawInfo) * factor;

            rotation = realRot;

            scale = GetScale(drawInfo) * factor;
        }

        public DrawData ToDrawData(PlayerDrawSet drawInfo, Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect)
        {
            //position = position.Floor(); //Makes it worse when idle
            return new DrawData(texture, position, sourceRect, color, rotation, origin, scale, effect, 0)
            {
                ignorePlayerRotation = true,
                shader = GetShader(drawInfo)
            };
        }
    }
}
