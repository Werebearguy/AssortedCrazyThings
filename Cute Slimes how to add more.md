## Moar Cute Slimes

 (0) Prerequisites

* Do the usual stuff when adding a pet (item, buff, projectile). Naming conventions:
  * Item name: `CuteSlimeColorNew`
  * Buff name: `CuteSlimeColorNewBuff`
  * Projectile name: `CuteSlimeColorNewProj`
  * In PetPlayer and Buff: `public bool CuteSlimeColorNew = false;`, the `CuteSlimeColorNew` is used
in the Buff class like this: `mPlayer.CuteSlimeColorNew`

* For the buff, use the template of previous cute slimes.

***


 (1) SlimePets.cs

* In LoadPets(): (all adds recommended in alphabetic order)
  * Add the NPC to `slimePetNPCs`
  * Add the pet to `slimePetList` **<-MUST HAVE FOR ACCESSORIES TO WORK**
  * Follow the "TEMPLATE EXPLANATION" comment or copypaste a similar pet and adjust that

***


 (2) Items/PetAccessories/PetAccessoryClass.cs

* if you want to add alternative textures based on the pet they are on (Suffixed with `_Draw<identifyingNumber>`), call AddPetVariation()
  on the PetAccessory object (watch the brackets) and assign each pet a texture to use
  (-1 is "not rendered", 0 is "default, > 0 is "use `_Draw<identifyingNumber>` texture")
  you can leave the other pet types out if you only need to adjust the texture of one pet


***

