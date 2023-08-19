using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace Main
{
	[HarmonyPatch(typeof(GameUtil) , "GetFormattedTemperature")]
	public static class Temperature_Patch
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
		{
			List<CodeInstruction> code = instr.ToList();
			foreach (CodeInstruction codeInstruction in code)
			{
				if (codeInstruction.opcode == OpCodes.Ldc_R4 && (float)codeInstruction.operand == 0.1f)
					codeInstruction.operand = float.MaxValue;
				yield return codeInstruction;
			}
		}
	}
}
