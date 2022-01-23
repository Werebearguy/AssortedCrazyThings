using Terraria;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderling
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingPlayer : AssPlayerBase
	{
		private bool hasValhallaArmorVisual = false;
		private bool prevHasValhallaArmorVisual = false;

		public bool hasMinion = false;

		public bool firstSummon = true;

		public override void ResetEffects()
		{
			prevHasValhallaArmorVisual = hasValhallaArmorVisual;
			hasValhallaArmorVisual = false;

			hasMinion = false;
		}

        public override void OnEnterWorld(Player player)
        {
			GoblinUnderlingSystem.OnEnterWorld(player);
		}

        public override void LoadData(TagCompound tag)
        {
			firstSummon = tag.GetBool("firstSummon");
        }

        public override void SaveData(TagCompound tag)
        {
			tag.Set("firstSummon", firstSummon);
        }

        public override void UpdateVisibleVanityAccessories()
		{
			if (Main.myPlayer != Player.whoAmI)
            {
				return;
            }

			hasValhallaArmorVisual = Player.head == 210 && Player.body == 204 && Player.legs == 152;

			if (hasValhallaArmorVisual && !prevHasValhallaArmorVisual)
			{
				foreach (var proj in GoblinUnderlingSystem.GetLocalGoblinUnderlings())
				{
					GoblinUnderlingSystem.TryCreate(proj, GoblinUnderlingMessageSource.OnValhallaArmorEquipped);
				}
			}
		}
    }
}
