using BepInEx;
using BepInEx.Logging;
using HarmonyLib;


namespace Bit
{
  [BepInPlugin(mod_guid, mod_name, mod_version)]
  public class Plugin : BaseUnityPlugin
  {
    public const string mod_name = "BetterInverseTeleporter";
    public const string mod_guid = "portable-navi.bit";
    public const string mod_version = "1.0.0";

    public static Plugin instance {get {return _instance;}}
    public Harmony patcher {get {return _patcher;}}
    public ManualLogSource log {get {return _log;}}

    private readonly Harmony _patcher = new Harmony(mod_guid);
    private static Plugin _instance;
    private ManualLogSource _log;

    private void Awake()
    {
      if (_instance == null)
      {
        _instance = this;
      }

      _log = BepInEx.Logging.Logger.CreateLogSource(mod_guid);
      _patcher.PatchAll();

      log.LogInfo("Patch applied!");
    }
  }
}
