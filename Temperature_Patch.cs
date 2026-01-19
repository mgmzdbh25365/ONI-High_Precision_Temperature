using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace Main
{
	[HarmonyPatch(typeof(GameUtil) , nameof(GameUtil.GetFormattedTemperature))]
	public static class Temperature_Patch
	{
		static bool Prepare() => typeof(GameUtil).GetMethod(nameof(GameUtil.GetFormattedTemperature)) != null;
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
		{
			List<CodeInstruction> code = instr.ToList();
			bool found = false;
			foreach (CodeInstruction codeInstruction in code)
			{
				if (codeInstruction.opcode == OpCodes.Ldc_R4 && (float)codeInstruction.operand == 0.1f){
					codeInstruction.operand = float.MaxValue;
					found = true;
					Debug.Log("High Precision Temperature: Successfully patched.");
				}
			}
			if (!found){
				foreach (CodeInstruction codeInstruction in code)
				{
					if ((codeInstruction.opcode == OpCodes.Ldc_R4 || codeInstruction.operand is float) && Math.Abs((float)codeInstruction.operand - 0.1f) < 0.0001f){
						codeInstruction.operand = float.MaxValue;
						Debug.Log("High Precision Temperature: Successfully patched in other circumstances.");
					}
				}
			}
			return code;
		}
	}

    // since [Game Update] - 674504 
	[HarmonyPatch(typeof(GameUtil) , nameof(GameUtil.AppendFormattedTemperature))]
	public static class Temperature_Patch_New
	{
		static bool Prepare() => typeof(GameUtil).GetMethod(nameof(GameUtil.AppendFormattedTemperature)) != null;
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
		{
			List<CodeInstruction> code = instr.ToList();
			bool found = false;
			foreach (CodeInstruction codeInstruction in code)
			{
				if (codeInstruction.opcode == OpCodes.Ldc_R4 && (float)codeInstruction.operand == 0.1f){
					codeInstruction.operand = float.MaxValue;
					found = true;
					Debug.Log("High Precision Temperature: Successfully patched in the new version.");
				}
			}
			if (!found){
				foreach (CodeInstruction codeInstruction in code)
				{
					if ((codeInstruction.opcode == OpCodes.Ldc_R4 || codeInstruction.operand is float) && Math.Abs((float)codeInstruction.operand - 0.1f) < 0.0001f){
						codeInstruction.operand = float.MaxValue;
						Debug.Log("High Precision Temperature: Successfully patched in other circumstances in the new version.");
					}
				}
			}
			return code;
		}
	}
}
