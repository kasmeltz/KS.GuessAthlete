using System;
using KS.GuessAthlete.Component.Logging.Interface;

namespace KS.GuessAthlete.Component.Logging.Implementation
{
    /// <summary>
    /// Represents an item that doesn't actually log anything. 
    /// </summary>
    public class NoOpLogger : ILogger
    {
        public void Crictical(object source, object message, Exception exception = null)
        {
        }

        public void Error(object source, object message, Exception exception = null)
        {
        }

        public void Information(object source, object message)
        {
        }

        public void Verbose(object source, object message)
        {
        }

        public void Warning(object source, object message)
        {
        }
    }
}
