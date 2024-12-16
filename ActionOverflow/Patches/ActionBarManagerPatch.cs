using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Collections;

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
            Array.Resize(ref __instance._actionSlotTransforms, 12);
            Array.Resize(ref actionSlotsRef(__instance), 12);

            GameObject bottombar = UnityEngine.Object.Instantiate(GameObject.Find("_cell_actionBar"));
            GameObject dolly = GameObject.Find("dolly_bottomBar");
            bottombar.transform.SetParent(dolly.transform);
            bottombar.name = "_cell_actionBar2";
            bottombar.transform.position = new Vector3(960f, 169f, 0);
            for (int i = bottombar.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = bottombar.transform.GetChild(i);
                UnityEngine.Object.Destroy(child.gameObject);
            }

            for (int i = 6; i < 12; i++)
            {

                // create new actionslot GO
                string name = (i < 10) ? $"_actionSlot_0{i}" : $"_actionSlot_{i}";
                GameObject new_slot_GO = UnityEngine.Object.Instantiate(GameObject.Find("_actionSlot_00"));
                
                new_slot_GO.transform.SetParent(bottombar.transform);
                Vector3 current_transform = new_slot_GO.transform.position;
                Vector3 new_transformation = new Vector3(981.6442f + 116.022f * (i - 6), 244f, 0);
                new_slot_GO.transform.position = new_transformation;
                new_slot_GO.name = name;

                RectTransform rect = new_slot_GO.GetComponent<RectTransform>();
                
                // configure transform
                rect.name = name;
                rect.anchoredPosition = new Vector2(35f + 70*(i-6), -50f);
                rect.anchoredPosition3D = new Vector3(35f + 70 * (i - 6), -50f, 0f);
                rect.anchorMax = new Vector2(0f, 1f);
                rect.anchorMin = new Vector2(0f, 1f);
                
                rect.offsetMax = new Vector2(63.84f + 70f*(i - 6), 10f);
                rect.offsetMin = new Vector2(3.8243f + 70f*(i -  6), -50f);
                rect.pivot = new Vector2(0.5f, 0.5f);

                rect.sizeDelta = new Vector2(60f, 60f);
                rect.position = new Vector3(989.962f + 116.022f*(i-6), 244f, 0);

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
                //new_slot_GO.GetComponent<ActionSlot>()._skillIcon = newslot.AddComponent<Image>(); // Add an Image component for the skill icon
                new_slot_GO.GetComponent<ActionSlot>()._skillIcon.sprite = ActionBarManager._current._emptySkillIcon; // Assign default empty icon

                // event reflection
                

                EventTrigger trigger = new_slot_GO.GetComponent<EventTrigger>();


                if (trigger != null)
                {
                    // first one
                    var entry = trigger.triggers[0];
                    // this is going to suck
                    //get callback
                    FieldInfo callback = entry.GetType().GetField("callback", BindingFlags.NonPublic | BindingFlags.Instance);
                    EventTrigger.TriggerEvent callback_instance = (EventTrigger.TriggerEvent)callback.GetValue(entry);

                    FieldInfo m_Calls = typeof(UnityEventBase).GetType().GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);    
                    var m_Calls_instance = m_Calls.GetValue(callback_instance);

                    FieldInfo m_ExecutingCallsField = m_Calls_instance.GetType().GetField("m_ExecutingCalls", BindingFlags.NonPublic | BindingFlags.Instance);
                    var executingCalls = m_ExecutingCallsField.GetValue(m_Calls_instance) as IList;

                }

                __instance._actionSlotTransforms[i] = rect;
                actionSlotsRef(__instance)[i] = new_slot_GO.GetComponent<ActionSlot>();
            }

        }

        [HarmonyPatch("Handle_Actionkeys")]
        [HarmonyPostfix]
        private static void HandleActionkeysPatch(ref ActionBarManager __instance, ref ActionSlot[] ____actionSlots)
        {
            if (!ActionOverflowConfig.barToggle)
            {
                //ChatBehaviour._current.New_ChatMessage("Falling out of ActionKeysPatch");
                return;
            }

            // _actionSlots[0]._actionSlotHotkey.text = InputControlManager.current.actionBar_0.ToString();
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
