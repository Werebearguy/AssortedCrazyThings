## Moar Cute Slimes

 (0) Prerequisites

* Do the usual stuff when adding a pet (item, buff, projectile). Naming conventions:
  * Item name: `CuteSlimeColorNew`
  * Buff name: `CuteSlimeColorNewBuff`
  * Projectile name: `CuteSlimeCorruptNewPet`
  * in PetPlayer and Buff: `public bool CuteSlimeColorNew = false;`, the `CuteSlimeColorNew` is used
in the Buff class like this: `mPlayer.CuteSlimeColorNew`

* For the buff, use the template of previous cute slimes, and write a new custom color.

***


 (1) Projectiles/Pets/CuteSlimeBasePet.cs

* Add a new color **in alphabetic order** to `enum PetColor`.

***


 (2) Items/PetAccessories/PetAccessoryClass.cs

* in AddAltTextures(), add a new argument **in alphabetic odrder** and into `intArray`
that corresponds to the color you added in (1)

***

 (3) AssortedCrazyThings.cs

* in LoadPets(), if the pet has a noHair texture, add it to `slimePetNoHair`. Then add the NPC to
`slimePetNPCs`. (recommended in alphabetic order but not necessary)
