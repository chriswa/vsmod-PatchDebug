using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;

[assembly: ModInfo("PatchDebug")]

namespace PatchDebug {
  public class PatchDebugMod : ModSystem {
    public override double ExecuteOrder() {
      return 0.049; // ModJsonPatchLoader is 0.05
    }
    public override void Start(ICoreAPI api) {
      api.Logger.Debug("[PatchDebug] Start");
      var harmony = new Harmony("goxmeor.PatchDebug");
      harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
  }
}
