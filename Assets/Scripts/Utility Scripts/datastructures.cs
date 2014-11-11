using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


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

}
