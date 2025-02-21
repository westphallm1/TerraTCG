using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.CardData;
using TerraTCG.Common.GameSystem.GameState;

namespace TerraTCG.Common.GameSystem.BotPlayer
{
    internal class BotDecks
    {
        public static Card GetCard<T>() where T : BaseCardTemplate, ICardTemplate
            => ModContent.GetInstance<T>().Card;
        
        public static CardCollection GetDeck(int deckIdx = -1)
        {
            var allDecks = new List<Func<CardCollection>> {
                GetJungleDeck,
                GetForestDeck,
                GetBloodMoonDeck,
                GetSkeletonDeck,
                GetGoblinDeck,
                GetMimicDeck,
                GetCrabDeck,
                GetMushroomDeck,
                GetCurseDeck,
                GetSlimeDeck,
            };
            var randIdx = Main.rand.Next(allDecks.Count);
            return allDecks[deckIdx == -1 ? randIdx : deckIdx].Invoke();
        }

        public static CardCollection GetStarterDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Bunny>(), 
                    GetCard<Bunny>(), 
                    GetCard<Squirrel>(), 
                    GetCard<Squirrel>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<Shackle>(), 
                    GetCard<Shackle>(), 
                    GetCard<Zombie>(), 
                    GetCard<Zombie>(), 
                    GetCard<SlimedZombie>(), 
                    GetCard<SlimedZombie>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<DemonEye>(), 
                    GetCard<DemonEye>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                ]
            };

        public static CardCollection GetStarterJungleDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Bunny>(), 
                    GetCard<Bunny>(), 
                    GetCard<Dryad>(), 
                    GetCard<Goldfish>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<JungleTurtle>(), 
                    GetCard<JungleTurtle>(), 
                    GetCard<SporeSkeleton>(),
                    GetCard<SporeSkeleton>(),
                    GetCard<Wizard>(),
                    GetCard<Piranha>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<Shackle>(), 
                    GetCard<Skeleton>(), 
                    GetCard<Skeleton>(), 
                ]
            };

        public static CardCollection GetJungleDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Bunny>(), 
                    GetCard<Bunny>(), 
                    GetCard<Dryad>(), 
                    GetCard<Goldfish>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<JungleTurtle>(), 
                    GetCard<JungleTurtle>(), 
                    GetCard<GiantTortoise>(),
                    GetCard<GiantTortoise>(),
                    GetCard<Wizard>(),
                    GetCard<Wizard>(),
                    GetCard<Piranha>(),
                    GetCard<Piranha>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<CobaltShield>(), 
                ]
            };

		public static CardCollection GetJungleAdvancedDeck() =>
			new()
			{
				Cards = [
					GetCard<QueenBee>(),
					GetCard<QueenBee>(),
					GetCard<Wizard>(),
					GetCard<Wizard>(),
					GetCard<Hornet>(),
					GetCard<Hornet>(),
					GetCard<MossHornet>(),
					GetCard<MossHornet>(),
					GetCard<JungleBat>(),
					GetCard<SpikedJungleSlime>(),
					GetCard<SpikedJungleSlime>(),
					GetCard<JungleTurtle>(),
					GetCard<JungleTurtle>(),
                    GetCard<GiantTortoise>(),
                    GetCard<GiantTortoise>(),
					GetCard<Guide>(),
					GetCard<Guide>(),
					GetCard<HealingPotion>(),
					GetCard<HealingPotion>(),
					GetCard<Nurse>(),
				]
			};

        public static CardCollection GetStarterBloodMoonDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Dryad>(), 
                    GetCard<Dryad>(), 
                    GetCard<Wizard>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<ArmsDealer>(), 
                    GetCard<SwiftnessPotion>(), 
                    GetCard<BloodZombie>(), 
                    GetCard<BloodZombie>(), 
                    GetCard<ZombieMerman>(), 
                    GetCard<ZombieMerman>(), 
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<DemonEye>(), 
                    GetCard<DemonEye>(), 
                    GetCard<Drippler>(), 
                    GetCard<Drippler>(), 
                    GetCard<WanderingEyeFish>(), 
                    GetCard<WanderingEyeFish>(), 
                ]
            };

        public static CardCollection GetBloodMoonDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Dryad>(), 
                    GetCard<Dryad>(), 
                    GetCard<ArmsDealer>(), 
                    GetCard<ArmsDealer>(), 
                    GetCard<SwiftnessPotion>(), 
                    GetCard<SwiftnessPotion>(), 
                    GetCard<ViciousGoldfish>(), 
                    GetCard<ViciousGoldfish>(), 
                    GetCard<ZombieMerman>(), 
                    GetCard<ZombieMerman>(), 
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<BloodZombie>(), 
                    GetCard<BloodZombie>(), 
                    GetCard<Drippler>(), 
                    GetCard<Drippler>(), 
                    GetCard<WanderingEyeFish>(), 
                    GetCard<WanderingEyeFish>(), 
                ]
            };
        public static CardCollection GetBloodMoonAdvancedDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Dryad>(), 
                    GetCard<Dryad>(), 
                    GetCard<ArmsDealer>(), 
                    GetCard<ArmsDealer>(), 
                    GetCard<Shuriken>(), 
                    GetCard<Shuriken>(), 
                    GetCard<BrainOfCthulhu>(), 
                    GetCard<BrainOfCthulhu>(), 
                    GetCard<ViciousBunny>(), 
                    GetCard<ViciousBunny>(), 
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<BloodZombie>(), 
                    GetCard<BloodZombie>(), 
                    GetCard<Drippler>(), 
                    GetCard<Drippler>(), 
                    GetCard<WanderingEyeFish>(), 
                    GetCard<WanderingEyeFish>(), 
                ]
            };

        public static CardCollection GetStarterForestDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Bunny>(), 
                    GetCard<Bunny>(), 
                    GetCard<Squirrel>(), 
                    GetCard<Squirrel>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<Shackle>(), 
                    GetCard<Shackle>(), 
                    GetCard<Zombie>(), 
                    GetCard<Zombie>(), 
                    GetCard<GreenSlime>(), 
                    GetCard<GreenSlime>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<EnchantedNightcrawler>(), 
                    GetCard<EnchantedNightcrawler>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                ]
            };

        public static CardCollection GetForestDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Wizard>(), 
                    GetCard<Wizard>(), 
                    GetCard<Bunny>(), 
                    GetCard<Bunny>(), 
                    GetCard<PlatinumBroadsword>(), 
                    GetCard<PlatinumBroadsword>(), 
                    GetCard<Zombie>(), 
                    GetCard<Zombie>(), 
                    GetCard<SlimedZombie>(), 
                    GetCard<SlimedZombie>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<PosessedArmor>(), 
                    GetCard<PosessedArmor>(), 
                    GetCard<Wraith>(), 
                    GetCard<Wraith>(), 
                ]
            };

		internal static CardCollection GetForestAdvancedDeck() =>
            new ()
            {
                Cards = [
                    GetCard<WallOfFlesh>(), 
                    GetCard<WallOfFlesh>(), 
                    GetCard<Leech>(), 
                    GetCard<TorturedSoul>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                    GetCard<Nurse>(),
                    GetCard<Nurse>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<PlatinumBroadsword>(), 
                    GetCard<PlatinumBroadsword>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<Wizard>(),
                    GetCard<Wizard>(),
                ]
            };

        public static CardCollection GetStarterSkeletonDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Shackle>(), 
                    GetCard<Shackle>(), 
                    GetCard<FledglingWings>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<PlatinumBroadsword>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<AngryBones>(), 
                    GetCard<Muramasa>(), 
                    GetCard<UndeadMiner>(), 
                    GetCard<UndeadMiner>(), 
                    GetCard<Skeleton>(), 
                    GetCard<Skeleton>(), 
                    GetCard<UndeadViking>(), 
                    GetCard<UndeadViking>(), 
                    GetCard<HealingPotion>(), 
                ]
            };

        public static CardCollection GetSkeletonDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Wizard>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<FledglingWings>(), 
                    GetCard<PlatinumBroadsword>(), 
                    GetCard<PlatinumBroadsword>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<AngryBones>(), 
                    GetCard<Muramasa>(), 
                    GetCard<UndeadMiner>(), 
                    GetCard<UndeadMiner>(), 
                    GetCard<ArmoredSkeleton>(), 
                    GetCard<ArmoredSkeleton>(), 
                    GetCard<UndeadViking>(), 
                    GetCard<UndeadViking>(), 
                    GetCard<HealingPotion>(), 
                ]
            };

        public static CardCollection GetStarterGoblinDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<GoblinThief>(), 
                    GetCard<GoblinThief>(), 
                    GetCard<ThrowingKnife>(), 
                    GetCard<ThrowingKnife>(), 
                    GetCard<WanderingEyeFish>(), 
                    GetCard<WanderingEyeFish>(), 
                    GetCard<GoblinArcher>(), 
                    GetCard<GoblinArcher>(), 
                    GetCard<GoblinScout>(), 
                    GetCard<GoblinScout>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<Shackle>(), 
                    GetCard<Squirrel>(), 
                    GetCard<Squirrel>(), 
                    GetCard<SwiftnessPotion>(), 
                ]
            };

        public static CardCollection GetGoblinDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<GoblinThief>(), 
                    GetCard<GoblinThief>(), 
                    GetCard<ThrowingKnife>(), 
                    GetCard<ThrowingKnife>(), 
                    GetCard<GoblinWarlock>(), 
                    GetCard<GoblinWarlock>(), 
                    GetCard<GoblinArcher>(), 
                    GetCard<GoblinArcher>(), 
                    GetCard<GoblinScout>(), 
                    GetCard<GoblinScout>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<AngelStatue>(), 
                    GetCard<AngelStatue>(), 
                    GetCard<Shuriken>(), 
                    GetCard<Shuriken>(), 
                ]
            };

        public static CardCollection GetMimicDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<LostGirl>(), 
                    GetCard<LostGirl>(), 
                    GetCard<ThrowingKnife>(), 
                    GetCard<ThrowingKnife>(), 
                    GetCard<Mimic>(), 
                    GetCard<Mimic>(), 
                    GetCard<Tim>(), 
                    GetCard<Tim>(), 
                    GetCard<GoblinThief>(), 
                    GetCard<GoblinThief>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<IronskinPotion>(), 
                    GetCard<IronskinPotion>(), 
                    GetCard<AngelStatue>(), 
                    GetCard<AngelStatue>(), 
                    GetCard<SwiftnessPotion>(), 
                    GetCard<SwiftnessPotion>(), 
                ]
            };

        public static CardCollection GetStarterCrabDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Squirrel>(), 
                    GetCard<Squirrel>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<ThrowingKnife>(), 
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<Dolphin>(), 
                    GetCard<Dolphin>(), 
                    GetCard<Crab>(), 
                    GetCard<Crab>(), 
                    GetCard<Jellyfish>(), 
                    GetCard<Jellyfish>(), 
                    GetCard<UndeadViking>(), 
                    GetCard<UndeadViking>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<HealingPotion>(), 
                ]
            };

        public static CardCollection GetCrabDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<PartyGirl>(), 
                    GetCard<PartyGirl>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<CopperShortsword>(), 
                    GetCard<Muramasa>(), 
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<Dolphin>(), 
                    GetCard<Dolphin>(), 
                    GetCard<Crab>(), 
                    GetCard<Crab>(), 
                    GetCard<Jellyfish>(), 
                    GetCard<Jellyfish>(), 
                    GetCard<Shark>(), 
                    GetCard<Shark>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<HealingPotion>(), 
                ]
            };

        public static CardCollection GetCrabAdvancedDeck() =>
            new()
            {
                Cards = [
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<PartyGirl>(), 
                    GetCard<PartyGirl>(), 
                    GetCard<Shuriken>(), 
                    GetCard<Shuriken>(), 
                    GetCard<BloodButcherer>(), 
                    GetCard<BloodButcherer>(), 
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<Dolphin>(), 
                    GetCard<Dolphin>(), 
                    GetCard<Crab>(), 
                    GetCard<Crab>(), 
                    GetCard<Jellyfish>(), 
                    GetCard<Jellyfish>(), 
                    GetCard<Shark>(), 
                    GetCard<Shark>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<FeralClaws>(), 
                ]
            };
        public static CardCollection GetStarterMushroomDeck() =>
            new ()
            {
                Cards = [
                    GetCard<GlowingSnail>(), 
                    GetCard<GlowingSnail>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<MushroomZombie>(), 
                    GetCard<MushroomZombie>(), 
                    GetCard<Bunny>(),
                    GetCard<Goldfish>(),
                    GetCard<Nurse>(),
                    GetCard<Nurse>(),
                    GetCard<SporeSkeleton>(),
                    GetCard<SporeSkeleton>(),
                    GetCard<SporeBat>(),
                    GetCard<SporeBat>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Shackle>(), 
                    GetCard<Shackle>(), 
                    GetCard<CopperShortsword>(),
                    GetCard<PlatinumBroadsword>(),
                ]
            };

        public static CardCollection GetMushroomDeck() =>
            new ()
            {
                Cards = [
                    GetCard<GlowingSnail>(), 
                    GetCard<GlowingSnail>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<MushroomZombie>(), 
                    GetCard<MushroomZombie>(), 
                    GetCard<AnomuraFungus>(),
                    GetCard<AnomuraFungus>(),
                    GetCard<Nurse>(),
                    GetCard<Nurse>(),
                    GetCard<SporeSkeleton>(),
                    GetCard<SporeSkeleton>(),
                    GetCard<DarkCaster>(),
                    GetCard<DarkCaster>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<PlatinumBroadsword>(),
                    GetCard<PlatinumBroadsword>(),
                ]
            };

        public static CardCollection GetCurseDeck() =>
            new ()
            {
                Cards = [
                    GetCard<GlowingSnail>(), 
                    GetCard<GlowingSnail>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<CursedSkull>(), 
                    GetCard<ToxicSludge>(),
                    GetCard<Wizard>(),
                    GetCard<Wizard>(),
                    GetCard<DarkCaster>(),
                    GetCard<DarkCaster>(),
                    GetCard<JungleTurtle>(),
                    GetCard<JungleTurtle>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<GiantTortoise>(),
                    GetCard<GiantTortoise>(),
                ]
            };

        public static CardCollection GetSalamanderDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Salamander>(), 
                    GetCard<Salamander>(), 
                    GetCard<GiantShelly>(), 
                    GetCard<GiantShelly>(),
                    GetCard<Crawdad>(),
                    GetCard<Crawdad>(),
                    GetCard<Bunny>(),
                    GetCard<Bunny>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<Wizard>(),
                    GetCard<Wizard>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<CobaltShield>(), 
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<LightsBane>(),
                    GetCard<LightsBane>(),
                ]
            };

        public static CardCollection GetStarterSlimeDeck() =>
            new ()
            {
                Cards = [
                    GetCard<BlueSlime>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<GreenSlime>(), 
                    GetCard<GreenSlime>(),
                    GetCard<Wizard>(),
                    GetCard<Wizard>(),
                    GetCard<ToxicSludge>(),
                    GetCard<ToxicSludge>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Pinky>(), 
                    GetCard<Pinky>(), 
                    GetCard<SlimedZombie>(),
                    GetCard<SlimedZombie>(),
                    GetCard<MotherSlime>(),
                    GetCard<MotherSlime>(),
                ]
            };

        public static CardCollection GetSlimeDeck() =>
            new ()
            {
                Cards = [
                    GetCard<BlueSlime>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<GreenSlime>(), 
                    GetCard<GreenSlime>(),
                    GetCard<Wizard>(),
                    GetCard<Wizard>(),
                    GetCard<ToxicSludge>(),
                    GetCard<ToxicSludge>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Pinky>(), 
                    GetCard<Pinky>(), 
                    GetCard<SlimedZombie>(),
                    GetCard<SlimedZombie>(),
                    GetCard<MotherSlime>(),
                    GetCard<MotherSlime>(),
                ]
            };
		internal static CardCollection GetStarterSnowDeck() =>
            new ()
            {
                Cards = [
                    GetCard<IceBat>(), 
                    GetCard<IceBat>(), 
                    GetCard<FrozenShield>(), 
                    GetCard<FrozenZombie>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<IceElemental>(),
                    GetCard<IceElemental>(),
                    GetCard<Dryad>(),
                    GetCard<Dryad>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<FrozenZombie>(),
                    GetCard<FrozenZombie>(),
                    GetCard<Wizard>(), 
                    GetCard<Wizard>(),
                    GetCard<SnowFlinx>(), 
                    GetCard<SnowFlinx>(),
                ]
            };

		internal static CardCollection GetSnowDeck() =>
            new ()
            {
                Cards = [
                    GetCard<IceTortoise>(), 
                    GetCard<IceTortoise>(), 
                    GetCard<FrozenShield>(), 
                    GetCard<FrozenZombie>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<DarkCaster>(),
                    GetCard<IceMimic>(),
                    GetCard<IceElemental>(),
                    GetCard<IceElemental>(),
                    GetCard<Dryad>(),
                    GetCard<Dryad>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<FrozenZombie>(),
                    GetCard<IceMimic>(),
                    GetCard<Wizard>(), 
                    GetCard<DarkCaster>(),
                    GetCard<SnowFlinx>(), 
                    GetCard<SnowFlinx>(),
                ]
            };

		internal static CardCollection GetSlimeAdvancedDeck() =>
            new ()
            {
                Cards = [
                    GetCard<KingSlime>(),
                    GetCard<KingSlime>(),
                    GetCard<BlueSlime>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<SpikedSlime>(), 
                    GetCard<SpikedSlime>(),
                    GetCard<Wizard>(),
                    GetCard<TorturedSoul>(),
                    GetCard<ToxicSludge>(),
                    GetCard<ToxicSludge>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Pinky>(), 
                    GetCard<Pinky>(), 
                    GetCard<MotherSlime>(),
                    GetCard<MotherSlime>(),
                ]
            };

		internal static CardCollection GetStarterEvilDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Crimera>(),
                    GetCard<Crimera>(),
                    GetCard<FaceMonster>(), 
                    GetCard<FaceMonster>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<Crab>(), 
                    GetCard<Crab>(),
                    GetCard<Zombie>(),
                    GetCard<Zombie>(),
                    GetCard<ViciousBunny>(),
                    GetCard<ViciousBunny>(),
                    GetCard<FeralClaws>(),
                    GetCard<FeralClaws>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Shackle>(), 
                    GetCard<Shackle>(), 
                    GetCard<CopperShortsword>(),
                    GetCard<CopperShortsword>(),
                ]
            };

		internal static CardCollection GetEvilDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Skeleton>(), 
                    GetCard<Skeleton>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<Devourer>(), 
                    GetCard<Devourer>(),
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<EaterOfSouls>(),
                    GetCard<EaterOfSouls>(),
                    GetCard<EnchantedNightcrawler>(),
                    GetCard<EnchantedNightcrawler>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<LightsBane>(), 
                    GetCard<LightsBane>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                ]
            };

		internal static CardCollection GetEvilAdvancedDeck() =>
            new ()
            {
                Cards = [
                    GetCard<EaterOfWorlds>(), 
                    GetCard<EaterOfWorlds>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<Devourer>(), 
                    GetCard<Devourer>(),
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<EaterOfSouls>(),
                    GetCard<EaterOfSouls>(),
                    GetCard<EnchantedNightcrawler>(),
                    GetCard<EnchantedNightcrawler>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<TorturedSoul>(), 
                    GetCard<LightsBane>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                ]
            };

		public static CardCollection GetQueenBeeDeck() =>
			new()
			{
				Cards = [
					GetCard<QueenBee>(),
					GetCard<Bee>(),
					GetCard<Bee>(),
					GetCard<Hornet>(),
					GetCard<Hornet>(),
					GetCard<MossHornet>(),
					GetCard<MossHornet>(),
					GetCard<JungleBat>(),
					GetCard<JungleBat>(),
					GetCard<SpikedJungleSlime>(),
					GetCard<SpikedJungleSlime>(),
					GetCard<Guide>(),
					GetCard<Guide>(),
					GetCard<HealingPotion>(),
					GetCard<HealingPotion>(),
					GetCard<GiantTortoise>(),
					GetCard<GiantTortoise>(),
					GetCard<JungleTurtle>(),
					GetCard<Shackle>(),
					GetCard<Shackle>(),
				]
			};

		internal static CardCollection GetKingSlimeDeck() =>
            new ()
            {
                Cards = [
                    GetCard<KingSlime>(),
                    GetCard<Pinky>(),
                    GetCard<BlueSlime>(), 
                    GetCard<BlueSlime>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<SpikedSlime>(), 
                    GetCard<SpikedSlime>(),
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<ToxicSludge>(),
                    GetCard<ToxicSludge>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<SpikedJungleSlime>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<SlimedZombie>(), 
                    GetCard<SlimedZombie>(), 
                    GetCard<MotherSlime>(),
                    GetCard<MotherSlime>(),
                ]
            };

		internal static CardCollection GetBoCDeck() =>
            new ()
            {
                Cards = [
                    GetCard<BrainOfCthulhu>(), 
                    GetCard<BrainOfCthulhu>(), 
                    GetCard<Guide>(), 
                    GetCard<Guide>(), 
                    GetCard<Dryad>(), 
                    GetCard<Dryad>(), 
                    GetCard<ArmsDealer>(), 
                    GetCard<ArmsDealer>(), 
                    GetCard<Shuriken>(), 
                    GetCard<Shuriken>(), 
                    GetCard<ViciousBunny>(), 
                    GetCard<Crimera>(), 
                    GetCard<BloodButcherer>(), 
                    GetCard<BloodButcherer>(), 
                    GetCard<Crab>(), 
                    GetCard<Crimera>(), 
                    GetCard<Drippler>(), 
                    GetCard<FaceMonster>(), 
                    GetCard<ViciousGoldfish>(), 
                    GetCard<WanderingEyeFish>(), 
                ]
            };

		internal static CardCollection GetEoCDeck() =>
            new ()
            {
                Cards = [
                    GetCard<EyeOfCthulhu>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<ServantOfCthulhu>(), 
                    GetCard<ServantOfCthulhu>(),
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<ThornsPotion>(),
                    GetCard<WanderingEye>(),
                    GetCard<WanderingEye>(),
                    GetCard<DemonEye>(),
                    GetCard<DemonEye>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Harpy>(), 
                    GetCard<Harpy>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                ]
            };

		internal static CardCollection GetEoWDeck() =>
            new ()
            {
                Cards = [
                    GetCard<EaterOfWorlds>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<Devourer>(), 
                    GetCard<Devourer>(),
                    GetCard<ThornsPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<EaterOfSouls>(),
                    GetCard<EaterOfSouls>(),
                    GetCard<EnchantedNightcrawler>(),
                    GetCard<EnchantedNightcrawler>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<LightsBane>(), 
                    GetCard<LightsBane>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                ]
            };

		internal static CardCollection GetSkeletronDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Skeletron>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<Dryad>(), 
                    GetCard<Dryad>(),
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                    GetCard<RagePotion>(),
                    GetCard<RagePotion>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<LightsBane>(), 
                    GetCard<LightsBane>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<Nurse>(),
                    GetCard<Wizard>(),
                    GetCard<PlatinumBroadsword>(), 
                ]
            };

		internal static CardCollection GetWallOfFleshDeck() =>
            new ()
            {
                Cards = [
                    GetCard<WallOfFlesh>(), 
                    GetCard<Leech>(), 
                    GetCard<Leech>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                    GetCard<Nurse>(),
                    GetCard<Nurse>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(),
                    GetCard<Wizard>(), 
                    GetCard<Wizard>(),
                    GetCard<PlatinumBroadsword>(), 
                    GetCard<PlatinumBroadsword>(),
                    GetCard<CopperShortsword>(),
                ]
            };

		internal static CardCollection GetDeerclopsDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Deerclops>(), 
                    GetCard<FrozenShield>(), 
                    GetCard<FrozenShield>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<IceMimic>(),
                    GetCard<IceMimic>(),
                    GetCard<IceElemental>(),
                    GetCard<IceElemental>(),
                    GetCard<Dryad>(),
                    GetCard<Dryad>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<IceTortoise>(),
                    GetCard<IceTortoise>(),
                    GetCard<Wizard>(), 
                    GetCard<Wizard>(),
                    GetCard<SnowFlinx>(), 
                    GetCard<SnowFlinx>(),
                    GetCard<FrozenZombie>(),
                ]
            };

		internal static CardCollection GetHallowedDeck() =>
            new ()
            {
                Cards = [
                    GetCard<IlluminantBat>(), 
                    GetCard<IlluminantBat>(), 
                    GetCard<RagePotion>(),
                    GetCard<RagePotion>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<EnchantedSword>(),
                    GetCard<EnchantedSword>(),
                    GetCard<FeralClaws>(),
                    GetCard<FeralClaws>(),
                    GetCard<Dryad>(),
                    GetCard<Dryad>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Pixie>(),
                    GetCard<Pixie>(),
                    GetCard<Gastropod>(), 
                    GetCard<Gastropod>(),
                    GetCard<Unicorn>(), 
                    GetCard<Unicorn>(),
                ]
            };

		internal static CardCollection GetDesertDeck() =>
            new ()
            {
                Cards = [
                    GetCard<AntlionCharger>(), 
                    GetCard<AntlionCharger>(), 
                    GetCard<AntlionSwarmer>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<Ghoul>(),
                    GetCard<Ghoul>(),
                    GetCard<Vulture>(),
                    GetCard<Lamia>(),
                    GetCard<Lamia>(),
                    GetCard<ArmsDealer>(),
                    GetCard<ArmsDealer>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<DesertSpirit>(),
                    GetCard<SandstormInABottle>(),
                    GetCard<Basilisk>(), 
                    GetCard<SandShark>(),
                    GetCard<SandShark>(), 
                    GetCard<Scorpion>(),
                ]
            };

		internal static CardCollection GetBatsDeck() =>
            new ()
            {
                Cards = [
                    GetCard<IlluminantBat>(), 
                    GetCard<IlluminantBat>(), 
                    GetCard<RagePotion>(),
                    GetCard<RagePotion>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<GiantJungleBat>(),
                    GetCard<GiantJungleBat>(),
                    GetCard<JungleBat>(),
                    GetCard<JungleBat>(),
                    GetCard<SporeBat>(),
                    GetCard<SporeBat>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<PartyGirl>(),
                    GetCard<Bat>(),
                    GetCard<BatWings>(), 
                    GetCard<BatWings>(),
                    GetCard<IceBat>(), 
                    GetCard<IceBat>(),
                ]
            };

		internal static CardCollection GetCrittersDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Bunny>(), 
                    GetCard<Bunny>(), 
                    GetCard<DoctorBones>(),
                    GetCard<DoctorBones>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<ExplosiveBunny>(),
                    GetCard<ExplosiveBunny>(),
                    GetCard<Goldfish>(),
                    GetCard<Goldfish>(),
                    GetCard<GuideToCritters>(),
                    GetCard<GuideToCritters>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Dolphin>(),
                    GetCard<GlowingSnail>(),
                    GetCard<Squirrel>(), 
                    GetCard<Squirrel>(),
                    GetCard<Dryad>(), 
                    GetCard<Dryad>(),
                ]
            };

		internal static CardCollection GetQueenSlimeDeck() =>
            new ()
            {
                Cards = [
                    GetCard<QueenSlime>(), 
                    GetCard<QueenSlime>(), 
                    GetCard<RagePotion>(),
                    GetCard<RagePotion>(),
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<EnchantedSword>(),
                    GetCard<EnchantedSword>(),
                    GetCard<FeralClaws>(),
                    GetCard<FeralClaws>(),
                    GetCard<Dryad>(),
                    GetCard<Dryad>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Pixie>(),
                    GetCard<Pixie>(),
                    GetCard<Gastropod>(), 
                    GetCard<Gastropod>(),
                    GetCard<Unicorn>(), 
                    GetCard<Unicorn>(),
                ]
            };

		internal static CardCollection GetSkeletronPrimeDeck() =>
            new ()
            {
                Cards = [
                    GetCard<SkeletronPrime>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<BandOfRegeneration>(),
                    GetCard<BandOfRegeneration>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                    GetCard<PlatinumBroadsword>(),
                    GetCard<PlatinumBroadsword>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<AvengerEmblem>(), 
                    GetCard<ThornChakram>(), 
                    GetCard<ThornChakram>(),
                    GetCard<Shackle>(),
                    GetCard<Nurse>(),
                    GetCard<CopperShortsword>(),
                    GetCard<CopperShortsword>(), 
                    GetCard<BloodButcherer>(), 
                    GetCard<BloodButcherer>(), 
                ]
            };

		internal static CardCollection GetDestroyerDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Destroyer>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Nurse>(),
                    GetCard<Nurse>(),
                    GetCard<IronskinPotion>(),
                    GetCard<IronskinPotion>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                    GetCard<ThrowingKnife>(),
                    GetCard<ThrowingKnife>(),
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(), 
                    GetCard<LightDisc>(), 
                    GetCard<LightDisc>(), 
                    GetCard<Shuriken>(), 
                ]
            };

		internal static CardCollection GetTwinsDeck() =>
            new ()
            {
                Cards = [
                    GetCard<Twins>(), 
                    GetCard<Guide>(),
                    GetCard<Guide>(),
                    GetCard<HealingPotion>(), 
                    GetCard<HealingPotion>(), 
                    GetCard<Nurse>(),
                    GetCard<Nurse>(),
                    GetCard<FairyBell>(),
                    GetCard<FairyBell>(),
                    GetCard<CobaltShield>(),
                    GetCard<CobaltShield>(),
                    GetCard<MagicalHarp>(),
                    GetCard<MagicalHarp>(),
                    GetCard<RagePotion>(), 
                    GetCard<RagePotion>(), 
                    GetCard<Shackle>(),
                    GetCard<Shackle>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<FeralClaws>(), 
                    GetCard<ArmsDealer>(), 
                ]
            };
	}
}
