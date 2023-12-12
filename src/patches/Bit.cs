using System.Reflection;
using System.Collections;
using HarmonyLib;
using GameNetcodeStuff;
using UnityEngine;

namespace Bit.patches
{
  
  [HarmonyPatch]
  class Bit
  {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ShipTeleporter), "TeleportPlayerOutWithInverseTeleporter")]
    public static bool teleport(int playerObj, ref Vector3 teleportPos, ShipTeleporter __instance)
    { 

      // Fetching private methods...
      MethodInfo teleportBodyOut = typeof(ShipTeleporter).GetMethod("teleportBodyOut", BindingFlags.NonPublic | BindingFlags.Instance);
      MethodInfo SetPlayerTeleporterId = typeof(ShipTeleporter).GetMethod("SetPlayerTeleporterId", BindingFlags.NonPublic | BindingFlags.Instance);


       // Modified method body that does not drop items //
      ///////////////////////////////////////////////////

      if (StartOfRound.Instance.allPlayerScripts[playerObj].isPlayerDead)
      {
        __instance.StartCoroutine((IEnumerator)teleportBodyOut.Invoke(__instance, new object[] {playerObj, teleportPos}));
        return false;
      }

      //TODO: Should only certaint items be be allowed to be brought through the teleporter? 

      PlayerControllerB playerControllerB = StartOfRound.Instance.allPlayerScripts[playerObj];
      SetPlayerTeleporterId.Invoke(__instance, new object[] {playerControllerB, -1});

      if ((bool)Object.FindObjectOfType<AudioReverbPresets>())
      {
        Object.FindObjectOfType<AudioReverbPresets>().audioPresets[2].ChangeAudioReverbForPlayer(playerControllerB);
      }

      playerControllerB.isInElevator = false;
      playerControllerB.isInHangarShipRoom = false;
      playerControllerB.isInsideFactory = true;
      playerControllerB.averageVelocity = 0f;
      playerControllerB.velocityLastFrame = Vector3.zero;

      StartOfRound.Instance.allPlayerScripts[playerObj].TeleportPlayer(teleportPos);
      StartOfRound.Instance.allPlayerScripts[playerObj].beamOutParticle.Play();
      __instance.shipTeleporterAudio.PlayOneShot(__instance.teleporterBeamUpSFX);
      StartOfRound.Instance.allPlayerScripts[playerObj].movementAudio.PlayOneShot(__instance.teleporterBeamUpSFX);
      
      if (playerControllerB == GameNetworkManager.Instance.localPlayerController)
      {
        HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
      }

      // Return false so that the original's body isn't run.
      return false;
    }
  }
}
