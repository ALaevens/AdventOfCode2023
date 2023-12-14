
using AdventOfCode2023;
using System;
using System.Diagnostics.CodeAnalysis;


internal class Day14 : BaseDay {
    private class World {
        public World(List<List<char>> worldData) {
            this.worldData = worldData;
        }

        public void rollUp() {
            // sorry
            for (int row = 0; row < worldData.Count; row++) {
                for (int reachedRow = row; reachedRow > 0; reachedRow--) {
                    for (int j = 0; j < worldData[row].Count; j++) {
                        if (worldData[reachedRow][j] == 'O' && worldData[reachedRow - 1][j] == '.') {
                            worldData[reachedRow][j] = '.';
                            worldData[reachedRow - 1][j] = 'O';
                        } 
                    }
                }
            }
        }

        public int weight() {
            return worldData.Select((x, i) => x.Count(c => c == 'O') * (worldData.Count - i)).Sum();
        }
        public World rotate() {
            List<List<char>> newWorldData = new List<List<char>>();

            for (int i = 0; i < worldData[0].Count; i++) {
                List<char> newLine = worldData.Select(x => x[i]).ToList();
                newLine.Reverse();
                newWorldData.Add(newLine);
            }

            return new World(newWorldData);
        }

        public void print() {
            foreach (List<char> line in worldData) {
                Console.WriteLine(string.Concat(line));
            }
        }

        public string oneLine() {
            return string.Concat(worldData.Select(x => string.Concat(x)));
        }

        private List<List<char>> worldData;
    }

    public override void Part1(string[] inputLines) {
        World world = new World(inputLines.Select(x => x.Trim().ToList()).ToList());
        world.rollUp();
        Console.WriteLine(world.weight());
    }

    public override void Part2(string[] inputLines) {

        World world = new World(inputLines.Select(x => x.Trim().ToList()).ToList());
        Dictionary<string, int> worldCache = new Dictionary<string, int>();
        Dictionary<int, int> weightCache = new Dictionary<int, int>();

        int cycle = 1;
        int cycleTarget = 1000000000;
        while (cycle <= cycleTarget) {
            if (worldCache.ContainsKey(world.oneLine())) {
                int loopStart = worldCache[world.oneLine()];
                int loopLength = cycle - loopStart;
                int finalCycleOffset = (cycleTarget - loopStart) % loopLength;
                Console.WriteLine($"LOOP: cycle {cycle} == cycle {loopStart}");
                Console.WriteLine($"LOOP: cycle {cycleTarget} == cycle {loopStart + finalCycleOffset}");

                Console.WriteLine($"Cycle {cycleTarget} has weight: {weightCache[loopStart + finalCycleOffset]}");
                break;
            } else {
                worldCache.Add(world.oneLine(), cycle);
            }

            for (int dir = 0; dir < 4; dir++) {
                world.rollUp();
                world = world.rotate();
            }

            int cycleWeight = world.weight();
            weightCache[cycle] = cycleWeight;

            //Console.WriteLine($"After {cycle} cycles: {world.weight()}");
            //world.print();
            //Console.WriteLine("\n\n");

            cycle++;
        }
    }
}

