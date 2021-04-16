using System;
using System.IO;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace PatchDebug {
  [HarmonyPatch(typeof(ModJsonPatchLoader))]
  [HarmonyPatch("ApplyPatch")]
  public class Patch_ModJsonPatchLoader_ApplyPatch {
    static string lastAssetDumped = "";
    static IAsset GetPatchableAsset(AssetLocation patchSourcefile, JsonPatch jsonPatch, ICoreAPI api) {
      // side skip logic from original function
      if ((int)((!jsonPatch.Side.HasValue) ? jsonPatch.File.Category.SideType : jsonPatch.Side.Value) != 3 && jsonPatch.Side != (EnumAppSide?)api.Side) {
        return null;
      }

      // skip wildcard patches (we'll get recursed anyways)
      if (jsonPatch.File.Path.EndsWith("*")) { return null; }
      // skip patch source files without ".debug." in the name
      if (!patchSourcefile.Path.Contains(".debug.")) { return null; }

      // load target asset
      AssetLocation targetLocation = jsonPatch.File.Clone();
      if (!targetLocation.Path.EndsWith(".json")) {
        targetLocation.Path = targetLocation.Path + ".json";
      }
      IAsset targetAsset = api.Assets.TryGet(targetLocation, true);

      return targetAsset;
    }
    static void Prefix(AssetLocation patchSourcefile, JsonPatch jsonPatch, int patchIndex, ICoreAPI ___api) {
      ___api.Logger.VerboseDebug("=== PatchDebug === checking {0} => {1} #{2}", patchSourcefile.Path, jsonPatch.File, patchIndex);
      var patchableAsset = GetPatchableAsset(patchSourcefile, jsonPatch, ___api);
      if (patchableAsset != null) {
        if (lastAssetDumped != jsonPatch.File.ToString()) {
          ___api.Logger.VerboseDebug("=== PatchDebug === {0} => {1} #{2} === BEFORE ===", patchSourcefile.Path, jsonPatch.File, patchIndex);
          ___api.Logger.VerboseDebug("{0}", patchableAsset.ToText());
          ___api.Logger.VerboseDebug("=== PatchDebug === {0} => {1} #{2} === /BEFORE ===", patchSourcefile.Path, jsonPatch.File, patchIndex);
        }
      }
    }
    static void Postfix(AssetLocation patchSourcefile, JsonPatch jsonPatch, int patchIndex, ICoreAPI ___api) {
      var patchableAsset = GetPatchableAsset(patchSourcefile, jsonPatch, ___api);
      if (patchableAsset != null) {
        ___api.Logger.VerboseDebug("=== PatchDebug === {0} => {1} #{2} === AFTER ===", patchSourcefile.Path, jsonPatch.File, patchIndex);
        ___api.Logger.VerboseDebug("{0}", patchableAsset.ToText());
        ___api.Logger.VerboseDebug("=== PatchDebug === {0} => {1} #{2} === /AFTER ===", patchSourcefile.Path, jsonPatch.File, patchIndex);
      }
      lastAssetDumped = jsonPatch.File.ToString();
    }
  }
}
