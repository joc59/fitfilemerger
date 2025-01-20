using Dynastream.Fit;

class Program
{
    static void Main(string[] args)
    {
        string originalFile = "examples/original.fit";
        string mergedFile = "examples/merged.fit";

        using var sourceFileStream = new FileStream(originalFile, FileMode.Open);
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
            decode.Read(sourceFileStream);
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