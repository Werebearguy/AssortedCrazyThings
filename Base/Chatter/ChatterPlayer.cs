using AssortedCrazyThings.Base.Data;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	[Content(ConfigurationSystem.AllFlags)]
	public class ChatterPlayer : AssPlayerBase
	{
		private EquipSnapshot currentEquips;
		private EquipSnapshot prevEquips;

		private int currentItemType = -1;
		private int prevItemType = -1;

		public override void Initialize()
		{
			currentEquips = new EquipSnapshot();
			prevEquips = new EquipSnapshot();
		}

		public override void ResetEffects()
		{
			prevEquips.CopyFrom(currentEquips);
			currentEquips.Reset();

			prevItemType = currentItemType;
			currentItemType = -1;
		}

		public override void OnEnterWorld()
		{
			ChatterSystem.OnEnterWorld(Player);
		}

		public override void PostUpdate()
		{
			HandleItemSelectedChatter();
		}

		public override void UpdateVisibleVanityAccessories()
		{
			HandleArmorEquipChatter();
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (Player.whoAmI != Main.myPlayer)
			{
				return;
			}

			if (hurtInfo.Damage < Player.statLifeMax2 * 0.2f)
			{
				return;
			}

			ChatterSystem.OnPlayerHurt(Player, npc, hurtInfo);
		}

		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
		{
			if (proj.npcProj || Player.whoAmI != Main.myPlayer)
			{
				return;
			}
			
			//Handle hits from traps separately and on lower cd
			if (proj.trap)
			{
				ChatterSystem.OnPlayerHurtByTrap(Player, proj, hurtInfo);
			}
			else if (hurtInfo.Damage > Player.statLifeMax2 * 0.2f)
			{
				ChatterSystem.OnPlayerHurt(Player, proj, hurtInfo);
			}
		}

		private void HandleItemSelectedChatter()
		{
			if (Main.myPlayer != Player.whoAmI)
			{
				return;
			}

			currentItemType = Player.HeldItem.type;

			if (currentItemType != prevItemType)
			{
				ChatterSystem.OnItemSelected(Player, currentItemType, prevItemType);
			}
		}

		private void HandleArmorEquipChatter()
		{
			if (Main.gameMenu || Main.myPlayer != Player.whoAmI)
			{
				return;
			}

			currentEquips.TakeSnapshot(Player);

			if (!currentEquips.Equals(prevEquips))
			{
				ChatterSystem.OnArmorEquipped(Player, currentEquips, prevEquips);
			}
		}
	}
}
