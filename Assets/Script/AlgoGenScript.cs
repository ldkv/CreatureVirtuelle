using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//___________________________ ALGO GEN _______________________________

public class AlgoGenScript : MonoBehaviour {

    [SerializeField]
    private MonsterCreatorScript mScript;

    private TreeElementBranche root;

    private List<TreeElementBranche> roots;

    private List<TreeElementJoint> joints;

    private float targetTime = 10.0f;

    // Use this for initialization
    void Start()
    {
        joints = new List<TreeElementJoint>();
        root = new TreeElementBranche(mScript);
        roots = new List<TreeElementBranche>();
        preparetest();
        //roots[0].RandomTreeGeneration();
        roots[0].InstanciateMonster();
        /*
        for (int i = 0; i < joints.Count; i++)
        {
            Debug.Log(" joints size"+joints.Count+" : joint " + joints[i]._treeIteration);
        }*/
        //root.DestroyMonster();
    }

	// Update is called once per frame
	void Update () {
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            nextTest();
            targetTime = 10.0f;
        }
	}

    public void CompareGen()
    {

    }

    public void nextTest()
    {
        roots[0].DestroyMonster();
        randomMutation();
        roots[0].InstanciateMonster();
    }

    public void MuteGenOnIteration(int indexIteration)
    {

    }

    public void preparetest()
    {
        roots.Add(new TreeElementBranche(mScript));
        //roots.Add(new TreeElementBranche(mScript));
        //roots.Add(new TreeElementBranche(mScript));
        roots[0].RandomTreeGeneration();
        //roots[1].RandomTreeGeneration();
        //roots[2].RandomTreeGeneration();
    }

    public void randomMutation()
    {
        joints.Clear();
        roots[0].getAllJointInTree(joints);
        int res =Random.Range(0, joints.Count);
        MuteGenOnTree(res);

        Debug.Log(" muta "+res);
    }

    public void MuteGenOnTree(int indexJoint)
    {
        joints[indexJoint].GenerateMonsterWithTree();
    }

    public void CrossGen(int index, int length)
    {

    }
}

//___________________________ TREE ELEMENT _______________________________

public class TreeElement {

    public int _treeIteration;

    public MonsterCreatorScript _monsterCreator;

    public TreeElement RandomCreateTreeElement(int treeIteration)
    {
        return null;
    }

    public TreeElementBranche RandomCreateTreeBranche(TreeElementJoint joint)
    {
        return new TreeElementBranche(joint, _monsterCreator.getRandomPartID());
    }

    public TreeElementJoint RandomCreateTreeJoint(int treeIteration)
    {
        return null;
    }

}

//___________________________ TREE ELEMENT : JOINT _______________________________

public class TreeElementJoint : TreeElement
{
    private Quaternion _rotationJoint;

    private TreeElementBranche _rootBranche;
    private TreeElementBranche _branche;

    //A ca creation un joint creer automatique la branche suivante car il n'a aucune autre utilité que de relié 2 branche.
    //C'est ici qu'est generer aleatoirement la prochaine branche
    public TreeElementJoint(TreeElementBranche rootBranche)
    {
        this._monsterCreator = rootBranche._monsterCreator;
        _treeIteration = rootBranche._treeIteration + 1;
        //-------------------Debug Log---------------------
        Debug.Log("new Tree joint" + this._treeIteration);
        //-------------------Debug Log---------------------
        _rootBranche = rootBranche;
        _branche = RandomCreateTreeBranche(this);
    }

    public void InstanciateMonster(int index)
    {
        this._branche.InstanciateMonster();
        this._rootBranche._instanciatedPart.JointWith(index, _branche._instanciatedPart._joints[0].gameObject);
        this._branche._instanciatedPart.JointWith(0, _rootBranche._instanciatedPart._joints[index].gameObject);
    }

    public void GenerateMonsterWithTree()
    {
        _branche.RandomTreeGeneration();
    }

    public void DestroyMonster()
    {
        this._branche.DestroyMonster();
    }

    public void DeleteTree()
    {
        this._branche.DeleteTree();
        this._branche = null;
    }

