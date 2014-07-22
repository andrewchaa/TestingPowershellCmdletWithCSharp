using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Text;
using Machine.Specifications;

namespace CSharpRunsPowerShell
{
    public class FileCopyParamTests
    {
        [Subject("FileCopy")]
        public class When_check_if_a_file_exists
        {
            private static PowerShell _shellInstance;
            private static Collection<PSObject> _output;

            Establish context = () =>
            {
                const string testFilePath = "c:\\temp\\test.txt";

                if (File.Exists(testFilePath))
                    File.Delete(testFilePath);
                
                using (var fileStream = File.Create(testFilePath))
                {
                    var bytes = new UTF8Encoding().GetBytes("This is a test for the sake of testing");
                    fileStream.Write(bytes, 0, bytes.Length);
                }

                string script =
                    "param([string]$testFilePath, [string]$testTemp); " + 
                    "Test-Path $testFilePath;" +
                    "$testTemp;" + 
                    "Test-Path $testFilePathNew";
                
                _shellInstance = PowerShell.Create();
                _shellInstance.AddScript(script);
                _shellInstance.AddParameter("testFilePath", testFilePath);
//                _shellInstance.AddParameter("testTemp", "Hello");
            };

            Because of = () =>
            {
                _output = _shellInstance.Invoke();
                foreach (var psObject in _output)
                {
                    if (psObject != null)
                        Console.WriteLine("output: " + psObject);
                }

                foreach (var errorRecord in _shellInstance.Streams.Error)
                {
                    if (errorRecord != null)
                        Console.WriteLine("error: " + errorRecord);
                }
            };

            It should_have_the_file_in_the_temp_folder = () => ((bool)_output[0].BaseObject).ShouldBeTrue();

            It should_have_run_the_script_without_any_error = () =>
                _shellInstance.Streams.Error.ShouldBeEmpty();
        }

    }
}