using System;
using System.Runtime.Serialization;

namespace SlackPollingApp.Business.Exception
{
    [Serializable]
    public class CantMultiVoteException : System.Exception
    {
        public CantMultiVoteException() {}

        protected CantMultiVoteException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}
    }
}