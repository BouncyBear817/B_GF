using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Example_WebSocket.LoginData loginData = new Example_WebSocket.LoginData("zhangsan", "123456");
        var json = JsonConvert.SerializeObject(loginData);
        Debug.Log(json);
        var request = UnityWebRequest.Post("http://shixun.ruzhoukj.com/auth/login/in", json, "application/json");
        yield return request.SendWebRequest();
        
        Debug.Log(request.GetResponseHeader("Content-Type"));
        
        Debug.Log(request.error);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
