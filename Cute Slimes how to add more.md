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
  * Add a new Color to `enum PetColor` at the bottom **in alphabetic order**

***


 (2) Items/PetAccessories/PetAccessoryClass.cs

* In AddAltTextures(), add a new argument **in alphabetic order** and into `intArray`
that corresponds to the color you added in (1)

***

