using UnityEngine;
using System.Collections;

public class MonsterJointScript : MonoBehaviour {

    [SerializeField]
    private SpringJoint _springJoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateJoint()
    {
        this.gameObject.SetActive(true);
    }

    public void DesactivateJoint()
    {
        this.gameObject.SetActive(false);
    }

    public void JointWith(GameObject obj)
    {
        _springJoint.connectedBody = obj.GetComponent<Rigidbody>();
    }
}
