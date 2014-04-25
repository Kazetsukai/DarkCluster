using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

    float _lifetime = 1;

    public GameObject Origin;
    public GameObject Target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Origin.transform.position;
        transform.LookAt(Target.transform);
        transform.localScale = new Vector3(1, 1, (Target.transform.position - Origin.transform.position).magnitude);

        if (_lifetime < 0)
        {
            Destroy(this.gameObject);
        }

        _lifetime -= Time.deltaTime;
	}
}
