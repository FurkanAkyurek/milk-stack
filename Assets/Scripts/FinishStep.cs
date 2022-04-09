using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenta.Core.Runtime.Managers;

public class FinishStep : MonoBehaviour
{
    [SerializeField] private Transform milkTarget;

    public float multiplyAmount;

    public bool isPassable = false;
    private bool isPassed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPassed)
            {
                if (isPassable && multiplyAmount != 12)
                {
                    Collectible lastCollectible = PlayerController.Instance.Bottles[PlayerController.Instance.Bottles.Count - 1];

                    PlayerController.Instance.RemoveBottle(lastCollectible);

                    lastCollectible.gameObject.GetComponent<BoxCollider>().enabled = false;

                    lastCollectible.gameObject.transform.parent = null;

                    lastCollectible.gameObject.transform.DOMove(milkTarget.position, 0.7f);
                }
                else if (!isPassable || multiplyAmount == 12)
                {
                    FinishSequenceDelay();
                }

                isPassed = true;
            }
        }
    }

    void FinishSequenceDelay()
    {
        PlayerMovement.Instance.splineFollower.speed = 0f;

        GameManager.Instance.CompleteLevel();
    }
}
