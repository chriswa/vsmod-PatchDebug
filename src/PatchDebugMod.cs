using ProtoBuf;
using System;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using System.Linq;
using System.Collections.Generic;
using Vintagestory.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using System.IO;

[assembly: ModInfo("PatchDebug")]

namespace PatchDebug {
  public class PatchDebugMod : ModSystem {
    public override double ExecuteOrder() {
      return 0.04; // ModJsonPatchLoader is 0.05
    }
    public override void Start(ICoreAPI api) {
      api.Logger.Debug("[PatchDebug] Start");
      base.Start(api);

      var harmony = new Harmony("goxmeor.PatchDebug");
      harmony.PatchAll();
    }
  }
}
