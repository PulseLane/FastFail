using HarmonyLib;
using LogLevel = IPA.Logging.Logger.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;

namespace FastFail.HarmonyPatches
{
    [HarmonyPatch]
    class MissionFailedPatcher
    {
        static System.Reflection.MethodBase TargetMethod()
        {
            return typeof(MissionLevelFailedController)
                .GetNestedTypes(BindingFlags.NonPublic)
                .Single(x => x.FullName == "MissionLevelFailedController+<LevelFailedCoroutine>d__13")
                .GetMethod("MoveNext", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            removeMethodCallFromInstructions(ref newInstructions, "Void StopSpawning()");
            removeMethodCallFromInstructions(ref newInstructions, "Void DissolveAllObjects()");
            removeMethodCallFromInstructions(ref newInstructions, "Void ShowEffect()");

            // yield WaitForSeconds(2) -> yield WaitForSeconds(0)
            for (int i = 0; i < newInstructions.Count; i++)
            {
                if (newInstructions[i].opcode == OpCodes.Ldc_R4 && newInstructions[i].operand.ToString() == "2")
                {
                    newInstructions[i] = new CodeInstruction(OpCodes.Ldc_R4, 0.02F);
                }
            }

            return newInstructions;
        }

        private static void removeMethodCallFromInstructions(ref List<CodeInstruction> instructions, String methodName)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                var instruction = instructions[i];
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand.ToString() == methodName)
                {
                    // Remove:
                    //  ldloc.1
                    //  ldfld
                    //  callvirt
                    for (int j = -2; j <= 0; j++)
                    {
                        instructions[i + j] = new CodeInstruction(OpCodes.Nop);
                    }
                    return;
                }
            }
        }
    }
}
