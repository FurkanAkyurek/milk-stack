using UnityEngine;

namespace WSMGameStudio.Splines
{
    public class BakedSegment : MonoBehaviour
    {
        public Transform endPoint;
        public GameObject operationTarget;

        [ContextMenu("Connect Target")]
        public void ConnectTarget()
        {
            ConnectTarget(operationTarget);
        }

        public void ConnectTarget(GameObject target)
        {
            if (endPoint == null)
            {
                Debug.Log(string.Format("{0}: End point not found", name));
                return;
            }

            if (target != null)
            {
                target.transform.position = endPoint.position;
                target.transform.rotation = endPoint.rotation;
            }
        }
    } 
}
