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

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPassed)
            {
                if (isPassable && multiplyAmount != 12)
                {
                    
                }
                else if (!isPassable || multiplyAmount == 12)
                {
                    StartCoroutine(FinishSequenceDelay());
                }

                isPassed = true;
            }
        }
    }

    IEnumerator FinishSequenceDelay()
    {
        yield return new WaitForSeconds(1f);


        yield return new WaitForSeconds(2f);
        
        GameManager.Instance.CompleteLevel();
    }
}
