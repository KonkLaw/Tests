using System.Diagnostics;
using System.Numerics;

namespace TestDotNet.Utils;

partial class RunHelper
{
    public static void CheckEnviroment()
    {
        Console.WriteLine($"Local processors count = {Environment.ProcessorCount}");
        bool is64 = Environment.Is64BitProcess;
        if (!is64)
        {
            EndWithMessage("Use 64 bit process.");
        }
        if (!Vector.IsHardwareAccelerated)
        {
            EndWithMessage("Hardware acceleration is dissabled or not supported.");
        }

        int sizeOfRegister = Vector<float>.Count;
        if (sizeOfRegister < 4)
        {
            EndWithMessage("Wrong register size. Test should be uncorrected.");
        }
        Console.WriteLine(
            $"Register size = {sizeOfRegister} int/float numbers = {sizeOfRegister * sizeof(int)} bytes = {sizeOfRegister * sizeof(int) * 8} bits");
    }

    public static void CheckRunModeAndRequestEnter()
    {
        bool isDebug = false;
#if (DEBUG)
        isDebug = true;
#endif
        if (isDebug)
        {
            if (Debugger.IsAttached)
                return;
            else
            {
                EndWithMessage("Run in debug (without debuger).");
            }
        }
        else
        {
            if (Debugger.IsAttached)
            {
                EndWithMessage("Run in release but with debuger.");
            }
            else
            {
                Console.WriteLine("Press 'Enter' to start.");
                Console.ReadLine();
            }
        }
    }

    private static void EndWithMessage(string message)
    {
        throw new Exception(message);
    }
}
