using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using System.Management.Automation;

namespace CSharpRunsPowerShell
{
    [Subject("The simplest way")]
    public class SimplestWay
    {
        Establish context = () =>
        {
            using (var shell = PowerShell.Create())
            {
                shell.AddScript(
                    "param($param1) $d = get-date; " + 
                    "$s = 'test string value'; " +
                    "$d; $s; $param1; get-service"
                    );
                shell.AddParameter("param1", "parameter 1 value!");

                Collection<PSObject> output = shell.Invoke();

                foreach (var item in output)
                {
                    if (item != null)
                        Console.WriteLine(item.BaseObject);
                }

                foreach (var error in shell.Streams.Error)
                {
                    Console.WriteLine(error);
                }
            }
        };

        It should_be_true = () => true.ShouldBeTrue();

    }
}
