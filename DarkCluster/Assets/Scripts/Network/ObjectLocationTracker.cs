using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// TODO: This class desperately needs to be able to assert that
//       it will never get tracking IDs out of sync across
//       different clients...
public class ObjectLocationTracker : MonoBehaviour {

    Dictionary<int, TrackedObject> _trackedObjects = new Dictionary<int,TrackedObject>();
    int _nextTrackingId = 0;
    public bool ReestimatePosition;

	void Start() 
    {
        TrackObject((GameObject)Instantiate(Resources.Load<GameObject>("StarShip")));
        Debug.Log("Created ship");
	}
	
	void Update()
    {
	}

    public int TrackObject(GameObject obj)
    {
        var trackedObj = obj.AddComponent<TrackedObject>();

        trackedObj.ID = _nextTrackingId;
        _trackedObjects.Add(trackedObj.ID, trackedObj);

        _nextTrackingId++;

        return trackedObj.ID;
    }

    public void StopTrackingObject(GameObject obj)
    {
        var trackedObj = obj.GetComponent<TrackedObject>();
        if (trackedObj != null)
        {
            _trackedObjects.Remove(trackedObj.ID);
        }
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
            else Debug.Log("Ignored frame!");

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
        var objects = _trackedObjects.Values;
        int count = objects.Count;

        stream.SendNext(count);

        int i = 0;
        foreach (var obj in objects)
        {
            if (i >= count)
                break;

            var objTransform = obj.transform;
            var objRigidbody = obj.rigidbody;

            stream.SendNext(obj.ID);

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
}