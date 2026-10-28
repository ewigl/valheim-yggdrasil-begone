using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace YggdrasilBegone
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    internal class YggdrasilBegone : BaseUnityPlugin
    {
        public const string PluginGUID = "ewigl.YggdrasilBegone";
        public const string PluginName = "Yggdrasil Begone";
        public const string PluginVersion = "0.0.2";

        private readonly Harmony _harmony = new("ewigl.YggdrasilBegone");

        public static ConfigEntry<bool> ModEnabled;

        public static GameObject YggdrasilObject;

        public void Awake()
        {
            ModEnabled = Config.Bind("General", "Enabled", true, "Hide Yggdrasil.");

            ModEnabled.SettingChanged += (sender, e) =>
            {
                UpdateYggdrasilState();
            };

            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void UpdateYggdrasilState()
        {
            if (YggdrasilObject == null)
            {
                YggdrasilObject = GameObject.Find("YggdrasilBranch");
            }

            YggdrasilObject?.SetActive(!ModEnabled.Value);
        }
    }

    [HarmonyPatch(typeof(ZNetScene), "Awake")]
    internal static class ZNetSceneAwakePatch
    {
        private static void Postfix(ZNetScene __instance)
        {
            YggdrasilBegone.YggdrasilObject = GameObject.Find("YggdrasilBranch");
            YggdrasilBegone.UpdateYggdrasilState();
        }
    }
}