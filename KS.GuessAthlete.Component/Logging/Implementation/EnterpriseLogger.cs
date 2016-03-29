using KS.GuessAthlete.Component.Logging.Interface;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System;
using System.Configuration;
using System.Diagnostics;

namespace KS.GuessAthlete.Component.Logging.Implemetation
{
    /// <summary>
    /// Implementation of a logging class using Microsoft Enterprise logging.
    /// </summary>
    public class EnterpriseLogger : ILogger
    {
        /// <summary>
        /// 
        /// </summary>
        public static TraceEventType ApplicationTraceLevel = TraceEventType.Information;

        private static volatile EnterpriseLogger instance;
        private static object syncRoot = new Object();

        public static EnterpriseLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new EnterpriseLogger();
                    }
                }

                return instance;
            }
        }

        private EnterpriseLogger()
        {
            // Formatter
//            TextFormatter briefFormatter = new TextFormatter(
//@"Timestamp: {timestamp(local)} {tab}Message: {message}");
            TextFormatter briefFormatter = new TextFormatter(
@"Timestamp: {timestamp(local)}{newline}
{tab}Message: {message}{newline}
{tab}Category: {category}{newline}
{tab}Priority: {priority}{newline}
{tab}EventId: {eventid}{newline}
{tab}Severity: {severity}{newline}
{tab}Title:{title}{newline}
{tab}Machine: {machine}{newline}
{tab}Application Domain: {appDomain}{newline}
{tab}Process Id: {processId}{newline}
{tab}Process Name: {processName}{newline}
{tab}Win32 Thread Id: {win32ThreadId}{newline}
{tab}Thread Name: {threadName}{newline}
{newline}
{tab}Extended Properties: {dictionary({key} - {value})}");

            // Trace Listener
            FlatFileTraceListener flatFileTraceListener = new FlatFileTraceListener(
                ConfigurationManager.AppSettings["LogFilePath"],
                //null, null,
                "----------------------------------------",
                "----------------------------------------",
                briefFormatter);

            // Build Configuration
            var config = new LoggingConfiguration();

            config.AddLogSource("Error", SourceLevels.All, true)
                .AddTraceListener(flatFileTraceListener);

            LogWriter writer = new LogWriter(config);
            Logger.SetLogWriter(writer, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object source, object message, Exception exception = null)
        {
            Write(source, message, exception, TraceEventType.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Crictical(object source, object message, Exception exception = null)
        {
            Write(source, message, exception, TraceEventType.Critical);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public void Information(object source, object message)
        {
            Write(source, message, message, TraceEventType.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public void Warning(object source, object message)
        {
            Write(source, message, message, TraceEventType.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public void Verbose(object source, object message)
        {
            Write(source, message, message, TraceEventType.Verbose);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <param name="severity"></param>
        private void Write(object source, object message, object details, TraceEventType severity)
        {
            if ((int)severity <= (int)ApplicationTraceLevel)
            {
                LogEntry log = new LogEntry
                {
                    TimeStamp = DateTime.UtcNow,
                    Severity = severity,
                    MachineName = System.Environment.MachineName,
                    ProcessId = Process.GetCurrentProcess().Id.ToString(),
                };

                if (details != null)
                {
                    log.Message = details.ToString();
                }

                if (source != null && message != null)
                {
                    log.Title = string.Format("{0} -> {1}", source.ToString(), message.ToString());
                }

                Logger.Write(log);
            }
        }
    }
}
