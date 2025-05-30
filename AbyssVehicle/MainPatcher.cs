using HarmonyLib;
using BepInEx;
using Nautilus.Json;
using Nautilus.Options.Attributes;
using Nautilus.Handlers;
using VehicleFramework;
using VehicleFramework.VehicleTypes;

/*
 * "Pilot Seat" (https://skfb.ly/ot766) by ali_hidayat is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
 * */

namespace AbyssVehicle
{
    [BepInPlugin("com.mikjaw.subnautica.abyss.mod", "AbyssVehicle", "1.3.5")]
    [BepInDependency(VehicleFramework.PluginInfo.PLUGIN_GUID, VehicleFramework.PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(Nautilus.PluginInfo.PLUGIN_GUID)]
    public class MainPatcher : BaseUnityPlugin
    {
        internal static AbyssVehicleConfig config { get; private set; }

        public void Start()
        {
            config = OptionsPanelHandler.RegisterModOptions<AbyssVehicleConfig>();
            var harmony = new Harmony("com.mikjaw.subnautica.abyss.mod");
            harmony.PatchAll();
            Abyss.GetAssets();
            Submarine abyss = Abyss.model.EnsureComponent<Abyss>() as Submarine;
            abyss.name = "Abyss"; // hovertext and spawn-command name
            VehicleRegistrar.RegisterVehicleLater(abyss); // set it and forget it
            //UWE.CoroutineHost.StartCoroutine(Register(abyss)); // manage registration closely
        }

        public System.Collections.IEnumerator Register(Submarine abyss)
        {
            UnityEngine.Coroutine abyssRegistration = UWE.CoroutineHost.StartCoroutine(VehicleRegistrar.RegisterVehicle(abyss));
            yield return abyssRegistration;
            // do something after registration is complete
        }
    }

    [Menu("Abyss Vehicle Options")]
    public class AbyssVehicleConfig : ConfigFile
    {
        [Slider("Abyss GUI Size", Min =0.1f, Max =3f, Step =0.01f)]
        public float guiSize = 0.8f;

        [Slider("Abyss GUI X Position", Min = -900f, Max = 900f, Step = 0.1f)]
        public float guiXPosition = 820;

        [Slider("Abyss GUI Y Position", Min = -500f, Max = 500f, Step = 0.1f)]
        public float guiYPosition = -150f;
    }
}
