using HarmonyLib;
using System;
using System.Reflection;
using Unity.TLS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace ActionOverflow.Patches
{
    [HarmonyPatch(typeof(ActionBarManager))]
    public static class ActionBarManagerPatch
    {
        static readonly AccessTools.FieldRef<ActionBarManager, ActionSlot[]> actionSlotsRef = AccessTools.FieldRefAccess<ActionBarManager, ActionSlot[]>("_actionSlots");
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void AddSlotsPatch(ref ActionBarManager __instance)
        {
            // Resizing arrays originally created with 6 slots in mind
            Array.Resize(ref __instance._actionSlotTransforms, 12);
            Array.Resize(ref actionSlotsRef(__instance), 12);

            // Clone original action bar to repopulate with new slots
            GameObject bottombar = UnityEngine.Object.Instantiate(GameObject.Find("_cell_actionBar"));
            GameObject dolly = GameObject.Find("dolly_bottomBar");

            bottombar.transform.SetParent(dolly.transform);
            bottombar.name = "_cell_actionBar2";
            bottombar.transform.position = new Vector3(960f, 144f, 0);

            // Empty cloned slots in newly created bottom bar
            for (int i = bottombar.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = bottombar.transform.GetChild(i);
                UnityEngine.Object.Destroy(child.gameObject);
            }

            // Build each slot from a clone of the first one (_actionSlot_00)
            // Reconfigure relevant data to be unique to the index.
            for (int i = 6; i < 12; i++)
            {

                // create new actionslot GO
                string name = (i < 10) ? $"_actionSlot_0{i}" : $"_actionSlot_{i}";
                GameObject new_slot_GO = UnityEngine.Object.Instantiate(GameObject.Find("_actionSlot_00"));
                
                new_slot_GO.transform.SetParent(bottombar.transform);

                Vector3 current_transform = new_slot_GO.transform.position;
                Vector3 new_transformation = new Vector3(981.6442f + 116.022f * (i - 6), 234f, 0);

                new_slot_GO.transform.position = new_transformation;
                new_slot_GO.name = name;

                // configure actionslot
                new_slot_GO.GetComponent<ActionSlot>().name = name;
                string actionslothotkeytext;

                switch (i)
                {
                    case 6:
                        actionslothotkeytext = ActionOverflowConfig.action6Key.Value.ToString(); break;
                    case 7:
                        actionslothotkeytext = ActionOverflowConfig.action7Key.Value.ToString(); break;
                    case 8:
                        actionslothotkeytext = ActionOverflowConfig.action8Key.Value.ToString(); break;
                    case 9:
                        actionslothotkeytext = ActionOverflowConfig.action9Key.Value.ToString(); break;
                    case 10:
                        actionslothotkeytext = ActionOverflowConfig.action10Key.Value.ToString(); break;
                    case 11:
                        actionslothotkeytext = ActionOverflowConfig.action11Key.Value.ToString(); break;
                    default:
                        actionslothotkeytext = string.Empty; break;
                }
                new_slot_GO.GetComponent<ActionSlot>()._actionSlotHotkey.text = actionslothotkeytext;

                // Assign default values for required fields
                new_slot_GO.GetComponent<ActionSlot>()._scriptSkill = null; // No skill assigned initially
                new_slot_GO.GetComponent<ActionSlot>()._skillIcon.sprite = ActionBarManager._current._emptySkillIcon;

                // Event trigger fix
                var entry = new_slot_GO.GetComponent<EventTrigger>();

                // Ensure the EventTrigger is initialized
                if (entry.triggers == null)
                    entry.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

                if (entry.triggers.Count == 0)
                {
                    var newEntry = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerClick,
                        callback = new EventTrigger.TriggerEvent()
                    };
                    entry.triggers.Add(newEntry);
                }

                // Replace the entire callback
                var existingEntry = entry.triggers[0];
                existingEntry.callback = new EventTrigger.TriggerEvent();

                MethodInfo methodInfo = typeof(ActionBarManager).GetMethod("Init_SkillToolTip", BindingFlags.Public | BindingFlags.Instance);
                if (methodInfo != null)
                {
                    CachedInvokableCall<int> newInvoker = new CachedInvokableCall<int>(__instance, methodInfo, i);

                    existingEntry.callback.AddListener((eventData) => newInvoker.Invoke(i));
                }

                // Update action slot and action slot transform entries with new values
                __instance._actionSlotTransforms[i] = (RectTransform)new_slot_GO.transform;
                actionSlotsRef(__instance)[i] = new_slot_GO.GetComponent<ActionSlot>();
            }

        }

        [HarmonyPatch("Handle_Actionkeys")]
        [HarmonyPostfix]
        private static void HandleActionkeysPatch(ref ActionBarManager __instance, ref ActionSlot[] ____actionSlots)
        {
            if (!ActionOverflowConfig.barToggle) return;

            ____actionSlots[6]._actionSlotHotkey.text = ActionOverflowConfig.action6Key.Value.ToString();
            ____actionSlots[7]._actionSlotHotkey.text = ActionOverflowConfig.action7Key.Value.ToString();
            ____actionSlots[8]._actionSlotHotkey.text = ActionOverflowConfig.action8Key.Value.ToString();
            ____actionSlots[9]._actionSlotHotkey.text = ActionOverflowConfig.action9Key.Value.ToString();
            ____actionSlots[10]._actionSlotHotkey.text = ActionOverflowConfig.action10Key.Value.ToString();
            ____actionSlots[11]._actionSlotHotkey.text = ActionOverflowConfig.action11Key.Value.ToString();

            KeyCode[] extraKeys = new KeyCode[]
            {
                ActionOverflowConfig.action6Key.Value,
                ActionOverflowConfig.action7Key.Value,
                ActionOverflowConfig.action8Key.Value,
                ActionOverflowConfig.action9Key.Value,
                ActionOverflowConfig.action10Key.Value,
                ActionOverflowConfig.action11Key.Value,
            };

            for (int i = 6; i < 12; i++) 
            {
                int localIndex = i - 6;
                KeyCode slotKey = extraKeys[localIndex];

                if (actionSlotsRef(__instance)[i] != null && actionSlotsRef(__instance)[i]._actionSlotHotkey != null)
                {
                    actionSlotsRef(__instance)[i]._actionSlotHotkey.text = slotKey.ToString();
                }
                    
                if (Input.GetKeyDown(slotKey))
                {
                    __instance.SendMessage("OnActionkeyPress", i);
                }
            }
        }
    }
}
