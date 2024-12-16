using HarmonyLib;
using UnityEngine;

namespace ActionOverflow.Patches
{
    [HarmonyPatch(typeof(PlayerCasting))]
    public static class PlayerCastingPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void SkillSlotsPatch(ref SkillLoadOutSlot[] ____skillLoadoutSlots, ref float[] ____skillCoolDowns)
        {
            SkillLoadOutSlot[] _newLoadoutSlots = new SkillLoadOutSlot[12];
            float[] _newCoolDowns = new float[12];

            for (int i = 0; i < 6; i++)
            {
                _newLoadoutSlots[i] = ____skillLoadoutSlots[i];
                _newCoolDowns[i] = ____skillCoolDowns[i];
            }

            for (int i = 6 ; i < 12; i++)
            {
                SkillLoadOutSlot _newSlot = new SkillLoadOutSlot
                {
                    _scriptSkill = null,
                    _skillStructIndex = 0
                };
                _newLoadoutSlots[i] = _newSlot;
                _newCoolDowns[i] = 0f;
            }

            ____skillLoadoutSlots = _newLoadoutSlots;
            ____skillCoolDowns = _newCoolDowns;
        }

        [HarmonyPatch("Client_SkillControl")]
        [HarmonyPostfix]
        public static void SkillControlPatch(ref PlayerCasting __instance, ref KeyCode ____cachedInput)
        {
            // if (Input.GetKeyDown(InputControlManager.current.actionBar_0))

            if (Input.GetKeyDown(ActionOverflowConfig.action6Key.Value))
            {
                __instance.SendMessage("Client_SelectSkill", 6);
                ____cachedInput = ActionOverflowConfig.action6Key.Value;
            }
            if (Input.GetKeyDown(ActionOverflowConfig.action7Key.Value))
            {
                __instance.SendMessage("Client_SelectSkill", 7);
                ____cachedInput = ActionOverflowConfig.action7Key.Value;
            }
            if (Input.GetKeyDown(ActionOverflowConfig.action8Key.Value))
            {
                __instance.SendMessage("Client_SelectSkill", 8);
                ____cachedInput = ActionOverflowConfig.action8Key.Value;
            }
            if (Input.GetKeyDown(ActionOverflowConfig.action9Key.Value))
            {
                __instance.SendMessage("Client_SelectSkill", 9);
                ____cachedInput = ActionOverflowConfig.action9Key.Value;
            }
            if (Input.GetKeyDown(ActionOverflowConfig.action10Key.Value))
            {
                __instance.SendMessage("Client_SelectSkill", 10);
                ____cachedInput = ActionOverflowConfig.action10Key.Value;
            }
            if (Input.GetKeyDown(ActionOverflowConfig.action11Key.Value))
            {
                __instance.SendMessage("Client_SelectSkill", 11);
                ____cachedInput = ActionOverflowConfig.action11Key.Value;
            }
        }
    }
}
