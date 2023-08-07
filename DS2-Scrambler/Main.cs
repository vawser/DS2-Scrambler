using Microsoft.WindowsAPICodePack.Dialogs;
using System.Configuration;
using SoulsFormats;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using static SoulsFormats.PARAM;
using System.Reflection;

namespace DS2_Scrambler
{
    public partial class Main : Form
    {
        // Mod Path
        public CommonOpenFileDialog ModPath_Dialog;
        public string Path_BaseMod;
        public string Path_ScrambledMod;

        // Interface
        private Progress<string> ProgressText;

        public ScramblerData scramblerData;
        public Regulation reg;

        public Main()
        {
            InitializeComponent();

            ProgressText = new Progress<string>(status => l_status.Text = status);
        }

        #region Interface
        private async void Main_Load(object sender, EventArgs e)
        {
            ModPath_Dialog = new CommonOpenFileDialog();
            ModPath_Dialog.InitialDirectory = "";
            ModPath_Dialog.IsFolderPicker = true;

            t_ModPath.Text = "";
        }

        private void b_SelectModPath_Click(object sender, EventArgs e)
        {
            if (ModPath_Dialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrEmpty(ModPath_Dialog.FileName))
            {
                Path_BaseMod = ModPath_Dialog.FileName;
                Path_ScrambledMod = Path_BaseMod + $"-Scrambled";
                t_ModPath.Text = Path_BaseMod;
            }
        }

        private async void b_Scramble_Click(object sender, EventArgs e)
        {
            // If Copy Mod fails, do not attempt to scramble.
            if (Util.CopyMod(Path_BaseMod, Path_ScrambledMod))
            {
                ToggleControls(false);

                await Task.Run(() => Scramble(ProgressText));

                ToggleControls(true);
            }
        }

        
        #endregion
        private void Scramble(IProgress<string> progress)
        {
            progress.Report("Loading regulation.");

            // Return if scrambled path is empty
            if (Path_BaseMod == "" || Path_BaseMod == null || Path_ScrambledMod == "" || Path_ScrambledMod == null)
            {
                progress.Report("Aborted.");
                MessageBox.Show("No base path specified.");
                return;
            }

            // Load regulation params
            reg = new Regulation(Path_ScrambledMod);

            if (reg.LoadParams() && reg.LoadLooseParams())
            {
                progress.Report("Params loaded.");

                scramblerData = new ScramblerData(reg, Path_ScrambledMod);
                progress.Report("Scrambler data built.");

                // Scramble
                reg = ScrambleParams(progress, reg);
            }

            // Save regulation params
            //if (reg.SaveParams() && reg.SaveLooseParams())
            if(reg.SaveAllParamsAsLoose())
            {
                SystemSounds.Asterisk.Play();
                progress.Report("Params saved.");
            }
        }

