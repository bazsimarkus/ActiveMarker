using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrackingLib
{
    //Várakozási sor template osztály
    public class Q<T>
    {
        Queue<T> q = new Queue<T>();
        public readonly AutoResetEvent Event=new AutoResetEvent(false);

        public void Add(T item)
        {
            lock (q)
            {
                q.Enqueue(item);
            }
            Event.Set();
        }

        public T GetMostRecent()
        {
            lock (q)
            {
                T item = default(T);
                while (true)
                {
                    if (q.Count == 0) { break; }
                    item = q.Dequeue();
                }

                return item;
            }
        }

        public bool IsEmpty()
        {
            lock (q)
            {
                return q.Count == 0;
            }
        }

    }
}
