using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using DG.Tweening;
using Zenta.Core.Runtime.Managers;
using Zenta.Core.Runtime;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<Collectible> bottles;

    [SerializeField] private Transform bottleStacks;

    public ReadOnlyCollection<Collectible> Bottles
    {
        get => bottles.AsReadOnly();
    }

    #region Singleton

    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            AddMoney(other.gameObject.GetComponent<Collectible>());

           if (bottles.Count > 1)
            {
                other.transform.parent = bottleStacks;

                other.transform.localPosition = new Vector3(bottles[bottles.Count - 2].transform.localPosition.x, bottles[bottles.Count - 2].transform.localPosition.y, bottles[bottles.Count - 2].transform.localPosition.z + 0.75f);
            }
            else
            {
                other.transform.parent = bottleStacks;

                other.transform.position = bottleStacks.transform.position;
            }

            other.transform.DOScale(.75f, .25f);
        }
        if (other.gameObject.CompareTag("Money"))
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 1);

            other.gameObject.GetComponent<Money>().moneyPrefab.transform.parent = null;

            other.gameObject.GetComponent<Money>().moneyPrefab.SetActive(true);

            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {

        }
    }

    public bool IsContains(Collectible money)
    {
        return bottles.Contains(money);
    }

    public void AddMoney(Collectible money)
    {
        if (!IsContains(money))
        {
            bottles.Add(money);
        }
    }
    public void RemoveMoney(Collectible money)
    {
        if (IsContains(money))
        {
            bottles.Remove(money);
        }
    }
}
