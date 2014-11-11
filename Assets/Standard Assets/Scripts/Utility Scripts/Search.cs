using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public delegate bool successorCheck(Node pos, BlockType[,] Grid);


public enum Action
{
    Up,
    Down,
    Left,
    Right
}

public class Search
{

    static Search instance = null;

    public Search()
    {


    }


    public static Search getInstance()
    {
        if (instance != null)
            return instance;

        instance = new Search();
        return instance;
        
    }

    public static Search resetInstance()
    {
        if (instance != null)
            instance = null; //Garbage collection

        return getInstance();
    }
    

    bool PathFindSuccessor(Node currentNode, BlockType[,] Grid)
    {
        //if it's a road return true.
        if (Grid[currentNode.pos.row, currentNode.pos.column] == BlockType.Path)
            return true;

        return false; //if it's a wall can't walk into walls, return false

    }

    /// <summary>
    /// returns a list of blocks that are accesable from the current block
    /// </summary>
    /// <param name="currentNode"> current position </param>
    /// <param name="Grid"> The grid information </param>
    /// <returns></returns>
    /// 
    List<Node> getSuccesors(Node currentNode, BlockType[,] Grid, successorCheck isSuccessor)
    {
        List<Node> successors = new List<Node>();

        int GridHeight = Grid.GetLength(0); //get the dimensions of the grid
        int GridWidth = Grid.GetLength(1);

        //for every possible add the possible successors to the list
        for (int i = 0; i < Enum.GetNames(typeof(Action)).Length; i++)
        {
            Node successor = new Node(currentNode);
            successor.pos = currentNode.pos;


            switch ((Action)i)
            {
                case Action.Left:
                    {
                        successor.pos.column--;

                        if (successor.pos.column >= 0 && isSuccessor(successor, Grid)) //if the block is not out of bounds and satisfies the conditions of isSuccessor, add it
                            successors.Add(successor);
                        break;
                    }
                case Action.Right:
                    {
                        successor.pos.column++;

                        if (successor.pos.column < GridWidth && isSuccessor(successor, Grid))
                            successors.Add(successor);
                        break;
                    }
                case Action.Up:
                    {
                        successor.pos.row--;

                        if (successor.pos.row >= 0 && isSuccessor(successor, Grid))
                            successors.Add(successor);
                        break;
                    }
                case Action.Down:
                    {
                        successor.pos.row++;

                        if (successor.pos.row < GridHeight && isSuccessor(successor, Grid))
                            successors.Add(successor);
                        break;
                    }
            }
        }

        return successors;
    }

    bool isGoalState(Node currentNode, Node GoalNode)
    {
        if (currentNode.Equals(GoalNode))
            return true;

        return false;
    }

    /// <summary>
    /// returns the manhattan distance between two nodes
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static int ManhattanDistance(BlockPosition start, BlockPosition end)
    {
        return Mathf.Abs(start.row - end.row) + Mathf.Abs(start.column - end.column);
    }

    /// <summary>
    /// Give the start position of the agent and a goal, provide the Maze and a container to handle the pathfinding. can give different containers implementing IContainer interface
    /// to have different search results
    /// </summary>
    /// <param name="pos">Start position</param>
    /// <param name="GoalNode">Goal position</param>
    /// <param name="Grid">The Maze Grid</param>
    /// <param name="frontier">Container to use (i.e. Stack, Queue, PriorityQueue)</param>
    /// <returns></returns>
    public List<BlockPosition> GraphSearch(Node pos, Node GoalNode, BlockType[,] Grid, datastructures.IContainer<Node> frontier)
    {     
        frontier.Push(pos);
        List<Node> explored = new List<Node>();

        while (!frontier.isEmpty())
        {
            Node currentNode = frontier.Pop();

            if (isGoalState(currentNode, GoalNode)) //path found, return
                return currentNode.Path();

            else //still searching..
            {
                explored.Add(currentNode);

                foreach (Node n in getSuccesors(currentNode, Grid, PathFindSuccessor))
                {
                    if (!explored.Contains(n))
                        frontier.Push(n);
                }

            }

        }


        return null; //there are no more nodes, and the goal hasn't been reached. return null
    }

 

}
