using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CanvasCursor : MonoBehaviour
{
    private class CursorInfo
    {
        private XRRayInteractor interactor;
        private GameObject cursor;

        public CursorInfo(XRRayInteractor interactor, GameObject cursorPrefab)
        {
            this.interactor = interactor;
            cursor = Instantiate(cursorPrefab);

            Canvas canvas = cursor.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }

        public void UpdateCursor()
        {
            Vector3 hitPosition = new Vector3();
            Vector3 normalPosition = new Vector3();
            int positionInLine = 0;
            bool isTargetValid = false;
            if (interactor.TryGetHitInfo(ref hitPosition, ref normalPosition, ref positionInLine, ref isTargetValid))
            {
                cursor.SetActive(isTargetValid);
                cursor.transform.position = hitPosition + (normalPosition * 0.01f);
                cursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, normalPosition);
                return;
            }

            cursor.SetActive(false);
        }
    }

    public GameObject cursorPrefab;
    private List<CursorInfo> cursors = new List<CursorInfo>();

    private void Start()
    {
        //#if UNITY_EDITOR
        //        return;
        //#endif

        XRRayInteractor[] rayInteractors = FindObjectsOfType<XRRayInteractor>();

        for (int i = 0; i < rayInteractors.Length; i++)
        {
            cursors.Add(new CursorInfo(rayInteractors[i], cursorPrefab));
        }
    }

    private void Update()
    {
        cursors.ForEach(cursor => cursor.UpdateCursor());
    }
}
