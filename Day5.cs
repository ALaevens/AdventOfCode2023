
using AdventOfCode2023;
using System;
using System.Text.RegularExpressions;

public class ConversionRange {
    public ConversionRange(long dstStart, long srcStart, long length) {
        this.dstStart = dstStart;
        this.srcStart = srcStart;
        this.length = length;
    }

    public long srcToDest(long src) {
        if (src >= srcStart && src - srcStart < length) {
            return dstStart + (src - srcStart);
        } else {
            return src;
        }
            
    }

    public long dstStart { get; private set; }
    public long srcStart { get; private set; }
    public long length { get; private set; }
}

public class ConversionTable {
    public ConversionTable() {
        keys = new List<long>();
        conversionRanges = new Dictionary<long, ConversionRange>();
    }

    public void addRange(long dstStart, long srcStart, long length) {
        int index = keys.BinarySearch(srcStart);
        if (index < 0) {
            keys.Insert(~index, srcStart);
            conversionRanges.Add(srcStart, new ConversionRange(dstStart, srcStart, length));
        }
    }
    public long convertOne(long src) {
        int index = keys.BinarySearch(src);
        if (index < 0) {
            index = Math.Max(~index - 1, 0); // want lower bracket not upper
        }

        return conversionRanges[keys[index]].srcToDest(src);
    }

    public List<Tuple<long, long>> convertRange(Tuple<long, long> range) {
        long low = range.Item1;
        long length = range.Item2;

        int indexLow = keys.BinarySearch(low);
        int indexHigh = keys.BinarySearch(low + length - 1);

        if (indexLow < 0) {
            indexLow = Math.Max(~indexLow - 1, 0);
            //indexLow = ~indexLow - 1;

        }

        if (indexHigh < 0) {
            indexHigh = ~indexHigh;
        }

        List<Tuple<long, long>> ranges = new List<Tuple<long, long>>();
        for (int i = indexLow; i < indexHigh; i++) {
            long mapStart = conversionRanges[keys[i]].srcStart;
            long mapEnd = mapStart + conversionRanges[keys[i]].length - 1;

            Console.WriteLine($"    Map Start: {mapStart}, Map End: {mapEnd}");

            if (low < mapStart) { // range start outside of map
                long sectionLength = Math.Min(length, mapStart - low);
                Console.WriteLine($"      Unshifted Range [low: {low}, length: {sectionLength}]");
                ranges.Add(new Tuple<long, long>(low, sectionLength));
                low += sectionLength;
                length -= sectionLength;
            }

            if (low >= mapStart && low <= mapEnd) { // range start inside map
                long sectionLength = Math.Min(length, mapEnd - low + 1);
                long shift = conversionRanges[keys[i]].dstStart - mapStart;
                Console.WriteLine($"      Shifted Range [low: {low}, length: {sectionLength}] -[{shift}]-> Range [low: {low+shift}, length: {sectionLength}]");
                ranges.Add(new Tuple<long, long>(low + shift, sectionLength));
                low += sectionLength;
                length -= sectionLength;
            } 
        }

        if (length > 0) { // still part of the range left over
            Console.WriteLine($"      Final Range [low: {low}, length: {length}]");
            ranges.Add(new Tuple<long, long>(low, length));
        }

        Console.WriteLine();

        return ranges;
    }

    private List<long> keys;
    private Dictionary<long, ConversionRange> conversionRanges;
}

public class Almanac {
    public Almanac(string[] definition) {
        maps = new Dictionary<string, ConversionTable>();

        Regex rNumbers = new Regex(@"\d+");
        seeds = rNumbers.Matches(definition[0]).Select(x => {
            long val = Int64.Parse(x.Value);
            Console.WriteLine($"Parse Seed {x} as {val}");
            return val;
        }).ToArray();

        Regex rMapName = new Regex(@"([\w-]+) map:");
        string currentMap = "";

        for (int i = 2; i < definition.Length; i++) { 
            Match matchMap = rMapName.Match(definition[i]);
            MatchCollection matchNumbers = rNumbers.Matches(definition[i]);

            if (matchMap.Success) {
                currentMap = matchMap.Groups[1].Value;
                maps.Add(currentMap, new ConversionTable());
                Console.WriteLine($"Parse Map: {currentMap}");
            } else if (matchNumbers.Count > 0) {
                long[] values = matchNumbers.Select(x => Int64.Parse(x.Value)).ToArray();

                maps[currentMap].addRange(values[0], values[1], values[2]);
                Console.WriteLine($"Add Range ({values[1]}..{values[1] + values[2] - 1}) => ({values[0]}..{values[0] + values[2] - 1}) to map {currentMap}");

            }
        }
    }

    public long[] seeds;
    public Dictionary<string, ConversionTable> maps;
}

public class Day5 : BaseDay {
    public override void Part1(string[] inputLines) {
        Almanac almanac = new Almanac(inputLines);
        string[] sequence = {
            "seed-to-soil",
            "soil-to-fertilizer",
            "fertilizer-to-water",
            "water-to-light",
            "light-to-temperature",
            "temperature-to-humidity",
            "humidity-to-location"
        };

        long lowest = Int64.MaxValue;
        foreach (long seed in almanac.seeds) {
            long val = seed;
            Console.WriteLine($"Seed: {seed}");
            foreach(string mapName in sequence) {
                val = almanac.maps[mapName].convertOne(val);
                Console.WriteLine($"  --[ {mapName} ]--> {val}");
            }

            lowest = Math.Min(lowest, val);
        }

        Console.WriteLine($"Lowest # location: {lowest}");
    }

    public override void Part2(string[] inputLines) {
        Almanac almanac = new Almanac(inputLines);
        string[] sequence = {
            "seed-to-soil",
            "soil-to-fertilizer",
            "fertilizer-to-water",
            "water-to-light",
            "light-to-temperature",
            "temperature-to-humidity",
            "humidity-to-location"
        };

        long lowest = Int64.MaxValue;

        List<Tuple<long, long>> ranges = new List<Tuple<long, long>>();
        for (int i = 0; i <= almanac.seeds.Length / 2; i+=2) {
            long low = almanac.seeds[i];
            long length = almanac.seeds[i + 1];
            Console.WriteLine($"Range [low: {low}, length: {length}]");
            ranges.Add(new Tuple<long, long>(low, length));
        }

        foreach (string mapName in sequence) {
            Console.WriteLine($"\nProcess map {mapName}");
            List<Tuple<long, long>> nextRanges = new List<Tuple<long, long>>();

            foreach(Tuple<long, long> range in ranges) {
                Console.WriteLine($"  SRC Range [low: {range.Item1}, length: {range.Item2}]");
                nextRanges.AddRange(almanac.maps[mapName].convertRange(range));
            }

            ranges = nextRanges;
        }

        foreach (Tuple<long, long> range in ranges) {
            lowest = Math.Min(lowest, range.Item1);
        }

        Console.WriteLine($"Lowest # location: {lowest}");

    }

}
