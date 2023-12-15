
using AdventOfCode2023;
using System.Text;
using System.Text.RegularExpressions;

using Lens = System.Tuple<string, int>;
internal class Day15 : BaseDay {
    private int hash(string s) {
        byte[] bytes = Encoding.ASCII.GetBytes(s);
        int value = 0;
        foreach (byte b in bytes) {
            value = ((value + b) * 17) % 256;
        }

        return value;
    }

    private void printState(ref Dictionary<int, List<Lens>> boxes) {
        for (int i = 0; i < boxes.Count; i++) {
            if (boxes[i] != null && boxes[i].Count > 0) {
                Console.WriteLine($"Box {i}: {string.Join(" ", boxes[i].Select(x => $"[{x.Item1} {x.Item2}]"))}");
            }
        }
    }

    public override void Part1(string[] inputLines) {
        List<string> steps = string.Concat(inputLines.Select(x => x.Trim())).Split(',').ToList();

        int sum = steps.Select(x => hash(x)).Sum();
        Console.WriteLine(sum);
    }

    public override void Part2(string[] inputLines) {
        List<string> steps = string.Concat(inputLines.Select(x => x.Trim())).Split(',').ToList();

        Dictionary<int, List<Lens>> boxes = new Dictionary<int, List<Lens>>();

        // initalize map
        for (int i = 0; i < 256; i++) {
            boxes.Add(i, new List<Lens>());
        }

        Regex r = new Regex(@"([a-zA-Z]+)([=-])(\d?)");
        foreach (string step in steps) {
            Match match = r.Match(step);
            string label = match.Groups[1].Value;
            string operation = match.Groups[2].Value;

            int box = hash(label);
            if (operation == "=") {
                int focalLength = Int32.Parse(match.Groups[3].Value);
                int index = boxes[box].FindIndex(x => x.Item1 == label);
                if (index < 0) {
                    boxes[box].Add(new Lens(label, focalLength));
                } else {
                    boxes[box][index] = new Lens(label, focalLength);
                }
            } else if (operation == "-") {
                int index = boxes[box].FindIndex(x => x.Item1 == label);
                if (index >= 0) {
                    boxes[box].RemoveAt(index);
                }
            }

            //Console.WriteLine($"\nAfter \"{step}\":");
            //printState(ref boxes);
        }

        int sum = boxes.Select(box => (box.Key + 1) * box.Value.Select((lens, i) => (i + 1) * lens.Item2).Sum()).Sum();
        Console.WriteLine(sum);
    }
}

