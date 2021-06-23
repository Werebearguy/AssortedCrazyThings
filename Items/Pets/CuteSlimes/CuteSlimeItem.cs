using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    [Content(ContentType.CuteSlimes)]
    public abstract class CuteSlimeItem : SimplePetItemBase
    {
        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            try
            {
                PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>();
                if (pPlayer.HasValidSlimePet(out SlimePet slimePet))
                {
                    Projectile projectile = Main.projectile[pPlayer.slimePetIndex];
                    //checks if this item is infact a pet slime summoning item
                    if (Item.shoot == projectile.type)
                    {
                        for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
                        {
                            PetAccessory petAccessory = pPlayer.GetAccessoryInSlot(slotNumber);

                            string tooltip = "";

                            if (slimePet.IsSlotTypeBlacklisted[slotNumber])
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

                            tooltips.Add(new TooltipLine(Mod, ((SlotType)slotNumber).ToString(), Enum2String(slotNumber) + tooltip));
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
