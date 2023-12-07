using AdventOfCode2023;
using System.Text.RegularExpressions;

public class Day4 : BaseDay {
    public override void Part1(string[] inputLines) {
        Regex rParts = new Regex(@"Card\s+\d+: ([\d ]+)\|([\d ]+)");
        Regex rValues = new Regex(@"\d+");

        int sum = 0;

        foreach (string line in inputLines) {
            Match m = rParts.Match(line);
            string winningNumbersString = m.Groups[1].Value;
            string cardNumbersString = m.Groups[2].Value;

            HashSet<int> winningNumbers = new HashSet<int>(rValues.Matches(winningNumbersString).Select(m => Int32.Parse(m.Value)));
            HashSet<int> cardNumbers = new HashSet<int>(rValues.Matches(cardNumbersString).Select(m => Int32.Parse(m.Value)));

            cardNumbers.IntersectWith(winningNumbers);

            Console.WriteLine($"{winningNumbersString.Trim()} | {cardNumbersString.Trim()} => {string.Join(" ", cardNumbers)}");

            sum += (int) Math.Floor(Math.Pow(2, cardNumbers.Count - 1));

        }

        Console.WriteLine($"Sum: {sum}");
    }

    public override void Part2(string[] inputLines) {
        Regex rParts = new Regex(@"Card\s+(\d+): ([\d ]+)\|([\d ]+)");
        Regex rValues = new Regex(@"\d+");

        Dictionary<int, int> cards = new Dictionary<int, int>();

        foreach (string line in inputLines) {
            Match m = rParts.Match(line);
            int card = Int32.Parse(m.Groups[1].Value);
            string winningNumbersString = m.Groups[2].Value;
            string cardNumbersString = m.Groups[3].Value;

            HashSet<int> winningNumbers = new HashSet<int>(rValues.Matches(winningNumbersString).Select(m => Int32.Parse(m.Value)));
            HashSet<int> cardNumbers = new HashSet<int>(rValues.Matches(cardNumbersString).Select(m => Int32.Parse(m.Value)));

            cardNumbers.IntersectWith(winningNumbers);

            // Count original card instance
            if (cards.ContainsKey(card)) {
                cards[card]++;
            } else {
                cards[card] = 1;
            }

            // add copies
            for (int i = 1; i <= cardNumbers.Count; i++) {
                if (cards.ContainsKey(card + i)) {
                    cards[card + i] += cards[card];
                } else {
                    cards[card + i] = cards[card];
                }
            }
        }
        Console.WriteLine($"Sum: {cards.Values.Sum()}");
    }
}