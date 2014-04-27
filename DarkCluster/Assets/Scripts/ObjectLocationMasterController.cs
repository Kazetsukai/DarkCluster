using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ObjectLocationMasterController : MonoBehaviour {

    Dictionary<int, TrackedObject> _trackedObjects = new Dictionary<int,TrackedObject>();
    int _nextTrackingId = 0;

	// Use this for initialization
	void Start() 
    {
        TrackObject((GameObject)Instantiate(Resources.Load<GameObject>("StarShip")));
        Debug.Log("Created ship");
	}
	
	// Update is called once per frame
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

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            WriteTrackedObjectsTo(stream);
        }
        else
        {
            UpdateTrackedObjectsFrom(stream);
        }
    }

    private void UpdateTrackedObjectsFrom(PhotonStream stream)
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

            if ((bool)stream.ReceiveNext())
            {
                var position = (Vector3)stream.ReceiveNext();
                var rotation = (Quaternion)stream.ReceiveNext();

                if (obj.transform != null)
                {
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                }
            }

            if ((bool)stream.ReceiveNext())
            {
                var velocity = (Vector3)stream.ReceiveNext();
                var angularVelocity = (Vector3)stream.ReceiveNext();

                if (obj.rigidbody != null)
                {
                    obj.rigidbody.velocity = velocity;
                    obj.rigidbody.angularVelocity = angularVelocity;
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