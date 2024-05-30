using System.Linq;
using UnityEngine;

public abstract class CheckForPlayerOverlap : MonoBehaviour
{
    public GameObject player;

    public void CheckOverlap()
    {
        if (Physics.OverlapBox(transform.position, transform.localScale / 2).Where(x => x.transform.gameObject == player).Count() > 0)
        {
            OnOverlap(true);
            return;
        }

        OnOverlap(false);
    }

    public abstract void OnOverlap(bool overlapping);
}
