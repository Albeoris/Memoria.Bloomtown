using System.Windows;

internal sealed class ProgramContext
{
    public ISplitTablesCommandPerformer GetSplitTablesCommandPerformer() => new SplitTablesCommandPerformer();
}