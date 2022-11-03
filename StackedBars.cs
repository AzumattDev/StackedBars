using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using PieceManager;
using ServerSync;
using UnityEngine;

namespace StackedBars
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class StackedBarsPlugin : BaseUnityPlugin
    {
        internal const string ModName = "StackedBars";
        internal const string ModVersion = "1.0.1";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource StackedBarsLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public void Awake()
        {
            _serverConfigLocked = config("General", "Force Server Config", true, "Force Server Config");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            BuildPiece Blackmetal = new("stackedbars", "stack_blackmetalbars");
            Blackmetal.Name.English("Blackmetal Stack");
            Blackmetal.Description.English("Stacked set of Blackmetal bars");
            Blackmetal.RequiredItems.Add("BlackMetal", 20, true);
            Blackmetal.Category.Add(BuildPieceCategory.Crafting);
            Blackmetal.Crafting.Set(CraftingTable.ArtisanTable);

            BuildPiece Bronze = new("stackedbars", "stack_bronzebars");
            Bronze.Name.English("Bronze Stack");
            Bronze.Description.English("Stacked set of bars");
            Bronze.RequiredItems.Add("Bronze", 20, true);
            Bronze.Category.Add(BuildPieceCategory.Crafting);
            Bronze.Crafting.Set(CraftingTable.Forge);

            BuildPiece Copper = new("stackedbars", "stack_copperbars");
            Copper.Name.English("Copper Stack");
            Copper.Description.English("Stacked set of bars");
            Copper.RequiredItems.Add("Copper", 20, true);
            Copper.Category.Add(BuildPieceCategory.Crafting);
            Copper.Crafting.Set(CraftingTable.Forge);

            BuildPiece Flametal = new("stackedbars", "stack_flametalbars");
            Flametal.Name.English("Flametal Stack");
            Flametal.Description.English("Stacked set of bars");
            Flametal.RequiredItems.Add("Flametal", 20, true);
            Flametal.Category.Add(BuildPieceCategory.Crafting);
            Flametal.Crafting.Set(CraftingTable.ArtisanTable);

            BuildPiece Iron = new("stackedbars", "stack_ironbars");
            Iron.Name.English("Iron Stack");
            Iron.Description.English("Stacked set of bars");
            Iron.RequiredItems.Add("Iron", 20, true);
            Iron.Category.Add(BuildPieceCategory.Crafting);
            Iron.Crafting.Set(CraftingTable.Forge);

            BuildPiece Silver = new("stackedbars", "stack_silverbars");
            Silver.Name.English("Silver Stack");
            Silver.Description.English("Stacked set of bars");
            Silver.RequiredItems.Add("Silver", 20, true);
            Silver.Category.Add(BuildPieceCategory.Crafting);
            Silver.Crafting.Set(CraftingTable.Forge);

            BuildPiece Tin = new("stackedbars", "stack_tinbars");
            Tin.Name.English("Tin Stack");
            Tin.Description.English("Stacked set of bars");
            Tin.RequiredItems.Add("Tin", 20, true);
            Tin.Category.Add(BuildPieceCategory.Crafting);
            Tin.Crafting.Set(CraftingTable.Workbench);


            _harmony.PatchAll();
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                StackedBarsLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                StackedBarsLogger.LogError($"There was an issue loading your {ConfigFileName}");
                StackedBarsLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<bool>? _serverConfigLocked;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            public bool? Browsable = false;
        }

        #endregion
    }
}