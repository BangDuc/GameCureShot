public static class EndingRuntime
{
    public static EndingId CurrentEnding { get; private set; }
    public static bool HasSelection { get; private set; }

    public static void SelectEnding(EndingId endingId)
    {
        CurrentEnding = endingId;
        HasSelection = true;
    }

    public static void Clear()
    {
        HasSelection = false;
    }
}