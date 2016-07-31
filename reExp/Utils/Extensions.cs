using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Controllers.rundotnet;
using System.Text.RegularExpressions;
using System.Text;

namespace reExp.Utils
{
    public static class MyExtensions
    {
        public static string ToLanguage(this LanguagesEnum number)
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
                case LanguagesEnum.MySql:
                    return "MySQL";
                case LanguagesEnum.Oracle:
                    return "Oracle";
                case LanguagesEnum.Postgresql:
                    return "PostgreSQL";
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

        public static LanguagesEnum ToLanguageEnum(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return LanguagesEnum.Unknown;
            }
            for (int i = 0; i <= Enum.GetNames(typeof(LanguagesEnum)).Length; i++)
            {
                if (s.ToLower() == ((LanguagesEnum)i).ToLanguage().ToLower())
                {
                    return (LanguagesEnum)i;
                }
            }

            return LanguagesEnum.Unknown;
        }


        public static GlobalConst.RundotnetStatus ToRundotnetStatus(this int number)
        {
            switch (number)
            { 
                case 0:
                    return GlobalConst.RundotnetStatus.Error;
                case 1:
                    return GlobalConst.RundotnetStatus.OK;
                case 2:
                    return GlobalConst.RundotnetStatus.Unknown;
                default:
                    return GlobalConst.RundotnetStatus.Unknown;
            }
        }

        public static string FirstLine(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            if (!text.Contains("\n"))
                return text.Trim();

            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return lines[0].Trim();
        }

        public static string BeginningOfString(this string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= 100)
                return text;
            return text.Substring(0, 100) + " ...";
        }


        public static string TimeAgo(this DateTime date)
        {
            var now = DateTime.UtcNow;
            var min = (now - date).TotalMinutes;
            if (min < 1)
                return "just now";
            if (min < 60)
                return string.Format("{0} min ago", (int)min);
            if (min < 1440)
                return string.Format("{0} hours ago", (int)(min / 60));
            if (min < 43200)
                return string.Format("{0} days ago", (int)(min / 1440));
            if (min < 518400)
                return string.Format("{0} months ago", (int)(min / 43200));

            return string.Format("{0} years ago", (int)(min / 518400));
        }

        public static string Number(this int? number)
        {
            if (number == null)
                return "0";
            if (number < 1000)
                return number.ToString();
            if (number < 1000000)
                return (number / 1000).ToString() + "K";

            return (number / 1000000).ToString() + "M";
        }

        public static string StringToUrl(this string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormKD);
            Encoding removal = Encoding.GetEncoding(Encoding.ASCII.CodePage,
                                                    new EncoderReplacementFallback(""),
                                                    new DecoderReplacementFallback(""));
            byte[] bytes = removal.GetBytes(normalized);
            return Regex.Replace(Encoding.ASCII.GetString(bytes), @"[^A-Za-z0-9]+", "-");
        }
    } 
}