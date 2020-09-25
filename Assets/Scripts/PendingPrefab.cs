using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PendingPrefab : MonoBehaviour
{

    [SerializeField]
    private GameObject idObject;
    [SerializeField]
    private GameObject drinkObject;
    [SerializeField]
    private GameObject countObject;
    [SerializeField]
    private GameObject button;

    [SerializeField]
    private List<string> ordersToText = new List<string>();

    public void initPendingPrefab(string id, int drink, int count) {
        idObject.GetComponent<TMP_Text>().text = id.Substring(0, 2);
        drinkObject.GetComponent<TMP_Text>().text = ordersToText[drink];
        countObject.GetComponent<TMP_Text>().text = count.ToString();
        button.GetComponent<OrderPendingStatus>().setUserId(id);
    }
}
