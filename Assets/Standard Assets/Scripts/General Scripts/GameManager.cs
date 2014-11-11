using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum BlockType
{
    Wall,
    Path,
    Side,
    Exit
}

public enum GridSide
{
    Top,
    Left,
    Right,
    Bottom
}

public enum GameMode
{
    Initial,
    Game,
    EndFail,
    EndSuccess
}

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    private GameMode mode = GameMode.Initial;

    public GameObject normalCube; //prefab of a normal cube.

	//spacing between each cube placed
	private static float CELL_SEPERATION = 5f;
	//How big the cubes in the world are (currently equal to the spacing)
	private static float CELL_SCALE = 5f;
    private static int MAZE_WIDTH = 20;
    private static int MAZE_HEIGHT = 20;
	
	//The 2D array to hole our grid of cubes
	private GameObject[,] GridCells;
	private Material[] materials = new Material[10];
    private Material visitedCellMaterial;
    private Material[] wallMaterial;

    private BlockType[,] Grid; //the map of the grid.
    private bool[,] VisitedCell; //this is used to mark the player's visited cells to paint them white. Re-used in maze shuffle

    bool MonsterMode = false;

    public GameMode CurrentMode { get { return mode; } 
        
        set { 
            mode = value;
                if (value == GameMode.EndSuccess || value == GameMode.EndFail) //destroy monster when game finished
                {
                    MonsterScript.Instance.gameObject.SetActive(false);
               
                }
                if (value == GameMode.Game)
                    Screen.showCursor = false; //don't want mouse to show up
                else
                    Screen.showCursor = true;
        
            } 
    
    }
    
	// Using awake because I want to initialize the player on it's start function after this one.
	void Awake () {
       
        Instance = this;

        CreateMaterials();

        if (MAZE_HEIGHT % 2 != 0)
            MAZE_HEIGHT++;
        if (MAZE_WIDTH % 2 != 0)
            MAZE_WIDTH++;
		
	}

    

    public void UpdatePlayerPosition(BlockPosition player) //called only when the player's position changed in block coordinates
    {
        
        if (VisitedCell[player.row, player.column] == false) //if player hasn't visited this cell yet
        {
            GridCells[player.row, player.column].GetComponent<MeshRenderer>().sharedMaterial = visitedCellMaterial; // change the color of the visited cell
            VisitedCell[player.row, player.column] = true; //mark it as visited
        }

        if (Grid[player.row, player.column] == BlockType.Exit) //player wins!
        {
            PlayerScript.Instance.PlayerWin();
            return;
        }

        if (MonsterMode)   //if there is a monster, feed the location of the player to the monster
            MonsterScript.Instance.PlayerLocationUpdated(player);


        

    }
    

    void InitializeMaze(bool Monster)
    {
        MonsterMode = Monster; //required for player position updates
        //Initialze the array of GameObjects
        GridCells = new GameObject[MAZE_HEIGHT, MAZE_WIDTH];
        Grid = GenerateMaze(MAZE_HEIGHT, MAZE_WIDTH);
        VisitedCell = new bool[MAZE_HEIGHT, MAZE_WIDTH];

        //Loop through the array
        for (int i = 0; i < MAZE_HEIGHT; i++)
        {
            for (int j = 0; j < MAZE_WIDTH; j++)
            {
                //Create a new cube for every position in the array
                GameObject go = (GameObject)Instantiate(normalCube);

                //Set up its rendering.
                MeshRenderer mr = go.GetComponent<MeshRenderer>();


                //set a random color
                Material mat = getRandomMaterial();

                //using shared material instead of material for dynammic batching
                mr.sharedMaterial = mat;

                //Set the cubes position and scale
                float BlockHeight = 0f;
                if (Grid[i, j] == BlockType.Wall || Grid[i, j] == BlockType.Side)
                {
                    BlockHeight = CELL_SCALE;
                    mr.sharedMaterial = getRandomWallMaterial();
                }



                go.transform.position = new Vector3(i * CELL_SEPERATION, BlockHeight, j * CELL_SEPERATION);
                


                go.name = i + "x" + j; //these names later will be used to get the gameobject by name
                GridCells[i, j] = go;
                go.transform.parent = transform;
            }
        }

        PlayerScript.Instance.gameObject.SetActive(true); //enable the player


        if (Monster)
        {
            MonsterModeInit();
            BlockPosition monsterStartCoord;
            monsterStartCoord.row = UnityEngine.Random.Range(2, MAZE_HEIGHT - 2);
            monsterStartCoord.column = UnityEngine.Random.Range(2, MAZE_WIDTH - 2);
            monsterStartCoord = getClosestDistressedPosition(monsterStartCoord.row, monsterStartCoord.column);

            Vector3 MonsterPosition = GridCells[monsterStartCoord.row, monsterStartCoord.column].transform.position;
            MonsterScript.Instance.SetStartingPosition(MonsterPosition);

        }
        else
        {
            RoamamazeModeInit();
           
        }

        
        BlockPosition startPos = getClosestDistressedPosition((int)(MAZE_HEIGHT / 2), (int)(MAZE_WIDTH / 2));
        

        Vector3 PlayerNewPosition = GridCells[startPos.row, startPos.column].transform.position;
        PlayerNewPosition.y = 10;
        PlayerScript.Instance.SetPlayerStartingPosition(PlayerNewPosition);

        CurrentMode = GameMode.Game;
    }

    /// <summary>
    /// gets the closest avialable cell's coordinates given a desired location. 
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    BlockPosition getClosestDistressedPosition(int height, int width)
    {
        BlockPosition startPosition;
        startPosition.row = height;
        startPosition.column = width;

        int checkNext = (int)Action.Down;

        BlockPosition possibleStartPoint = startPosition;
        int checkrange = 1;
        while (Grid[possibleStartPoint.row, possibleStartPoint.column] != BlockType.Path)
        {
            switch (checkNext)
            {
                case (int)Action.Down: //already have these as enum, I'll add the corners by number
                    {
                        possibleStartPoint = startPosition;
                        possibleStartPoint.row += checkrange;
                        if (possibleStartPoint.row > height) //if the possible position is out of bounds, ignore this direction.
                        {
                            possibleStartPoint = startPosition;                         
                        }
                        break;
                    }

                case (int)Action.Up:
                    {
                        possibleStartPoint = startPosition;
                        possibleStartPoint.row -= checkrange;

                        if (possibleStartPoint.row < 0) //if the possible position is out of bounds, ignore this direction.
                        {
                            possibleStartPoint = startPosition;
                            
                        }
                        break;
                    }

                case (int)Action.Left:
                    {
                        possibleStartPoint = startPosition;
                        possibleStartPoint.column -= checkrange;
                        if (possibleStartPoint.column < 0) //if the possible position is out of bounds, ignore this direction.
                        {
                            possibleStartPoint = startPosition;
                            
                        }

                        break;
                    }

                case (int)Action.Right:
                    {
                        possibleStartPoint = startPosition;
                        possibleStartPoint.column += checkrange;
                        if (possibleStartPoint.column > width) //if the possible position is out of bounds, ignore this direction.
                        {
                            possibleStartPoint = startPosition;
                            
                        }


                        break;
                    }
                case 4: //top right
                    {
                        possibleStartPoint = startPosition;
                        possibleStartPoint.row -= checkrange;
                        possibleStartPoint.column += checkrange;

                        if (possibleStartPoint.column > width || possibleStartPoint.row < 0) //if the possible position is out of bounds, ignore this direction.
                        {
                            possibleStartPoint = startPosition;
                            
                        }
                        break;
                    }
                case 5: //top left
                    {
                        possibleStartPoint = startPosition;
                        possibleStartPoint.row -= checkrange;
                        possibleStartPoint.column -= checkrange;
                        if (possibleStartPoint.column < 0 || possibleStartPoint.row < 0) //if the possible position is out of bounds, ignore this direction.
                        {
                            possibleStartPoint = startPosition;
                            
                        }

                        break;
                    }

                case 6: //down left
                    {
                        possibleStartPoint = startPosition;
                        possibleStartPoint.row += checkrange;
                        possibleStartPoint.column -= checkrange;
                        if (possibleStartPoint.column < 0 || possibleStartPoint.row > height) //if the possible position is out of bounds, ignore this direction.
                        {
                            possibleStartPoint = startPosition;
                           
                        }

                        break;
                    }

                case 7: //down right
                    {
                        possibleStartPoint = startPosition;
                        possibleStartPoint.row += checkrange;
                        possibleStartPoint.column += checkrange;

                        if (possibleStartPoint.column > width || possibleStartPoint.row > height) //if the possible position is out of bounds, ignore this direction.
                        {
                            possibleStartPoint = startPosition;
                           
                        }
                        break;
                    }

            }
           

            checkNext++;
   
            if ((int)checkNext > 7)
            {
                checkrange++; //reaching here's odds are minuscule but, increase the range of checks each time all 8 neighboors have been tested and a road couldn't be found
                checkNext = (int)Action.Down;
            }
           
        }

        return possibleStartPoint;

    }

    void MonsterModeInit()
    {
        GameObject.FindGameObjectWithTag(Tags.MazeLight).GetComponent<Light>().intensity = 0.25f;
        RenderSettings.ambientLight = new Color(0.09f, 0.09f, 0.09f);
        Camera.main.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        MonsterScript.Instance.ActivateMonster();
        PlayerScript.Instance.Activate(MonsterMode);
        
    }

    void RoamamazeModeInit()
    {
        PlayerScript.Instance.Activate(MonsterMode);
        RenderSettings.ambientLight = new Color(0.67f, 0.67f, 0.67f);
        Camera.main.backgroundColor = new Color(0.49f, 0.83f, 0.8f);
    }

    /// <summary>
    /// Generates a maze given the dimensions
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    BlockType[,] GenerateMaze(int height, int width)
    {
        BlockType[,] Grid = new BlockType[height, width]; //initializes the maze to all walls by default (enum Wall is = 0)
       
        //int r = random.Next(height);
        BlockPosition Exitpos; 
        Exitpos.row = 0; // defaultt values
        Exitpos.column = 0;

        BlockPosition startPos;
        startPos.row = 0;
        startPos.column = 0;


        int Side = UnityEngine.Random.Range(0, Enum.GetNames(typeof(GridSide)).Length); // Determine which side the exit will be

        switch ((GridSide)Side)
        {
            case GridSide.Top:
                {
                    print("top");
                    Exitpos.row = 0;
                    Exitpos.column = UnityEngine.Random.Range(1, width - 1); // I don't want the corners to be the exit. 

                    startPos.row = 1;
                    startPos.column = Exitpos.column;
                    break;
                }

            case GridSide.Bottom:
                {
                    print("bottom");
                    Exitpos.row = height - 1;
                    Exitpos.column = UnityEngine.Random.Range(1, width - 1);

                    startPos.row = Exitpos.row - 1;
                    startPos.column = Exitpos.column;
                    break;
                }
            
            case GridSide.Left:
                {
                    print("left");
                    Exitpos.row = UnityEngine.Random.Range(1, height -1);
                    Exitpos.column = 0;

                    startPos.row = Exitpos.row;
                    startPos.column = 1;
                    break;
                }

            case GridSide.Right:
                {
                    print("right");
                    Exitpos.row = UnityEngine.Random.Range(1, height - 1);
                    Exitpos.column = width - 1;

                    startPos.row = Exitpos.row ;
                    startPos.column = Exitpos.column - 1;
                    break;
                }
        }

        MarkSide(GridSide.Top, 0, width, ref Grid);
        MarkSide(GridSide.Bottom, height - 1, width,ref Grid);
        MarkSide(GridSide.Left, 0, height, ref Grid);
        MarkSide(GridSide.Right, width - 1, height, ref Grid);


        
        Grid[Exitpos.row, Exitpos.column] = BlockType.Exit;



        DFSMazeGenerator(startPos, ref Grid);


        return Grid;
    }

    void MarkSide(GridSide side, int sideIndex, int size, ref BlockType[,] Grid)
    {
        if (side == GridSide.Bottom || side == GridSide.Top)
        {
            for (int i = 0; i < size; i++)
                Grid[sideIndex, i] = BlockType.Side;

        }

        else
        {
            for (int i = 0; i < size; i++)
                Grid[i, sideIndex] = BlockType.Side;

        }

    }



    void DFSMazeGenerator(BlockPosition start, ref BlockType[,] Grid)
    {
        

        BlockPosition currentCell = start;
        datastructures.Stack<BlockPosition> backtrack = new datastructures.Stack<BlockPosition>();

        backtrack.Push(currentCell);
        Grid[currentCell.row, currentCell.column] = BlockType.Path;
        

        while (!backtrack.isEmpty())
        {
            List<Action> actions = Grid.getPossibleActions(currentCell);
            if (actions.Count == 0)
            {
                currentCell = backtrack.Pop();
                continue;
            }

            Action chosenAction = actions.getRandomElement<Action>();
            currentCell= Grid.markPath(currentCell, chosenAction);
            backtrack.Push(currentCell);
        }

    }


    public Vector3 getBlockCoordinates(BlockPosition position)
    {
        return GridCells[position.row, position.column].transform.position;

    }
    void DestroyGrid()
    {
        foreach(GameObject go in GridCells)
            Destroy(go);

    }
    #region GUI functions
    void OnGUI()
    {
        switch (mode)
        {
            case GameMode.Initial:
                {
                    InitialScreenGUI();
                    break;
                }

            case GameMode.Game:
                {
                    GameModeGUI();
                    break;
                }

            case GameMode.EndFail:
                {
                    EndGameGUIFail();
                    break;
                }

            case GameMode.EndSuccess:
                {
                    EndGameGUISuccess();
                    break;
                }

        }
    }

    void InitialScreenGUI()
    {
        GUI.color = Color.red;
        if (GUI.Button(new Rect(Screen.width - 300, 0, 300, 400), "Monster Maze \n(hint: run)"))
            InitializeMaze(true);

        GUI.color = Color.green;
        if (GUI.Button(new Rect(0, 0, 300, 400), "Roam-amaze \n(a regular old maze, nothing special)"))
            InitializeMaze(false);


    }

    void GameModeGUI()
    {
        //nothing here
    }

    void EndGameGUIFail()
    {
        GUI.color = Color.red;
        GUI.Box(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 200, 400, 400), "You are dead.");

        GUI.color = Color.green;
        if (GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) - 50, 100, 50), "Retry"))
        {
            CurrentMode = GameMode.Initial;
            DestroyGrid();
            PlayerScript.Instance.gameObject.SetActive(false);
            


        }
        GUI.color = Color.green;
        if (GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) + 25, 100, 50), "Exit"))
        {
            Application.Quit();
        }

    }

    void EndGameGUISuccess()
    {
        GUI.color = Color.green;

        GUI.Box(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 200, 400, 400), "You have successfully found the exit!");

        GUI.color = Color.green;
        if (GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) - 50, 100, 50), "Go again"))
        {
            CurrentMode = GameMode.Initial;
            DestroyGrid();
            PlayerScript.Instance.gameObject.SetActive(false);
        }
        GUI.color = Color.green;
        if (GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) + 25, 100, 50), "Exit"))
        {
            Application.Quit();
        }
    }
    #endregion

    #region Material Creation
    void CreateMaterials()
    {
        
        visitedCellMaterial = new Material(Shader.Find("Diffuse"));
        visitedCellMaterial.color = Color.white;

        wallMaterial = new Material[4];

        for (int i = 0; i < 4; i++)
        {
            wallMaterial[i] = new Material(Shader.Find("Diffuse"));
            wallMaterial[i].color = new Color(0.96f, 0.71f - (i * 0.1f), 0.56f - (i * 0.1f));
        }
        

        for (int i = 0; i < materials.Length; i++)
        {
            Material mat = new Material(Shader.Find("Diffuse"));

            mat.color = getColor(i);
            materials[i] = mat;
        }

    }

    Material getRandomWallMaterial()
    {
        return wallMaterial[UnityEngine.Random.Range(0, wallMaterial.Length)];
    }

    Color getColor(int clr)
    {
        //default color
        Color color = Color.red;

        switch (clr)
        {
            case 0:
                {
                    color = Color.blue;
                    break;
                }
            case 1:
                {
                    color = Color.cyan;
                    break;
                }
            case 2:
                {
                    color = Color.green;
                    break;
                }
            case 3:
                {
                    color = Color.magenta;
                    break;
                }
            case 4:
                {
                    color = Color.red;
                    break;
                }
            case 5:
                {
                    color = Color.yellow;
                    break;
                }
            case 6:
                {
                    color = Color.gray;
                    break;
                }
            case 7:
                {
                    //orange
                    color = new Color(1f, .73f, .12f);
                    break;
                }
            case 8:
                {
                    //purple
                    color = new Color(.45f, .27f, 1f);
                    break;
                }
            case 9:
                {
                    color = new Color(.2f, .63f, .9f);
                    break;
                }
            default:
                {
                    Debug.LogWarning("More materials than colors in getColor()");
                    break;
                }
        }


        return color;
    }

    Material getRandomMaterial()
    {

        //I want to do dynamic batching, so I'm limiting the variety of colors.
        int randIndex = UnityEngine.Random.Range(0, materials.Length);

      
        return materials[randIndex];
    }

    Material getGreyMaterial()
    {

        return materials[6];
    }
    #endregion

    public BlockType[,] MazeInfo
    {
        get { return Grid; }
    }

}

