using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.Bosses)]
	public abstract class CaughtDungeonSoulBase : AssItem
	{
		protected int animatedTextureSelect;
		private int sincounter;
		float sinY = -10f;
		protected int frame2CounterCount;
		private int frame2Counter;
		private int frame2;

		private static bool loaded = false;

		public override void Load()
		{
			if (!loaded)
			{
				On_Item.CanCombineStackInWorld += Item_CanCombineStackInWorld;
				loaded = true;
			}
		}

		private static bool Item_CanCombineStackInWorld(On_Item.orig_CanCombineStackInWorld orig, Item self)
		{
			var ret = orig(self);

			if (self.type >= ItemID.Count && self.type == ModContent.ItemType<CaughtDungeonSoulFreed>())
			{
				return false;
			}

			return ret;
		}

		public override void Unload()
		{
			loaded = false;
		}

		public sealed override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(copper: 50);
			Item.rare = 2;
			Item.color = Color.White;

			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults()
		{

		}

		public override void PostUpdate()
		{
			Animate();
			sincounter = sincounter > 120 ? 0 : sincounter + 1;
			sinY = (float)((Math.Sin((sincounter / 120f) * MathHelper.TwoPi) - 1) * 10);

			Lighting.AddLight(Item.Center, new Vector3(0.15f, 0.15f, 0.35f));
		}

		public void Animate()
		{
			if (frame2CounterCount <= 0)
			{
				return;
			}

			frame2Counter++;
			if (frame2Counter >= frame2CounterCount)
			{
				frame2++;
				frame2Counter = 0;
				if (frame2 >= AssortedCrazyThings.animatedSoulFrameCount)
				{
					frame2 = 0;
				}
			}
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (frame2CounterCount <= 0)
			{
				return true;
			}

			return false;
		}

		//draw only in world, not in inventory
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (frame2CounterCount <= 0)
			{
				return;
			}

			lightColor = Item.GetAlpha(lightColor) * 0.99f; //1f is opaque
			lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
			lightColor.G = Math.Max(lightColor.G, (byte)200);
			lightColor.B = Math.Max(lightColor.B, (byte)200);

			SpriteEffects effects = SpriteEffects.None;
			Asset<Texture2D> asset = AssortedCrazyThings.animatedSoulTextures[animatedTextureSelect];
			Texture2D image = asset.Value;
			Rectangle bounds = image.Frame(1, AssortedCrazyThings.animatedSoulFrameCount, frameY: frame2);

			Vector2 stupidOffset = new Vector2(Item.width / 2, Item.height - 10f + sinY);

			Main.spriteBatch.Draw(image, Item.position - Main.screenPosition + stupidOffset, bounds, lightColor, rotation, bounds.Size() / 2, scale, effects, 0f);
		}
	}
}
