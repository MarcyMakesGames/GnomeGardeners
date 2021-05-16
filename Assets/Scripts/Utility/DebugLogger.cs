using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public static class DebugLogger
    {
        private static bool debug = true;

        public static void Log(System.Type type, string msg)
        {
            if (debug)
                Debug.Log("[" + type.ToString() + "]: " + msg);
        }

        public static void LogWarning(System.Type type, string msg)
        {
            if (debug)
                Debug.LogWarning("[" + type.ToString() + "]: " + msg);
        }
    }
}