public static class Extensions
{
    public static T getRandomElement<T>(this List<T> currentList)
    {
        return currentList[UnityEngine.Random.Range(0, currentList.Count)];
    }

   
    /// <summary>
    /// Marks the path on the grid given a direction, returns the end point.
    /// </summary>
    /// <param name="Grid"></param>
    /// <param name="currentPosition"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static BlockPosition markPath(this BlockType[,] Grid, BlockPosition currentPosition, Action direction)
    {
        int TrappedChance = UnityEngine.Random.Range(0, 101);
        BlockPosition newPosition = currentPosition;

        switch (direction)
        {
            case Action.Left:
                {
                    Grid[currentPosition.row, currentPosition.column - 1] = BlockType.Path;
                    Grid[currentPosition.row, currentPosition.column - 2] = BlockType.Path;


                    newPosition.column -= 2;
                    break;
                }

            case Action.Right:
                {

                     Grid[currentPosition.row, currentPosition.column + 1] = BlockType.Path;
                     Grid[currentPosition.row, currentPosition.column + 2] = BlockType.Path;


                    newPosition.column += 2;
                    break;
                }

            case Action.Up:
                {

                    Grid[currentPosition.row - 1, currentPosition.column] = BlockType.Path;
                    Grid[currentPosition.row - 2, currentPosition.column] = BlockType.Path;


                    newPosition.row -= 2;
                    break;
                }

            case Action.Down:
                {
                    Grid[currentPosition.row + 1, currentPosition.column] = BlockType.Path;
                    Grid[currentPosition.row + 2, currentPosition.column] = BlockType.Path;

                    newPosition.row += 2;
                    break;
                }
        }

        return newPosition;
    }

