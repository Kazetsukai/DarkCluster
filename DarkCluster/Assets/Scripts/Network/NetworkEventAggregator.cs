using UnityEngine;
using System.Collections;
using Caliburn.Micro;
using System;
using System.Xml.Serialization;
using System.IO;

public class NetworkEventAggregator : Photon.MonoBehaviour, IHandle<string>, IHandle<int>
{

    IEventAggregator _eventAggregator;

	// Use this for initialization
	void Start () {
        _eventAggregator = new EventAggregator();

        _eventAggregator.Subscribe(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Publish<T>(T message)
    {
        var serializedValue = Serialize(message);
        photonView.RPC("PublishRemote", PhotonTargets.AllViaServer, serializedValue, typeof(T).FullName);
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

    public void Handle(string message)
    {
        Debug.Log("Handled a string!");
    }
    public void Handle(int message)
    {
        Debug.Log("Handled an int!");
    }
}
