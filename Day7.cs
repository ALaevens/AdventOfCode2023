using AdventOfCode2023;
using System.Text.RegularExpressions;

public class Hand : IComparable<Hand> {
    public Hand(string pCards, bool jokerRule) {
        cards = pCards;

        Console.WriteLine($"\nNew hand [{pCards}]");
        if (jokerRule) {
            
            if (pCards.Contains('J')) {
                string convertedCards = transformJokerHand(cards);
                setTypeScore(convertedCards);
            } else {
                setTypeScore(cards);
            }
            
            setCardScore(cards, "J23456789TQKA");
        } else {
            setTypeScore(cards);
            setCardScore(cards, "23456789TJQKA");
        }
        
    }

    private Dictionary<char, int> countCards(string handCards) {
        Dictionary<char, int> cardCount = new Dictionary<char, int>();
        foreach (char card in handCards) {
            if (cardCount.ContainsKey(card)) {
                cardCount[card]++;
            } else {
                cardCount.Add(card, 1);
            }
        }

        return cardCount;
    }
    private string transformJokerHand(string handCards) {
        Dictionary<char, int> cardCount = countCards(handCards);

        int jokers = cardCount['J'];
        cardCount.Remove('J');

        if (jokers < 5) {
            int most = cardCount.Values.Max();
            char jokerBecomes = cardCount.FirstOrDefault(x => x.Value == most).Key;
            cardCount[jokerBecomes] += jokers;

            string final = "";
            foreach (KeyValuePair<char, int> pair in cardCount) {
                final += string.Concat(Enumerable.Repeat(pair.Key, pair.Value));
            }

            Console.WriteLine($"Convert [{handCards}] to [{final}]");
            return final;
        } else {
            return "JJJJJ"; // cards here dont matter. Just needs to be 5 of a kind for setTypeScore()
        }

        
    }

    private void setTypeScore(string handCards) {
        Dictionary<char, int> cardCount = countCards(handCards);

        List<int> counts = cardCount.Values.ToList();
        counts.Sort();

        if (counts.Count == 1) { // 5 of a kind
            typeScore = 6;
        } else if (counts.Count == 2 && counts[1] == 4) { // 4 of a kind
            typeScore = 5;
        } else if (counts.Count == 2 && counts[1] == 3) { // Full house
            typeScore = 4;
        } else if (counts.Count == 3 && counts[2] == 3) { // 3 of a kind
            typeScore = 3;
        } else if (counts.Count == 3 && counts[2] == 2) { // Two pair
            typeScore = 2;
        } else if (counts.Count == 4 && counts[3] == 2) { // one pair
            typeScore = 1;
        } else {
            typeScore = 0;
        }
    }

    private void setCardScore(string handCards, string ranking) {
        int score = 0;
        for (int i = 0; i < handCards.Length; i++) {
            char card = handCards[i];
            // Treat the cards as digits in a base 13 number and calculate total value
            int power = (int)Math.Pow(ranking.Length, 5 - i);
            int digit = ranking.IndexOf(card);
            score += power*digit;
        }
        cardScore = score;
    }

    public int CompareTo(Hand? other) {
        if (other == null) return 1;
        if (other == this) return 0;

        if (typeScore == other.typeScore) {
            if (cardScore < other.cardScore) {
                return -1;
            }
            else if (cardScore > other.cardScore) {
                return 1;
            } else {
                return 0;
            }
        }
        else if (typeScore > other.typeScore) {
            return 1;
        } else {
            return -1;
        }
    }

    public string cards { get; private set; }
    private int typeScore;
    private int cardScore;
}
public class Day7 : BaseDay {

    public override void Part1(string[] inputLines) {
        Regex r = new Regex(@"(\w+) (\d+)");
        List<Tuple<Hand, int>> cardsAndBids = new List<Tuple<Hand, int>>();

        foreach (string line in inputLines) {
            Match m = r.Match(line);
            cardsAndBids.Add(new Tuple<Hand, int>(new Hand(m.Groups[1].Value, false), Int32.Parse(m.Groups[2].Value)));
        }

        cardsAndBids.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        long winnings = 0;
        for (int i = 0; i < cardsAndBids.Count; i++) {
            Console.WriteLine(cardsAndBids[i].Item1.cards);
            winnings += (i + 1) * cardsAndBids[i].Item2;
        }

        Console.WriteLine($"Winnings: {winnings}");
    }

    public override void Part2(string[] inputLines) {
        Regex r = new Regex(@"(\w+) (\d+)");
        List<Tuple<Hand, int>> cardsAndBids = new List<Tuple<Hand, int>>();

        foreach (string line in inputLines) {
            Match m = r.Match(line);
            cardsAndBids.Add(new Tuple<Hand, int>(new Hand(m.Groups[1].Value, true), Int32.Parse(m.Groups[2].Value)));
        }

        cardsAndBids.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        long winnings = 0;
        for (int i = 0; i < cardsAndBids.Count; i++) {
            Console.WriteLine(cardsAndBids[i].Item1.cards + " " + cardsAndBids[i].Item2);
            winnings += (i + 1) * cardsAndBids[i].Item2;
        }

        Console.WriteLine($"Winnings: {winnings}");
    }
}