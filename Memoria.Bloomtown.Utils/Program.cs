using System.CommandLine;
using System.IO;
using System.Windows;
using Microsoft.Win32;

internal sealed class Program
{
    // Must be instantiated to call open file dailog
    private static readonly Application WpfApplication = new Application();

    [STAThread]
    public static Int32 Main(string[] args)
    {
        try
        {
            ProgramContext context = new ProgramContext();

            RootCommand rootCommand =
            [
                BuildSplitTablesCommand(context)
            ];

            return rootCommand.Invoke(args);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("The operation has been cancelled.");
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
            return -1;
        }
    }

    private static Command BuildSplitTablesCommand(ProgramContext context)
    {
        Command cmd = new Command("split-tables", "Splits .json tables from the input file");

        Argument<FileInfo> inputFile = new("inputFile", getDefaultValue: () =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select input file...",
                Filter = "Text files (*.txt)|*.txt",
                CheckFileExists = true
            };

            if (openFileDialog.ShowDialog() == true)
                return new FileInfo(openFileDialog.FileName);

            throw new OperationCanceledException();
        }) { Description = "Input text file" };

        cmd.AddArgument(inputFile);

        Option<DirectoryInfo> outputDirectory = new("--outputDir") { Description = "Output directory" };
        cmd.AddOption(outputDirectory);

        ISplitTablesCommandPerformer performer = context.GetSplitTablesCommandPerformer();
        cmd.SetCancellableHandler(performer.SplitTables, inputFile, outputDirectory);
        return cmd;
    }
}