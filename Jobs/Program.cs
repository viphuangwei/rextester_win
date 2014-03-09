using GlobalUtils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Jobs
{
    class Program
    {
        static void Main(string[] args)
        {
            Elastic el = new Elastic();
            el.Do();
        }        
    }
}
