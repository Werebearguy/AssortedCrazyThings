using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles
{
    class PetAccessoryProj: GlobalProjectile
    {
        public PetAccessoryProj()
        {

        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        private const uint mask = 255;//0000 0000|0000 0000|0000 0000|1111 1111 
        private uint slots = 0;       //0000 0000|0000 0000|0000 0000|0000 0000 
                                      //slt3     |slt2     |slt1     |slt0     

        private bool AddAccessory(byte slotNumber, uint type)
        {
            //type is between 0 and 255

            //returns false if accessory was already equipped   //for slotNumber = 1:
            uint setmask = mask << (slotNumber * 8);            //0000 0000|0000 0000|1111 1111|0000 0000
            uint clearmask = ~setmask; //setmask but inverted   //1111 1111|1111 1111|0000 0000|1111 1111

            //Main.NewText("added " + type%256 + " to slot " + slotNumber);
            type = type << (slotNumber * 8);
            uint tempslots = slots & setmask;
            if (type == tempslots) return false;

            //if accessory not the same as the applied one: override/set
            slots &= clearmask; //delete only current slot
            slots |= type; //set current slot

            return true;
        }

        private void DelAccessory(byte slotNumber)
        {
            uint setmask = mask << (slotNumber * 8);
            uint clearmask = ~setmask; //setmask but inverted
            slots &= clearmask; //delete only current slot

            //Main.NewText("deleted from slot " + slotNumber);
        }

        public uint ToggleAccessory(byte slotNumber, uint type)
        {
            if (slotNumber == 0) return slots;
            slotNumber -= 1;
            //Main.NewText("before: " + slots);
            if (!AddAccessory(slotNumber, type)) DelAccessory(slotNumber);
            //Main.NewText("after : " + slots);
            return slots;
        }

        public uint SetAccessoryAll(uint slotsvar)
        {
            slots = slotsvar;
            return slots;
        }

        public uint GetAccessoryAll()
        {
            return slots;
        }

        public uint GetAccessory(byte slotNumber)
        {
            slotNumber -= 1;
            return (slots >> (slotNumber * 8)) & mask; //shift the selected 8 bits of the slot into the rightmost position
        }
    }
}
