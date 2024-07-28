using log4net.Config;
using log4net.Core;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Text;
using UsaWeb.Service.Controllers;

namespace UsaWeb.Service.Data
{
    public class DBHelper
    {
        private readonly ILogger<DBHelper> _logger;

        public DBHelper(ILogger<DBHelper> logger)
        {
            _logger = logger;
        }

        public static string RawSqlQuery(string query, IDictionary<string, string> paramList)
        {
            try
            {
                using (var context = new Usaweb_DevContext())
                {
                    using (var cmd = context.Database.GetDbConnection().CreateCommand())
                    {
                        if (paramList != null)
                        {
                            foreach (KeyValuePair<string, string> p in paramList)
                            {
                                IDbDataParameter dp;
                                dp = cmd.CreateParameter();
                                dp.ParameterName = p.Key;

                                if (p.Value == null)
                                    dp.Value = DBNull.Value;
                                else
                                    dp.Value = p.Value;

                                cmd.Parameters.Add(dp);
                            }
                        }

                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;


                        context.Database.OpenConnection();

                        var jsonResult = new StringBuilder();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                jsonResult.Append("[]");
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    jsonResult.Append(reader.GetValue(0).ToString());
                                }
                            }
                        }
                        return jsonResult.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                throw;
            }
            return string.Empty;
        }

        public static void LogError(string message)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            ILog _logger = LogManager.GetLogger(typeof(LoggerManager));
            _logger.Info(message);
        }
    }
}
