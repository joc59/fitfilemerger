using Dynastream.Fit;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: fitfilemerger.exe path\\to\\main.fit path\\to\\secondary.fit path\\to\\merged.fit");
            return;
        }
        string mainFile = args[0];
        string secondaryFile = args[1];
        string mergedFile = args[2];

        using var mainFileStream = new FileStream(mainFile, FileMode.Open);
        using var secondaryFileStream = new FileStream(secondaryFile, FileMode.Open);
        using var destinationFileStream = new FileStream(mergedFile, FileMode.Create, FileAccess.ReadWrite);

        Decode decodeMain = new();
        Decode decodeSecondary = new();
        Encode encode = new(ProtocolVersion.V20);

        PowerCalculator powerCalculator = new();

        // Recording power values from the secondary file
        decodeSecondary.MesgEvent += (_, e) => powerCalculator.OnPowerMesg(e.mesg);

        // Adding power values to the mesages from main file and passing them to the merged file
        decodeMain.MesgEvent += (_, e) => powerCalculator.OnMainMesg(e.mesg);
        powerCalculator.MesgEvent += (_, e) => encode.OnMesg(e.mesg);

        // Passing the mesg definitions to the main file to the merged file unaltered
        decodeMain.MesgDefinitionEvent += (_, e) => encode.OnMesgDefinition(e.mesgDef);

        try
        {
            decodeSecondary.Read(secondaryFileStream);
            encode.Open(destinationFileStream);
            decodeMain.Read(mainFileStream);
        }
        catch (FitException ex)
        {
            Console.WriteLine("A FitException occurred when trying to decode/encode FIT file. Message: " + ex.Message);
        }
        finally
        {
            encode?.Close();
        }
    }
}
