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
using System.Data;
using Oracle.ManagedDataAccess.Types;

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
            //Regex regex1 = new Regex(@"\[?sys\]?\.\[?databases\]?", RegexOptions.IgnoreCase);
            //Regex regex2 = new Regex(@"\[?sys\]?\.\[?sysdatabases\]?", RegexOptions.IgnoreCase);
            //if (!string.IsNullOrEmpty(sql) && (regex1.IsMatch(sql) || regex2.IsMatch(sql)))
            //{
            //    Console.Error.WriteLine("[sys].[databases] and [sys].[sysdatabases] are not allowed to be used in code due to security reasons");
            //    return;
            //}
            //regex1 = new Regex(@"begin\s+tran", RegexOptions.IgnoreCase);
            //regex2 = new Regex(@"commit", RegexOptions.IgnoreCase);
            //var regex3 = new Regex(@"rollback", RegexOptions.IgnoreCase);
            //if (!string.IsNullOrEmpty(sql) && (regex1.IsMatch(sql) || regex2.IsMatch(sql) || regex3.IsMatch(sql)))
            //{
            //    Console.Error.WriteLine("Transaction related keywords (begin tran, commit and rollback) are not permitted in code due to security reasons");
            //    return;
            //}

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

        void ShowResultSet(OracleDataReader reader, OracleCommand command)
        {
            //it is included dbms_output
            command.CommandText = "begin dbms_output.enable (32000); end;";
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();

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


            // create parameter objects for the anonymous pl/sql block
            int NUM_TO_FETCH = 8;
            OracleParameter p_lines = new OracleParameter("", OracleDbType.Varchar2, NUM_TO_FETCH, "", ParameterDirection.Output);
            p_lines.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            p_lines.ArrayBindSize = new int[NUM_TO_FETCH];
            // set the bind size value for each element
            for (int i = 0; i < NUM_TO_FETCH; i++)
            {
                p_lines.ArrayBindSize[i] = 32000;
            }


            // this is an input output parameter...
            // on input it holds the number of lines requested to be fetched from the buffer
            // on output it holds the number of lines actually fetched from the buffer
            OracleParameter p_numlines = new OracleParameter("", OracleDbType.Decimal, "", ParameterDirection.InputOutput);
            // set the number of lines to fetch
            p_numlines.Value = NUM_TO_FETCH;
            // set up command object and execute anonymous pl/sql block
            command.CommandText = "begin dbms_output.get_lines(:1, :2); end;";
            command.CommandType = CommandType.Text;
            command.Parameters.Add(p_lines);
            command.Parameters.Add(p_numlines);
            command.ExecuteNonQuery();

            // get the number of lines that were fetched (0 = no more lines in buffer)
            int numLinesFetched = ((OracleDecimal)p_numlines.Value).ToInt32();

            // as long as lines were fetched from the buffer...
            while (numLinesFetched > 0)
            {
                // write the text returned for each element in the pl/sql
                // associative array to the console window
                for (int i = 0; i < numLinesFetched; i++)
                {
                    string out_string = (string)(p_lines.Value as OracleString[])[i];
                    if (!string.IsNullOrEmpty(out_string))
                    {
                        Console.WriteLine(System.Web.HttpUtility.HtmlEncode(out_string));
                    }
                }

                // re-execute the command to fetch more lines (if any remain)
                command.ExecuteNonQuery();
                // get the number of lines that were fetched (0 = no more lines in buffer)
                numLinesFetched = ((OracleDecimal)p_numlines.Value).ToInt32();
            }
            // clean up

            p_numlines.Dispose();
            p_lines.Dispose();
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
                                ShowResultSet(reader, command);
                                while (reader.NextResult())
                                    ShowResultSet(reader, command);
                            }
                        }



                        //string out_string;
                        //int status = 0;
                        //command.CommandText = "BEGIN DBMS_OUTPUT.GET_LINE (:out_string, :status); END;";
                        //command.CommandType = CommandType.Text;
                        //command.Parameters.Clear();
                        //command.Parameters.Add("out_string", OracleDbType.Varchar2, 32000);
                        //command.Parameters.Add("status", OracleDbType.Double);
                        //command.Parameters[0].Direction = System.Data.ParameterDirection.Output;
                        //command.Parameters[1].Direction = System.Data.ParameterDirection.Output;
                        //command.ExecuteNonQuery();
                        //out_string = command.Parameters[0].Value.ToString();
                        //status = int.Parse(command.Parameters[1].Value.ToString());
                        //if (!string.IsNullOrEmpty(out_string))
                        //{
                        //    Console.WriteLine(System.Web.HttpUtility.HtmlEncode(out_string));
                        //}



                        

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
