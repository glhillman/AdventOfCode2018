using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    internal class Worker
    {
        public Worker(int workerID, int offset)
        {
            WorkerID = workerID;
            Offset = offset;
            _stepID = null;
        }

        public int WorkerID {  get; private set; }
        public int Offset { get; private set; }
        private char? _stepID;
        public char? StepID
        {
            get {  return _stepID; }

            set
            {
                _stepID = value;
                if (char.IsAsciiLetterUpper(_stepID.Value))
                {
                    Seconds = (int)_stepID.Value - (int)'A' + 1 + Offset;
                }
            }
        }

        public int Seconds { get; private set; }

        public char? Tick()
        {
            char? rtrn = null;

            if (Seconds > 0)
            {
                Seconds--;
                if (Seconds == 0)
                {
                    rtrn = _stepID;
                    _stepID = null;
                }
            }
            return rtrn;
        }

        public override string ToString()
        {
            return string.Format("ID: {0}, StepID: {1}, Seconds: {2}", WorkerID, StepID, Seconds);
        }
    }


}
