using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    [CreateAssetMenu(fileName = "Need", menuName = "Plants/Need")]
    public class Need : ScriptableObject
    {
        public NeedType type;
        public PoolKey popUpType;
    }
}
