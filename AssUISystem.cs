using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Accessories.Vanity;
using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Tiles;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace AssortedCrazyThings
{
	[Content(ConfigurationSystem.AllFlags)]
	public class AssUISystem : AssSystem
	{
		/// <summary>
		/// Zoom level, (for UIs). 0f == fully zoomed out, 1f == fully zoomed in
		/// </summary>
		public static Vector2 ZoomFactor;

		//UI stuff
		internal static UserInterface CircleUIInterface;
		internal static CircleUI CircleUI;

		internal static UserInterface HoverNPCUIInterface;
		internal static HoverNPCUI HoverNPCUI;

		internal static UserInterface EnhancedHunterUIInterface;
		internal static EnhancedHunterUI EnhancedHunterUI;

		internal static UserInterface PetVanityUIInterface;
		internal static PetVanityUI PetVanityUI;

		/// <summary>
		/// Contains a list of CircleUIHandlers that are used in CircleUIStart/End in Mod
		/// </summary>
		public static List<CircleUIHandler> CircleUIList;

		/// <summary>
		/// Contains a list of CircleUIHandlers that are used in CircleUIStart/End in Mod
		/// </summary>
		public static List<CircleUIHandler> CircleUIListPets;

		public override void PostSetupContent()
		{
			if (!Main.dedServ && Main.netMode != NetmodeID.Server)
			{
				CircleUI = new CircleUI();
				CircleUI.Activate();
				CircleUIInterface = new UserInterface();
				CircleUIInterface.SetState(CircleUI);

				HoverNPCUI = new HoverNPCUI();
				HoverNPCUI.Activate();
				HoverNPCUIInterface = new UserInterface();
				HoverNPCUIInterface.SetState(HoverNPCUI);

				if (ContentConfig.Instance.Bosses)
				{
					EnhancedHunterUI = new EnhancedHunterUI();
					EnhancedHunterUI.Activate();
					EnhancedHunterUIInterface = new UserInterface();
					EnhancedHunterUIInterface.SetState(EnhancedHunterUI);
				}

				if (ContentConfig.Instance.CuteSlimes)
				{
					PetVanityUI = new PetVanityUI();
					PetVanityUI.Activate();
					PetVanityUIInterface = new UserInterface();
					PetVanityUIInterface.SetState(PetVanityUI);
				}

				CircleUIList = new List<CircleUIHandler>();

				if (ContentConfig.Instance.VanityAccessories)
				{
					CircleUIList.AddRange(new List<CircleUIHandler>
					{
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<SillyBalloonKit>(),
						condition: () => true,
						uiConf: SillyBalloonKit.GetUIConf,
						onUIStart: () => (int)Main.LocalPlayer.GetModPlayer<AssPlayer>().selectedSillyBalloonType,
						onUIEnd: delegate
						{
							AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
							mPlayer.selectedSillyBalloonType = (BalloonType)(byte)CircleUI.returned;
							AssUtils.UIText(AssLocalization.SelectedText.Format(SillyBalloonKit.Enum2string(mPlayer.selectedSillyBalloonType)), CombatText.HealLife);
						}
					),
					});
				}

				if (ContentConfig.Instance.Weapons)
				{
					CircleUIList.AddRange(new List<CircleUIHandler>
					{
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<SlimeHandlerKnapsack>(),
						condition: () => true,
						uiConf: SlimeHandlerKnapsack.GetUIConf,
						onUIStart: () => (int)Main.LocalPlayer.GetModPlayer<AssPlayer>().selectedSlimePackMinionType,
						onUIEnd: delegate
						{
							AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
							mPlayer.selectedSlimePackMinionType = (SlimeType)(byte)CircleUI.returned;
							AssUtils.UIText(AssLocalization.SelectedText.Format(SlimeHandlerKnapsack.Enum2string(mPlayer.selectedSlimePackMinionType)), CombatText.HealLife);
						},
						triggerLeft: false
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<DroneController>(),
						condition: () => true,
						uiConf: DroneController.GetUIConf,
						onUIStart: delegate
						{
							AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
							if (Utils.IsPowerOfTwo((int)mPlayer.selectedDroneControllerMinionType))
							{
								return (int)Math.Log((int)mPlayer.selectedDroneControllerMinionType, 2);
							}
							return 0;
						},
						onUIEnd: delegate
						{
							AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
							mPlayer.selectedDroneControllerMinionType = (DroneType)(byte)Math.Pow(2, CircleUI.returned);
							AssUtils.UIText(AssLocalization.SelectedText.Format(DroneController.GetDroneData(mPlayer.selectedDroneControllerMinionType).NameSingular), CombatText.HealLife);
						},
						triggerLeft: false
					)}
					);
				}

				if (ContentConfig.Instance.Bosses)
				{
					CircleUIList.Add(new CircleUIHandler(
					triggerItem: ModContent.ItemType<EverhallowedLantern>(),
					condition: () => true,
					uiConf: EverhallowedLantern.GetUIConf,
					onUIStart: delegate
					{
						if (Utils.IsPowerOfTwo((int)Main.LocalPlayer.GetModPlayer<AssPlayer>().selectedSoulMinionType))
						{
							return (int)Math.Log((int)Main.LocalPlayer.GetModPlayer<AssPlayer>().selectedSoulMinionType, 2);
						}
						return 0;
					},
					onUIEnd: delegate
					{
						AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
						mPlayer.selectedSoulMinionType = (SoulType)(byte)Math.Pow(2, CircleUI.returned);
						AssUtils.UIText(AssLocalization.SelectedText.Format(EverhallowedLantern.GetSoulData(mPlayer.selectedSoulMinionType).NameSingular), CombatText.HealLife);
					},
					triggerLeft: false
					));
				}

				// after filling the list, set the trigger list
				for (int i = 0; i < CircleUIList.Count; i++)
				{
					var circleUIHandler = CircleUIList[i];

					//set the trigger list
					circleUIHandler.AddTriggers();
					circleUIHandler.UIConf(true); //Localization
				}

				CircleUIListPets = new List<CircleUIHandler>();

				if (ContentConfig.Instance.OtherPets)
				{
					CircleUIListPets.AddRange(new List<CircleUIHandler>
					{
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().LifelikeMechanicalFrog,
						uiConf: PetPlayer.GetLifelikeMechanicalFrogConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().mechFrogType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().mechFrogType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().DocileDemonEye,
						uiConf: PetPlayer.GetDocileDemonEyeConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petEyeType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petEyeType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().CursedSkull,
						uiConf: PetPlayer.GetCursedSkullConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().cursedSkullType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().cursedSkullType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().YoungWyvern,
						uiConf: PetPlayer.GetYoungWyvernConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().youngWyvernType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().youngWyvernType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().PetMoon,
						uiConf: PetPlayer.GetPetMoonConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petMoonType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petMoonType = (byte)CircleUI.returned,
						triggerLeft: false
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().Abeemination,
						uiConf: PetPlayer.GetAbeeminationConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().abeeminationType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().abeeminationType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().VampireBat,
						uiConf: PetPlayer.GetVampireBatConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().vampireBatType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().vampireBatType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().Pigronata,
						uiConf: PetPlayer.GetPigronataConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().pigronataType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().pigronataType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().PetGoldfish,
						uiConf: PetPlayer.PetGoldfishConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petGoldfishType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petGoldfishType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().PetAnomalocaris,
						uiConf: PetPlayer.GetAnomalocarisConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petAnomalocarisType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petAnomalocarisType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().PetCloudfish,
						uiConf: PetPlayer.GetPetCloudfishConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petCloudfishType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petCloudfishType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().DynamiteBunny,
						uiConf: PetPlayer.GetDynamiteBunnyConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().dynamiteBunnyType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().dynamiteBunnyType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().KingGuppy,
						uiConf: PetPlayer.GetKingGuppyConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().kingGuppyType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().kingGuppyType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().ShortfuseCrab,
						uiConf: PetPlayer.GetShortfuseCrabConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().shortfuseCrabType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().shortfuseCrabType = (byte)CircleUI.returned
					),
					//ALTERNATE
					//    new CircleUIHandler(
					//    triggerItem: ModContent.ItemType<VanitySelector>(),
					//    condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().ClassName,
					//    uiConf: PetPlayer.GetClassNameConf,
					//    onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().classNameType,
					//    onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().classNameType = (byte)CircleUI.returned
					//),
					});
				}

				if (ContentConfig.Instance.DroppedPets)
				{
					int vanitySelector = ModContent.ItemType<VanitySelector>();
					CircleUIListPets.AddRange(new List<CircleUIHandler>()
					{
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().QueenLarva,
						uiConf: PetPlayer.GetQueenLarvaConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().queenLarvaType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().queenLarvaType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().MiniAntlion,
						uiConf: PetPlayer.GetMiniAntlionConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().miniAntlionType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().miniAntlionType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().LilWraps,
						uiConf: PetPlayer.GetLilWrapsConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().lilWrapsType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().lilWrapsType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().WallFragment,
						uiConf: PetPlayer.GetWallFragmentConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().wallFragmentType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().wallFragmentType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().MetroidPet,
						uiConf: PetPlayer.GetMetroidPetConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().metroidPetType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().metroidPetType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().CuteLamiaPet,
						uiConf: PetPlayer.GetCuteLamiaPetConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().cuteLamiaPetType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().cuteLamiaPetType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().SkeletronHand,
						uiConf: PetPlayer.GetSkeletronHandConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().skeletronHandType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().skeletronHandType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().SkeletronPrimeHand,
						uiConf: PetPlayer.GetSkeletronPrimeHandConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().skeletronPrimeHandType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().skeletronPrimeHandType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().PetCultist,
						uiConf: PetPlayer.GetPetCultistConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petCultistType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petCultistType = (byte)CircleUI.returned,
						triggerLeft: false
					),
						new CircleUIHandler(
						triggerItem: vanitySelector,
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().PetFishron,
						uiConf: PetPlayer.GetPetFishronConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petFishronType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().petFishronType = (byte)CircleUI.returned
					)
					});
				}

				if (ContentConfig.Instance.FriendlyNPCs)
				{
					CircleUIListPets.AddRange(new List<CircleUIHandler>()
					{
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().YoungHarpy,
						uiConf: PetPlayer.GetYoungHarpyConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().youngHarpyType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().youngHarpyType = (byte)CircleUI.returned
					),
					});

					CircleUIListPets.AddRange(new List<CircleUIHandler>()
					{
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().JoyousSlime,
						uiConf: PetPlayer.GetJoyousSlimeConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().joyousSlimeType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().joyousSlimeType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().OceanSlime,
						uiConf: PetPlayer.GetOceanSlimeConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().oceanSlimeType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().oceanSlimeType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().StingSlime,
						uiConf: PetPlayer.GetStingSlimeConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().stingSlimeType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().stingSlimeType = (byte)CircleUI.returned
					),
						new CircleUIHandler(
						triggerItem: ModContent.ItemType<VanitySelector>(),
						condition: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().AnimatedTome,
						uiConf: PetPlayer.GetAnimatedTomeConf,
						onUIStart: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().animatedTomeType,
						onUIEnd: () => Main.LocalPlayer.GetModPlayer<PetPlayer>().animatedTomeType = (byte)CircleUI.returned
					)
					});
				}

				//after filling the list
				for (int i = 0; i < CircleUIListPets.Count; i++)
				{
					var circleUIHandler = CircleUIListPets[i];

					//set the trigger list
					circleUIHandler.AddTriggers();
					circleUIHandler.UIConf(true); //Localization
				}
			}
		}

		public override void Unload()
		{
			if (!Main.dedServ && Main.netMode != NetmodeID.Server)
			{
				CircleUIInterface = null;
				CircleUI = null;

				HoverNPCUIInterface = null;
				HoverNPCUI = null;

				EnhancedHunterUIInterface = null;
				EnhancedHunterUI = null;

				PetVanityUIInterface = null;
				PetVanityUI = null;

				EnhancedHunterUI.arrowTexture = null;
				PetVanityUI.redCrossTexture = null;
				CircleUI.UIConf = null;
				CircleUIHandler.TriggerListLeft.Clear();
				CircleUIHandler.TriggerListRight.Clear();
			}
		}

		/// <summary>
		/// Returns language agnostic representation of ": "
		/// </summary>
		public static string GetColon()
		{
			string divider = ": ";
			if (Language.ActiveCulture.LegacyId == (int)GameCulture.CultureName.Chinese)
			{
				divider = ":";
			}
			return divider;
		}

		/// <summary>
		/// Creates golden dust particles at the projectiles location with that type and LocalPlayer as owner. (Used for pets)
		/// </summary>
		private void PoofVisual(int projType)
		{
			int projIndex = -1;
			//find first occurence of a player owned projectile
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active)
				{
					if (Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == projType)
					{
						projIndex = i;
						break;
					}
				}
			}

			if (projIndex != -1)
			{
				Dust dust;
				for (int i = 0; i < 14; i++)
				{
					dust = Dust.NewDustDirect(Main.projectile[projIndex].position, Main.projectile[projIndex].width, Main.projectile[projIndex].height, 204, Main.projectile[projIndex].velocity.X, Main.projectile[projIndex].velocity.Y, 0, new Color(255, 255, 255), 0.8f);
					dust.noGravity = true;
					dust.noLight = true;
				}
			}
		}

		/// <summary>
		/// Called when CircleUI starts
		/// </summary>
		private static void CircleUIStart(int triggerType, bool triggerLeft = true, bool fromDresser = false)
		{
			AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
			PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>();

			//combine both lists of the players (split for organization and player load shenanigans)
			List<CircleUIHandler> l = CircleUIList;
			l.AddRange(CircleUIListPets);

			bool found = false;
			for (int i = 0; i < l.Count; i++)
			{
				var handler = l[i];
				if (handler.Condition())
				{
					if (handler.TriggerItem == triggerType && handler.TriggerLeft == triggerLeft)
					{
						CircleUI.UIConf = handler.UIConf(false);
						CircleUI.currentSelected = handler.OnUIStart();
						found = true;
						break;
					}
				}
			}
			//extra things that happen
			if (!found)
			{
				if (triggerType == ModContent.ItemType<VanitySelector>())
				{
					var text = triggerLeft ? ModContent.GetInstance<VanityDresserTile>().NoCostumesFoundPetText :
						ModContent.GetInstance<VanityDresserTile>().NoCostumesFoundLightPetText;
					AssUtils.UIText(text.ToString(), CombatText.DamagedFriendly);
					return;
				}
			}

			//Spawn UI
			CircleUI.Start(triggerType, triggerLeft, fromDresser);
		}

		/// <summary>
		/// Called when CircleUI ends
		/// </summary>
		private void CircleUIEnd(bool triggerLeft = true)
		{
			if (CircleUI.returned != CircleUI.NONE && CircleUI.returned != CircleUI.currentSelected)
			{
				//if something returned AND if the returned thing isn't the same as the current one

				try
				{
					SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.6f });
				}
				catch
				{
					//No idea why but this threw errors one time
				}

				List<CircleUIHandler> l = CircleUIList;
				l.AddRange(CircleUIListPets);
				for (int i = 0; i < l.Count; i++)
				{
					if (l[i].Condition())
					{
						if (l[i].TriggerItem == CircleUI.triggerItemType)
						{
							if (l[i].TriggerLeft == triggerLeft)
							{
								l[i].OnUIEnd();
								break;
							}
						}
					}
				}
				//extra things that happen
				if (CircleUI.triggerItemType == ModContent.ItemType<VanitySelector>())
				{
					PoofVisual(CircleUI.UIConf.AdditionalInfo);
					AssUtils.UIText(AssLocalization.SelectedText.Format(CircleUI.UIConf.Tooltips[CircleUI.returned]), CombatText.HealLife);
				}
			}

			CircleUI.returned = CircleUI.NONE;
			CircleUI.visible = false;
		}

		/// <summary>
		/// Called in UpdateUI
		/// </summary>
		private void UpdateCircleUI()
		{
			Player player = Main.LocalPlayer;
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

			int triggerType = player.HeldItem.type;
			bool openWithDresser = mPlayer.mouseoveredDresser;
			if (openWithDresser)
			{
				triggerType = ModContent.ItemType<VanitySelector>();
			}
			bool? left = null;
			if (mPlayer.LeftClickPressed && (CircleUIHandler.TriggerListLeft.Contains(triggerType) || openWithDresser))
			{
				left = true;
			}
			else if (mPlayer.RightClickPressed && (CircleUIHandler.TriggerListRight.Contains(triggerType) || openWithDresser))
			{
				left = false;
			}

			if (left != null && AllowedToOpenUI()) CircleUIStart(triggerType, (bool)left, openWithDresser);

			if (CircleUI.visible)
			{
				left = null;
				if (mPlayer.LeftClickReleased)
				{
					left = true;
				}
				else if (mPlayer.RightClickReleased)
				{
					left = false;
				}

				if (left != null && left == CircleUI.openedWithLeft) CircleUIEnd((bool)left);

				if (CircleUI.triggerItemType != triggerType && !CircleUI.triggeredFromDresser) //cancel the UI when you switch items
				{
					CircleUI.returned = CircleUI.NONE;
					CircleUI.visible = false;
				}
			}
		}

		/// <summary>
		/// Called in UpdateUI
		/// </summary>
		private void UpdatePetVanityUI()
		{
			Player player = Main.LocalPlayer;
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
			PetPlayer pPlayer = player.GetModPlayer<PetPlayer>();

			int itemType = player.HeldItem.type;
			if (mPlayer.LeftClickPressed && AllowedToOpenUI() && PetAccessory.TryGetAccessoryFromItem(itemType, out PetAccessory petAccessory))
			{
				if (petAccessory.HasAlts && pPlayer.HasValidSlimePet(out SlimePet slimePet) &&
					!slimePet.IsSlotTypeBlacklisted[(int)petAccessory.Slot])
				{
					//Spawn UI
					PetVanityUI.Start(petAccessory);
				}
			}

			if (!PetVanityUI.visible)
			{
				return;
			}

			if (mPlayer.LeftClickReleased)
			{
				if (PetVanityUI.returned > PetVanityUI.NONE)
				{
					//if something returned AND if the returned thing isn't the same as the current one

					try
					{
						SoundEngine.PlaySound(SoundID.Item1, player.position);
					}
					catch
					{
						//No idea why but this threw errors one time
					}
					//UIText(SelectedText.Format(PetVanityUI.petAccessory.AltTextureSuffixes[PetVanityUI.returned]), CombatText.HealLife);

					PetVanityUI.petAccessory.AltTextureIndex = (byte)PetVanityUI.returned;
					pPlayer.ToggleAccessory(PetVanityUI.petAccessory);
				}
				else if (PetVanityUI.hasEquipped && PetVanityUI.returned == PetVanityUI.NONE)
				{
					//hovered over the middle and had something equipped: take accessory away
					pPlayer.DelAccessory(PetVanityUI.petAccessory);
				}
				//else if (returned == PetVanityUI.IGNORE) {nothing happens}

				PetVanityUI.returned = PetVanityUI.NONE;
				PetVanityUI.visible = false;
			}

			if (PetVanityUI.petAccessory.Type != itemType) //cancel the UI when you switch items
			{
				PetVanityUI.returned = PetVanityUI.NONE;
				PetVanityUI.visible = false;
			}
		}

		private void UpdateHoverNPCUI(GameTime gameTime)
		{
			HoverNPCUI?.Update(gameTime);
		}

		private void UpdateEnhancedHunterUI(GameTime gameTime)
		{
			if (Main.LocalPlayer.GetModPlayer<AssPlayer>().enhancedHunterBuff)
			{
				EnhancedHunterUI.visible = true;
			}
			else
			{
				EnhancedHunterUI.visible = false;
			}
			EnhancedHunterUI?.Update(gameTime);
		}

		public override void UpdateUI(GameTime gameTime)
		{
			UpdateCircleUI();
			UpdateHoverNPCUI(gameTime);
			UpdateEnhancedHunterUI(gameTime);
			UpdatePetVanityUI();
		}

		/// <summary>
		/// Checks if LocalPlayer can open a UI
		/// </summary>
		private bool AllowedToOpenUI()
		{
			return Main.hasFocus &&
				!Main.gamePaused &&
				!Main.LocalPlayer.dead &&
				!Main.LocalPlayer.mouseInterface &&
				!Main.drawingPlayerChat &&
				!Main.editSign &&
				!Main.editChest &&
				!Main.blockInput &&
				!Main.mapFullscreen &&
				!Main.HoveringOverAnNPC &&
				Main.LocalPlayer.cursorItemIconID != -1 &&
				Main.LocalPlayer.talkNPC == -1 &&
				Main.LocalPlayer.itemTime == 0 && Main.LocalPlayer.itemAnimation == 0 &&
				!(Main.LocalPlayer.frozen || Main.LocalPlayer.webbed || Main.LocalPlayer.stoned);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Hotbar"));
			if (inventoryIndex != -1)
			{
				if (CircleUI.visible && CircleUIInterface != null)
				{
					//remove the item icon when using the item while held outside the inventory
					int mouseItemIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Item / NPC Head"));
					if (mouseItemIndex != -1) layers.RemoveAt(mouseItemIndex);
					layers.Insert(++inventoryIndex, new LegacyGameInterfaceLayer
						(
						"ACT: Appearance Select",
						delegate
						{
							CircleUIInterface.Draw(Main.spriteBatch, new GameTime());
							return true;
						},
						InterfaceScaleType.UI)
					);
				}

				if (PetVanityUI.visible && PetVanityUIInterface != null)
				{
					//remove the item icon when using the item while held outside the inventory
					int mouseItemIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Item / NPC Head"));
					if (mouseItemIndex != -1) layers.RemoveAt(mouseItemIndex);
					layers.Insert(++inventoryIndex, new LegacyGameInterfaceLayer
						(
						"ACT: Pet Vanity Select",
						delegate
						{
							PetVanityUIInterface.Draw(Main.spriteBatch, new GameTime());
							return true;
						},
						InterfaceScaleType.UI)
					);
				}
			}

			int mouseOverIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Over"));
			if (mouseOverIndex != -1)
			{
				if (HoverNPCUIInterface != null)
				{
					layers.Insert(++mouseOverIndex, new LegacyGameInterfaceLayer
						(
						"ACT: NPC Mouse Over",
						delegate
						{
							HoverNPCUIInterface.Draw(Main.spriteBatch, new GameTime());
							return true;
						},
						InterfaceScaleType.UI)
					);
				}

				if (EnhancedHunterUI.visible && EnhancedHunterUIInterface != null)
				{
					layers.Insert(++mouseOverIndex, new LegacyGameInterfaceLayer
						(
						"ACT: Enhanced Hunter",
						delegate
						{
							EnhancedHunterUIInterface.Draw(Main.spriteBatch, new GameTime());
							return true;
						},
						InterfaceScaleType.UI)
					);
				}
			}
		}

		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			ZoomFactor = Transform.Zoom - (Vector2.UnitX + Vector2.UnitY);
		}
	}
}
