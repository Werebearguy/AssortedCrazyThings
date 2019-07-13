## Alternate Skins

 (0) Prerequisites

>If you just want to add new textures to existing pets that already have alt textures,
>add a new texture in (1), and expand the tooltip list in (4), nothing else required.
>
>For pets that don't have alt textures yet, do everything from scratch besides the declaration of
>the `public bool ClassName = false;` thing that you should already have

* "ClassName" excludes the "Proj" suffix, it will be mentioned in the guide when you need to add it
* if the projectile doesn't have "Proj" in its name, you don't need to include it manually, but for things like
classNameType, there should be no "Proj" in it
* in PetPlayer.cs: `public bool ClassName = false;` and `ClassName = false;` in ResetEffects()
* in each step after the second, the place where you need to add stuff is marked via `//ALTERNATE`,
please don't remove the commented out sample code, instead, add your stuff between the existing code and the sample code
* Example 1: `ClassName == YoungWyvern` (no Proj), `classNameType == youngWyvernType`
* Example 2: `ClassName == PetFishronProj`, `classNameType == petFishronType`

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
public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{
    PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
    SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    Texture2D image = mod.GetTexture("Projectiles/Pets/ClassNameProj_" + mPlayer.classNameType);
    Rectangle bounds = new Rectangle
    {
        X = 0,
        Y = projectile.frame,
        Width = image.Bounds.Width,
        Height = image.Bounds.Height / Main.projFrames[projectile.type]
    };
    bounds.Y *= bounds.Height;

    Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + projectile.gfxOffY);

    spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

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

public void GetFromClonedTypes(string mp = "")
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
adjust `stupidOffset` accordingly in PreDraw() (example: YoungWyvern.cs)


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
    triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
    condition: () => ClassName,
    uiConf: GetClassNameConf,
    onUIStart: () => classNameType,
    onUIEnd: () => classNameType = (byte)CircleUI.returned,
    needsSaving: true
),
```

***


Finally, you can go into PetPlayer.cs and search for "//ALTERNATE" to see if you implemented everything (examples included in each instance):
 * 7 occurences (2 of which are ticked off already if you do it on an existing pet)
 