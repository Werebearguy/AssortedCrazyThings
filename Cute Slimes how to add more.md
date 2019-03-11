## Moar Cute Slimes

 (0) Prerequisites

* Do the usual stuff when adding a pet (item, buff, projectile). Naming conventions:
  * Item name: `CuteSlimeColorNew`
  * Buff name: `CuteSlimeColorNewBuff`
  * Projectile name: `CuteSlimeColorNewProj`
  * In PetPlayer and Buff: `public bool CuteSlimeColorNew = false;`, the `CuteSlimeColorNew` is used
in the Buff class like this: `mPlayer.CuteSlimeColorNew`

* For the buff, use the template of previous cute slimes, and write a new custom color.

***


 (1) Projectiles/Pets/CuteSlimeBaseProj.cs

* Add a new color **in alphabetic order** to `enum PetColor`.

***


 (2) Items/PetAccessories/PetAccessoryClass.cs

* In AddAltTextures(), add a new argument **in alphabetic order** and into `intArray`
that corresponds to the color you added in (1)

***

 (3) AssortedCrazyThings.cs

* In LoadPets():  (all adds recommended in alphabetic order but not necessary)
  * Add the NPC to `slimePetNPCs`
  * Add the pet to `slimePets` **<-MUST HAVE FOR ACCESSORIES TO WORK**
  * If the pet has a noHair texture, add it to `slimePetNoHair`

* If you want to add an `Addition` texture or want to have it so a certain slime
can't equip certain accessories, ask me

