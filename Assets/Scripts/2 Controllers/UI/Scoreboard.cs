using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GnomeGardeners
{
    public class Scoreboard : CoreUIElement<int>
    {
        [SerializeField] private TMP_Text scoreText;

        public override void UpdateUI(int primaryData)
        {
            if (ClearedIfEmpty(primaryData))
                return;

            UpdateNumericText(scoreText, "{0}", primaryData);
        }

        protected override bool ClearedIfEmpty(int newData)
        {
            if (newData != 0)
                return false;
            return true;
        }
    }
}
