using Dynastream.Fit;

class PowerCalculator
{
    // we don't return a power value if the closests is more than 3 seconds apart
    private const int MAX_TIMESTAMP_DIFF_IN_SECONDS = 3;
    private readonly List<RecordMesg> powerRecords = [];

    public event MesgEventHandler? MesgEvent;

    public void OnPowerMesg(Mesg message)
    {
        if (message.Num == 20) // Record message
        {
            RecordMesg recordMesg = new(message);
            if (recordMesg.GetPower() != null)
            {
                powerRecords.Add(recordMesg);
            }
        }
    }

    public void OnMainMesg(Mesg message)
    {
        MesgEvent?.Invoke(this, new MesgEventArgs(AddPowerValues(message)));
    }

    private Mesg AddPowerValues(Mesg message)
    {
        switch (message.Num)
        {
            case 20: // Record message
                RecordMesg recordMesg = new(message);
                RecordMesg? matchingPowerRecord = FindClosestPowerRecord(recordMesg.GetTimestamp());
                if (matchingPowerRecord != null)
                {
                    recordMesg.SetPower(matchingPowerRecord.GetPower());
                }
                return recordMesg;
            case 19: // Lap message
                LapMesg lapMesg = new(message);
                lapMesg.SetAvgPower(GetAveragePower(lapMesg.GetStartTime(), lapMesg.GetTimestamp()));
                lapMesg.SetMaxPower(GetMaxPower(lapMesg.GetStartTime(), lapMesg.GetTimestamp()));
                return lapMesg;
            case 18: // Session message
                SessionMesg sessionMesg = new(message);
                sessionMesg.SetAvgPower(GetAveragePower(sessionMesg.GetStartTime(), sessionMesg.GetTimestamp()));
                sessionMesg.SetMaxPower(GetMaxPower(sessionMesg.GetStartTime(), sessionMesg.GetTimestamp()));
                return sessionMesg;
            default:
                return message;
        }
    }

    private ushort? GetAveragePower(Dynastream.Fit.DateTime start, Dynastream.Fit.DateTime finish)
    {
        double? average = powerRecords
            .FindAll((powerRecord) => powerRecord.GetTimestamp().GetTimeStamp() >= start.GetTimeStamp()
                                        && powerRecord.GetTimestamp().GetTimeStamp() <= finish.GetTimeStamp())
            .Average((powerRecord) => powerRecord.GetPower());
        return average.HasValue ? Convert.ToUInt16(average.Value) : null;
    }

    private ushort? GetMaxPower(Dynastream.Fit.DateTime start, Dynastream.Fit.DateTime finish)
    {
        return powerRecords
            .FindAll((powerRecord) => powerRecord.GetTimestamp().GetTimeStamp() >= start.GetTimeStamp()
                                        && powerRecord.GetTimestamp().GetTimeStamp() <= finish.GetTimeStamp())
            .Max((powerRecord) => powerRecord.GetPower());
    }

    private RecordMesg? FindClosestPowerRecord(Dynastream.Fit.DateTime dateTime)
    {
        RecordMesg? closestPowerRecord = powerRecords
                    // Order by timestamp ascending
                    .OrderBy(e => Math.Abs(e.GetTimestamp().GetTimeStamp()))
                    // Find the latest record, which was just before the given timestamp
                    .LastOrDefault(powerRecord => powerRecord.GetTimestamp().GetTimeStamp() <= dateTime.GetTimeStamp());

        if (IsWithinBoundaries(closestPowerRecord, dateTime))
        {
            return closestPowerRecord;
        }
        return null;
    }

    private bool IsWithinBoundaries(RecordMesg? powerRecord, Dynastream.Fit.DateTime dateTime)
    {
        return powerRecord != null
            && Math.Abs(powerRecord.GetTimestamp().GetTimeStamp() - dateTime.GetTimeStamp()) < MAX_TIMESTAMP_DIFF_IN_SECONDS;
    }
}
