using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Text;
using Machine.Specifications;

namespace CSharpRunsPowerShell
{
    public class FileCopyClosureTests
    {
        public class Config
        {
            public string TestFilePath { get { return "c:\\temp\\test.txt"; } }
        }

        [Subject("FileCopy")]
        public class When_check_if_a_file_exists
        {
            private static PowerShell _shellInstance;
            private static Collection<PSObject> _output;

            Establish context = () =>
            {
                var config = new Config();

                if (File.Exists(config.TestFilePath)) File.Delete(config.TestFilePath);
                
                using (var fileStream = File.Create(config.TestFilePath))
                {
                    var bytes = new UTF8Encoding().GetBytes("This is a test for the sake of testing");
                    fileStream.Write(bytes, 0, bytes.Length);
                }

                string script =
                    "$config = New-Object CSharpRunsPowerShell.FileCopyClosureTests+Config;" + 
                    "Test-Path $config.TestFilePath";
                
                _shellInstance = PowerShell.Create();
                _shellInstance.AddScript(script);
            };

            Because of = () =>
            {
                _output = _shellInstance.Invoke();
                foreach (var psObject in _output)
                {
                    if (psObject != null)
                        Console.WriteLine(psObject);
                }

                foreach (var errorRecord in _shellInstance.Streams.Error)
                {
                    if (errorRecord != null)
                        Console.WriteLine(errorRecord);
                }
            };

            It should_have_the_file_in_the_temp_folder = () => ((bool)_output[0].BaseObject).ShouldBeTrue();
        }

    }
}