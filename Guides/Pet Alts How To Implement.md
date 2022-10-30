## Alternate Skins

 (0) Prerequisites

>If you just want to add new textures to existing pets that already have alt textures,
>add a new texture in (1), and expand the tooltip list in (4), nothing else required.
>
>For pets that don't have alt textures yet, do everything from scratch besides the declaration of
>the `public bool ClassName = false;` thing that you should already have

* for things like classNameType, there should be no "Proj" in it
* in PetPlayer.cs: `public bool ClassName = false;` and `ClassName = false;` in ResetEffects()
* in each step after the second, the place where you need to add stuff is marked via `//ALTERNATE`,
please don't remove the commented out sample code, instead, add your stuff between the existing code and the sample code
* Example: `ClassName == PetFishronProj`, `classNameType == petFishronType`

***


 (1) Textures

* Name the textures like this: ClassNameProj_number and put them in the same folder as the class

***


 (2) Class File

* Add this just after `public class ... {`:

```csharp
public override string Texture
{
    get
    {
        return "AssortedCrazyThings/Projectiles/Pets/ClassNameProj_0";
    }
}
```
* Add this at the very end (it should work for both flying and walking pets)
```csharp
//you need those usings:
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using AssortecCrazyThings.Base;
public override bool PreDraw(ref Color lightColor)
{
    PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
    SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    Texture2D image =  Mod.Assets.Request<Texture2D>("Projectiles/Pets/ClassNameProj_" + mPlayer.classNameType).Value;
    Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

    Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + projectile.gfxOffY);

    Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

    return false;
}
```

***



 (3) PetPlayer File

* Create a byte variable where the alt texture is saved and add it to the these two functions:

```csharp
//name pet texture
public byte classNameType = 0;

[...]

public override void Initialize()
{
    [..all the other pet types..]
    classNameType = 0;
}

public void GetFromClonedTypes()
{
    [..all the other pet types..]
    classNameType = ClonedTypes[index++];
}

public void SetClonedTypes()
{
	{
		[..all the other pet types..]
		ClonedTypes[++index] = classNameType;
	}
}
```

* At this point, the pet will render with its _0 texture selected.
Check with Modder's Toolkit if the hitbox aligns with the texture, if not,
adjust `stupidOffset` accordingly in PreDraw() (example: YoungWyvernProj.cs)


***


 (4) Register textures, add tooltips in PetPlayer.cs

* Before Initialize(), add this:
* "Default" is the \_0 texture, "AltName1" is the \_1 texture etc.
* It will only register the number of textures specified as tooltips,
so if you have 10 textures but only name 6 tooltips it will only pick the textures from 0 to 5 (as opposed to 0 to 9)
```csharp
public static CircleUIConf GetClassNameConf()
{
    List<string> tooltips = new List<string>() { "Default", "AltName1", "AltName2" };

    return CircleUIHandler.PetConf("ClassNameProj", tooltips);
}
```
* At the end of Initialize(), add this:

```csharp
    new CircleUIHandler(
    triggerItem: ModContent.ItemType<VanitySelector>(),
    condition: () => ClassName,
    uiConf: GetClassNameConf,
    onUIStart: () => classNameType,
    onUIEnd: () => classNameType = (byte)CircleUI.returned
),
```

***


Finally, you can go into PetPlayer.cs and search for "//ALTERNATE" to see if you implemented everything (examples included in each instance):
 * 7 occurences (2 of which are ticked off already if you do it on an existing pet)
 