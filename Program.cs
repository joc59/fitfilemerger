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
        using var destinationFileStream = new FileStream(mergedFile, FileMode.Create, FileAccess.ReadWrite);
        Decode decode = new();
        Encode encode = new(ProtocolVersion.V20);

        PowerCalculator powerCalculator = new();

        decode.MesgDefinitionEvent += (_, e) => encode.OnMesgDefinition(e.mesgDef);
        decode.MesgEvent += (_, e) => powerCalculator.OnMesg(e.mesg);
        powerCalculator.MesgEvent += (_, e) => encode.OnMesg(e.mesg);

        try
        {
            encode.Open(destinationFileStream);
            decode.Read(mainFileStream);
        }
        catch (Exception ex)
        {
            Console.WriteLine("A FitException occurred when trying to decode/encode FIT file. Message: " + ex.Message);
        }
        finally
        {
            encode?.Close();
        }
    }
}