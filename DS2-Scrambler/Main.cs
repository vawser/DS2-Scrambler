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

        // TODO: add 'map' tab, for scrambling stuff such as objects

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

            t_StatSkew.Value = 6;
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
            Random rand = new Random();

            if (t_seed.Text != string.Empty)
                rand = new Random(t_seed.Text.GetHashCode());

            progress.Report("Scramble started.");

            //********************
            // Param Randomisation
            //********************
            try
            {
                ParamScrambler scrambler = new ParamScrambler(rand, reg, scramblerData);

                // Items
                if (c_Scramble_ItemParam.Checked)
                {
                    progress.Report("Scramble: Item Attributes");
                    reg = scrambler.Scramble_ItemParam("ItemParam", c_Generate_ItemParam.Checked);
                }
                if (c_Scramble_RingParam.Checked)
                {
                    progress.Report("Scramble: Ring Attributes");
                    reg = scrambler.Scramble_RingParam("RingParam", c_Generate_RingParam.Checked);
                }

                // Spells
                if (c_Scramble_SpellParam.Checked)
                {
                    progress.Report("Scramble: Spell Attributes");
                    reg = scrambler.Scramble_SpellParam("SpellParam", c_Generate_SpellParam.Checked, c_IgnoreRequirements_SpellParam.Checked);
                }
                if (c_Scramble_BulletParam.Checked)
                {
                    progress.Report("Scramble: Projectile Attributes");
                    reg = scrambler.Scramble_BulletParam("BulletParam", c_Generate_BulletParam.Checked, c_ForceVisuals_BulletParam.Checked);
                }

                // Armor
                if (c_Scramble_ArmorParam.Checked)
                {
                    progress.Report("Scramble: Armor Attributes");
                    reg = scrambler.Scramble_ArmorParam("ArmorParam", c_Generate_ArmorParam.Checked, c_IgnoreRequirements_ArmorParam.Checked);
                }
                if (c_Scramble_ArmorReinforceParam.Checked)
                {
                    progress.Report("Scramble: Armor Reinforcement");
                    reg = scrambler.Scramble_ArmorReinforceParam("ArmorReinforceParam", c_Generate_ArmorReinforceParam.Checked);
                }

                // Weapons
                if (c_Scramble_ArrowParam.Checked)
                {
                    progress.Report("Scramble: Ammo Attributes");
                    reg = scrambler.Scramble_ArrowParam("ArrowParam", c_Generate_ArrowParam.Checked);
                }
                if (c_Scramble_WeaponActionCategoryParam.Checked)
                {
                    progress.Report("Scramble: Weapon Moveset Attributes");
                    reg = scrambler.Scramble_WeaponActionCategoryParam("WeaponActionCategoryParam");
                }
                if (c_Scramble_WeaponParam.Checked)
                {
                    progress.Report("Scramble: Weapon Attributes");
                    reg = scrambler.Scramble_WeaponParam("WeaponParam", c_Generate_WeaponParam.Checked, c_IgnoreFists_WeaponParam.Checked, c_IgnoreRequirements_WeaponParam.Checked);
                }
                if (c_Scramble_WeaponReinforceParam.Checked)
                {
                    progress.Report("Scramble: Weapon Reinforcement Attributes");
                    reg = scrambler.Scramble_WeaponReinforceParam("WeaponReinforceParam", c_Generate_WeaponReinforceParam.Checked);
                }
                if (c_Scramble_WeaponTypeParam.Checked)
                {
                    progress.Report("Scramble: Weapon Attributes");
                    reg = scrambler.Scramble_WeaponTypeParam("WeaponTypeParam", c_Generate_WeaponTypeParam.Checked);
                }

                // Player
                if (c_Scramble_EventCommonParam.Checked)
                {
                    progress.Report("Scramble: Event Common");
                    reg = scrambler.Scramble_EventCommonParam("EventCommonParam");
                }
                if (c_Scramble_LockOnParam_Distance.Checked || c_Scramble_LockOnParam_FOV.Checked)
                {
                    progress.Report("Scramble: Camera Attributes");
                    reg = scrambler.Scramble_LockOnParam("LockOnParam", c_Scramble_LockOnParam_Distance.Checked, c_Scramble_LockOnParam_FOV.Checked);
                }
                if (c_Scramble_PlayerLevelUpSoulsParam.Checked)
                {
                    progress.Report("Scramble: Level Up Costs");
                    reg = scrambler.Scramble_PlayerLevelUpSoulsParam("PlayerLevelUpSoulsParam");
                }
                if (c_Scramble_PlayerStatusParam_Classes.Checked || c_Scramble_PlayerStatusParam_Gifts.Checked)
                {
                    progress.Report("Scramble: Player Starting Setup");
                    reg = scrambler.Scramble_PlayerStatusParam("PlayerStatusParam", c_Scramble_PlayerStatusParam_Classes.Checked, c_Scramble_PlayerStatusParam_Gifts.Checked, (int)t_StatSkew.Value, c_ClassScramble_LimitEquipment.Checked);
                }

                // Character
                if (c_Scramble_ChrMoveParam.Checked)
                {
                    progress.Report("Scramble: Character Movement Attributes");
                    reg = scrambler.Scramble_ChrMoveParam("ChrMoveParam");
                }

                // Bosses
                if (c_Scramble_BossBattleParam.Checked)
                {
                    progress.Report("Scramble: Boss Rewards");
                    reg = scrambler.Scramble_BossBattleParam("BossBattleParam", c_Generate_BossBattleParam.Checked);
                }
                if (c_Scramble_EnemyParam_Boss.Checked)
                {
                    progress.Report("Scramble: Boss Base Attributes");
                    reg = scrambler.Scramble_EnemyParam("EnemyParam", c_Generate_EnemyParam_Boss.Checked, true);
                }
                if (c_Scramble_EnemyMoveParam_Boss.Checked)
                {
                    progress.Report("Scramble: Boss Movement Attributes");
                    reg = scrambler.Scramble_EnemyMoveParam("EnemyMoveParam", c_Generate_EnemyMoveParam_Boss.Checked, true);
                }
                if (c_Scramble_EnemyDamageParam_Boss.Checked)
                {
                    progress.Report("Scramble: Boss Damage Attributes");
                    reg = scrambler.Scramble_EnemyDamageParam("EnemyDamageParam", c_Generate_EnemyDamageParam_Boss.Checked, true);
                }
                if (c_Scramble_EnemyBulletParam_Boss.Checked)
                {
                    progress.Report("Scramble: Boss Projectile Attributes");
                    reg = scrambler.Scramble_EnemyBulletParam("EnemyBulletParam", c_Generate_EnemyBulletParam_Boss.Checked, c_ForceVisuals_EnemyBulletParam_Boss.Checked, true);
                }
                if (c_Scramble_EnemyBehaviorParam_Boss.Checked)
                {
                    progress.Report("Scramble: Boss Behavior Attributes");
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorParam", c_Generate_EnemyBehaviorParam_Boss.Checked, true);
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorSecondParam", c_Generate_EnemyBehaviorParam_Boss.Checked, true);
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorThirdParam", c_Generate_EnemyBehaviorParam_Boss.Checked, true);
                }

                // Enemies
                if (c_Scramble_NpcPlayerStatusParam.Checked)
                {
                    progress.Report("Scramble: Character Equipment");
                    reg = scrambler.Scramble_NpcPlayerStatusParam("NpcPlayerStatusParam", c_Generate_NpcPlayerStatusParam.Checked);
                }
                if (c_Scramble_EnemyParam.Checked)
                {
                    progress.Report("Scramble: Enemy Base Attributes");
                    reg = scrambler.Scramble_EnemyParam("EnemyParam", c_Generate_EnemyParam.Checked);
                }
                if (c_Scramble_EnemyMoveParam.Checked)
                {
                    progress.Report("Scramble: Enemy Movement Attributes");
                    reg = scrambler.Scramble_EnemyMoveParam("EnemyMoveParam", c_Generate_EnemyMoveParam.Checked);
                }
                if (c_Scramble_EnemyDamageParam.Checked)
                {
                    progress.Report("Scramble: Enemy Damage Attributes");
                    reg = scrambler.Scramble_EnemyDamageParam("EnemyDamageParam", c_Generate_EnemyDamageParam.Checked);
                }
                if (c_Scramble_EnemyBulletParam.Checked)
                {
                    progress.Report("Scramble: Enemy Projectile Attributes");
                    reg = scrambler.Scramble_EnemyBulletParam("EnemyBulletParam", c_Generate_EnemyBulletParam.Checked, c_ForceVisuals_EnemyBulletParam.Checked);
                }
                if (c_Scramble_EnemyBehaviorParam.Checked)
                {
                    progress.Report("Scramble: Enemy Behavior Attributes");
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorParam", c_Generate_EnemyBehaviorParam.Checked);
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorSecondParam", c_Generate_EnemyBehaviorParam.Checked);
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorThirdParam", c_Generate_EnemyBehaviorParam.Checked);
                }

                // Map
                if (c_Scramble_TreasureBoxParam.Checked)
                {
                    progress.Report("Scramble: Chest Traps");
                    reg = scrambler.Scramble_TreasureBoxParam();
                }
                if (c_Scramble_LogicComParam.Checked)
                {
                    progress.Report("Scramble: Enemy AI Attributes");
                    reg = scrambler.Scramble_LogicComParam("LogicComParam", c_Generate_LogicComParam.Checked);
                }
                if (c_Scramble_SystemBulletParam.Checked)
                {
                    progress.Report("Scramble: Map Projectile Attributes");
                    reg = scrambler.Scramble_SystemBulletParam("SystemBulletParam", c_Generate_SystemBulletParam.Checked, c_ForceVisuals_SystemBulletParam.Checked, c_LimitToTraps_SystemBulletParam.Checked);
                }

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
                    reg = enemy_scrambler.Scramble_Enemies(c_Scramble_Enemy_Location.Checked, c_EnemyShareAggro.Checked, c_Include_Characters.Checked, c_Things_Betwixt.Checked, c_Majula.Checked, c_Forest_of_Fallen_Giants.Checked, c_Brightstone_Cove_Tseldora.Checked, c_Aldias_Keep.Checked, c_Lost_Bastille.Checked, c_Earthen_Peak.Checked, c_No_mans_Wharf.Checked, c_Iron_Keep.Checked, c_Huntmans_Copse.Checked, c_Gutter.Checked, c_Dragon_Aerie.Checked, c_Path_to_Shaded_Woods.Checked, c_Unseen_Path_to_Heides.Checked, c_Heides_Tower_of_Flame.Checked, c_Shaded_Woods.Checked, c_Doors_of_Pharros.Checked, c_Grave_of_Saints.Checked, c_Giant_Memories.Checked, c_Shrine_of_Amana.Checked, c_Drangleic_Castle.Checked, c_Undead_Crypt.Checked, c_Dragon_Memories.Checked, c_Dark_Chasm_of_Old.Checked, c_Shulva.Checked, c_Brume_Tower.Checked, c_Eleum_Loyce.Checked, c_Memory_of_the_King.Checked);
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
        private void b_ToggleRecommended_Click(object sender, EventArgs e)
        {
            c_Scramble_Map_Loot.Checked = true;
            c_Include_Enemy_Loot.Checked = true;
            c_Include_Shops.Checked = true;
            c_IgnoreKeys_Treasure_Map.Checked = true;
            c_IgnoreTools_Treasure_Map.Checked = true;
        }

        private void b_ToggleOff_Click(object sender, EventArgs e)
        {
            // *** Item Scrambler ***
            // Core
            c_Scramble_Map_Loot.Checked = false;

            // Inclusions
            c_Include_Shops.Checked = false;
            c_IncludeCharacterTreasure_Treasure_Map.Checked = false;
            c_IncludeCovenantTreasure_Treasure_Map.Checked = false;
            c_IncludeBirdTreasure_Treasure_Map.Checked = false;
            c_IncludEventTreasure_Treasure_Map.Checked = false;
            c_Include_Enemy_Loot.Checked = false;
            c_IncludeBossTreasure_Treasure_Map.Checked = false;
            c_Include_Boss_Trades.Checked = false;

            // Exlcusions
            c_IgnoreKeys_Treasure_Map.Checked = false;
            c_IgnoreTools_Treasure_Map.Checked = false;

            // Tweaks
            c_EnsureLifegems.Checked = false;
            c_RetainShopSpread.Checked = false;

            // ** Enemy Scrambler ***
            // Enemy Location

            // Tweaks
            c_EnemyShareAggro.Checked = false;

            // *** Param Scrambler ***
            // SystemBulletParam
            c_Scramble_SystemBulletParam.Checked = false;
            c_Generate_SystemBulletParam.Checked = false;
            c_ForceVisuals_SystemBulletParam.Checked = false;
            c_LimitToTraps_SystemBulletParam.Checked = false;

            // LogicComParam
            c_Scramble_LogicComParam.Checked = false;
            c_Generate_LogicComParam.Checked = false;

            // TreasureBoxParam
            c_Scramble_TreasureBoxParam.Checked = false;

            // EventCommonParam
            c_Scramble_EventCommonParam.Checked = false;

            // LockOnParam
            c_Scramble_LockOnParam_Distance.Checked = false;
            c_Scramble_LockOnParam_FOV.Checked = false;

            // PlayerLevelUpSoulsParam
            c_Scramble_PlayerLevelUpSoulsParam.Checked = false;

            // PlayerStatusParam
            c_Scramble_PlayerStatusParam_Classes.Checked = false;
            c_Scramble_PlayerStatusParam_Gifts.Checked = false;
            c_ClassScramble_LimitEquipment.Checked = false;
            t_StatSkew.Value = 6;

            // ChrMoveParam
            c_Scramble_ChrMoveParam.Checked = false;

            // NpcPlayerStatusParam
            c_Scramble_NpcPlayerStatusParam.Checked = false;
            c_Generate_NpcPlayerStatusParam.Checked = false;

            // BossBattleParam
            c_Scramble_BossBattleParam.Checked = false;
            c_Generate_BossBattleParam.Checked = false;

            // EnemyParam
            c_Scramble_EnemyParam.Checked = false;
            c_Scramble_EnemyParam_Boss.Checked = false;
            c_Generate_EnemyParam.Checked = false;
            c_Generate_EnemyParam_Boss.Checked = false;

            // EnemyMoveParam
            c_Scramble_EnemyMoveParam.Checked = false;
            c_Scramble_EnemyMoveParam_Boss.Checked = false;
            c_Generate_EnemyMoveParam.Checked = false;
            c_Generate_EnemyMoveParam_Boss.Checked = false;

            // EnemyDamageParam 
            c_Scramble_EnemyDamageParam.Checked = false;
            c_Scramble_EnemyDamageParam_Boss.Checked = false;
            c_Generate_EnemyDamageParam.Checked = false;
            c_Generate_EnemyDamageParam_Boss.Checked = false;

            // EnemyBulletParam
            c_Scramble_EnemyBulletParam.Checked = false;
            c_Scramble_EnemyBulletParam_Boss.Checked = false;
            c_Generate_EnemyBulletParam.Checked = false;
            c_Generate_EnemyBulletParam_Boss.Checked = false;
            c_ForceVisuals_EnemyBulletParam.Checked = false;
            c_ForceVisuals_EnemyBulletParam_Boss.Checked = false;

            // EnemyBehaviorParam
            c_Scramble_EnemyBehaviorParam.Checked = false;
            c_Scramble_EnemyBehaviorParam_Boss.Checked = false;
            c_Generate_EnemyBehaviorParam.Checked = false;
            c_Generate_EnemyBehaviorParam_Boss.Checked = false;

            // ArrowParam
            c_Scramble_ArrowParam.Checked = false;
            c_Generate_ArrowParam.Checked = false;

            // WeaponActionCategoryParam
            c_Scramble_WeaponActionCategoryParam.Checked = false;

            // WeaponParam
            c_Scramble_WeaponParam.Checked = false;
            c_Generate_WeaponParam.Checked = false;
            c_IgnoreFists_WeaponParam.Checked = false;
            c_IgnoreRequirements_WeaponParam.Checked = false;

            // WeaponReinforceParam
            c_Scramble_WeaponReinforceParam.Checked = false;
            c_Generate_WeaponReinforceParam.Checked = false;

            // WeaponTypeParam
            c_Scramble_WeaponTypeParam.Checked = false;
            c_Generate_WeaponTypeParam.Checked = false;

            // ArmorParam
            c_Scramble_ArmorParam.Checked = false;
            c_Generate_ArmorParam.Checked = false;
            c_IgnoreRequirements_ArmorParam.Checked = false;

            // ArmorReinforceParam
            c_Scramble_ArmorReinforceParam.Checked = false;
            c_Generate_ArmorReinforceParam.Checked = false;

            // SpellParam
            c_Scramble_SpellParam.Checked = false;
            c_Generate_SpellParam.Checked = false;
            c_IgnoreRequirements_SpellParam.Checked = false;

            // BulletParam
            c_Scramble_BulletParam.Checked = false;
            c_Generate_BulletParam.Checked = false;
            c_ForceVisuals_BulletParam.Checked = false;

            // ItemParam
            c_Scramble_ItemParam.Checked = false;
            c_Generate_ItemParam.Checked = false;

            // RingParam
            c_Scramble_RingParam.Checked = false;
            c_Generate_RingParam.Checked = false;

        }

        private void b_ChaosMode_Click(object sender, EventArgs e)
        {
            // *** Item Scrambler ***
            // Core
            c_Scramble_Map_Loot.Checked = true;

            // Inclusions
            c_Include_Shops.Checked = true;
            c_IncludeCharacterTreasure_Treasure_Map.Checked = true;
            c_IncludeCovenantTreasure_Treasure_Map.Checked = true;
            c_IncludeBirdTreasure_Treasure_Map.Checked = true;
            c_IncludEventTreasure_Treasure_Map.Checked = true;
            c_Include_Enemy_Loot.Checked = true;
            c_IncludeBossTreasure_Treasure_Map.Checked = true;
            c_Include_Boss_Trades.Checked = true;

            // Exlcusions
            c_IgnoreKeys_Treasure_Map.Checked = false;
            c_IgnoreTools_Treasure_Map.Checked = false;

            // Tweaks
            c_EnsureLifegems.Checked = false;
            c_RetainShopSpread.Checked = false;

            // ** Enemy Scrambler ***

            // Tweaks
            c_EnemyShareAggro.Checked = false;

            // *** Param Scrambler ***
            // SystemBulletParam
            c_Scramble_SystemBulletParam.Checked = true;
            c_Generate_SystemBulletParam.Checked = true;
            c_ForceVisuals_SystemBulletParam.Checked = true;
            c_LimitToTraps_SystemBulletParam.Checked = false;

            // LogicComParam
            c_Scramble_LogicComParam.Checked = false;
            c_Generate_LogicComParam.Checked = false;

            // TreasureBoxParam
            c_Scramble_TreasureBoxParam.Checked = true;

            // EventCommonParam
            c_Scramble_EventCommonParam.Checked = true;

            // LockOnParam
            c_Scramble_LockOnParam_Distance.Checked = false;
            c_Scramble_LockOnParam_FOV.Checked = false;

            // PlayerLevelUpSoulsParam
            c_Scramble_PlayerLevelUpSoulsParam.Checked = true;

            // PlayerStatusParam
            c_Scramble_PlayerStatusParam_Classes.Checked = true;
            c_Scramble_PlayerStatusParam_Gifts.Checked = true;
            c_ClassScramble_LimitEquipment.Checked = false;
            t_StatSkew.Value = 12;

            // ChrMoveParam
            c_Scramble_ChrMoveParam.Checked = true;

            // NpcPlayerStatusParam
            c_Scramble_NpcPlayerStatusParam.Checked = true;
            c_Generate_NpcPlayerStatusParam.Checked = true;

            // BossBattleParam
            c_Scramble_BossBattleParam.Checked = true;
            c_Generate_BossBattleParam.Checked = true;

            // EnemyParam
            c_Scramble_EnemyParam.Checked = true;
            c_Scramble_EnemyParam_Boss.Checked = true;
            c_Generate_EnemyParam.Checked = true;
            c_Generate_EnemyParam_Boss.Checked = true;

            // EnemyMoveParam
            c_Scramble_EnemyMoveParam.Checked = true;
            c_Scramble_EnemyMoveParam_Boss.Checked = true;
            c_Generate_EnemyMoveParam.Checked = true;
            c_Generate_EnemyMoveParam_Boss.Checked = true;

            // EnemyDamageParam 
            c_Scramble_EnemyDamageParam.Checked = true;
            c_Scramble_EnemyDamageParam_Boss.Checked = true;
            c_Generate_EnemyDamageParam.Checked = true;
            c_Generate_EnemyDamageParam_Boss.Checked = true;

            // EnemyBulletParam
            c_Scramble_EnemyBulletParam.Checked = true;
            c_Scramble_EnemyBulletParam_Boss.Checked = true;
            c_Generate_EnemyBulletParam.Checked = true;
            c_Generate_EnemyBulletParam_Boss.Checked = true;
            c_ForceVisuals_EnemyBulletParam.Checked = true;
            c_ForceVisuals_EnemyBulletParam_Boss.Checked = true;

            // EnemyBehaviorParam
            c_Scramble_EnemyBehaviorParam.Checked = true;
            c_Scramble_EnemyBehaviorParam_Boss.Checked = true;
            c_Generate_EnemyBehaviorParam.Checked = true;
            c_Generate_EnemyBehaviorParam_Boss.Checked = true;

            // ArrowParam
            c_Scramble_ArrowParam.Checked = true;
            c_Generate_ArrowParam.Checked = true;

            // WeaponActionCategoryParam
            c_Scramble_WeaponActionCategoryParam.Checked = true;

            // WeaponParam
            c_Scramble_WeaponParam.Checked = true;
            c_Generate_WeaponParam.Checked = true;
            c_IgnoreFists_WeaponParam.Checked = true;
            c_IgnoreRequirements_WeaponParam.Checked = false;

            // WeaponReinforceParam
            c_Scramble_WeaponReinforceParam.Checked = true;
            c_Generate_WeaponReinforceParam.Checked = true;

            // WeaponTypeParam
            c_Scramble_WeaponTypeParam.Checked = true;
            c_Generate_WeaponTypeParam.Checked = true;

            // ArmorParam
            c_Scramble_ArmorParam.Checked = true;
            c_Generate_ArmorParam.Checked = true;
            c_IgnoreRequirements_ArmorParam.Checked = false;

            // ArmorReinforceParam
            c_Scramble_ArmorReinforceParam.Checked = true;
            c_Generate_ArmorReinforceParam.Checked = true;

            // SpellParam
            c_Scramble_SpellParam.Checked = true;
            c_Generate_SpellParam.Checked = true;
            c_IgnoreRequirements_SpellParam.Checked = false;

            // BulletParam
            c_Scramble_BulletParam.Checked = true;
            c_Generate_BulletParam.Checked = true;
            c_ForceVisuals_BulletParam.Checked = true;

            // ItemParam
            c_Scramble_ItemParam.Checked = true;
            c_Generate_ItemParam.Checked = true;

            // RingParam
            c_Scramble_RingParam.Checked = true;
            c_Generate_RingParam.Checked = true;
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }

        private void t_StatSkew_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}