using dnlib.DotNet;

namespace exetops1
{
    internal class Program
    {
        private static readonly string _template = """

                                                   function Invoke-%%NAMESPACE%%
                                                   {

                                                       [CmdletBinding()]
                                                       Param (
                                                           [String]
                                                           $Command = ""

                                                       )
                                                       [byte[]]$byteOutArray = [Convert]::FromBase64String("%%BASE64%%")
                                                       $RAS = [System.Reflection.Assembly]::Load($byteOutArray)
                                                   
                                                       [%%NAMESPACE%%.%%CLASS%%]::%%ENTRYPOINT%%($Command.Split(" "))
                                                   }

                                                   """;
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: exetops1.exe <file.exe> <output.ps1>");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File not found");
                return;
            }

            var exeFile = args[0];
            var psFile = args[1];

            var assembly = ReadAssembly(exeFile);
            if (assembly is null) return;

            var script = BuildScript(assembly, exeFile);
            if (script == "") return;
            File.WriteAllText(psFile, script);
            Console.WriteLine("File written. Happy AMSI Bypassing!");
        }

        static string BuildScript(AssemblyDef assembly, string filename)
        {
            var entryPoint = assembly.ManifestModule.EntryPoint;
            if (!entryPoint.IsPublic)
            {
                Console.WriteLine("The entrypoint is marked as private. Please ensure it is public in order to continue.");
                return "";
            }
            var parent = entryPoint.DeclaringType;
            if (!parent.IsPublic)
            {
                Console.WriteLine("The entrypoint type is not public. Please ensure it is public in order to continue.");
                return "";
            }
            var nameSpace = parent.Namespace;
            var base64 = Convert.ToBase64String(File.ReadAllBytes(filename));

            return _template
                .Replace("%%BASE64%%", base64)
                .Replace("%%NAMESPACE%%", nameSpace.String)
                .Replace("%%CLASS%%", parent.Name.String)
                .Replace("%%ENTRYPOINT%%", entryPoint.Name.String);
        }

        static AssemblyDef? ReadAssembly(string file)
        {
            try
            {
                var assembly = AssemblyDef.Load(file);
                return assembly;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
