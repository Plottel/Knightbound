using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using NativeWebSocket;

public class NetworkTest : MonoBehaviour
{
    const string wwwURI = "https://izo74v7osh.execute-api.us-east-1.amazonaws.com/default/HelloWorld";
    const string webSocketURI = "wss://b1y9fhlqtj.execute-api.us-east-1.amazonaws.com/production";

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
        if (Input.GetKeyDown(KeyCode.Space))
            SendSocketMessage("Ping");

        Debug.Log("Socket State: " + natSocket.State.ToString());
    }

    async void SendSocketMessage(string message)
    {
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
