using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Nessie.ATLYSS.EasySettings;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ActionOverflow
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class ActionOverflow : BaseUnityPlugin
    {
        // logger
        public static ManualLogSource mls;
        
        // metadata
        private const string modGUID = "ActionOverflow";
        private const string modName = "ActionOverflow";
        private const string modVersion = "1.0.0";

        // harmony instance
        private readonly Harmony harmony = new Harmony(modGUID);

        // EasySettings config file
        public ConfigFile modConfig = new ConfigFile(Path.Combine("BepInEx", "config", "ActionOverflow.cfg"), true);

        // self reference
        private static ActionOverflow actionoverflow;

        private void Awake()
        {
            if (actionoverflow != null) actionoverflow = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            harmony.PatchAll();

            mls.LogInfo("ActionOverflow initialized!");

            // Config
            InitializeConfig();
            Settings.OnInitialized.AddListener(AddEasySettings);
            Settings.OnApplySettings.AddListener
            (
                () => { Config.Save(); }
            );
        }

        public void InitializeConfig()
        {
            var slot7_def = new ConfigDefinition("Keybindings", "Slot 7");
            var slot7_desc = new ConfigDescription("Slot 7 KeyCode");
            ActionOverflowConfig.action6Key = Config.Bind(slot7_def, KeyCode.Alpha7, slot7_desc);

            var slot8_def = new ConfigDefinition("Keybindings", "Slot 8");
            var slot8_desc = new ConfigDescription("Slot 8 KeyCode");
            ActionOverflowConfig.action7Key = Config.Bind(slot8_def, KeyCode.Alpha8, slot8_desc);

            var slot9_def = new ConfigDefinition("Keybindings", "Slot 9");
            var slot9_desc = new ConfigDescription("Slot 9 KeyCode");
            ActionOverflowConfig.action8Key = Config.Bind(slot9_def, KeyCode.Alpha9, slot9_desc);

            var slot10_def = new ConfigDefinition("Keybindings", "Slot 10");
            var slot10_desc = new ConfigDescription("Slot 10 KeyCode");
            ActionOverflowConfig.action9Key = Config.Bind(slot10_def, KeyCode.Alpha0, slot10_desc);

            var slot11_def = new ConfigDefinition("Keybindings", "Slot 11");
            var slot11_desc = new ConfigDescription("Slot 11 KeyCode");
            ActionOverflowConfig.action10Key = Config.Bind(slot11_def, KeyCode.F8, slot11_desc);

            var slot12_def = new ConfigDefinition("Keybindings", "Slot 12");
            var slot12_desc = new ConfigDescription("Slot 12 KeyCode");
            ActionOverflowConfig.action11Key = Config.Bind(slot12_def, KeyCode.F9, slot12_desc);

            var barToggle_def = new ConfigDefinition("Bar", "Toggle Bar");
            var barToggle_desc = new ConfigDescription("Toggle the bar on and off");
            ActionOverflowConfig._barToggle = Config.Bind(barToggle_def, true, barToggle_desc);
        }

        public void AddEasySettings()
        {
            SettingsTab tab = Settings.ModTab;

            tab.AddHeader("ActionOverflow Settings");

            tab.AddToggle("Additional Bar Toggle", ActionOverflowConfig._barToggle);

            tab.AddKeyButton("Slot 7", ActionOverflowConfig.action6Key);
            tab.AddKeyButton("Slot 8", ActionOverflowConfig.action7Key);
            tab.AddKeyButton("Slot 9", ActionOverflowConfig.action8Key);
            tab.AddKeyButton("Slot 10", ActionOverflowConfig.action9Key);
            tab.AddKeyButton("Slot 11", ActionOverflowConfig.action10Key);
            tab.AddKeyButton("Slot 12", ActionOverflowConfig.action11Key);
        }
    }
}
