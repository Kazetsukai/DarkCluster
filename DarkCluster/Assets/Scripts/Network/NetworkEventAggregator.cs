using UnityEngine;
using System.Collections;
using Caliburn.Micro;
using System;
using System.Xml.Serialization;
using System.IO;
using DarkCluster.Core.Services;
using Assets.Scripts.Events;
using Assets.Scripts.Network;

public class NetworkEventAggregator : MonoBehaviour
{
    IEventAggregator _eventAggregator;

    // First 9999 ids are reserved.
    int _nextEventId = 10000;

	void Start () {
        _eventAggregator = new EventAggregator();

        foreach (var service in ServiceBootstrapper.GetServices())
        {
            Debug.Log("Registering " + service);
            _eventAggregator.Subscribe(service);
        }
	}
	
	void Update () {
	
	}

    public void Publish<T>(T message)
    {
        var serializedValue = Serialize(message);
    	Debug.Log("Publishing " + message + " offline");
    	_eventAggregator.Publish(message);
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

        if (obj is IEvent)
            ((IEvent)obj).EventId = _nextEventId++;

        _eventAggregator.Publish(obj);
    }
}
