using System;
using System.IO;
using Dynastream.Fit;

class Program
{
    private static int msgCounter = 0;
    private static int  msgDefinitionCounter = 0;
    private static int developerFieldDescriptionCounter = 0;

    static void Main(string[] args)
    {
        string originalFile = "examples/original.fit";
        string mergedFile = "output.fit";

        FileStream? originalFileStream = null;
        

        try
        {
            Decode decode = new();
            FitListener fitListener = new();
            decode.MesgEvent += fitListener.OnMesg;
            decode.MesgEvent += OnMesgCustom;
            decode.MesgDefinitionEvent += OnMesgDefinitionCustom;
            decode.DeveloperFieldDescriptionEvent += OnDeveloperFieldDescriptionCustom;

            originalFileStream = new(originalFile, FileMode.Open);

            decode.Read(originalFileStream);

            FitMessages fitMessages = fitListener.FitMessages;

            Console.WriteLine("Number of Messages: " + msgCounter);
            Console.WriteLine("Number of Message Definitions: " + msgDefinitionCounter);
            Console.WriteLine("Number of Developer Field Descriptions: " + developerFieldDescriptionCounter);
            Console.WriteLine("Number of Records: " + fitMessages.RecordMesgs.Count);
        }
        catch (FitException ex)
        {
            Console.WriteLine("A FitException occurred when trying to decode the FIT file. Message: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred when trying to decode the FIT file. Message: " + ex.Message);
        }
        finally
        {
            originalFileStream?.Close();
        }
    }

    private static void OnDeveloperFieldDescriptionCustom(object? sender, DeveloperFieldDescriptionEventArgs e)
    {
        developerFieldDescriptionCounter++;
    }

    private static void OnMesgDefinitionCustom(object sender, MesgDefinitionEventArgs e)
    {
        msgDefinitionCounter++;
    }

    private static void OnMesgCustom(object sender, MesgEventArgs e)
    {
        msgCounter++;
    }

    // Example Placeholder for Calculating Power
    static float CalculatePower(RecordMesg record)
    {
        // Replace with your logic to calculate power
        // For demonstration, we return a static value
        return 200.0f; // Example value in watts
    }
}