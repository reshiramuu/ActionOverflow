using BepInEx.Configuration;
using Nessie.ATLYSS.EasySettings.UIElements;
using UnityEngine;

namespace ActionOverflow
{
    public static class ActionOverflowConfig
    {
        // raw data types
        public static KeyCode _actionBarInput_6;
        public static KeyCode _actionBarInput_7;
        public static KeyCode _actionBarInput_8;
        public static KeyCode _actionBarInput_9;
        public static KeyCode _actionBarInput_10;
        public static KeyCode _actionBarInput_11;
        public static bool barToggle;

        // EasySettings data types
        public static AtlyssKeyButton _actionBarInput_6_element;
        public static AtlyssKeyButton _actionBarInput_7_element;
        public static AtlyssKeyButton _actionBarInput_8_element;
        public static AtlyssKeyButton _actionBarInput_9_element;
        public static AtlyssKeyButton _actionBarInput_10_element;
        public static AtlyssKeyButton _actionBarInput_11_element;
        public static AtlyssToggle barToggle_element;

        /*
         * ConfigEntries
         * 
         */

        public static ConfigEntry<KeyCode> action6Key;
        public static ConfigEntry<KeyCode> action7Key;
        public static ConfigEntry <KeyCode> action8Key;
        public static ConfigEntry<KeyCode> action9Key;
        public static ConfigEntry<KeyCode> action10Key;
        public static ConfigEntry<KeyCode> action11Key;
        public static ConfigEntry<bool> _barToggle;
    }
}
