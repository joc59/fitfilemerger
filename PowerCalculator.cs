using Dynastream.Fit;

class PowerCalculator
{
    public event MesgEventHandler? MesgEvent;

    public void OnMesg(Mesg message)
    {
        MesgEvent?.Invoke(this, new MesgEventArgs(AddPowerValues(message)));
    }

    private Mesg AddPowerValues(Mesg message)
    {
        switch (message.Num)
        {
            case 20: // Record message
                RecordMesg recordMesg = new(message);
                recordMesg.SetPower(123);
                return recordMesg;
            case 19: // Lap message
                LapMesg lapMesg = new(message);
                lapMesg.SetAvgPower(123);
                lapMesg.SetMaxPower(123);
                return lapMesg;
            case 18: // Session message
                SessionMesg sessionMesg = new(message);
                sessionMesg.SetAvgPower(123);
                sessionMesg.SetMaxPower(123);
                return sessionMesg;
            default:
                return message;
        }
    }
}