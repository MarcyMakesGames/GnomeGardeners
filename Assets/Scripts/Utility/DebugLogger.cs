using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public static class DebugLogger
    {
        private static bool debug = false;

        public static void Log(System.Object obj, string msg)
        {
            if (debug)
                Debug.Log("[" + obj.ToString() + "]: " + msg);
        }

        public static void LogWarning(System.Object obj, string msg)
        {
            if (debug)
                Debug.LogWarning("[" + obj.ToString() + "]: " + msg);
        }

        public static void LogUpdate(System.Object obj, string msg)
        {
            if (!debug) return;
            if (Time.time % 3f <= Time.deltaTime)
            {
                Debug.Log("[" + obj.ToString() + "]: " + msg);
            }
        }
    }
}
