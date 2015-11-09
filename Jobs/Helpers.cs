using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs
{
    public class Helpers
    {
        public enum LanguagesEnum : int
        {
            CSharp = 1,
            VB = 2,
            FSharp = 3,
            Java = 4,
            Python = 5,
            C = 6,
            CPP = 7,
            Php = 8,
            Pascal = 9,
            ObjectiveC = 10,
            Haskell = 11,
            Ruby = 12,
            Perl = 13,
            Lua = 14,
            Nasm = 15,
            SqlServer = 16,
            Javascript = 17,
            Lisp = 18,
            Prolog = 19,
            Go = 20,
            Scala = 21,
            Scheme = 22,
            Nodejs = 23,
            Python3 = 24,
            Octave = 25,
            CClang = 26,
            CPPClang = 27,
            VCPP = 28,
            VC = 29,
            D = 30,
            R = 31,
            Tcl = 32,
            Unknown = 0
        }

        public static string ToLanguage(LanguagesEnum number)
        {
            switch (number)
            {
                case LanguagesEnum.CSharp:
                    return "C#";
                case LanguagesEnum.VB:
                    return "Visual Basic";
                case LanguagesEnum.FSharp:
                    return "F#";
                case LanguagesEnum.Java:
                    return "Java";
                case LanguagesEnum.Python:
                    return "Python";
                case LanguagesEnum.C:
                    return "C (gcc)";
                case LanguagesEnum.CPP:
                    return "C++ (gcc)";
                case LanguagesEnum.CClang:
                    return "C (clang)";
                case LanguagesEnum.CPPClang:
                    return "C++ (clang)";
                case LanguagesEnum.VCPP:
                    return "C++ (vc++)";
                case LanguagesEnum.VC:
                    return "C (vc)";
                case LanguagesEnum.Php:
                    return "Php";
                case LanguagesEnum.Pascal:
                    return "Pascal";
                case LanguagesEnum.ObjectiveC:
                    return "Objective-C";
                case LanguagesEnum.Haskell:
                    return "Haskell";
                case LanguagesEnum.Ruby:
                    return "Ruby";
                case LanguagesEnum.Perl:
                    return "Perl";
                case LanguagesEnum.Lua:
                    return "Lua";
                case LanguagesEnum.Nasm:
                    return "Assembly";
                case LanguagesEnum.SqlServer:
                    return "Sql Server";
                case LanguagesEnum.Javascript:
                    return "Javascript";
                case LanguagesEnum.Lisp:
                    return "Common Lisp";
                case LanguagesEnum.Prolog:
                    return "Prolog";
                case LanguagesEnum.Go:
                    return "Go";
                case LanguagesEnum.Scala:
                    return "Scala";
                case LanguagesEnum.Scheme:
                    return "Scheme";
                case LanguagesEnum.Nodejs:
                    return "Node.js";
                case LanguagesEnum.Python3:
                    return "Python 3";
                case LanguagesEnum.Octave:
                    return "Octave";
                case LanguagesEnum.D:
                    return "D";
                case LanguagesEnum.R:
                    return "R";
                case LanguagesEnum.Tcl:
                    return "Tcl";
                default:
                    return "Unknown";
            }
        }
    }

    public class DB
    {
        static string location = @"C:\inetpub\wwwroot\rextester\App_Data\db.s3db";
        static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(string.Format("Data Source={0};Version=3;", location));
        }

        public static void ExecuteNonQuery(string query, List<SQLiteParameter> pars)
        {
            using (var Conn = GetConnection())
            {
                Conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, Conn))
                {
                    foreach (var par in pars)
                        command.Parameters.Add(par);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Dictionary<string, object>> ExecuteQuery(string query, List<SQLiteParameter> pars)
        {
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();
            using (var Conn = GetConnection())
            {
                Conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, Conn))
                {
                    foreach (var par in pars)
                        command.Parameters.Add(par);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                res.Add(new Dictionary<string, object>());
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    res[res.Count - 1][reader.GetName(i)] = reader[i];
                                }
                            }
                        }
                    }
                }
            }
            return res;
        }
    }
}
