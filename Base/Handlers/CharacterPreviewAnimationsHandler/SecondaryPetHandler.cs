using System.Collections.Generic;
using Terraria;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using System.Linq;

namespace AssortedCrazyThings.Base.Handlers.CharacterPreviewAnimationsHandler
{
	[Content(ConfigurationSystem.AllFlags)]
	public class SecondaryPetHandler : AssSystem
	{
		private static Dictionary<int, List<(int, bool)>> MainProjToProjs { get; set; }

		private delegate Projectile CreatePetProjectileDummyDelegate(int projectileId);
		private static FieldInfo PetProjectilesField { get; set; }
		private static FieldInfo PlayerField { get; set; }
		private static MethodInfo CreatePetProjectileDummyMethod { get; set; }

		public override void OnModLoad()
		{
			MainProjToProjs = new Dictionary<int, List<(int, bool)>>();

			var type = typeof(UICharacter);
			PetProjectilesField = type.GetField("_petProjectiles", BindingFlags.Instance | BindingFlags.NonPublic);
			PlayerField = type.GetField("_player", BindingFlags.Instance | BindingFlags.NonPublic);
			CreatePetProjectileDummyMethod = type.GetMethod("PreparePetProjectiles_CreatePetProjectileDummy", BindingFlags.Instance | BindingFlags.NonPublic);

			On_UICharacter.PreparePetProjectiles += On_UICharacter_PreparePetProjectiles;
		}

		public override void Unload()
		{
			MainProjToProjs = null;
		}

		/// <summary>
		/// <paramref name="proj"/> will appear alongside <paramref name="mainProj"/> (via <see cref="Terraria.ID.ProjectileID.Sets.CharacterPreviewAnimations"/>)
		/// <br/><paramref name="behind"/> dictates if <paramref name="proj"/> should appear behind or infront of <paramref name="mainProj"/>
		/// </summary>
		public static void AddToMainProj(int mainProj, int proj, bool behind = true)
		{
			if (!MainProjToProjs.ContainsKey(mainProj))
			{
				MainProjToProjs[mainProj] = new List<(int, bool)>();
			}

			MainProjToProjs[mainProj].Add((proj, behind));
		}

		private static void On_UICharacter_PreparePetProjectiles(On_UICharacter.orig_PreparePetProjectiles orig, UICharacter self)
		{
			orig(self);

			if (MainProjToProjs.Count == 0)
			{
				return;
			}

			try
			{
				var _petProjectiles = (Projectile[])PetProjectilesField.GetValue(self);

				if (_petProjectiles.Length == 0)
				{
					return;
				}

				//This now means that a pet exists, and the bonus pets can be registered
				var _player = (Player)PlayerField.GetValue(self);

				var main = _petProjectiles[0];

				if (!MainProjToProjs.TryGetValue(main.type, out var pets) || pets.Count == 0)
				{
					return;
				}
				var list = _petProjectiles.ToList();

				var CreatePetProjectileDummy = CreatePetProjectileDummyMethod.CreateDelegate<CreatePetProjectileDummyDelegate>(self);
				int index = 0;
				foreach (var pet in pets)
				{
					index++;
					var proj = CreatePetProjectileDummy(pet.Item1);
					proj.whoAmI = index; //Extra to be able to distinguish the order of the pet
					if (pet.Item2)
					{
						//Add to beginning so that secondary ones draw behind
						list.Insert(0, proj);
					}
					else
					{
						//Add to end so that secondary ones draw over
						list.Add(proj);
					}
				}

				PetProjectilesField.SetValue(self, list.ToArray());
			}
			catch
			{
				AssUtils.Instance.Logger.Warn("Error during " + nameof(On_UICharacter_PreparePetProjectiles));
			}
		}
	}
}
