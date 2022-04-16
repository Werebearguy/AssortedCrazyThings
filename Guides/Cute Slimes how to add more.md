## Moar Cute Slimes

 (0) Prerequisites

* Do the usual stuff when adding a pet (item, buff, projectile). Naming conventions:
  * Item name: `CuteSlimeColor`
  * Buff name: `CuteSlimeColorBuff`
  * Projectile name: `CuteSlimeColorProj`
  * In PetPlayer and Buff: `public bool CuteSlimeColor = false;`, the `CuteSlimeColor` is used
in the Buff class like this: `mPlayer.CuteSlimeColor`

* For the buff, use the template of previous cute slimes.

***


 (1) SlimePets.cs

* In LoadPets(): (all adds recommended in alphabetic order)
  * Add the NPC to `slimePetRegularNPCs` (if applicable)
  * Add the pet via `Add(SlimePet.NewSlimePet())` **<-MUST HAVE FOR ACCESSORIES TO WORK**
  * Follow the "TEMPLATE EXPLANATION" comments or copypaste a similar pet and adjust that

***


 (2) Items/PetAccessories/PetAccessoryClass.cs

* read the "ADD PET ACCESSORY PROPERTIES HERE" comments


***

