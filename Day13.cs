using AdventOfCode2023;

internal class Day13 : BaseDay {
    private class Map {
        public Map() {
            mapData = new List<string>();
            rowCount = new List<int>();
        }

        public void addRow(string row) {
            mapData.Add(row);
            rowCount.Add(row.Count(x => x == '#'));
        } 

        public Map rotate() {
            Map map = new Map();

            for (int i = 0; i < mapData[0].Length; i++) {
                string column = string.Concat(mapData.Select(x => x[i]));
                map.addRow(column);
            }

            return map;
        }

        public int findReflection() {
            for (int i = 0; i < rowCount.Count - 1; i++) {
                int reflectSize = Math.Min(i + 1, rowCount.Count - i - 1);

                List<int> left = rowCount.GetRange(i+1 - reflectSize, reflectSize).ToList();
                List<int> right = rowCount.GetRange(i+1, reflectSize);
                right.Reverse();

                if (left.SequenceEqual(right)) {
                    bool allMatch = true;
                    for (int offset = 0; offset < reflectSize; offset++) {
                        if (mapData[i - offset] != mapData[i + 1 + offset]) {
                            allMatch = false;
                        }
                    }

                    if (allMatch)
                        return i + 1;
                }
            }

            return 0;
        }

        public int findSmudgedReflection() {
            for (int i = 0; i < mapData.Count - 1; i++) {
                int reflectSize = Math.Min(i + 1, rowCount.Count - i - 1);
                int totalErrors = 0;

                for (int offset = 0; offset < reflectSize; offset++) {
                    List<bool> top = mapData[i - offset].Select(x => x == '#' ? true : false).ToList();
                    List<bool> bottom = mapData[i + 1 + offset].Select(x => x == '#' ? true : false).ToList();

                    List<bool> errors = top.Zip(bottom, (t, b) => t ^ b).Where(x => x).ToList();
                    totalErrors += errors.Count;

                    if (totalErrors > 1) {
                        break;
                    }

                }

                if (totalErrors == 1) {
                    return i + 1;
                }
            }

            return 0;
        }

        public void print() {
            Console.WriteLine("MAP: ");
            foreach (string row in mapData) {
                Console.WriteLine(row.Replace('#', '█').Replace('.', ' '));
            }
            Console.WriteLine("With row count: " + string.Join(", ", rowCount));
        }

        private List<string> mapData;
        private List<int> rowCount;
    }
    public override void Part1(string[] inputLines) {
        Map map = new Map();

        long sum = 0;
        foreach (string l in inputLines) {
            string line = l.Trim();

            if (line.Length == 0) {
                //map.print();

                int horizontal = map.findReflection();
                int vertical = map.rotate().findReflection();

                Console.WriteLine($"Reflections: H: {horizontal}, V: {vertical}\n\n");
                sum += 100 * horizontal;
                sum += vertical;

                map = new Map();

            } else {
                map.addRow(line);
            }
        }
        Console.WriteLine("Final Total: " + sum);
    }

    public override void Part2(string[] inputLines) {
        Map map = new Map();

        long sum = 0;
        foreach (string l in inputLines) {
            string line = l.Trim();

            if (line.Length == 0) {
                int horizontal = map.findSmudgedReflection();
                int vertical = map.rotate().findSmudgedReflection();

                Console.WriteLine($"Reflections: H: {horizontal}, V: {vertical}\n");
                sum += 100 * horizontal;
                sum += vertical;

                map = new Map();

            } else {
                map.addRow(line);
            }
        }
        Console.WriteLine("Final Total: " + sum);
    }
}