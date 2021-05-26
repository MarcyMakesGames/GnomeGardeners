using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GnomeGardeners
{
    public class GnomeSelectorController : MonoBehaviour
    {
        [SerializeField] private List<GnomeSkinObject> selectableGnomes;
        private List<GnomeSkinObject> unselectableGnomes;

        public void SelectGnome(GnomeSkinObject gnome)
        {
            if (!selectableGnomes.Contains(gnome))
                return;

            unselectableGnomes.Add(gnome);
            selectableGnomes.Remove(gnome);
        }

        public void UnselectGnome(GnomeSkinObject gnome)
        {
            if (!unselectableGnomes.Contains(gnome))
                return;

            selectableGnomes.Add(gnome);
            unselectableGnomes.Remove(gnome);
        }

        public GnomeSkinObject GetNextGnome(GnomeSkinObject gnome = null)
        {
            if (gnome == null || !selectableGnomes.Contains(gnome) || selectableGnomes.IndexOf(gnome) == selectableGnomes.Count - 1)
                return selectableGnomes[0];

            else
                for (int i = 0; i < selectableGnomes.Count - 1; i++)
                    if (selectableGnomes[i] == gnome)
                        return selectableGnomes[i + 1];

            DebugLogger.Log(this, "Something went wrong finding the next gnome.");
            return null;
        }

        public GnomeSkinObject GetPreviousGnome(GnomeSkinObject gnome = null)
        {
            if (gnome == null || !selectableGnomes.Contains(gnome) || selectableGnomes.IndexOf(gnome) == 0)
                return selectableGnomes[selectableGnomes.Count -1];

            else
                for (int i = selectableGnomes.Count - 1; i > 0; i--)
                    if (selectableGnomes[i] == gnome)
                        return selectableGnomes[i - 1];

            DebugLogger.Log(this, "Something went wrong finding the next gnome.");
            return null;
        }
    }
}
