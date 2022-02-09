using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using NativeWebSocket;

public class NetworkTest : MonoBehaviour
{
    const string wwwURI = "https://izo74v7osh.execute-api.us-east-1.amazonaws.com/default/HelloWorld";
    const string webSocketURI = "wss://2r9nszqos1.execute-api.us-east-1.amazonaws.com/dev";

    private WebSocket natSocket;

    private void Start()
    {
        natSocket = new WebSocket(webSocketURI);
        natSocket.OnOpen += OnNATSocketOpen;
        natSocket.OnMessage += OnNATSocketMessage;

        ConnectSocket();

        //StartCoroutine(TickWebRequest());
    }

    async void ConnectSocket()
    {
        await natSocket.Connect();
    }

    private void Update()
    {
        Debug.Log("Updating");
        if (Input.GetKeyDown(KeyCode.Space))
            SendSocketMessage("Ping");

        Debug.Log("Socket State: " + natSocket.State.ToString());
        natSocket.DispatchMessageQueue();
    }

    async void SendSocketMessage(string message)
    {
        Debug.Log("Sending Msg");
        await natSocket.SendText(message);
    }

    private void OnNATSocketMessage(byte[] data)
    {
        string message = Encoding.UTF8.GetString(data);
        Debug.Log("Socket Msg: " + message);
    }

    private void OnNATSocketOpen()
    {
        Debug.Log("Successfully opened NAT Socket!");
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
