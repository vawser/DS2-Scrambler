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
        public CommonOpenFileDialog modPathDialog;
        public string baseModPath;
        public string scrambledModPath;

        // Interface
        private Progress<string> progress;

        public Main()
        {
            InitializeComponent();

            progress = new Progress<string>(status => l_status.Text = status);
        }

        #region Interface
        private async void Main_Load(object sender, EventArgs e)
        {
            modPathDialog = new CommonOpenFileDialog();
            modPathDialog.InitialDirectory = "";
            modPathDialog.IsFolderPicker = true;

            t_ModPath.Text = "";

            c_IgnoreKeys_Treasure_Map.Checked = true;
            c_IgnoreTools_Treasure_Map.Checked = true;
            c_Scramble_Enemy_Loot.Checked = true;
            c_Scramble_Map_Loot.Checked = true;
        }

        private void b_SelectModPath_Click(object sender, EventArgs e)
        {
            if (modPathDialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrEmpty(modPathDialog.FileName))
            {
                baseModPath = modPathDialog.FileName;
                scrambledModPath = baseModPath + $"-Scrambled";
                t_ModPath.Text = baseModPath;
            }
        }

        private async void b_Scramble_Click(object sender, EventArgs e)
        {
            Util.CopyMod(baseModPath, scrambledModPath);

            ToggleControls(false);

            await Task.Run(() => Scramble(progress));

            ToggleControls(true);
        }

        
        #endregion
        private void Scramble(IProgress<string> progress)
        {
            progress.Report("Loading regulation.");

            // Return if scrambled path is empty
            if (baseModPath == "" || baseModPath == null || scrambledModPath == "" || scrambledModPath == null)
            {
                progress.Report("Aborted.");
                MessageBox.Show("No base path specified.");
                return;
            }

            // Load regulation params
            Regulation reg = new Regulation(scrambledModPath);

            if (reg.LoadParams() && reg.LoadLooseParams())
            {
                progress.Report("Params loaded.");

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
                ParamScrambler scrambler = new ParamScrambler(rand, reg, scrambledModPath);

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
                Util.ShowError($"Failed to scramble params:\n{reg.regulationPath}\n\n{ex}");
                return reg;
            }

            //********************
            // Item Randomisation
            //********************
            try
            {
                ItemScrambler item_scrambler = new ItemScrambler(rand, reg, scrambledModPath);

                if (c_Scramble_Map_Loot.Checked)
                {
                    progress.Report("Scramble: Loot");
                    reg = item_scrambler.Scramble_Loot(
                      c_Scramble_Map_Loot.Checked,
                      c_Scramble_Enemy_Loot.Checked,
                      c_Scramble_Shops.Checked,
                      c_Scramble_Boss_Trades.Checked,
                      c_IncludeBossTreasure_Treasure_Map.Checked,
                      c_IncludeCharacterTreasure_Treasure_Map.Checked,
                      c_IncludeCovenantTreasure_Treasure_Map.Checked,
                      c_IncludeBirdTreasure_Treasure_Map.Checked,
                      c_IncludEventTreasure_Treasure_Map.Checked,
                      c_IgnoreKeys_Treasure_Map.Checked,
                      c_IgnoreTools_Treasure_Map.Checked,
                      c_IgnoreBossSouls_Treasure_Map.Checked
                    );
                }
            }
            catch (Exception ex)
            {
                progress.Report("Aborted.");
                Util.ShowError($"Failed to scramble items:\n{reg.regulationPath}\n\n{ex}");
                return reg;
            }

            //********************
            // Enemy Randomisation
            //********************
            try
            {
                EnemyScrambler enemy_scrambler = new EnemyScrambler(rand, reg, scrambledModPath);

                if (c_Scramble_Enemy_Location.Checked || c_Scramble_Enemy_Type_Basic.Checked || c_FuriousEnemies.Checked)
                {
                    // This method is decent, but variety is limited and bosses don't mesh with position changes.
                    progress.Report("Scramble: Enemies");
                    reg = enemy_scrambler.Scramble_Enemies(
                        c_Scramble_Enemy_Location.Checked, 
                        c_Scramble_Enemy_Type_Basic.Checked, 
                        c_Enemy_Location_Ordered.Checked, 
                        c_Enemy_Location_IncludeCharacters.Checked, 
                        c_Enemy_Location_IncludeSpecial.Checked, 
                        c_Scramble_Enemy_Type_Boss.Checked, 
                        c_Scramble_Enemy_Type_Characters.Checked,
                        c_FuriousEnemies.Checked
                    );
                }
            }
            catch (Exception ex)
            {
                progress.Report("Aborted.");
                Util.ShowError($"Failed to scramble enemies:\n{reg.regulationPath}\n\n{ex}");
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
            c_Scramble_Enemy_Loot.Checked = true;
            c_Scramble_Shops.Checked = true;
            c_IgnoreKeys_Treasure_Map.Checked = true;
            c_IgnoreTools_Treasure_Map.Checked = true;
            c_IgnoreBossSouls_Treasure_Map.Checked = true;
        }

        private void b_ToggleOff_Click(object sender, EventArgs e)
        {
            c_Scramble_ArmorParam.Checked = false;
            c_Scramble_ArrowParam.Checked = false;
            c_Scramble_ItemParam.Checked = false;
            c_Scramble_WeaponActionCategoryParam.Checked = false;
            c_Scramble_WeaponParam.Checked = false;
            c_Scramble_BossBattleParam.Checked = false;
            c_Scramble_NpcPlayerStatusParam.Checked = false;
            c_Scramble_EnemyBehaviorParam.Checked = false;
            c_Scramble_EnemyBehaviorParam_Boss.Checked = false;
            c_Scramble_EnemyBulletParam.Checked = false;
            c_Scramble_EnemyBulletParam_Boss.Checked = false;
            c_Scramble_EnemyDamageParam.Checked = false;
            c_Scramble_EnemyDamageParam_Boss.Checked = false;
            c_Scramble_EnemyMoveParam.Checked = false;
            c_Scramble_EnemyMoveParam_Boss.Checked = false;
            c_Scramble_EnemyParam.Checked = false;
            c_Scramble_EnemyParam_Boss.Checked = false;
            c_Scramble_RingParam.Checked = false;
            c_Scramble_SpellParam.Checked = false;
            c_Scramble_BulletParam.Checked = false;
            c_Scramble_TreasureBoxParam.Checked = false;
            c_Scramble_SystemBulletParam.Checked = false;
            c_Generate_ArmorParam.Checked = false;
            c_Generate_ArrowParam.Checked = false;
            c_Generate_ItemParam.Checked = false;
            c_Generate_WeaponParam.Checked = false;
            c_Generate_BossBattleParam.Checked = false;
            c_Generate_EnemyBehaviorParam.Checked = false;
            c_Generate_EnemyBehaviorParam_Boss.Checked = false;
            c_Generate_EnemyBulletParam.Checked = false;
            c_Generate_EnemyBulletParam_Boss.Checked = false;
            c_Generate_EnemyDamageParam.Checked = false;
            c_Generate_EnemyDamageParam_Boss.Checked = false;
            c_Generate_EnemyMoveParam.Checked = false;
            c_Generate_EnemyMoveParam_Boss.Checked = false;
            c_Generate_EnemyParam.Checked = false;
            c_Generate_EnemyParam_Boss.Checked = false;
            c_Generate_NpcPlayerStatusParam.Checked = false;
            c_Generate_RingParam.Checked = false;
            c_Generate_SpellParam.Checked = false;
            c_Generate_SystemBulletParam.Checked = false;
            c_Generate_BulletParam.Checked = false;
            c_IgnoreRequirements_WeaponParam.Checked = false;
            c_IgnoreRequirements_ArmorParam.Checked = false;
            c_IgnoreRequirements_SpellParam.Checked = false;
            c_Scramble_LogicComParam.Checked = false;
            c_LimitToTraps_SystemBulletParam.Checked = false;
            c_IgnoreFists_WeaponParam.Checked = false;
            c_ForceVisuals_BulletParam.Checked = false;
            c_ForceVisuals_EnemyBulletParam.Checked = false;
            c_ForceVisuals_EnemyBulletParam_Boss.Checked = false;
            c_ForceVisuals_SystemBulletParam.Checked = false;

            c_Scramble_Map_Loot.Checked = false;
            c_Scramble_Enemy_Loot.Checked = false;
            c_Scramble_Shops.Checked = false;
            c_Scramble_Boss_Trades.Checked = false;
            c_IgnoreKeys_Treasure_Map.Checked = false;
            c_IgnoreTools_Treasure_Map.Checked = false;
            c_IgnoreBossSouls_Treasure_Map.Checked = false;
            c_IncludeBossTreasure_Treasure_Map.Checked = false;
            c_IncludeCharacterTreasure_Treasure_Map.Checked = false;
            c_IncludeCovenantTreasure_Treasure_Map.Checked = false;
            c_IncludeBirdTreasure_Treasure_Map.Checked = false;
            c_IncludEventTreasure_Treasure_Map.Checked = false;
            c_FuriousEnemies.Checked = false;

            c_Scramble_Enemy_Location.Checked = false;
            c_Scramble_Enemy_Type_Basic.Checked = false;
            c_Enemy_Location_Ordered.Checked = false;
            c_Enemy_Location_IncludeCharacters.Checked = false;
            c_Enemy_Location_IncludeSpecial.Checked = false;
            c_Scramble_Enemy_Type_Boss.Checked = false;
            c_Scramble_Enemy_Type_Characters.Checked = false;
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }
    }
}