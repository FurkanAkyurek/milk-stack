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
            PlayerMovement.Instance.splineFollower.speed += .5f;

            Vector3 tempPosition = other.transform.position;

            tempPosition.y += 0.5f;

            int stepCount = PlayerController.Instance.Bottles.Count;

            for (int i = 0; i < finishSteps.Length; i++)
            {
                if (stepCount > i)
                {
                    finishSteps[i].isPassable = true;
                }
                else
                {
                    finishSteps[i].isPassable = false;
                }
            }
        }
    }
}
