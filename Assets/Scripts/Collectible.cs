using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenta.Core.Runtime.Managers;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Status status;

    [SerializeField] private GameObject[] bottleTypes;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            PlayerController.Instance.RemoveBottle(this.gameObject.GetComponent<Collectible>());

            Destroy(this.gameObject);

            if(PlayerController.Instance.Bottles.Count <= 0)
            {
                GameManager.Instance.FailLevel();
            }
        }
        if (other.gameObject.CompareTag("Capping"))
        {
            TransformCappingBottle();
        }
        if (other.gameObject.CompareTag("Stuffing"))
        {
            TransformStuffingBottle();
        }
        if (other.gameObject.CompareTag("Pasteurizer"))
        {
            TransformPasteurizeBottle();
        }
    }
    void TransformStuffingBottle()
    {
        switch (status)
        {
            case Status.Empty:
                bottleTypes[0].SetActive(false);
                bottleTypes[1].SetActive(true);
                status = Status.Full;
                break;
            case Status.EmptyWithCap:
                break;
            case Status.Full:
                break;
            case Status.FullWithCap:
                bottleTypes[3].SetActive(false);
                bottleTypes[1].SetActive(true);
                status = Status.Finish;
                break;
            case Status.Finish:
                break;
            default:
                break;
        }
    }
    void TransformPasteurizeBottle()
    {
        switch (status)
        {
            case Status.Empty:
                break;
            case Status.EmptyWithCap:
                break;
            case Status.Full:
                bottleTypes[2].SetActive(true);
                status = Status.Full;
                break;
            case Status.FullWithCap:
                break;
            case Status.Finish:
                bottleTypes[2].SetActive(true);
                break;
            default:
                break;
        }
    }
    void TransformCappingBottle()
    {
        switch (status)
        {
            case Status.Empty:
                bottleTypes[0].SetActive(false);
                bottleTypes[3].SetActive(true);
                status = Status.EmptyWithCap;
                break;
            case Status.EmptyWithCap:
                break;
            case Status.Full:
                bottleTypes[1].SetActive(false);
                bottleTypes[2].SetActive(false);
                bottleTypes[4].SetActive(true);
                status = Status.FullWithCap;
                break;
            case Status.FullWithCap:
                break;
            case Status.Finish:
                break;
            default:
                break;
        }
    }
}

public enum Status
{
    Empty,
    EmptyWithCap,
    Full,
    FullWithCap,
    Finish
}
