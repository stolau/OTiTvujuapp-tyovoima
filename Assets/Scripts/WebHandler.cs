using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class WebHandler : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    private string address = "http://134.209.248.117:8000";
    [SerializeField]
    private string room = "otit32";
    [SerializeField]
    private string user = "roimies";

    [SerializeField]
    private GameObject prefabUserObject;

    [SerializeField]
    private GameObject pendingOrderScrollView;

    [SerializeField]
    private List<string> pendingOrders = new List<string>();


    void Start() {
        askOrders();
    }


    public void askOrders() {
        StartCoroutine(getUsersStatus());
    }
    public void resetUserStatus(string user_id) {
        StartCoroutine(resetUserStatusRequest(user_id));
    }
    IEnumerator waitingForNewRequest() {
        yield return new WaitForSeconds(45.0f);
        StartCoroutine(getUsersStatus());
    }
    IEnumerator resetUserStatusRequest(string user_id) {
        // string path = address + "/" + room + "/received?user_id=" + user_id;
        // string path = address + "/" + room + "/order?user_id=" + user_id + "&" + "product_id=" + "0";
        Debug.Log(user_id);
        string path = address + "/" + room + "/received?user_id=" + user_id;
        Debug.Log(path);
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Kaydaaks");
            pendingOrders.Remove(user_id);
        }
    }

    IEnumerator getUsersStatus() {
        string path = address + "/" + room + "/all_users";
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            string json = www.downloadHandler.text;
            JSONNode jsonData = JSON.Parse(json);
            if(jsonData == null) {
                Debug.Log("---NULL---");
            }
            else {
                Debug.Log(jsonData["results"][0]);
                for(int i = 0; i < jsonData["results"].Count; i++) {
                    if(jsonData["results"][i]["order_status"] != 0 && !pendingOrders.Contains(jsonData["results"][i]["user_id"])) {
                        GameObject userObject = Instantiate(prefabUserObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        userObject.transform.SetParent(pendingOrderScrollView.transform);
                        userObject.transform.localScale = new Vector3(1f, 1f, 1f);
                        string user_arg = jsonData["results"][i]["user_id"];
                        int drink_arg = jsonData["results"][i]["order_status"];
                        int count_arg = jsonData["results"][i]["drink_count"];
                        pendingOrders.Add(jsonData["results"][i]["user_id"]);
                        userObject.GetComponent<PendingPrefab>().initPendingPrefab(user_arg, drink_arg, count_arg);

                    }
                }
            }
        }
        // StartCoroutine(getUsersStatus());
        StartCoroutine(waitingForNewRequest());
    }
}
