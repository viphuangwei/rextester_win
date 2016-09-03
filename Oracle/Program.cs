using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;

namespace SqlServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string sql = "";
            string path = args[0].Replace("|_|", " ");
            using (TextReader tr = new StreamReader(path))
            {
                sql = tr.ReadToEnd();
            }
            Regex regex1 = new Regex(@"\[?sys\]?\.\[?databases\]?", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@"\[?sys\]?\.\[?sysdatabases\]?", RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(sql) && (regex1.IsMatch(sql) || regex2.IsMatch(sql)))
            {
                Console.Error.WriteLine("[sys].[databases] and [sys].[sysdatabases] are not allowed to be used in code due to security reasons");
                return;
            }
            regex1 = new Regex(@"begin\s+tran", RegexOptions.IgnoreCase);
            regex2 = new Regex(@"commit", RegexOptions.IgnoreCase);
            var regex3 = new Regex(@"rollback", RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(sql) && (regex1.IsMatch(sql) || regex2.IsMatch(sql) || regex3.IsMatch(sql)))
            {
                Console.Error.WriteLine("Transaction related keywords (begin tran, commit and rollback) are not permitted in code due to security reasons");
                return;
            }

            Job job = new Job(sql, path);
            Thread t = new Thread(job.DoWork);
            t.Start();
            t.Join(10000);
            if (t.ThreadState != ThreadState.Stopped)
            {
                t.Abort();
                Console.Error.WriteLine("Job taking too long. Aborted.");
            }
        }
    }


    public class Job
    {
        string com { get; set; }
        string path { get; set; }
        public Job(string command, string path)
        {
            this.com = command;
            this.path = path;
        }

        void ShowResultSet(OracleDataReader reader)
        {
            bool headerPrinted = false;
            int count = 1;
            while (reader.Read())
            {
                if (!headerPrinted)
                {
                    Console.WriteLine(@"<table class=""sqloutput""><tbody><tr><th>&nbsp;&nbsp;</th>");
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string name = reader.GetName(i);
                        Console.WriteLine(@"<th>{0}</th>", string.IsNullOrEmpty(name) ? "(No column name)" : name);
                    }
                    Console.WriteLine("</tr>");
                    headerPrinted = true;
                }
                Console.WriteLine(@"<tr><td>{0}</td>", count++);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader[i] == DBNull.Value)
                    {
                        Console.WriteLine(@"<td><i>NULL</i></td>");
                    }
                    else
                    {
                        if (reader[i] as string == null && reader[i] as IEnumerable != null)
                        {
                            string res = "";
                            foreach (var a in (reader[i] as IEnumerable))
                                res += Convert.ToString(a);
                            Console.WriteLine(@"<td>{0}</td>", res);
                        }
                        else
                        {
                            Console.WriteLine(@"<td>{0}</td>", HttpUtility.HtmlEncode(reader[i]));
                        }
                    }
                }
                Console.WriteLine("</tr>");
            }
            if (headerPrinted)
                Console.WriteLine("</tbody></table>");
        }

        List<string> GetCommands(string text)
        {
            List<string> commands = new List<string>();
            if (!string.IsNullOrEmpty(text))
            {
                text = "\n" + text + "\n";
                Regex regex = new Regex(@"\\{2}[\s\t\n\r]+", RegexOptions.IgnoreCase);
                if (regex.IsMatch(text))
                {
                    List<string> parts = regex.Split(text).Where(f => !string.IsNullOrEmpty(f.Trim())).ToList<string>();
                    foreach (string c in parts)
                        commands.AddRange(GetCommands(c));
                }
                else
                {
                    commands.Add(text.Trim());
                }
            }
            return commands;
        }
        public void DoWork()
        {
            using (OracleConnection conn = new OracleConnection(GlobalUtils.TopSecret.OracleCS))
            using (OracleCommand command = new OracleCommand())
            {
                try
                {
                    conn.Open();
                    //conn.StatisticsEnabled = true;
                    command.Connection = conn;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    return;
                }
                try
                {
                    using (OracleTransaction sqlTran = conn.BeginTransaction())
                    {
                        command.Transaction = sqlTran;
                        OracleDataReader reader;

                        List<string> commands = GetCommands(com);
                        foreach (string c in commands)
                        {
                            command.CommandText = c;
                            using (reader = command.ExecuteReader())
                            {
                                ShowResultSet(reader);
                                while (reader.NextResult())
                                    ShowResultSet(reader);
                            }
                        }
                        //var stats = conn.RetrieveStatistics();
                        //using (TextWriter tw = new StreamWriter(path + ".stats"))
                        //{
                        //    tw.WriteLine("Execution time: {0} sec, rows selected: {1}, rows affected: {2}",
                        //                    Math.Round((double)(long)stats["ExecutionTime"] / 1000, 2),
                        //                    stats["SelectRows"],
                        //                    stats["IduRows"]);
                        //}
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    if (command != null)
                        command.Cancel();
                }
            }
        }
    }
}
