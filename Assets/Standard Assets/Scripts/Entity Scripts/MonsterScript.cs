using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(CharacterController))]

public class MonsterScript : MonoBehaviour {

    public float Speed = 1f;
    public bool activated;

    public static MonsterScript Instance;

    private datastructures.Queue<Vector3> _positionsToGo;
    private Vector3? currentDestination;
    private bool travellingToDestination;
    private CharacterController controller;

    private BlockPosition playerLocation;

    private Animator _animator;
    
	// Use this for initialization
	void Awake() 
    {
        Instance = this;
        gameObject.SetActive(false);
        _positionsToGo = new datastructures.Queue<Vector3>();
        controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        
	}

    public void ActivateMonster()
    {
        gameObject.SetActive(true);
        GetComponent<AudioSource>().Play();
        activated = true;
       
    }

    public void SetStartingPosition(Vector3 starting)
    {
        starting.y = 2.5f;
        transform.position = starting;
    }

    public void PlayerLocationUpdated(BlockPosition newPos)
    {

        Node goalNode = new Node();
        goalNode.pos = newPos;

        if(playerLocation.Equals(newPos))
            return;

        else
            playerLocation = newPos;

        _positionsToGo.Clear();

        BlockPosition currPos = getCoordinates();
        Node startNode = new Node();
        startNode.pos = currPos;

        datastructures.PriortyQueue<Node> Pqueue = new datastructures.PriortyQueue<Node>(Search.ManhattanDistance, goalNode);

        List<BlockPosition> path = Search.getInstance().GraphSearch(startNode, goalNode, GameManager.Instance.MazeInfo, Pqueue);

        print("Monster location =" + currPos.row + " , " + currPos.column);

        currentDestination = null;
        foreach (BlockPosition n in path) //put all waypoints into array
        {
            _positionsToGo.Push(getBlockCoordinates(n));
        }

      
      

    }

    Vector3 getBlockCoordinates(BlockPosition pos)
    {
       return GameManager.Instance.getBlockCoordinates(pos);
    }

    BlockPosition getCoordinates()
    {
        Ray DownRay = new Ray(transform.position + (Vector3.up), Vector3.down);
        BlockPosition CellPosition;
        CellPosition.row = 0; CellPosition.column = 0;
        RaycastHit hitinfo;
        if (Physics.Raycast(DownRay, out hitinfo))
        {
            string CellName = hitinfo.collider.gameObject.name;
            string[] cellCoordinates = CellName.Split('x');


            CellPosition.row = int.Parse(cellCoordinates[0]); CellPosition.column = int.Parse(cellCoordinates[1]);




        }
        return CellPosition;
    }


    public void DeactivateMonster()
    {
        activated = false;

        _positionsToGo.Clear();
        currentDestination = null;
        gameObject.SetActive(false);
    }

	void Update () 
    {
        if (activated) //if the monster is active
        {
            if (!_positionsToGo.isEmpty())
            {
                
                if (currentDestination == null)
                {
                    Vector3 tmpDest = _positionsToGo.Pop();
                    tmpDest.y = transform.position.y; // make the way point as high as the transform so I can use lookat()
                    currentDestination = tmpDest;
                }

                Vector3 moveDirection = currentDestination.Value - transform.position;
              

                if (moveDirection.magnitude < 0.25f) //almost at the location
                {
                    transform.position = currentDestination.Value;
                    currentDestination = null;
                    

                }
                else
                {
                    transform.LookAt(currentDestination.Value);
                    controller.Move(moveDirection.normalized * Speed * Time.deltaTime);
                    
                    _animator.SetFloat("Speed", moveDirection.magnitude);
                }


            }

            
        }

        if (playerLocation.Equals(getCoordinates()))
            PlayerScript.Instance.PlayerKill();
	
	}

  
}