        public Regulation ScrambleParams(IProgress<string> progress, Regulation reg)
        {
            Random rand = new Random(DateTime.Now.GetHashCode());

            progress.Report("Scramble started.");

            //********************
            // Param Randomisation
            //********************
            try
            {
                ParamScrambler scrambler = new ParamScrambler(rand, reg, scramblerData);

                progress.Report("Scramble: Weapon Attributes");
                reg = scrambler.Scramble_WeaponAttributes(
                    c_ItemParam_Weapon_Price.Checked,
                    c_ItemParam_Weapon_Effect.Checked,
                    c_WeaponParam_Weapon_Weight.Checked,
                    c_WeaponParam_Weapon_Durability.Checked,
                    c_ItemParam_Weapon_Animation_Speed.Checked,
                    c_WeaponParam_StatRequirements.Checked,
                    c_WeaponParam_Damage.Checked,
                    c_WeaponReinforceParam_Reinforcement.Checked,
                    c_WeaponParam_StaminaConsumption.Checked,
                    c_WeaponTypeParam_CastSpeed.Checked,
                    c_WeaponTypeParam_BowDistance.Checked,
                    c_ArrowParam_AmmoDamage.Checked,
                    c_WeaponActionCategoryParam_Moveset.Checked,
                    c_Tweak_WeaponParam_RemoveStatRequirements.Checked
                );

                progress.Report("Scramble: Armor Attributes");
                reg = scrambler.Scramble_ArmorAttributes(
                    c_ItemParam_Armor_Price.Checked,
                    c_ItemParam_Armor_Effect.Checked,
                    c_ArmorParam_Armor_Weight.Checked,
                    c_ArmorParam_Armor_Durability.Checked,
                    c_ArmorParam_Defence.Checked,
                    c_ArmorParam_StatRequirements.Checked,
                    c_ArmorParam_Poise.Checked,
                    c_ArmorReinforceParam_Absorption.Checked,
                    c_Tweak_ArmorParam_RemoveStatRequirements.Checked
                );

                progress.Report("Scramble: Ring Attributes");
                reg = scrambler.Scramble_RingAttributes(
                    c_ItemParam_Ring_Price.Checked,
                    c_ItemParam_Ring_Effect.Checked,
                    c_RingParam_Ring_Weight.Checked,
                    c_RingParam_Ring_Durability.Checked
                );

                progress.Report("Scramble: Item Attributes");
                reg = scrambler.Scramble_ItemAttributes(
                    c_ItemParam_Item_Price.Checked,
                    c_ItemParam_Item_Animation_Speed.Checked,
                    c_ItemParam_Item_Max_Hold_Count.Checked,
                    c_ItemParam_Item_Effect.Checked
                );


                progress.Report("Scramble: Spell Attributes");
                reg = scrambler.Scramble_SpellAttributes(
                    c_ItemParam_Spell_Price.Checked,
                    c_SpellParam_StatRequirements.Checked,
                    c_SpellParam_StartupSpeed.Checked,
                    c_SpellParam_CastAnimations.Checked,
                    c_SpellParam_StaminaConsumption.Checked,
                    c_SpellParam_CastSpeed.Checked,
                    c_SpellParam_SlotsUsed.Checked,
                    c_SpellParam_Casts.Checked,
                    c_Tweak_SpellParam_RemoveStatRequirements.Checked
                );

                progress.Report("Scramble: Bullets");
                reg = scrambler.Scramble_BulletParams(
                    c_Bullet_IncludePlayer.Checked,
                    c_Bullet_IncludeEnemy.Checked,
                    c_Bullet_IncludeBoss.Checked,
                    c_Bullet_IncludeTraps.Checked,
                    c_Bullet_VFX.Checked,
                    c_Bullet_Movement.Checked,
                    c_Bullet_Angle.Checked,
                    c_Bullet_SpawnDistance.Checked,
                    c_Bullet_Duration.Checked,
                    c_Bullet_Tracking.Checked,
                    c_Bullet_Effect.Checked,
                    c_Bullet_Count.Checked
                );

                progress.Report("Scramble: Player");
                reg = scrambler.Scramble_PlayerParams(
                    c_PlayerStatusParam_StartingAttributes.Checked,
                    c_PlayerStatusParam_StartingEquipment.Checked,
                    c_PlayerStatusParam_StartingGifts.Checked,
                    c_PlayerLevelUpSoulsParam_LevelupCost.Checked,
                    c_EventCommonParam_ShrineOfWinter_Cost.Checked,
                    c_BossBattleParam_BossSoulDrops.Checked,
                    c_LockOnParam_CameraDistance.Checked,
                    c_LockOnParam_CameraFOV.Checked,
                    c_ChrMoveParam_Walk.Checked,
                    c_ChrMoveParam_Run.Checked,
                    c_ChrMoveParam_Jump.Checked,
                    c_ChrMoveParam_Ladder.Checked,
                    c_ChrMoveParam_Turn.Checked,
                    c_ChrMoveParam_Evasion.Checked,
                    c_Tweak_AnyEquipmentForStartingEquipment.Checked,
                    c_Tweak_BigJumpMode.Checked
                );

                progress.Report("Scramble: Map");
                reg = scrambler.Scramble_MapParams(
                    c_TreasureBoxParam_TrappedChests.Checked
                );

                progress.Report("Scramble: Characters");
                reg = scrambler.Scramble_CharacterParams(
                    c_NpcPlayerStatusParam_Equipment.Checked
                );

                progress.Report("Scramble: Enemies");
                reg = scrambler.Scramble_EnemyParams(
                    c_Enemy_IncludeBosses.Checked,
                    c_Enemy_IncludeCharacters.Checked,
                    c_Enemy_IncludeSummons.Checked,
                    c_Enemy_IncludeHostileCharacters.Checked,
                    c_LogicComParam_Detection.Checked,
                    c_EnemyParam_HP.Checked,
                    c_EnemyParam_Souls.Checked,
                    c_EnemyParam_Stamina.Checked,
                    c_EnemyParam_Defence.Checked,
                    c_EnemyParam_ShieldDefence.Checked,
                    c_EnemyParam_Poise.Checked,
                    c_EnemyDamageParam_Damage.Checked,
                    c_EnemyDamageParam_Knockback.Checked,
                    c_EnemyDamageParam_AttackSpeed.Checked,
                    c_EnemyMoveParam_Walk.Checked,
                    c_EnemyMoveParam_Run.Checked,
                    c_EnemyMoveParam_Jump.Checked,
                    c_EnemyMoveParam_Climb.Checked,
                    c_EnemyMoveParam_Turn.Checked,
                    c_EnemyMoveParam_Evasion.Checked
                );
            }
            catch (Exception ex)
            {
                progress.Report("Aborted.");
                Util.ShowError($"Failed to scramble params:\n{reg.Path_Regulation}\n\n{ex}");
                return reg;
            }

            //********************
            // Item Randomisation
            //********************
            try
            {
                ItemScrambler item_scrambler = new ItemScrambler(rand, reg, scramblerData);

                if (c_Scramble_Map_Loot.Checked)
                {
                    progress.Report("Scramble: Loot");
                    reg = item_scrambler.Scramble_Loot(
                      c_Scramble_Map_Loot.Checked,
                      c_Include_Enemy_Loot.Checked,
                      c_Include_Shops.Checked,
                      c_Include_Boss_Trades.Checked,
                      c_IncludeBossTreasure_Treasure_Map.Checked,
                      c_IncludeCharacterTreasure_Treasure_Map.Checked,
                      c_IncludeCovenantTreasure_Treasure_Map.Checked,
                      c_IncludeBirdTreasure_Treasure_Map.Checked,
                      c_IncludEventTreasure_Treasure_Map.Checked,
                      c_IgnoreKeys_Treasure_Map.Checked,
                      c_IgnoreTools_Treasure_Map.Checked,
                      c_EnsureLifegems.Checked,
                      c_RetainShopSpread.Checked
                    );
                }
            }
            catch (Exception ex)
            {
                progress.Report("Aborted.");
                Util.ShowError($"Failed to scramble items:\n{reg.Path_Regulation}\n\n{ex}");
                return reg;
            }

            //********************
            // Enemy Randomisation
            //********************
            try
            {
                EnemyScrambler enemy_scrambler = new EnemyScrambler(rand, reg, scramblerData);

                if (c_Scramble_Enemy_Location.Checked || c_EnemyShareAggro.Checked)
                {
                    progress.Report("Scramble: Enemies");
                    reg = enemy_scrambler.Scramble_Enemies(
                        c_Scramble_Enemy_Location.Checked, 
                        c_EnemyShareAggro.Checked, 
                        c_Include_Characters.Checked, 
                        c_Things_Betwixt.Checked, 
                        c_Majula.Checked, 
                        c_Forest_of_Fallen_Giants.Checked, 
                        c_Brightstone_Cove_Tseldora.Checked, 
                        c_Aldias_Keep.Checked, 
                        c_Lost_Bastille.Checked, 
                        c_Earthen_Peak.Checked, 
                        c_No_mans_Wharf.Checked, 
                        c_Iron_Keep.Checked, 
                        c_Huntmans_Copse.Checked, 
                        c_Gutter.Checked, 
                        c_Dragon_Aerie.Checked, 
                        c_Path_to_Shaded_Woods.Checked, 
                        c_Unseen_Path_to_Heides.Checked, 
                        c_Heides_Tower_of_Flame.Checked, 
                        c_Shaded_Woods.Checked, 
                        c_Doors_of_Pharros.Checked, 
                        c_Grave_of_Saints.Checked, 
                        c_Giant_Memories.Checked, 
                        c_Shrine_of_Amana.Checked, 
                        c_Drangleic_Castle.Checked, 
                        c_Undead_Crypt.Checked, 
                        c_Dragon_Memories.Checked, 
                        c_Dark_Chasm_of_Old.Checked, 
                        c_Shulva.Checked, 
                        c_Brume_Tower.Checked, 
                        c_Eleum_Loyce.Checked, 
                        c_Memory_of_the_King.Checked
                    );
                }
            }
            catch (Exception ex)
            {
                progress.Report("Aborted.");
                Util.ShowError($"Failed to scramble enemies:\n{reg.Path_Regulation}\n\n{ex}");
                return reg;
            }
            progress.Report("Scramble finished.");
            return reg;
        }
        private void ToggleControls(bool state)
        {
            b_SelectModPath.Enabled = state;
            b_Scramble.Enabled = state;
        }

