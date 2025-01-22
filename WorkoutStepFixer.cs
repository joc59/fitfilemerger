using Dynastream.Fit;

class WorkoutStepFixer
{
    public event MesgEventHandler? MesgEvent;

    public void OnMesg(Mesg message)
    {
        if (message.Num == 27) // WorkoutStep message
        {
            // When workout step message is found, remove the notes
            // The notes sometimes cause issues when writing the workout step to the merged file using Encode, this
            // seems to be a bug in the FIT SDK
            // As a workaround, the notes are removed from the workout step message as they are not needed for analysis
            // purposes
            WorkoutStepMesg workoutStepMesg = new(message);
            workoutStepMesg.RemoveField(workoutStepMesg.GetField(WorkoutStepMesg.FieldDefNum.Notes));
            MesgEvent?.Invoke(this, new MesgEventArgs(workoutStepMesg));
        }
        else
        {
            MesgEvent?.Invoke(this, new MesgEventArgs(message));
        }
    }
}