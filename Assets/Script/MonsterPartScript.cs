using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterPartScript : MonoBehaviour {//obs

    [SerializeField]
    private GameObject _objectPart;

    [SerializeField]
    public List<MonsterJointScript> _joints;

    [SerializeField]
    private int nbJoint;

    [SerializeField]
    private List<Vector3> positionRelative;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
	}

    public float smooth = 2.0F;
    public float tiltAngle = 60.0F;
    public void Rotate()
    {
        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

    public void ActivateJoints(int nbJointToActivate)
    {
        if (nbJointToActivate <= this._joints.Count)
        {
            for (int i = 0; i < nbJointToActivate; i++)
            {
                this._joints[i].ActivateJoint();
            }
        }
    }

    public void DesactivateJoints()
    {
        for (int i = 0; i < this._joints.Count; i++)
        {
            this._joints[i].DesactivateJoint();
        }
    }

    public void JointWith(int nbJoint, GameObject obj)
    {
        //Debug.Log("" + nbJoint + "   " + obj.name);
        this._joints[nbJoint].JointWith(obj);
    }

    public GameObject getGO()
    {
        return this.gameObject;
    }

    public void DestroyPart()
    {
        Destroy(this.gameObject);
    }
}