        private void b_ClearSelection_Click(object sender, EventArgs e)
        {
            ToggleCheckboxes(false);
        }

        private void ToggleMapCheckboxes(bool state)
        {
            c_Heides_Tower_of_Flame.Checked = state;
            c_Memory_of_the_King.Checked = state;
            c_Eleum_Loyce.Checked = state;
            c_Brume_Tower.Checked = state;
            c_Shulva.Checked = state;
            c_Dark_Chasm_of_Old.Checked = state;
            c_Dragon_Memories.Checked = state;
            c_Undead_Crypt.Checked = state;
            c_Drangleic_Castle.Checked = state;
            c_Shrine_of_Amana.Checked = state;
            c_Giant_Memories.Checked = state;
            c_Grave_of_Saints.Checked = state;
            c_Doors_of_Pharros.Checked = state;
            c_Shaded_Woods.Checked = state;
            c_Unseen_Path_to_Heides.Checked = state;
            c_Path_to_Shaded_Woods.Checked = state;
            c_Dragon_Aerie.Checked = state;
            c_Gutter.Checked = state;
            c_Huntmans_Copse.Checked = state;
            c_Iron_Keep.Checked = state;
            c_No_mans_Wharf.Checked = state;
            c_Earthen_Peak.Checked = state;
            c_Lost_Bastille.Checked = state;
            c_Aldias_Keep.Checked = state;
            c_Brightstone_Cove_Tseldora.Checked = state;
            c_Forest_of_Fallen_Giants.Checked = state;
            c_Majula.Checked = state;
            c_Things_Betwixt.Checked = state;
        }

