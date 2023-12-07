
using AdventOfCode2023;
using System.Text.RegularExpressions;

public class Day2 : BaseDay {
    public override void Part1(string[] inputLines) {

        Regex r_1 = new Regex(@"Game (\d+): (.*)"); // Seperate the ID from the game sequences
        Regex r_2 = new Regex(@"(\d+) (blue|green|red)"); // Get information about a single pull

        Dictionary<string, int> maxAllowed = new Dictionary<string, int> {
            {"red", 12}, {"green", 13}, {"blue", 14}
        };

        int sum = 0;

        foreach (string line in inputLines) {
            Match m_1 = r_1.Match(line);
            int gameID = Int32.Parse(m_1.Groups[1].Value);

            string[] pulls = m_1.Groups[2].Value.Split(';');

            bool allValid = true;
            foreach (string p in pulls) {
                string pull = p.Trim();

                MatchCollection pullColors = r_2.Matches(pull);
                foreach (Match match in pullColors) {
                    if (maxAllowed[match.Groups[2].Value] < Int32.Parse(match.Groups[1].Value)) { // if (amount allowed for color) < amount of color removed from bag
                        allValid = false;
                        break;
                    }
                }
            }

            if (allValid) {
                sum += gameID;
            }
        }

        Console.WriteLine($"SUM: {sum}");
    }

    public override void Part2(string[] inputLines) {

        Regex r_1 = new Regex(@"Game (\d+): (.*)"); // Seperate the ID from the game sequences
        Regex r_2 = new Regex(@"(\d+) (blue|green|red)"); // Get information about a single pull

        int sum = 0;

        foreach (string line in inputLines) {
            Match m_1 = r_1.Match(line);
            Dictionary<string, int> maxFound = new Dictionary<string, int> {
                {"red", 0}, {"green", 0}, {"blue", 0}
            };

            string[] pulls = m_1.Groups[2].Value.Split(';');

            foreach (string p in pulls) {
                string pull = p.Trim();

                MatchCollection pullColors = r_2.Matches(pull);
                foreach (Match match in pullColors) {
                    string color = match.Groups[2].Value;
                    int quantity = Int32.Parse(match.Groups[1].Value);

                    maxFound[color] = Math.Max(maxFound[color], quantity);
                }
                
            }

            int power = maxFound.Values.Aggregate((a, b) => a * b);
            sum += power;
        }

        Console.WriteLine($"SUM: {sum}");
    }
}
