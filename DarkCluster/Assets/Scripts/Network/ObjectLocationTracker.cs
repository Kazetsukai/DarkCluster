using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using DarkCluster.Core;
using Assets.Scripts.Events;
using Assets.Scripts.Network;

public class ObjectLocationTracker : MonoBehaviour {

    Dictionary<int, TrackedObject> _trackedObjects = new Dictionary<int,TrackedObject>();
    public bool ReestimatePosition;

	void Start() 
    {
	}
	
	void Update()
    {
	}

    public void TrackObject(GameObject obj, int id)
    {
        var trackedObj = obj.AddComponent<TrackedObject>();

        trackedObj.Id = id;
        _trackedObjects.Add(trackedObj.Id, trackedObj);
    }

    public void StopTrackingObject(GameObject obj)
    {
        var trackedObj = obj.GetComponent<TrackedObject>();
        if (trackedObj != null)
        {
            _trackedObjects.Remove(trackedObj.Id);
        }
    }

    public GameObject Get(int id)
    {
        return _trackedObjects[id].gameObject;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            WriteTrackedObjectsTo(stream);
        }
        else
        {
            UpdateTrackedObjectsFrom(stream, info);
        }
    }

    private void UpdateTrackedObjectsFrom(PhotonStream stream, PhotonMessageInfo info)
    {
        var count = (int)stream.ReceiveNext();

        for (int i = 0; i < count; i++)
        {
            var id = (int)stream.ReceiveNext();

            if (!_trackedObjects.ContainsKey(id))
            {
                throw new Exception(String.Format("Object ID {0} doesn't exist, can't update its position!", id));
            }

            var obj = _trackedObjects[id];

            bool update = info.timestamp > obj.LatestTimestamp;
            if (update) obj.LatestTimestamp = info.timestamp;

            if ((bool)stream.ReceiveNext())
            {
                var position = (Vector3)stream.ReceiveNext();
                var rotation = (Quaternion)stream.ReceiveNext();

                if (obj.transform != null && update)
                {
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                }
            }

            if ((bool)stream.ReceiveNext())
            {
                var velocity = (Vector3)stream.ReceiveNext();
                var angularVelocity = (Vector3)stream.ReceiveNext();

                if (obj.rigidbody != null && update)
                {
                    obj.rigidbody.velocity = velocity;
                    obj.rigidbody.angularVelocity = angularVelocity;

                    // Estimate position based on lag
                    if (ReestimatePosition)
                    {
                        double timeLag = PhotonNetwork.time - info.timestamp;
                        Debug.Log(timeLag);
                        obj.transform.position += obj.rigidbody.velocity * (float)timeLag;
                        obj.transform.rotation *= Quaternion.AngleAxis((float)timeLag, obj.rigidbody.angularVelocity);
                    }
                }
            }
        }
    }

    private void WriteTrackedObjectsTo(PhotonStream stream)
    {
        var objects = _trackedObjects.Values.Where(t => !t.Paused);
        int count = objects.Count();

        stream.SendNext(count);

        int i = 0;
        foreach (var obj in objects)
        {
            if (i >= count)
                break;

            var objTransform = obj.transform;
            var objRigidbody = obj.rigidbody;

            stream.SendNext(obj.Id);

            if (objTransform != null)
            {
                stream.SendNext(true);
                stream.SendNext(objTransform.position);
                stream.SendNext(objTransform.rotation);
            }
            else
            {
                stream.SendNext(false);
            }

            if (objRigidbody != null)
            {
                stream.SendNext(true);
                stream.SendNext(objRigidbody.velocity);
                stream.SendNext(objRigidbody.angularVelocity);
            }
            else
            {
                stream.SendNext(false);
            }

            i++;
        }

        // In case any objects were deleted
        while (i < count)
        {
            stream.SendNext(-1);
            stream.SendNext(false);
            stream.SendNext(false);
            i++;
        }
    }
    
    public void PauseTrackingObject(GameObject obj)
    {
        var trackedObj = obj.GetComponent<TrackedObject>();
        if (trackedObj != null)
        {
            trackedObj.Paused = true;
        }
    }

    public void UnpauseTrackingObject(GameObject obj)
    {
        var trackedObj = obj.GetComponent<TrackedObject>();
        if (trackedObj != null)
        {
            trackedObj.Paused = false;
        }
    }
}