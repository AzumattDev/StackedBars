using System.Collections.Generic;
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
        internal const string ModVersion = "1.0.5";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource StackedBarsLogger = BepInEx.Logging.Logger.CreateLogSource(ModName);
        private static readonly ConfigSync ConfigSync = new(ModGUID) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };
        public static readonly List<GameObject> FixGameObjects = new();

        public void Awake()
        {
            _serverConfigLocked = config("General", "Force Server Config", true, "Force Server Config");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            BuildPiece Blackmetal = new("stackedbars", "stack_blackmetalbars");
            Blackmetal.Name.English("Blackmetal Stack");
            Blackmetal.Description.English("Stacked set of Blackmetal bars");
            Blackmetal.RequiredItems.Add("BlackMetal", 20, true);
            Blackmetal.Category.Set(BuildPieceCategory.Crafting);
            Blackmetal.Crafting.Set(CraftingTable.ArtisanTable);
            FixGameObjects.Add(Blackmetal.Prefab);

            BuildPiece Bronze = new("stackedbars", "stack_bronzebars");
            Bronze.Name.English("Bronze Stack");
            Bronze.Description.English("Stacked set of bars");
            Bronze.RequiredItems.Add("Bronze", 20, true);
            Bronze.Category.Set(BuildPieceCategory.Crafting);
            Bronze.Crafting.Set(CraftingTable.Forge);
            FixGameObjects.Add(Bronze.Prefab);

            BuildPiece Copper = new("stackedbars", "stack_copperbars");
            Copper.Name.English("Copper Stack");
            Copper.Description.English("Stacked set of bars");
            Copper.RequiredItems.Add("Copper", 20, true);
            Copper.Category.Set(BuildPieceCategory.Crafting);
            Copper.Crafting.Set(CraftingTable.Forge);
            FixGameObjects.Add(Copper.Prefab);

            BuildPiece Flametal = new("stackedbars", "stack_flametalbars");
            Flametal.Name.English("Flametal Stack");
            Flametal.Description.English("Stacked set of bars");
            Flametal.RequiredItems.Add("Flametal", 20, true);
            Flametal.Category.Set(BuildPieceCategory.Crafting);
            Flametal.Crafting.Set(CraftingTable.ArtisanTable);
            FixGameObjects.Add(Flametal.Prefab);

            BuildPiece Iron = new("stackedbars", "stack_ironbars");
            Iron.Name.English("Iron Stack");
            Iron.Description.English("Stacked set of bars");
            Iron.RequiredItems.Add("Iron", 20, true);
            Iron.Category.Set(BuildPieceCategory.Crafting);
            Iron.Crafting.Set(CraftingTable.Forge);
            FixGameObjects.Add(Iron.Prefab);

            BuildPiece Silver = new("stackedbars", "stack_silverbars");
            Silver.Name.English("Silver Stack");
            Silver.Description.English("Stacked set of bars");
            Silver.RequiredItems.Add("Silver", 20, true);
            Silver.Category.Set(BuildPieceCategory.Crafting);
            Silver.Crafting.Set(CraftingTable.Forge);
            FixGameObjects.Add(Silver.Prefab);

            BuildPiece Tin = new("stackedbars", "stack_tinbars");
            Tin.Name.English("Tin Stack");
            Tin.Description.English("Stacked set of bars");
            Tin.RequiredItems.Add("Tin", 20, true);
            Tin.Category.Set(BuildPieceCategory.Crafting);
            Tin.Crafting.Set(CraftingTable.Workbench);
            FixGameObjects.Add(Tin.Prefab);


            // Reduce by 25% to match vanilla size bars after many updates to the game since this mod was created
            foreach (var fab in FixGameObjects)
            {
                Transform? collider = Utils.FindChild(fab.transform, "collider");
                if (collider != null)
                {
                    collider.localScale = new Vector3(collider.localScale.x * 0.75f, collider.localScale.y * 0.75f, collider.localScale.z * 0.75f);
                }

                var meshRenderers = fab.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    meshRenderer.transform.localScale = new Vector3(meshRenderer.transform.localScale.x * 0.75f, meshRenderer.transform.localScale.y * 0.75f, meshRenderer.transform.localScale.z * 0.75f);
                }
            }

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

    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    static class ZNetScenePatch
    {
        [HarmonyPriority(Priority.Last)]
        static void Postfix(ZNetScene __instance)
        {
            // Get the stack_flametalbars prefab and swap all renderers to use the material from the FlametalNew prefab
            GameObject stack_flametalbars = __instance.GetPrefab("stack_flametalbars");
            GameObject flametalbars = __instance.GetPrefab("FlametalNew");
            Material flametalMaterial = flametalbars.GetComponentInChildren<MeshRenderer>().material;
            foreach (Renderer renderer in stack_flametalbars.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material = flametalMaterial;
            }
        }
    }
}