using System;
using System.Collections.Generic;
using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Projectiles.Pets;
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
                AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);
                //checks if: player even has (or had) a slime pet
                //that pet is currently active
                //its owner is the player
                //its a slime pet
                if (mPlayer.petIndex != -1 && Main.projectile[mPlayer.petIndex].active && Main.projectile[mPlayer.petIndex].owner == Main.myPlayer && typeof(CuteSlimeBasePet).IsInstanceOfType(Main.projectile[mPlayer.petIndex].modProjectile))
                {
                    //checks if this item is infact a pet slime summoning item
                    if (item.shoot == Main.projectile[mPlayer.petIndex].type)
                    {
                        for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
                        {
                            uint accessory = mPlayer.GetAccessoryPlayer(slotNumber);
                            int type;
                            Item itemTemp;

                            if (accessory != 0)
                            {
                                type = PetAccessory.Items[accessory - 1];
                                itemTemp = new Item();
                                itemTemp.SetDefaults(type);
                                tooltips.Add(new TooltipLine(mod, ((SlotType)slotNumber).ToString(), Enum2String(slotNumber) + itemTemp.Name.TrimStart("Cute ".ToCharArray())));
                            }
                            else
                            {
                                tooltips.Add(new TooltipLine(mod, ((SlotType)slotNumber).ToString(), Enum2String(slotNumber) + "None"));
                            }
                        }
                    }
                }
            }
            catch
            {

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
            if (b == (byte)SlotType.Misc)
            {
                return "Misc slot: ";
            }
            return "UNINTENDED BEHAVIOR, REPORT TO DEV";
        }
    }
}
