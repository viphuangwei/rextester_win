using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using reExp.Utils;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace reExp.Controllers.rundotnet
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/
        [ValidateInput(false)]
        public string Codecompletion(string code, int position, int language, int line, int ch)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();

            if (string.IsNullOrEmpty(code))
                return null;

            try
            {
                if (language == 1)
                {
                    position--;
                    bool is_using = false;
                    string[] lines = code.Split("\n".ToCharArray(), StringSplitOptions.None);
                    string l = lines[line];
                    if ((new Regex(@"^\s*using\s+[^(]+")).IsMatch(l))
                        is_using = true;

                    if (code[position] != '.')
                    {
                        while (code[position] != '.' && "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890_".Count(f => f == code[position]) != 0)
                        {
                            position--;
                        }
                        if (code[position] != '.')
                            return null;
                    }
                    if (is_using)
                    {
                        for (int i = position; code[i] != '\n';)
                            code = code.Remove(i, 1);
                    }
                    else
                    {
                        code = code.Insert(position, " ");
                    }
                        
                    
                    var tree = Roslyn.Compilers.CSharp.SyntaxTree.ParseCompilationUnit(code);
                    var mscorlib = new AssemblyFileReference(typeof(object).Assembly.Location);
                    var core = new AssemblyFileReference(typeof(Queryable).Assembly.Location);
                    var data = new AssemblyFileReference(typeof(System.Data.DataTable).Assembly.Location);
                    var xml = new AssemblyFileReference(typeof(System.Xml.XmlAttribute).Assembly.Location);
                    var xml_linq = new AssemblyFileReference(typeof(System.Xml.Linq.XAttribute).Assembly.Location);
                    var c_sharp = new AssemblyFileReference(typeof(Microsoft.CSharp.CSharpCodeProvider).Assembly.Location);
                    var anot = new AssemblyFileReference(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly.Location);
                    var comp = new AssemblyFileReference(typeof(System.ComponentModel.Composition.ImportAttribute).Assembly.Location);
                    var web = new AssemblyFileReference(typeof(System.Web.HttpRequest).Assembly.Location);
                    var http = new AssemblyFileReference(typeof(System.Net.Http.HttpClient).Assembly.Location);
                    var numerics = new AssemblyFileReference(typeof(System.Numerics.BigInteger).Assembly.Location);

                    var compilation = Roslyn.Compilers.CSharp.Compilation.Create(
                        "MyCompilation",
                        syntaxTrees: new[] { tree },
                        references: new[] { mscorlib, core, data, xml, xml_linq, c_sharp, anot, comp, web, http, numerics });

                    var semanticModel = compilation.GetSemanticModel(tree);     

                    var p = tree.GetRoot().FindToken(position).Parent;

                    if (is_using)
                    {
                        var usn = p.AncestorsAndSelf().OfType<UsingDirectiveSyntax>().FirstOrDefault();
                        if (usn != null)
                        {
                            List<string> namespaces = new List<string>();
                            var nameInfo = semanticModel.GetSymbolInfo(usn.Name);
                            var systemSymbol = (NamespaceSymbol)nameInfo.Symbol;
                            foreach (var ns in systemSymbol.GetNamespaceMembers())
                                namespaces.Add(ns.Name);

                            return json.Serialize(namespaces);
                        }
                        else
                            return null;
                    }


                    ExpressionSyntax identifier = p as ExpressionSyntax;                    
                    if (p is MemberAccessExpressionSyntax)
                    {
                        identifier = p.ChildNodes().FirstOrDefault() as ExpressionSyntax;
                    }

                    if (identifier == null)
                        identifier = p as LiteralExpressionSyntax;
                    if (identifier == null)
                        identifier = p as ParenthesizedExpressionSyntax;

                    if (identifier == null)
                        identifier = p.Parent as InvocationExpressionSyntax;

                    if (identifier == null)
                        identifier = p.Parent as ObjectCreationExpressionSyntax;

                    if (identifier == null)
                        return null;

                    var semanticInfo = semanticModel.GetTypeInfo(identifier);
                    var type = semanticInfo.Type;

                    var symbols = semanticModel.LookupSymbols (position, container: type,
                                                                        options: LookupOptions.IncludeExtensionMethods | LookupOptions.Default);

                    List<string> res = new List<string>();
                    foreach (var symbol in symbols)
                    {
                        var result = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                        if (symbol.Kind == SymbolKind.Method)
                        {
                            result = result.Substring(result.IndexOf(" ")); //cut return type
                        }
                        result = result.Substring(result.IndexOf(".")+1); //cut containing type
                        if (symbol.Kind == SymbolKind.Method)
                        {
                            var prefix = result.Substring(0, result.IndexOf('(')); //cut redundant generics info
                            prefix = prefix.Substring(0, prefix.IndexOf('<') == -1 ? prefix.Length : prefix.IndexOf('<'));
                            result = prefix + result.Substring(result.IndexOf('('));
                        }
                        if (result.Length > 100)
                        {
                            result = result.Substring(0, 100) + " ...";
                            if (symbol.Kind == SymbolKind.Method)
                                result += ")";
                        }
                        if(!res.Contains(result))
                            res.Add(result);
                    }
                    res.Sort();

                    return json.Serialize(res);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Utils.Log.LogInfo(e.Message+"\n\n\n"+code, "code completion error, position: "+position);
                return null;
            }
        }

    }
}
