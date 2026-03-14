using Spectre.Console;
using TPICAP.Matching.Core.Abstractions;
using TPICAP.Matching.Core.Algorithms;
using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Console;

internal static class Program
{
    public static void Main()
    {
        var scenarios = new[]
        {
            new DemoScenario(
                "Price-Time-Priority",
                new PriceTimePriorityMatchingAlgorithm(),
                new []
                {
                    new Order("A", "A1", OrderDirection.Buy, 100, 4.99m, new TimeOnly(9, 27, 43)),
                    new Order("B", "B1", OrderDirection.Buy, 200, 5.00m, new TimeOnly(10, 21, 46)),
                    new Order("C", "C1", OrderDirection.Buy, 150, 5.00m, new TimeOnly(10, 26, 18)),
                    new Order("D", "D1", OrderDirection.Sell, 150, 5.00m, new TimeOnly(10, 32, 41)),
                    new Order("E", "E1", OrderDirection.Sell, 100, 5.00m, new TimeOnly(10, 33, 7))
                }),
            new DemoScenario(
                "Pro-Rata",
                new ProRataMatchingAlgorithm(),
                new []
                {
                    new Order("A", "A1", OrderDirection.Buy, 50, 5.00m, new TimeOnly(9, 27, 43)),
                    new Order("B", "B1", OrderDirection.Buy, 200, 5.00m, new TimeOnly(10, 21, 46)),
                    new Order("C", "C1", OrderDirection.Sell, 200, 5.00m, new TimeOnly(10, 26, 18))
                })
        };

        AnsiConsole.Write(new FigletText("TP ICAP").Color(Color.Cyan1));
        AnsiConsole.Write(new Rule("[yellow]Matching Challenge Demo[/]").LeftJustified());
        AnsiConsole.MarkupLine("[grey]Hard-coded handout scenarios shown for reviewer readability.[/]");
        AnsiConsole.WriteLine();

        foreach (var scenario in scenarios)
        {
            RenderScenario(scenario);
            AnsiConsole.WriteLine();
        }
    }

    private static void RenderScenario(DemoScenario scenario)
    {
        var orderBook = scenario.Algorithm.Match(scenario.Orders);

        AnsiConsole.Write(new Rule($"[green]{Markup.Escape(scenario.AlgorithmName)}[/]").LeftJustified());
        AnsiConsole.Write(RenderInputOrdersTable(scenario.Orders));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(RenderResultingOrderBookTable(orderBook));
    }

    private static Table RenderInputOrdersTable(IEnumerable<Order> orders)
    {
        var table = new Table()
            .RoundedBorder()
            .Title("[white]Input Orders[/]")
            .AddColumn("CompanyId")
            .AddColumn("OrderId")
            .AddColumn("Direction")
            .AddColumn("Volume")
            .AddColumn("Price")
            .AddColumn("Notional")
            .AddColumn("Time");

        foreach (var order in orders)
        {
            table.AddRow(
                Escape(order.CompanyId),
                Escape(order.OrderId),
                order.Direction.ToString(),
                order.Volume.ToString(),
                FormatCurrency(order.Price),
                FormatCurrency(order.Notional),
                order.Timestamp.ToString("HH:mm:ss"));
        }

        return table;
    }

    private static Table RenderResultingOrderBookTable(IEnumerable<MatchedOrder> orderBook)
    {
        var table = new Table()
            .RoundedBorder()
            .Title("[white]Resulting Order Book[/]")
            .AddColumn("CompanyId")
            .AddColumn("OrderId")
            .AddColumn("Direction")
            .AddColumn("Volume")
            .AddColumn("Notional")
            .AddColumn("MatchState")
            .AddColumn("Matches");

        foreach (var order in orderBook)
        {
            table.AddRow(
                Escape(order.CompanyId),
                Escape(order.OrderId),
                order.Direction.ToString(),
                order.Volume.ToString(),
                FormatCurrency(order.Notional),
                order.MatchState.ToString(),
                FormatMatches(order.Matches));
        }

        return table;
    }

    private static string FormatMatches(IReadOnlyList<MatchEntry> matches)
    {
        if (matches.Count == 0)
        {
            return "[grey]None[/]";
        }

        return string.Join(
            Environment.NewLine,
            matches.Select(match => $"{Escape(match.OrderId)} | Volume {match.Volume} | Notional {FormatCurrency(match.Notional)}"));
    }

    private static string FormatCurrency(decimal value) => value.ToString("0.00");

    private static string Escape(string value) => Markup.Escape(value);

    private sealed record DemoScenario(string AlgorithmName, IMatchingAlgorithm Algorithm, IReadOnlyList<Order> Orders);
}
