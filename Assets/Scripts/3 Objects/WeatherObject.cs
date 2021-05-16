using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace GnomeGardeners
{
    [System.Serializable]
    public struct WeatherObject
    {
        public WeatherType weatherType;
        public ParticleSystem weatherParticleSys;
        public PostProcessProfile weatherProfile;
    }
}
