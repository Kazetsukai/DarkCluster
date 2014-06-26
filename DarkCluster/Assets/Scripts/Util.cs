using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DarkCluster.Core
{
    public static class Util
    {
        public static NetworkEventAggregator GetEventAggregator()
        {
            return (NetworkEventAggregator)UnityEngine.Object.FindObjectOfType(typeof(NetworkEventAggregator));
        }

        public static ObjectLocationTracker GetObjectTracker()
        {
            return (ObjectLocationTracker)UnityEngine.Object.FindObjectOfType(typeof(ObjectLocationTracker));
        }

        public static GameObject GetShip()
        {
            return GameObject.Find("StarShip(Clone)");
        }
    }
}