        private void ToggleCheckboxes(bool state, bool includeTweaks = true)
        {
            // Item Scrambler
            c_Scramble_Map_Loot.Checked = state;

            c_Include_Boss_Trades.Checked = state;
            c_IncludEventTreasure_Treasure_Map.Checked = state;
            c_IncludeBirdTreasure_Treasure_Map.Checked = state;
            c_Include_Shops.Checked = state;
            c_IncludeBossTreasure_Treasure_Map.Checked = state;
            c_Include_Enemy_Loot.Checked = state;
            c_IncludeCovenantTreasure_Treasure_Map.Checked = state;
            c_IncludeCharacterTreasure_Treasure_Map.Checked = state;

            if(includeTweaks)
                c_IgnoreKeys_Treasure_Map.Checked = state;

            if (includeTweaks)
                c_IgnoreTools_Treasure_Map.Checked = state;

            if (includeTweaks)
                c_EnsureLifegems.Checked = state;

            if (includeTweaks)
                c_RetainShopSpread.Checked = state;

            // Enemy Scrambler
            c_Scramble_Enemy_Location.Checked = state;

            c_Include_Characters.Checked = state;

            c_Heides_Tower_of_Flame.Checked = state;
            c_Memory_of_the_King.Checked = state;
            c_Eleum_Loyce.Checked = state;
            c_Brume_Tower.Checked = state;
            c_Shulva.Checked = state;
            c_Dark_Chasm_of_Old.Checked = state;
            c_Dragon_Memories.Checked = state;
            c_Undead_Crypt.Checked = state;
            c_Drangleic_Castle.Checked = state;
            c_Shrine_of_Amana.Checked = state;
            c_Giant_Memories.Checked = state;
            c_Grave_of_Saints.Checked = state;
            c_Doors_of_Pharros.Checked = state;
            c_Shaded_Woods.Checked = state;
            c_Unseen_Path_to_Heides.Checked = state;
            c_Path_to_Shaded_Woods.Checked = state;
            c_Dragon_Aerie.Checked = state;
            c_Gutter.Checked = state;
            c_Huntmans_Copse.Checked = state;
            c_Iron_Keep.Checked = state;
            c_No_mans_Wharf.Checked = state;
            c_Earthen_Peak.Checked = state;
            c_Lost_Bastille.Checked = state;
            c_Aldias_Keep.Checked = state;
            c_Brightstone_Cove_Tseldora.Checked = state;
            c_Forest_of_Fallen_Giants.Checked = state;
            c_Majula.Checked = state;
            c_Things_Betwixt.Checked = state;

            if (includeTweaks)
                c_EnemyShareAggro.Checked = state;

            // Weapon Attributes
            c_ItemParam_Weapon_Price.Checked = state;
            c_ItemParam_Weapon_Effect.Checked = state;
            c_WeaponParam_Weapon_Weight.Checked = state;
            c_WeaponParam_Weapon_Durability.Checked = state;
            c_ItemParam_Weapon_Animation_Speed.Checked = state;
            c_WeaponParam_StatRequirements.Checked = state;
            c_WeaponParam_Damage.Checked = state;
            c_WeaponReinforceParam_Reinforcement.Checked = state;
            c_WeaponParam_StaminaConsumption.Checked = state;
            c_WeaponTypeParam_CastSpeed.Checked = state;
            c_WeaponTypeParam_BowDistance.Checked = state;
            c_ArrowParam_AmmoDamage.Checked = state;
            c_WeaponActionCategoryParam_Moveset.Checked = state;

            if (includeTweaks)
                c_Tweak_WeaponParam_RemoveStatRequirements.Checked = state;

            // Armor Attributes
            c_ItemParam_Armor_Price.Checked = state;
            c_ItemParam_Armor_Effect.Checked = state;
            c_ArmorParam_Armor_Weight.Checked = state;
            c_ArmorParam_Armor_Durability.Checked = state;
            c_ArmorParam_Defence.Checked = state;
            c_ArmorParam_StatRequirements.Checked = state;
            c_ArmorParam_Poise.Checked = state;
            c_ArmorReinforceParam_Absorption.Checked = state;

            if (includeTweaks)
                c_Tweak_ArmorParam_RemoveStatRequirements.Checked = state;

            // Ring Attributes
            c_ItemParam_Ring_Price.Checked = state;
            c_ItemParam_Ring_Effect.Checked = state;
            c_RingParam_Ring_Weight.Checked = state;
            c_RingParam_Ring_Durability.Checked = state;

            // Item Attributes
            c_ItemParam_Item_Price.Checked = state;
            c_ItemParam_Item_Animation_Speed.Checked = state;
            c_ItemParam_Item_Max_Hold_Count.Checked = state;
            c_ItemParam_Item_Effect.Checked = state;

            // Spell Attributes
            c_ItemParam_Spell_Price.Checked = state;
            c_SpellParam_StatRequirements.Checked = state;
            c_SpellParam_StartupSpeed.Checked = state;
            c_SpellParam_CastAnimations.Checked = state;
            c_SpellParam_StaminaConsumption.Checked = state;
            c_SpellParam_CastSpeed.Checked = state;
            c_SpellParam_SlotsUsed.Checked = state;
            c_SpellParam_Casts.Checked = state;

            if (includeTweaks)
                c_Tweak_SpellParam_RemoveStatRequirements.Checked = state;

            // Bullet
            c_Bullet_IncludePlayer.Checked = state;
            c_Bullet_IncludeEnemy.Checked = state;
            c_Bullet_IncludeBoss.Checked = state;
            c_Bullet_IncludeTraps.Checked = state;

            c_Bullet_VFX.Checked = state;
            c_Bullet_Movement.Checked = state;
            c_Bullet_Angle.Checked = state;
            c_Bullet_SpawnDistance.Checked = state;
            c_Bullet_Duration.Checked = state;
            c_Bullet_Tracking.Checked = state;
            c_Bullet_Effect.Checked = state;
            c_Bullet_Count.Checked = state;

            // Player
            c_PlayerStatusParam_StartingAttributes.Checked = state;
            c_PlayerStatusParam_StartingEquipment.Checked = state;
            c_PlayerStatusParam_StartingGifts.Checked = state;
            c_PlayerLevelUpSoulsParam_LevelupCost.Checked = state;

            c_EventCommonParam_ShrineOfWinter_Cost.Checked = state;
            c_BossBattleParam_BossSoulDrops.Checked = state;

            c_LockOnParam_CameraDistance.Checked = state;
            c_LockOnParam_CameraFOV.Checked = state;

            c_ChrMoveParam_Walk.Checked = state;
            c_ChrMoveParam_Run.Checked = state;
            c_ChrMoveParam_Jump.Checked = state;
            c_ChrMoveParam_Ladder.Checked = state;
            c_ChrMoveParam_Turn.Checked = state;
            c_ChrMoveParam_Evasion.Checked = state;

            if (includeTweaks)
                c_Tweak_AnyEquipmentForStartingEquipment.Checked = state;

            if (includeTweaks)
                c_Tweak_BigJumpMode.Checked = state;

            // Map
            c_TreasureBoxParam_TrappedChests.Checked = state;

            // Characters
            c_NpcPlayerStatusParam_Equipment.Checked = state;

            // Enemies
            c_Enemy_IncludeBosses.Checked = state;
            c_Enemy_IncludeCharacters.Checked = state;
            c_Enemy_IncludeSummons.Checked = state;
            c_Enemy_IncludeHostileCharacters.Checked = state;

            c_LogicComParam_Detection.Checked = state;

            c_EnemyParam_HP.Checked = state;
            c_EnemyParam_Souls.Checked = state;
            c_EnemyParam_Stamina.Checked = state;
            c_EnemyParam_Defence.Checked = state;
            c_EnemyParam_ShieldDefence.Checked = state;
            c_EnemyParam_Poise.Checked = state;
            c_EnemyDamageParam_Damage.Checked = state;
            c_EnemyDamageParam_Knockback.Checked = state;
            c_EnemyDamageParam_AttackSpeed.Checked = state;

            c_EnemyMoveParam_Walk.Checked = state;
            c_EnemyMoveParam_Run.Checked = state;
            c_EnemyMoveParam_Jump.Checked = state;
            c_EnemyMoveParam_Climb.Checked = state;
            c_EnemyMoveParam_Turn.Checked = state;
            c_EnemyMoveParam_Evasion.Checked = state;
        }

        private void b_QuickTick_Maps_Click(object sender, EventArgs e)
        {
            ToggleMapCheckboxes(true);
        }

        private void b_QuickClear_Maps_Click(object sender, EventArgs e)
        {
            ToggleMapCheckboxes(false);
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToggleCheckboxes(true, false);
        }
    }
}