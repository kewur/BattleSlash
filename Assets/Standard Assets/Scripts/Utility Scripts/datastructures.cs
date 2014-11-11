using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public delegate int heuristicFunction(BlockPosition Start, BlockPosition Goal); // implementing this as a delegate to change heuristic functions seamlessly

namespace datastructures
{



    public interface IContainer<T>
    {
        

        T Pop();
        void Push(T item);
        bool isEmpty();
        void Clear();

      
    }

    public class Stack<T> : IContainer<T>
    {      
        List<T> container;
        public Stack()
        {
            container = new List<T>();
        }

        public T Pop()
        {

            if (container.Count == 0) //if container is empty return default
                return default(T);

            T value = container[container.Count - 1];
            container.RemoveAt(container.Count - 1);

            return value;
        }


        public void Push(T item)
        {
            container.Add(item);
        }


        public void Clear()
        {
            container.Clear();
        }


        public bool isEmpty()
        {
            if (container.Count > 0)
                return false;

            return true;
        }
    }

    public class Queue<T> : IContainer<T>
    {
        List<T> container;

        public Queue()
        {
            container = new List<T>();
        }

        public T Pop()
        {
            if (container.Count == 0) //if container is empty return default
                return default(T);

            T value = container[container.Count - 1];

            container.RemoveAt(container.Count - 1);

            return value;
        }

        public void Push(T item)
        {
            container.Insert(0, item);
        }

        public bool isEmpty()
        {
            if (container.Count > 0)
                return false;

            return true;
        }

        public void Clear()
        {
            container.Clear();
        }
    }


    public class PriortyQueue<T> : IContainer<T> where T:Node
    {
        heuristicFunction heuristic; //heuristic to use
        List<PQueueElement> container;
        T goalNode; //used to calculate the priority

        public PriortyQueue()
        {
            container = new List<PQueueElement>();
        }

        public PriortyQueue(heuristicFunction hFunc, T goal)
        {
            heuristic = hFunc;
            goalNode = goal;
            container = new List<PQueueElement>();
        }

        public T Pop()
        {

            if (container.Count == 0) //if container is empty return default
                return default(T);

            T value = (T)container[0].element;

            container.RemoveAt(0);

            return value;
        }


        public void Push(T item)
        {
            PQueueElement el = new PQueueElement(item, item.PathCost + heuristic(item.pos, goalNode.pos));
            container.Add(el);
            container.Sort();
        }
     

        public bool isEmpty()
        {
            if (container.Count > 0)
                return false;

            return true;
        }

        public void Clear()
        {
            container.Clear();
        }
    }

    public class PQueueElement : IComparable<PQueueElement>
    {
       public Node element;
       int priority;

       public PQueueElement(Node item, int pri)
       {
           element = item;
           priority = pri;

       }

        public int CompareTo(PQueueElement other)
        {
            if (other.priority < this.priority)
                return -1;

            else if (other.priority == this.priority)
                return 0;

            else
                return 1;
        }

        public Node Element
        {
            get { return element; }
        }

    }
}
