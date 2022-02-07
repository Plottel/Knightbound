using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTest : MonoBehaviour
{
    private const string wwwURI = "https://izo74v7osh.execute-api.us-east-1.amazonaws.com/default/HelloWorld";

    private void Start()
    {
        StartCoroutine(TickWebRequest());
    }

    IEnumerator TickWebRequest()
    {
        UnityWebRequest www = UnityWebRequest.Get(wwwURI);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Can't Connect");
        }
        else if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success! - Code " + www.responseCode.ToString());
        }
    }
}
