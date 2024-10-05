using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

internal static class ExtensionMethodsCommand
{
    public static void SetCancellableHandler<T1, T2>(
        this Command command,
        Action<T1, T2, CancellationToken> handle,
        IValueDescriptor<T1> symbol1,
        IValueDescriptor<T2> symbol2)
    {
        command.SetHandler((context) =>
        {
            T1? value1 = GetValueForHandlerParameter(symbol1, context)!;
            T2? value2 = GetValueForHandlerParameter(symbol2, context)!;
            CancellationToken token = context.GetCancellationToken();

            handle(value1, value2, token);
        });
    }

    public static void SetAsyncHandler<T1, T2>(
        this Command command,
        Func<T1, T2, CancellationToken, Task> handle,
        IValueDescriptor<T1> symbol1,
        IValueDescriptor<T2> symbol2)
    {
        command.SetHandler(async (context) =>
        {
            T1? value1 = GetValueForHandlerParameter(symbol1, context)!;
            T2? value2 = GetValueForHandlerParameter(symbol2, context)!;
            CancellationToken token = context.GetCancellationToken();

            await handle(value1, value2, token);
        });
    }

    private static T? GetValueForHandlerParameter<T>(
        IValueDescriptor<T> symbol,
        InvocationContext context)
    {
        if (symbol is IValueSource valueSource &&
            valueSource.TryGetValue(symbol, context.BindingContext, out var boundValue) &&
            boundValue is T value)
        {
            return value;
        }
        else
        {
            return context.ParseResult.GetValueFor(symbol);
        }
    }

    private static T? GetValueFor<T>(this ParseResult result, IValueDescriptor<T> symbol)
    {
        return symbol switch
        {
            Argument<T> argument => result.GetValueForArgument(argument),
            Option<T> option => result.GetValueForOption(option),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}