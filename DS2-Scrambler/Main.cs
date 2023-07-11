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

            c_Treasure_IgnoreKeyItems.Checked = true;
            c_Treasure_IgnoreTools.Checked = true;
            c_Scramble_ItemLotParam2_Chr.Checked = true;
            c_Scramble_ItemLotParam2_Other.Checked = true;
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
            if (scrambledModPath == "")
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
            progress.Report("Scramble started.");

            try
            {
                Scrambler scrambler = new Scrambler(t_seed.Text, reg);

                // Treasure
                if (c_Scramble_ItemLotParam2_Chr.Checked)
                {
                    progress.Report("Scramble: Treasure - Enemies");
                    reg = scrambler.Scramble_ItemLotParam("ItemLotParam2_Chr", c_Generate_ItemLotParam2_Chr.Checked, c_Treasure_IgnoreKeyItems.Checked, c_Treasure_IgnoreTools.Checked);
                }
                if (c_Scramble_ItemLotParam2_Other.Checked)
                {
                    progress.Report("Scramble: Treasure - Enemies");
                    reg = scrambler.Scramble_ItemLotParam("ItemLotParam2_Other", c_Generate_ItemLotParam2_Other.Checked, c_Treasure_IgnoreKeyItems.Checked, c_Treasure_IgnoreTools.Checked, c_MapLoot_ItemLotParam2_Other.Checked);
                }
                if (c_Scramble_ItemLotParam2_SvrEvent.Checked)
                {
                    progress.Report("Scramble: Treasure - Enemies");
                    reg = scrambler.Scramble_ItemLotParam("ItemLotParam2_SvrEvent", c_Generate_ItemLotParam2_SvrEvent.Checked, c_Treasure_IgnoreKeyItems.Checked, c_Treasure_IgnoreTools.Checked);
                }

                // Items
                if (c_Scramble_ItemParam.Checked)
                {
                    progress.Report("Scramble: Items");
                    reg = scrambler.Scramble_ItemParam("ItemParam", c_Generate_ItemParam.Checked);
                }

                // Armor
                if (c_Scramble_ArmorParam.Checked)
                {
                    progress.Report("Scramble: Armor");
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
                    progress.Report("Scramble: Ammunition");
                    reg = scrambler.Scramble_ArrowParam("ArrowParam", c_Generate_ArrowParam.Checked);
                }
                if (c_Scramble_WeaponActionCategoryParam.Checked)
                {
                    progress.Report("Scramble: Weapon Moveset");
                    reg = scrambler.Scramble_WeaponActionCategoryParam("WeaponActionCategoryParam");
                }
                if (c_Scramble_WeaponParam.Checked)
                {
                    progress.Report("Scramble: Weapons");
                    reg = scrambler.Scramble_WeaponParam("WeaponParam", c_Generate_WeaponParam.Checked, c_IgnoreFists_WeaponParam.Checked, c_IgnoreRequirements_WeaponParam.Checked);
                }
                if (c_Scramble_WeaponReinforceParam.Checked)
                {
                    progress.Report("Scramble: Weapon Reinforcement");
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
                    reg = scrambler.Scramble_EnemyBulletParam("EnemyBulletParam", c_Generate_EnemyBulletParam_Boss.Checked, true);
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
                    reg = scrambler.Scramble_EnemyBulletParam("EnemyBulletParam", c_Generate_EnemyBulletParam.Checked);
                }
                if (c_Scramble_EnemyBehaviorParam.Checked)
                {
                    progress.Report("Scramble: Enemy Behavior Attributes");
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorParam", c_Generate_EnemyBehaviorParam.Checked);
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorSecondParam", c_Generate_EnemyBehaviorParam.Checked);
                    reg = scrambler.Scramble_EnemyBehaviorParam("EnemyBehaviorThirdParam", c_Generate_EnemyBehaviorParam.Checked);
                }

                // Map
                if (c_Scramble_EnemyPlacement.Checked)
                {
                    progress.Report("Scramble: Enemy Placement");
                    reg = scrambler.Scramble_EnemyPlacement(c_EnemyPlacement_IgnoreKeyCharacters.Checked, c_EnemyPlacement_IgnoreBosses.Checked);
                }
                if (c_Scramble_LogicComParam.Checked)
                {
                    progress.Report("Scramble: Enemy AI");
                    reg = scrambler.Scramble_LogicComParam("LogicComParam", c_Generate_LogicComParam.Checked);
                }

            }
            catch (Exception ex)
            {
                progress.Report("Aborted.");
                Util.ShowError($"Failed to scramble params:\n{reg.regulationPath}\n\n{ex}");
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

        private void ToggleCheckboxes(bool state)
        {
        }

        private void b_ToggleRecommended_Click(object sender, EventArgs e)
        {
            c_Scramble_ArmorParam.Checked = true;
            c_Scramble_ArrowParam.Checked = true;
            c_Scramble_ItemLotParam2_Other.Checked = true;
            c_Scramble_ItemParam.Checked = true;
            c_Scramble_WeaponActionCategoryParam.Checked = true;

            c_Treasure_IgnoreKeyItems.Checked = true;
            c_MapLoot_ItemLotParam2_Other.Checked = true;

            c_Scramble_EnemyPlacement.Checked = true;
            c_EnemyPlacement_IgnoreKeyCharacters.Checked = true;
        }

        private void b_ToggleOff_Click(object sender, EventArgs e)
        {
            c_Scramble_ArmorParam.Checked = false;
            c_Scramble_ArrowParam.Checked = false;
            c_Scramble_ItemLotParam2_Other.Checked = false;
            c_Scramble_ItemParam.Checked = false;
            c_Scramble_WeaponActionCategoryParam.Checked = false;
            c_Scramble_ItemLotParam2_Chr.Checked = false;
            c_Scramble_ItemLotParam2_SvrEvent.Checked = false;
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

            c_Generate_ArmorParam.Checked = false;
            c_Generate_ArrowParam.Checked = false;
            c_Generate_ItemLotParam2_Chr.Checked = false;
            c_Generate_ItemLotParam2_Other.Checked = false;
            c_Generate_ItemLotParam2_SvrEvent.Checked = false;
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

            c_Treasure_IgnoreKeyItems.Checked = false;
            c_Treasure_IgnoreTools.Checked = false;
            c_MapLoot_ItemLotParam2_Other.Checked = false;
            c_IgnoreRequirements_WeaponParam.Checked = false;
            c_IgnoreRequirements_ArmorParam.Checked = false;

            c_Scramble_EnemyPlacement.Checked = false;
            c_Scramble_LogicComParam.Checked = false;
            c_EnemyPlacement_IgnoreBosses.Checked = false;
            c_EnemyPlacement_IgnoreKeyCharacters.Checked = false;
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }
    }
}