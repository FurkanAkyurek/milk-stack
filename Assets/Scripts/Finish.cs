using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Finish : MonoBehaviour
{
    [SerializeField] private FinishStep[] finishSteps;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player arrived finish.");

            PlayerMovement.Instance.splineFollower.speed += .5f;

            Vector3 tempPosition = other.transform.position;

            tempPosition.y += 0.5f;

            int stepCount = PlayerController.Instance.Bottles.Count;

            for (int i = 0; i < finishSteps.Length; i++)
            {
                if (stepCount > i)
                {
                    finishSteps[i].isPassable = true;
                    Debug.Log(finishSteps[i].name + " : Açık");
                }
                else
                {
                    finishSteps[i].isPassable = false;
                    Debug.Log(finishSteps[i].name + " : Kapalı");
                }
            }
        }
    }
}
