using System;
using System.Collections.Generic;
using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CuteSlimeGlobalTooltip : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            try
            {
                PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>(mod);
                //checks if: player even has (or had) a slime pet
                //that pet is currently active
                //it's owner is the player
                //it's a slime pet
                if (pPlayer.slimePetIndex != -1 &&
                    Main.projectile[pPlayer.slimePetIndex].active &&
                    Main.projectile[pPlayer.slimePetIndex].owner == Main.myPlayer &&
                    SlimePets.slimePets.Contains(Main.projectile[pPlayer.slimePetIndex].type))
                {
                    //checks if this item is infact a pet slime summoning item
                    if (item.shoot == Main.projectile[pPlayer.slimePetIndex].type)
                    {
                        for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
                        {
                            PetAccessory petAccessory = pPlayer.GetAccessoryInSlot(slotNumber);

                            string tooltip = "";

                            if (SlimePets.GetPet(Main.projectile[pPlayer.slimePetIndex].type).IsSlotTypeBlacklisted[slotNumber])
                            {
                                tooltip = "Blacklisted";
                            }
                            else
                            {
                                if (petAccessory != null)
                                {
                                    //type = PetAccessory.Items[accessory - 1];
                                    Item itemTemp = new Item();
                                    itemTemp.SetDefaults(petAccessory.Type);
                                    tooltip = itemTemp.Name.StartsWith("Cute ") ? itemTemp.Name.Substring(5) : itemTemp.Name;
                                    tooltip += petAccessory.HasAlts ? " (" + petAccessory.AltTextureSuffixes[petAccessory.Color] + ")" : "";
                                }
                                else
                                {
                                    tooltip = "None";
                                }
                            }
                            
                            tooltips.Add(new TooltipLine(mod, ((SlotType)slotNumber).ToString(), Enum2String(slotNumber) + tooltip));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Main.NewText(e, Color.Red);
            }
        }

        private string Enum2String(byte b)
        {
            if (b == (byte)SlotType.Hat)
            {
                return "Hat slot: ";
            }
            if (b == (byte)SlotType.Body)
            {
                return "Body slot: ";
            }
            if (b == (byte)SlotType.Carried)
            {
                return "Carry slot: ";
            }
            if (b == (byte)SlotType.Accessory)
            {
                return "Accessory slot: ";
            }
            return "UNINTENDED BEHAVIOR, REPORT TO DEV! (" + b + ")";
        }
    }
}