    public static List<Action> getPossibleActions(this BlockType[,] Grid, BlockPosition currentPosition)
    {
        List<Action> actions = new List<Action>();

        BlockPosition desiredPosition = currentPosition;
        
        //check left
        desiredPosition.column -= 2;
        if (desiredPosition.column > 0 && !Grid.isRoad(desiredPosition))
        {
            actions.Add(Action.Left);
        }

        //right
        desiredPosition = currentPosition;
        desiredPosition.column += 2;
        if (desiredPosition.column < Grid.GetLength(1) && !Grid.isRoad(desiredPosition))
        {
            actions.Add(Action.Right);
        }


        //Up
        desiredPosition = currentPosition;
        desiredPosition.row -= 2;
        if (desiredPosition.row > 0 && !Grid.isRoad(desiredPosition))
        {
            actions.Add(Action.Up);
        }

        //Down
        desiredPosition = currentPosition;
        desiredPosition.row += 2;
        if (desiredPosition.row < Grid.GetLength(0) && !Grid.isRoad(desiredPosition))
        {
            actions.Add(Action.Down);
        }

        return actions;
    }

    public static bool isRoad(this BlockType[,] Grid, BlockPosition pos)
    {
        if (Grid[pos.row, pos.column] == BlockType.Side || Grid[pos.row, pos.column] == BlockType.Exit)
            return true;

        if (Grid[pos.row, pos.column] == BlockType.Path)
            return true;

        return false;

    }

}


//Each block on the Grid is represented as a Node object
public class Node : IEquatable<Node>
{
    public BlockPosition pos;

    private Node parent;
    private int pathCost = 0;

    public Node(Node parent = null)
    {
        this.parent = parent;

        if (parent != null)
            pathCost = parent.pathCost + 1; //used in heuristic function for A*
    }

    /// <summary>
    /// returns the path to follow to get to this node
    /// </summary>
    /// <returns></returns>
    public List<BlockPosition> Path() //used for A* pathfinding
    {

        List<BlockPosition> path = new List<BlockPosition>();

        Node currentNode = this;
        path.Add(currentNode.pos);
        while (currentNode.parent != null)
        {
            path.Add(currentNode.pos);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();

        return path;
    }

    public int PathCost { get { return pathCost; } }


    public bool Equals(Node other) // used if a node is already in a list. (see Search.cs, GraphSearch())
    {
        return this.pos.Equals(other.pos);
    }
}

public struct BlockPosition : IEquatable<BlockPosition>
{
    public int row;
    public int column;

    public bool Equals(BlockPosition other)
    {
        if ((this.row == other.row) && (this.column == other.column))
            return true;

        return false;
    } 

}


