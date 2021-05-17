using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    [CreateAssetMenu(fileName = "Object", menuName = "World/Object")]
    public class SetupObject : ScriptableObject
    {
        public GameObject gameObject;
        public Vector2Int position;
    }
}
