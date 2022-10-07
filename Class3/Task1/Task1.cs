// Колода

using System.Net.Sockets;
using Deck = System.Collections.Generic.List<Card>;
// Набор карт у игрока
using Hand = System.Collections.Generic.List<Card>;
// Набор карт, выложенных на стол
using Table = System.Collections.Generic.List<Card>;

// Масть
internal enum Suit
{
    Hearts,
    Spades,
    Diamonds,
    Clubs
}

// Значение
internal enum Rank
{
    Six = 6,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

// Карта
record Card
{
    public Rank CardRank;
    public Suit CardSuit;

    public Card(Rank r, Suit s)
    {
        CardRank = r;
        CardSuit = s;
    }
};

// Тип для обозначения игрока (первый, второй)
internal enum Player
{
    FirstPlayer,
    SecondPlayer,
    Draw
}

namespace Task1
{
    public class Task1
    {
        /*
        * Реализуйте игру "Пьяница" (в простейшем варианте, на колоде в 36 карт)
        * https://ru.wikipedia.org/wiki/%D0%9F%D1%8C%D1%8F%D0%BD%D0%B8%D1%86%D0%B0_(%D0%BA%D0%B0%D1%80%D1%82%D0%BE%D1%87%D0%BD%D0%B0%D1%8F_%D0%B8%D0%B3%D1%80%D0%B0)
        * Рука — это набор карт игрока. Карты выкладываются на стол из начала "рук" и сравниваются
        * только по значениям (масть игнорируется). При равных значениях сравниваются следующие карты.
        * Набор карт со стола перекладывается в конец руки победителя. Шестерка туза не бьёт.
        *
        * Реализация должна сопровождаться тестами.
        */

        // Размер колоды
        internal const int DeckSize = 36;

        // Возвращается null, если значения карт совпадают
        internal static Player? RoundWinner(Card card1, Card card2)
        {
            if (card1.CardRank > card2.CardRank)
            {
                return Player.FirstPlayer;
            }

            if (card1.CardRank == card2.CardRank)
            {
                return null;
            }

            return Player.SecondPlayer;
        }

// Возвращает полную колоду (36 карт) в фиксированном порядке
        internal static Deck FullDeck()
        {
            Deck deck = new Deck();
            foreach (Rank rankValue in Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit suitValue in Enum.GetValues(typeof(Suit)))
                {
                    deck.Add(new Card(rankValue, suitValue));
                }
            }

            return deck;
        }

// Раздача карт: случайное перемешивание (shuffle) и деление колоды пополам
        internal static Dictionary<Player, Hand> Deal(Deck deck)
        {
            int n = deck.Count;
            Random rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (deck[k], deck[n]) = (deck[n], deck[k]);
            }

            Dictionary<Player, Hand> dict = new Dictionary<Player, Hand> { };
            dict[Player.FirstPlayer] = deck.GetRange(0, deck.Count / 2);
            dict[Player.SecondPlayer] = deck.GetRange(deck.Count / 2, deck.Count / 2);
            return dict;
        }

// Один раунд игры (в том числе спор при равных картах).
// Возвращается победитель раунда и набор карт, выложенных на стол.
        internal static Tuple<Player, Table> Round(Dictionary<Player, Hand> hands)
        {
            var firstPlayer = Player.FirstPlayer;
            var secondPlayer = Player.SecondPlayer;
            Table table = new Table();
            while (hands[firstPlayer].Count != 0 && hands[secondPlayer].Count != 0)
            {
                Card firstPlayerCard = hands[firstPlayer][0];
                Card secondPlayerCard = hands[secondPlayer][0];
                var result = RoundWinner(firstPlayerCard, secondPlayerCard);
                table.Add(firstPlayerCard);
                table.Add(secondPlayerCard);
                hands[firstPlayer].RemoveAt(0);
                hands[secondPlayer].RemoveAt(0);
                if (result != null)
                {
                    Player player = (result == Player.FirstPlayer) ? Player.FirstPlayer : Player.SecondPlayer;
                    return new Tuple<Player, Table>(player, table);
                }
            }

            if (hands[firstPlayer].Count == 0 && hands[secondPlayer].Count == 0)
            {
                return new Tuple<Player, Table>(Player.Draw, table);
            }

            Player winner = (hands[firstPlayer].Count != 0) ? Player.FirstPlayer : Player.SecondPlayer;
            return new Tuple<Player, Deck>(winner, table);
        }

// Полный цикл игры (возвращается победивший игрок)
// в процессе игры печатаются ходы
        internal static Player Game(Dictionary<Player, Hand> hands)
        {
            Player winner = Player.Draw;
            while (hands[Player.FirstPlayer].Count != 0 && hands[Player.SecondPlayer].Count != 0)
            {
                var roundResult = Round(hands);
                winner = roundResult.Item1;
                if (winner == Player.Draw)
                {
                    return Player.Draw;
                }
                var rnd = new Random();
                for (int i = 0; i < roundResult.Item2.Count; i+=2)
                {
                    if (rnd.Next() % 2 == 0)
                    {
                        (roundResult.Item2[i], roundResult.Item2[i + 1]) = (roundResult.Item2[i + 1], roundResult.Item2[i]);
                    }
                }
                hands[winner].AddRange(roundResult.Item2);
                int tableSize = roundResult.Item2.Count;
                for (int i = 0; i < tableSize; i += 2)
                {
                    Console.WriteLine(
                        $"{roundResult.Item2[i].CardRank} {roundResult.Item2[i].CardSuit} vs {roundResult.Item2[i + 1].CardRank} {roundResult.Item2[i + 1].CardSuit}");
                }

                Console.WriteLine($"Winner of round: {winner}");
                Console.WriteLine(
                    $"Cards in hand: {hands[Player.FirstPlayer].Count} vs {hands[Player.SecondPlayer].Count}");
                Console.WriteLine("-------------");
            }

            return winner;
        }

        public static void Main(string[] args)
        {
            var deck = FullDeck();
            var hands = Deal(deck);
            var winner = Game(hands);
            Console.WriteLine($"Winner: {winner}");
        }
    }
}