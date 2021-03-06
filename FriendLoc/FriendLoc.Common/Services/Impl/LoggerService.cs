using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using FriendLoc.Common;
using FriendLoc.Common.Services;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace FriendLoc.Common.Services.Impl
{
    public enum LogTypes
    {
        Debug,
        Info,
        Error,
        Warning,
        Trace
    }

    public class LoggerService : ILoggerService
    {
        private Logger _logger;
        private IFileService _fileService;

        public LoggerService()
        {
            _fileService = ServiceInstances.FileService;
        }

        public LoggerService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public void Debug(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteLog(LogTypes.Debug, msg, file, member, line);
        }

        public void Error(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteLog(LogTypes.Error, msg, file, member, line);
        }

        public void Info(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteLog(LogTypes.Info, msg, file, member, line);
        }

        public void Trace(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteLog(LogTypes.Trace, msg, file, member, line);
        }

        public void Warning(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteLog(LogTypes.Warning, msg, file, member, line);
        }

        private void WriteLog(LogTypes type, string msg, string file = "", string member = "", int line = 0)
        {
            Console.WriteLine("Console: " + msg);

            if (_logger == null)
            {
                var logpath = Path.Combine(_fileService.GetSdCardFolder(), "Logs");

                if (!_fileService.FolderExists(logpath)) _fileService.CreateFolder(logpath);

                var config = new LoggingConfiguration();
                var fileTarget = new FileTarget();
                fileTarget.FileName = Path.Combine(logpath, DateTime.Now.ToString("yyyyMMdd") + ".txt"); ;
                fileTarget.Layout = "${longdate} | ${level} | ${message}";
                fileTarget.ConcurrentWrites = true;
                fileTarget.ConcurrentWriteAttemptDelay = 10;
                fileTarget.ConcurrentWriteAttempts = 8;
                fileTarget.AutoFlush = true;
                fileTarget.KeepFileOpen = true;
                fileTarget.DeleteOldFileOnStartup = false;

                config.AddTarget("file", fileTarget);

                var fileRule = new LoggingRule("*", LogLevel.Trace, fileTarget);
                config.LoggingRules.Add(fileRule);
                LogManager.Configuration = config;
                _logger = LogManager.GetCurrentClassLogger();
            }

            var logBuilder = new StringBuilder();
            logBuilder.Append("Version: " + 1.0);
            logBuilder.Append("\n* File: " + file);
            logBuilder.Append("\n* Member: " + member);
            logBuilder.Append("\n* Line: " + line);
            logBuilder.Append("\n* Message: " + msg.Replace("\n", "\t"));
            logBuilder.Append("\n-------------------------------------------------------------");

            switch (type)
            {
                case LogTypes.Debug:
                    _logger.Debug(logBuilder.ToString());
                    break;
                case LogTypes.Error:
                    _logger.Error(logBuilder.ToString());
                    break;
                case LogTypes.Info:
                    _logger.Info(logBuilder.ToString());
                    break;
                case LogTypes.Trace:
                    _logger.Trace(logBuilder.ToString());
                    break;
                case LogTypes.Warning:
                    _logger.Warn(logBuilder.ToString());
                    break;
                default:
                    break;
            }
        }
    }
}