    public List<TreeElementJoint> getAllJointInTree(List<TreeElementJoint> joints)
    {
        joints = _branche.getAllJointInTree(joints);
        joints.Add(this);
        return joints;
    }
}

//___________________________ TREE ELEMENT : BRANCHE _______________________________

public class TreeElementBranche : TreeElement
{
    private char _type;
    private int _idpart;
    public  MonsterPartScript _instanciatedPart;
    public List<TreeElementJoint> _joints;
    private int _maxJoints;

    public TreeElementBranche(MonsterCreatorScript monsterCreator)
    {
        //-------------------Debug Log---------------------
        Debug.Log("new Tree root" + this._treeIteration);
        //-------------------Debug Log---------------------
        this._treeIteration = 0;
        this._monsterCreator = monsterCreator;
        this._idpart = monsterCreator.getRandomPartID();
        this._joints = new List<TreeElementJoint>();
        this._maxJoints = monsterCreator.getNumberOfJointWithId(this._idpart);
    }

    public TreeElementBranche(TreeElementJoint rootJoint, int idPart)
    {
        this._treeIteration = rootJoint._treeIteration+1;
        //-------------------Debug Log---------------------
        Debug.Log("new Tree branche" + this._treeIteration);
        //-------------------Debug Log---------------------
        this._idpart = idPart;
        this._monsterCreator = rootJoint._monsterCreator;
        _joints = new List<TreeElementJoint>();
        _joints.Add(rootJoint);
        _maxJoints = this._monsterCreator.getNumberOfJointWithId(this._idpart);
    }

    public void AddJoint()
    {
        if (_joints.Count < _maxJoints)
        {
            _joints.Add(new TreeElementJoint(this));
        }
    }

    //Genere aleatoirement les joint
    public void RandomTreeGeneration()
    { 
        if (_treeIteration < 4)
        {
            int nbJoint = Random.Range(0, _maxJoints);
            //-------------------Debug Log---------------------
            Debug.Log("Tree" + this._treeIteration + " nb joint " + nbJoint);
            //-------------------Debug Log---------------------
            for (int i = 0; i < nbJoint; i++)//-<<<<<-
            {
                AddJoint();
            }
        }     
        this.GenerateMonsterWithTree();
    }

    //Pour chaque joint genere aleatoirement les branches
    public void GenerateMonsterWithTree()
    {
        if (_treeIteration < 4)
        {
            if (_treeIteration == 0 && this._joints.Count > 0)//root case
            {
                this._joints[0].GenerateMonsterWithTree();
            }
            for (int i = 1; i < this._joints.Count; i++ )//le root joint n'est pas pris en compte
            {
                //-------------------Debug Log---------------------
                Debug.Log("Tree" + this._treeIteration + " we need to go deeper , n " + this._joints[i]._treeIteration);
                //-------------------Debug Log---------------------
                
                this._joints[i].GenerateMonsterWithTree();
            }
        }
    }

    public void InstanciateMonster()
    {
        _instanciatedPart = this._monsterCreator.InstanciatePart(this._idpart);
        _instanciatedPart.DesactivateJoints();
        _instanciatedPart.ActivateJoints(this._joints.Count);

        if (_treeIteration == 0 && this._joints.Count > 0)
        {
            this._joints[0].InstanciateMonster(0);
        }
        for (int i = 1; i < this._joints.Count; i++)
        {
            this._joints[i].InstanciateMonster(i);
        }
    }

    public void DestroyMonster()
    {
        this._instanciatedPart.DestroyPart();
        if (_treeIteration == 0 && this._joints.Count > 0)
        {
            this._joints[0].DestroyMonster();
        }
        for (int i = 1; i < this._joints.Count; i++)
        {
            this._joints[i].DestroyMonster();
        }
    }

    public void DeleteTree()
    {

    }

    public List<TreeElementJoint> getAllJointInTree(List<TreeElementJoint> joints)
    {
        if (_treeIteration == 0 && this._joints.Count > 0)
        {
            joints = this._joints[0].getAllJointInTree(joints);
        }
        for (int i = 1; i < this._joints.Count; i++)
        {
            joints = this._joints[i].getAllJointInTree(joints);
        }
        return joints;
    }
}
