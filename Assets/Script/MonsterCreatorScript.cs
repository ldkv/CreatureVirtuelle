using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterCreatorScript : MonoBehaviour {

    [SerializeField]
    private List<GameObject> _partsPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject getRandomPart()
    {
        if (_partsPrefab.Count != 0)
        {
            int result = Random.Range(0, _partsPrefab.Count);
            //-------------------Debug Log---------------------
            Debug.Log("part" + result);
            //-------------------Debug Log---------------------
            return _partsPrefab[result];
        }
        return null;
    }

    public int getRandomPartID()
    {
        if (_partsPrefab.Count != 0)
        {
            int result = Random.Range(0, _partsPrefab.Count);
            return result;
        }
        return 0;
    }

    //Method codé en dur en atendant une meilleur option
    public int getNumberOfJointWithId(int id)
    {
        if (id == 0)
            return 4;
        if (id == 1)
            return 2;
        if (id == 2)
            return 6;
        return 0;
    }

    public MonsterPartScript InstanciatePart(int idPart)
    {
        GameObject part = (GameObject)Instantiate(this._partsPrefab[idPart], new Vector3(2.0f, 5.0f, 0), Quaternion.identity);
        return part.GetComponent<MonsterPartScript>();
    }
}

