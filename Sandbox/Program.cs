using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Sandbox
{
    //The Sandboxer class needs to derive from MarshalByRefObject so that we can create it in another
    // AppDomain and refer to it from the default AppDomain.
    class Sandboxer : MarshalByRefObject
    {
        private static object[] parameters = { new string[] { "parameter for the curious" } };

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            string pathToUntrusted = args[0].Replace("|_|", " ");
            string untrustedAssembly = args[1];
            string entryPointString = args[2];
            string[] parts = entryPointString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            string name_space = parts[0];
            string class_name = parts[1];
            string method_name = parts[2];

            //Setting the AppDomainSetup. It is very important to set the ApplicationBase to a folder
            //other than the one in which the sandboxer resides.
            AppDomainSetup adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = Path.GetFullPath(pathToUntrusted);

            //Setting the permissions for the AppDomain. We give the permission to execute and to
            //read/discover the location where the untrusted code is loaded.
            PermissionSet permSet = new PermissionSet(PermissionState.None);

            permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permSet.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));
            permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.ControlThread));
            permSet.AddPermission(new NetworkInformationPermission(PermissionState.Unrestricted));
            permSet.AddPermission(new WebPermission(PermissionState.Unrestricted));

            if (untrustedAssembly.StartsWith("fsharp_"))
            {
                //for F# printf to work
                var fileio = new FileIOPermission(PermissionState.None);
                fileio.AllLocalFiles = FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery;
                permSet.AddPermission(fileio);
            }
            //We want the sandboxer assembly's strong name, so that we can add it to the full trust list.
            //StrongName fullTrustAssembly = typeof(Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>();


            var a1 = typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly.GetName();
            var a2 = typeof(System.ComponentModel.Composition.ImportAttribute).Assembly.GetName();
            var a3 = typeof(System.Web.HttpRequest).Assembly.GetName();
            var a4 = typeof(System.Net.Http.HttpClient).Assembly.GetName();
            var a5 = typeof(System.Drawing.Image).Assembly.GetName();
            var a6 = typeof(Newtonsoft.Json.JsonSerializer).Assembly.GetName();


            adSetup.PartialTrustVisibleAssemblies = new string[]
            {
                string.Format("{0}, PublicKey={1}", a1.Name, ByteArrayToString(a1.GetPublicKey()).ToUpper()),
                string.Format("{0}, PublicKey={1}", a2.Name, ByteArrayToString(a2.GetPublicKey()).ToUpper()),                
                string.Format("{0}, PublicKey={1}", a3.Name, ByteArrayToString(a3.GetPublicKey()).ToUpper()),
                string.Format("{0}, PublicKey={1}", a4.Name, ByteArrayToString(a4.GetPublicKey()).ToUpper()),
                string.Format("{0}, PublicKey={1}", a5.Name, ByteArrayToString(a5.GetPublicKey()).ToUpper()),
                string.Format("{0}, PublicKey={1}", a6.Name, ByteArrayToString(a6.GetPublicKey()).ToUpper()),
            };

            //Now we have everything we need to create the AppDomain, so let's create it.
            AppDomain newDomain = AppDomain.CreateDomain("Sandbox", null, adSetup, permSet, /*fullTrustAssembly*/null);

            //Use CreateInstanceFrom to load an instance of the Sandboxer class into the
            //new AppDomain.
            ObjectHandle handle = Activator.CreateInstanceFrom(
                newDomain, typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName,
                typeof(Sandboxer).FullName
                );
            //Unwrap the new domain instance into a reference in this domain and use it to execute the
            //untrusted code.
            Sandboxer newDomainInstance = (Sandboxer)handle.Unwrap();

            Job job = new Job(newDomainInstance, untrustedAssembly, name_space, class_name, method_name, parameters);
            Thread thread = new Thread(new ThreadStart(job.DoJob));
            thread.Start();
            thread.Join(10000);
            if (thread.ThreadState != ThreadState.Stopped)
            {
                thread.Abort();
                Console.Error.WriteLine("Job taking too long. Aborted.");
            }
            AppDomain.Unload(newDomain);
        }

        public void ExecuteUntrustedCode(string assemblyName, string name_space, string class_name, string method_name, object[] parameters)
        {
            MethodInfo target = null;
            try
            {
                target = Assembly.Load(assemblyName).GetType(name_space + "." + class_name).GetMethod(method_name);
                if (target == null)
                    throw new Exception();
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Entry method '{0}' in class '{1}' in namespace '{2}' not found.", method_name, class_name, name_space);
                return;
            }
            try
            {

                var suppliedParams = target.GetParameters();
                if (suppliedParams == null || suppliedParams.Length != parameters.Length)
                    throw new Exception();

                for (int i = 0; i < suppliedParams.Length; i++)
                    if (suppliedParams[i].ParameterType.FullName != parameters[i].GetType().FullName)
                        throw new Exception();
            }
            catch (Exception)
            {
                string expectedParams = "";
                for (int i = 0; i < parameters.Length; i++)
                {
                    expectedParams += parameters[i].GetType().ToString();
                    if (i != parameters.Length - 1)
                        expectedParams += ", ";
                }
                Console.Error.WriteLine("'{0}' method is supplied with unexpected parameters. Expected parameters: {1}", method_name, expectedParams);
                return;
            }
            //Now invoke the method.
            try
            {
                target.Invoke(null, parameters);
            }
            catch (Exception e)
            {
                List<string> total = new List<string>();
                Exception inner = e.InnerException;
                if(inner != null)
                    total.Add(string.Format("\n\n{0}: {1}\n{2}", inner.GetType().ToString(), inner.Message, inner.StackTrace));
                while (inner != null && inner.InnerException != null)
                {
                    inner = inner.InnerException;
                    total.Add(string.Format("\n\n{0}: {1}\n{2}", inner.GetType().ToString(), inner.Message, inner.StackTrace));
                }

                total.Reverse();
                if (inner != null)
                    Console.Error.WriteLine("Exception in user code:" + total.Aggregate((a, b) => a+b));
                else
                    Console.Error.WriteLine("Exception in user code:\n{0}: {1}\n{2}", e.GetType().ToString(), e.Message, e.StackTrace);
            }
        }

        static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

    }

    class Job
    {
        Sandboxer sandboxer = null;
        string assemblyName;
        string name_space;
        string class_name;
        string method_name;
        object[] parameters;

        public Job(Sandboxer sandboxer, string assemblyName, string name_space, string class_name, string method_name, object[] parameters)
        {
            this.sandboxer = sandboxer;
            this.assemblyName = assemblyName;
            this.name_space = name_space;
            this.class_name = class_name;
            this.method_name = method_name;
            this.parameters = parameters;
        }

        public void DoJob()
        {
            try
            {
                sandboxer.ExecuteUntrustedCode(assemblyName, name_space, class_name, method_name, parameters);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}