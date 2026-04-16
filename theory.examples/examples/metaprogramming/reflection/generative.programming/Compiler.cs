using System;
using System.IO;
using Microsoft.CSharp;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace generativeprogramming
{
    public static class Compiler
    {

        /// <summary>
        /// Compiles a C# code into a library
        /// </summary>
        public static Assembly Compile(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            CSharpCompilation compilation = CSharpCompilation.Create("assemblyName",
                                                                    new[] { syntaxTree },
                                                                    new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) },
                                                                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                                                                    );

            using (var dllStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(dllStream, pdbStream);
                if (!emitResult.Success)
                {
                    IEnumerable<Diagnostic> failures = emitResult.Diagnostics.Where(diagnostic =>
                                diagnostic.IsWarningAsError ||
                                diagnostic.Severity == DiagnosticSeverity.Error);

                    string text = "Compile error: ";
                    foreach (Diagnostic diagnostic in failures)
                    {
                        text += String.Format("{0}: {1}\n", diagnostic.Id, diagnostic.GetMessage());
                    }
                    throw new Exception(text);
                }

                dllStream.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(dllStream.ToArray());
                return assembly;
            }
        }


        /// <summary>
        /// Loads a method of a class in the compiled code
        /// </summary>
        public static MethodInfo LoadFunction(Assembly compileResults, string className, string functionName)
        {
            Module module = compileResults.GetModules()[0];
            Type mt = null;
            MethodInfo methInfo = null;

            if (module != null)
            {
                mt = module.GetType(className);
            }

            if (mt != null)
            {
                methInfo = mt.GetMethod(functionName);
            }

            return methInfo;
        }
    }
}
