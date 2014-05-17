using System.Diagnostics;
using NLog;
using System;

namespace Core.Logic
{
	public class Logger
	{
		private readonly NLog.Logger _logger;

		public Logger()
		{
			_logger = LogManager.GetCurrentClassLogger();
		}

		public string WriteError(Exception ex)
		{
			var message = ex.ToString();
			return WriteToLog(message, LogLevel.Info);
		}

		public string WriteError(string message, Exception ex)
		{
			return WriteToLog(String.Format("{0}{1}{2}", message, Environment.NewLine, ex), LogLevel.Error);
		}


		public string WriteInfo(string message)
		{
			return WriteToLog(message, LogLevel.Info);
		}

		public void WriteDebugInfo(string message)
		{
			WriteToLog(message, LogLevel.Debug);
		}

		private string WriteToLog(string message, LogLevel logLevel)
		{
			try
			{
				var errorId = Guid.NewGuid().ToString();

				message = String.Format("ErrorId:{0}{1}{2}", errorId, Environment.NewLine, message);

				_logger.Log(logLevel, message);

				return errorId;
			}
			catch (Exception e)
			{
				HandleLoggerError(e);
				return null;
			}
		}

		private void HandleLoggerError(Exception logException)
		{
			var message = String.Format("Log Exception: {0}", logException);
			var eventLog = new EventLog("Application") { Source = "AppName" };
			eventLog.WriteEntry(message, EventLogEntryType.Error);
		}
	}
}
