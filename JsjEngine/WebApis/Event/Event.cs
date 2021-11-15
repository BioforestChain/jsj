using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine.WebApis.Event
{
    internal class Event
    {
        private static int _idCounter = 0;
        private EventType _type;
        private int _id;
        internal ScriptObject? ScriptObject;
        internal string? Function;
        internal string? Data;

        public Event(EventType type)
        {
            _id = Interlocked.Increment(ref _idCounter);
            _type = type; 
        }

        #region properties
        internal int Id => _id;
        internal EventType Type => _type;
        #endregion

        
    }
}
