using AssortedCrazyThings.Items.Pets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using AssortedCrazyThings.Items.Pets.CuteSlimes;

namespace AssortedCrazyThings.NPCs
{
    [Content(ContentType.DroppedPets)]
    public class DroppedPetsNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //Other pets
            if (npc.type == NPCID.Antlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.WalkingAntlion)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MiniAntlionItem>(), chanceDenominator: 75));
            }
            else if (npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinWarrior)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GobletItem>(), chanceDenominator: 200));
            }
            else if (npc.type == NPCID.DarkMummy || npc.type == NPCID.LightMummy || npc.type == NPCID.Mummy)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LilWrapsItem>(), chanceDenominator: 75));
            }
            else if (npc.type == NPCID.RainbowSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RainbowSlimeItem>(), chanceDenominator: 4));
            }
            else if (npc.type == NPCID.IlluminantSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IlluminantSlimeItem>(), chanceDenominator: 100));
            }

            //Boss pets
            else if (npc.type == NPCID.KingSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrinceSlimeItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.EyeofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SwarmofCthulhuItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.BrainofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainofConfusionItem>(), chanceDenominator: 10));
            }
            else if (npc.boss && Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetEaterofWorldsItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.QueenBee)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QueenLarvaItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkeletronHandItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WallFragmentItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.TheDestroyer)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetDestroyerItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.SkeletronPrime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkeletronPrimeHandItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
            {
                LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
                leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TinyTwinsItem>(), chanceDenominator: 10));
                npcLoot.Add(leadingConditionRule);
            }
            else if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CuteSlimeQueenItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.Plantera)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetPlanteraItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetGolemHeadItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetFishronItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.HallowBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FairySwarmItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.CultistBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetCultistItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TrueObservingEyeItem>(), chanceDenominator: 10));
            }
        }
    }
}
