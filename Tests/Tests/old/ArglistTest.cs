using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.old
{
    class ArglistTest
    {

        public static int WriteLine1(params string[] arr)
        {
            return arr.Length;
        }

        // Test prerformance of this way compared with params
        //public static void WriteLine(String format, __arglist)
        //{
        //    ArgIterator args = new ArgIterator(__arglist);
        //    int argCount = args.GetRemainingCount();
        //    object[] objArgs = new object[argCount];
        //    for (int i = 4; i < argCount; i++)
        //    {
        //        objArgs[i] = TypedReference.ToObject(args.GetNextArg());
        //    }
        //    //Out.WriteLine(format, objArgs);
        //}
    }
}
