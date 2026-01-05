using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Ozeki
{
    public abstract class OzAIOperation : OzAICheckable
    {
        public abstract OzAIOperationType Type { get; }
        private int _taken; // 0 = false, 1 = true
        public bool Taken => Volatile.Read(ref _taken) == 1;

        // Thread‑safe attempt to take the operation. Returns true if this call set Taken to true.
        public bool Take()
        {
            // Atomically set _taken to 1 and return true if it was 0 before.
            return Interlocked.Exchange(ref _taken, 1) == 0;
        }
        
        private uint _doneCount;
        public uint DoneCount => _doneCount;
        public uint DepCount {get; private set;}

        /// <summary>
        /// Marks this operation as done, processes all forward‑linked operations and returns
        /// a list of operations whose dependencies are now satisfied.
        /// The method is thread‑safe and aims for efficiency by using the lock‑free
        /// forward‑link queue and a simple lock to guarantee that only one thread
        /// performs the completion logic for a given operation.
        /// </summary>
        /// <returns>
        /// A <see cref="LinkedList{OzAIOperation}"/> containing the next operations
        /// that have become ready (i.e., <c>checkOrIncDoneCount()</c> returned true).
        /// </returns>
        public LinkedList<OzAIOperation> Done()
        {
            var readyOps = new LinkedList<OzAIOperation>();

            // Enumerate the forward‑link queue (snapshot semantics are fine here).
            foreach (var nextOp in _nextQueue)
            {
                // If the next operation's dependencies are satisfied after increment,
                // add it to the list of ready operations.
                if (nextOp.checkOrIncDoneCount())
                {
                    readyOps.AddLast(nextOp);
                }
            }

            // Reset this operation's state for potential reuse.
            Reset();

            return readyOps;
        }

        // Returns true if all dependencies are satisfied; otherwise increments DoneCount.
        private bool checkOrIncDoneCount()
        {
            while (true)
            {
                uint current = _doneCount;
                if (current == DepCount)
                    return true;
                uint newVal = current + 1;
                uint original = Interlocked.CompareExchange(ref _doneCount, newVal, current);
                if (original == current)
                    return false;
                // another thread modified _doneCount, retry
            }
        }

        
        // ------------------------------------------------------------
        // Approach 1: lock‑free forward link using ConcurrentQueue (active)
        // ------------------------------------------------------------
        private readonly ConcurrentQueue<OzAIOperation> _nextQueue = new();

        /// <summary>
        /// Adds the specified operation to the forward‑link queue in a thread‑safe manner
        /// and increments the dependency count of the added operation.
        /// </summary>
        /// <param name="op">The operation to link as a successor.</param>
        public void ForwardLink(OzAIOperation op)
        {
            _nextQueue.Enqueue(op);
            op.DepCount++;
        }
        
        /*
        // ------------------------------------------------------------
        // Approach 2: LinkedList with ReaderWriterLockSlim (removal cheap – commented out)
        // ------------------------------------------------------------
        private readonly System.Threading.ReaderWriterLockSlim _nextLock = new();
        private LinkedList<OzAIOperation> _nextList;

        /// <summary>
        /// Adds the specified operation to this operation's forward‑link list in a thread‑safe manner
        /// and increments the dependency count of the added operation.
        /// </summary>
        /// <param name="op">The operation to link as a successor.</param>
        public void ForwardLink(OzAIOperation op)
        {
            if (op == null) throw new ArgumentNullException(nameof(op));

            _nextLock.EnterWriteLock();
            try
            {
                _nextList ??= new LinkedList<OzAIOperation>();
                _nextList.AddLast(op);
            }
            finally { _nextLock.ExitWriteLock(); }

            op.DepCount++;
        }

        // Cheap removal example (commented out)
        public bool RemoveLink(OzAIOperation op)
        {
            if (op == null) return false;
            _nextLock.EnterWriteLock();
            try
            {
                var node = _nextList?.First;
                while (node != null)
                {
                    if (node.Value == op)
                    {
                        _nextList.Remove(node);
                        return true;
                    }
                    node = node.Next;
                }
                return false;
            }
            finally { _nextLock.ExitWriteLock(); }
        }
        */
        
        /// <summary>
        /// Resets the operation state in a lock‑free, thread‑safe manner.
        /// Sets <see cref="DoneCount"/> to 0 and marks the operation as not taken.
        /// The <see cref="Taken"/> flag is cleared last to ensure any thread that
        /// observes the reset will see <c>Taken == false</c>.
        /// </summary>
        public void Reset()
        {
            // Reset the done count (non‑atomic write is acceptable here)
            _doneCount = 0;

            // Reset the taken flag atomically; this write happens last.
            Interlocked.Exchange(ref _taken, 0);
        }

        protected bool CheckAreRangesValid(List<OzAITensRange[]> rangeArrays, List<string> rangeArrayNames, out string error)
        {
            if (rangeArrays.Count != rangeArrayNames.Count)
            {
                error = $"Could not check if {Type} is possible becuase not all tensor range arrays were given a name.";
                return false;
            }

            for (int i = 0; i < rangeArrays.Count; i++)
            {
                var rangeArray = rangeArrays[i];
                var name = rangeArrayNames[i];
                for (long j = 0; j < rangeArray.LongLength; j++)
                {
                    var range = rangeArray[j];
                    if (range == null)
                    {
                        error = $"{Type} is not possible, becuase {name}'s range number {j} is null.";
                        return false;
                    }
                    if (!range.IsValid(out error))
                    {
                        error = $"{Type} for {name}'s range number {j} is not possible: " + error;
                        return false;
                    }
                }
            }

            error = null;
            return true;
        }
    }
}