using System;
using System.IO;
using System.Net;
using Dynastream.Fit;

class Program
{
    // TODO: Check if this is the correct protocol version
    readonly static Encode encode = new(ProtocolVersion.V20);

    static void Main(string[] args)
    {
        string originalFile = "examples/original.fit";
        string mergedFile = "examples/merged.fit";

        FileStream? originalFileStream = null;
        FileStream? mergedFileStream = null;

        try
        {
            originalFileStream = new(originalFile, FileMode.Open);
            mergedFileStream = new FileStream(mergedFile, FileMode.Create, FileAccess.ReadWrite);

            Decode decode = new();
            decode.MesgEvent += OnMesgCustom;
            decode.MesgDefinitionEvent += OnMesgDefinitionCustom;
            
            encode.Open(mergedFileStream);

            decode.Read(originalFileStream);

            encode.Close();

            Console.WriteLine("Merged file saved to " + mergedFileStream.Name);
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
            mergedFileStream?.Close();
        }
    }

    private static void OnMesgDefinitionCustom(object sender, MesgDefinitionEventArgs e)
    {
        encode.Write(e.mesgDef);
    }

    private static void OnMesgCustom(object sender, MesgEventArgs e)
    {
        if(e.mesg.Num == 20) // Record message
        {
            RecordMesg recordMesg = new(e.mesg);
            recordMesg.SetPower(123);
            encode.Write(recordMesg);
        }
        else if (e.mesg.Num == 19) // Lap message
        {
            LapMesg lapMesg = new(e.mesg);
            lapMesg.SetAvgPower(123);
            lapMesg.SetMaxPower(123);
            encode.Write(lapMesg);
        }
        else if (e.mesg.Num == 18) // Session message
        {
            SessionMesg sessionMesg = new(e.mesg);
            sessionMesg.SetAvgPower(123);
            sessionMesg.SetMaxPower(123);
            encode.Write(sessionMesg);
        }
        else 
        {
            encode.Write(e.mesg);
        }
    }
}