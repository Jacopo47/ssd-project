using System.Collections.Generic;
using System.Linq;

namespace ssdProject.Model.Heuristics
{
    public class Queue<T>
    {
        public List<T> list = new List<T>();
        private int size;
        

        public Queue (int size)
        {
            this.size = size;
        } 

        public void add(T elem)
        {
            if(this.list.Count == size)
            {
                this.list.Remove(this.list.ElementAt(size - 1));
            }
            list.Insert(0,elem);
        }

        public List<T> getValues()
        {
            return this.list;
        }

        public bool contains(T elem)
        {
            return this.list.Contains(elem);
        }
            
    }
}