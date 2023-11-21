using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter.GoblinUnderlings
{
	//Designed to only have one message group
	public class GoblinUnderlingDialogueChatterGenerator : GoblinUnderlingChatterGenerator
	{
		public GoblinUnderlingDialogueChatterGenerator(GoblinUnderlingDialogueParticipantOrder participants, ChatterMessageGroup messageGroup) :
			base($"{participants.first}_{participants.second}" + (participants.third == GoblinUnderlingChatterType.None ? "" : $"_{participants.third}"), Color.White)
		{
			Chatters = new Dictionary<ChatterSource, ChatterMessageGroup>() { { ChatterSource.Dialogue, messageGroup } };
		}

		public bool TryCreate(GoblinUnderlingChatterHandler handler, Projectile projectile, IChatterParams param, Vector2? position = null, Vector2? velocity = null, int? cooldownOverride = null)
		{
			if (!handler.DialogueOngoing)
			{
				return false;
			}

			if (Main.myPlayer != projectile.owner)
			{
				return false;
			}

			if (ClientConfig.Instance.GoblinUnderlingChatterDisabled)
			{
				return false;
			}

			Vector2 pos = position ?? projectile.Top;
			Vector2 vel = velocity ?? new Vector2(0, Main.rand.NextFloat(-3.5f, -2f));

			//AssUtils.Print($"Try spawn dialogue for {projectile.Name[0..6]}");
			return TryCreate(ChatterSource.Dialogue, pos, vel, param, cooldownOverride);
		}

		protected override void ModifyRequest(ChatterSource source, IChatterParams param, ref AdvancedPopupRequest request)
		{
			if (param is not DialogueChatterParams p)
			{
				return;
			}

			request.Color = p.Color;
			request.DurationInFrames = p.Duration;
		}
	}
}
