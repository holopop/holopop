using UnityEngine;
using System.Collections;
using System;


public class NetworkService {

    private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?APPID=ece782509787d5776e77a5ad1b3d5b01&q=Chicago,us&mode=xml";

    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";
    private const string localApi = "http://localhost/ch9/api.php";
    
    private IEnumerator CallAPI(string url, Hashtable args, Action<string> callback)
    {
        WWW www;
        if (args == null)
        {
            www = new WWW(url);
        }
        else
        {
            WWWForm form = new WWWForm();
            foreach (DictionaryEntry arg in args)
            {
                form.AddField(arg.Key.ToString(), arg.Value.ToString());
            }
            www = new WWW(url, form);
        }
        yield return www;
    }

    private bool IsResponseValid(WWW www) {
        if (www.error != null)
        {
            Debug.Log("bad connection");
            return false;
        }
        else if (string.IsNullOrEmpty(www.text))
        {
            Debug.Log("null or empty data");
            return false;
        }
        else
        {
            return true;
        }
    }



    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        WWW www = new WWW(webImage);
        yield return www;
        callback(www.texture);
    }

    public IEnumerator LogWeather(string name, float cloudValue, Action<string> callback)
    {
        Hashtable args = new Hashtable();
        args.Add("message", name);
        args.Add("cloud_value", cloudValue);
        args.Add("timestamp", DateTime.UtcNow.Ticks);
        return CallAPI(localApi, args, callback);
    }

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        return CallAPI(xmlApi, null, callback);
    }
	
}
