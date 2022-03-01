using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Razor;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace RazorPad.Compilation
{
    public class TemplateCompilationParameters
    {

        public static TemplateCompilationParameters CSharp
        {
            get
            {  //

                //var csc = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
                //var settings = csc
                //    .GetType()
                //    .GetField("_providerOptions", BindingFlags.Instance | BindingFlags.NonPublic)
                //    .GetValue(csc);



                //var path = settings
                //    .GetType()
                //    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(r => r.Name.Contains("CompilerFullPath"));

                //path.SetValue(settings, ((string)path.GetValue(settings)).Replace(@"bin\roslyn\", @"roslyn\"));

                //return new TemplateCompilationParameters(new CSharpRazorCodeLanguage(), csc);
                return new TemplateCompilationParameters(new CSharpRazorCodeLanguage(), new CSharpCodeProvider(new Dictionary<string, string>()
                {
                    ["CompilerVersion"] = "v4.0"
                }));
            }
        }

        public static TemplateCompilationParameters VisualBasic
        {
            get { return new TemplateCompilationParameters(new VBRazorCodeLanguage(), new VBCodeProvider()); }
        }

        public CodeDomProvider CodeProvider { get; private set; }

        public RazorCodeLanguage Language { get; private set; }

        public CompilerParameters CompilerParameters { get; private set; }


        protected TemplateCompilationParameters(RazorCodeLanguage language, CodeDomProvider codeProvider, CompilerParameters compilerParameters = null)
        {
            Language = language;
            CodeProvider = codeProvider;
            CompilerParameters = compilerParameters ?? new CompilerParameters { GenerateInMemory = true };
            AddAssemblyReference(typeof(TemplateBase));
        }


        public void AddAssemblyReference(Type type)
        {
            Contract.Requires(type != null);

            AddAssemblyReference(type.Assembly.Location);
        }

        public void AddAssemblyReference(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            AddAssemblyReference(assembly.Location);
        }

        public void AddAssemblyReference(string location)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(location) == false);

            CompilerParameters.ReferencedAssemblies.Add(location);
        }

        public void SetReferencedAssemblies(IEnumerable<string> references)
        {
            CompilerParameters.ReferencedAssemblies.Clear();
            //foreach (var s in references)
            //    CompilerParameters.ReferencedAssemblies.Add(s);
            CompilerParameters.ReferencedAssemblies.AddRange(references.ToArray());
            //绝对引用的路径并不能访问，需要复制到本地目录来,但这可能会带来其它问题，先这样吧，没能解决
            foreach (string s in references)
            {
                string f = Path.Combine(AppContext.BaseDirectory, Path.GetFileName(s));
                if (!File.Exists(f) || md5(f) != md5(s))
                    try
                    {
                        File.Copy(s, f, true);
                    }
                    catch
                    {

                    }
            }
        }

        string md5(string fileName)
        {
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            var outputBye = m5.ComputeHash(File.ReadAllBytes(fileName));
            m5.Clear();
            return BitConverter.ToString(outputBye).Replace("-", "").ToLower();
        }

        public static TemplateCompilationParameters CreateFromFilename(string filename)
        {
            var extension = Path.GetExtension(filename ?? "test.cshtml") ?? string.Empty;

            if (extension.ToLower().Contains("vb"))
                return VisualBasic;

            return CSharp;
        }

    }
}