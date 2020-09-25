using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPendingStatus : MonoBehaviour
{
    [SerializeField]
    private int status = 0;
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private GameObject menuManager;

    private IEnumerator cor;

    [SerializeField]
    private string user_id;

    void Start() {
        gameObject.GetComponent<Image>().color = Color.green;
        menuManager = GameObject.FindWithTag("MenuManager");
    }


    public void changeStatus() {
        if(status == 0) {
            gameObject.GetComponent<Image>().color = Color.blue;
            status = 1;
        }
        else if(status == 1) {
            status = 2;
            gameObject.GetComponent<Image>().color = Color.grey;
            cor = destroying_Countdown();
            StartCoroutine(cor);
        }
        else if(status == 2)  {
            status = 1;
            gameObject.GetComponent<Image>().color = Color.blue;
            StopCoroutine(cor);
        }
    }
    public void setUserId(string user_id_args) {
        user_id = user_id_args;
    }

    IEnumerator destroying_Countdown() {
        yield return new WaitForSeconds(10.0f);
        menuManager.GetComponent<WebHandler>().resetUserStatus(user_id);

        Destroy(parent);
    }
}
