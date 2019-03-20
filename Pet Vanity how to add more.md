## Moar Pet Vanity

Everything is done in the Items/PetAccessories/PetAccessoryClass.cs file

(1) Add Item

* add a new class under `NEW CLASSES GO HERE vvvvvvvvvvvvvvvvvvv`
* suggestion for names : prefixed with "Cute ", so its easy to find in recipe browser 
* template: (replace `Thing` with whatever the item is)

```
public class PetAccessoryThing : PetAccessoryItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Cute Thing");
        Tooltip.SetDefault("'A thing for your cute slime to wear on a certain body part'");
    }
}
```

* Add the texture for the item to the folder, now the game should load
with the item properly, but it won't do anything yet

***


(2) Register properties

* In Load():
  * call `Add()` with the appropriate parameters in its appropriate place (by slots)
  * watch out for brackets, they're finnicky here
  * the first parameter of `Add()` is always the SlotType, the second one is a 'PetAccessory' object, which has the following parameters:
  * `id`: set that to the next highest ID thats specified in the other `Add()` calls for that particular SlotType
  * `name`: the name of the class without the 'PetAccessory' infront, to save space
  * `offsetX/Y`: self explanatory, remember, negative X is left, negative Y is up
  * `preDraw`: decides if that accessory should be drawn "behind" the actual slime (false means it will draw infront)
  * `alpha`: says by how much it should be transparent (0 is fully opaque, 255 fully transparent)
  * `useNoHair`: used for SlotType.Hat, if the accessory should cover the hair and should use a NoHair texture of the slime if available
  * `altTextures`: a List of names that denotes the selection options for the UI
  * For each `altTexture` thing you specify in the list, you need to add a texture (for the UI) suffixed with that thing,
and a `_Draw` texture (there will be a duplicate icon for the default item)

* after you've done that, add a `_Draw` texture with the same name as the item you add, now the item should work

* if you want to add alternative textures based on the pet they are on (Suffixed with `_Draw<identifyingNumber>`), call AddPetVariation()
  on the PetAccessory object (watch the brackets) and assign each pet a texture to use
  (-1 is "not rendered", 0 is "default, > 0 is "use `_Draw<identifyingNumber>` texture")
  you can leave the other pet types out if you only need to adjust the texture of one pet

* if you want to remove certain accessories from being usable for the system, comment the Add() call out with //

***

