using UnityEngine;
using System.Collections;
using Caliburn.Micro;
using System;
using System.Xml.Serialization;
using System.IO;
using DarkCluster.Core.Services;

public class NetworkEventAggregator : Photon.MonoBehaviour
{

    IEventAggregator _eventAggregator;

	// Use this for initialization
	void Start () {
        _eventAggregator = new EventAggregator();

        foreach (var service in ServiceBootstrapper.GetServices())
        {
            Debug.Log("Registering " + service);
            _eventAggregator.Subscribe(service);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Publish<T>(T message)
    {
        var serializedValue = Serialize(message);
        if (!PhotonNetwork.offlineMode)
        {
            Debug.Log("Publishing " + message);
            photonView.RPC("PublishRemote", PhotonTargets.AllViaServer, serializedValue, typeof(T).FullName);
        }
        else
        {
            Debug.Log("Publishing " + message + " offline");
            _eventAggregator.Publish(message);
        }
    }

    private static string Serialize<T>(T message)
    {
        var serializer = new XmlSerializer(typeof(T));
        var writer = new StringWriter();

        serializer.Serialize(writer, message);
        
        return writer.GetStringBuilder().ToString();
    }

    private static object Deserialize(string message, string type)
    {
        Type strongType = Type.GetType(type);

        var serializer = new XmlSerializer(strongType);

        var obj = serializer.Deserialize(new StringReader(message));
        return obj;
    }

    [RPC]
    private void PublishRemote(string message, string type)
    {
        var obj = Deserialize(message, type);

        _eventAggregator.Publish(obj);
    }
}
