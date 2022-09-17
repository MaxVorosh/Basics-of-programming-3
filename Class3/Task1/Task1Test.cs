using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    [Test]
    public void RoundWinnerTest()
    {
        That(RoundWinner(new Card(Rank.Ten, Suit.Clubs), new Card(Rank.Seven, Suit.Clubs)),
            Is.EqualTo(Player.FirstPlayer));
        That(RoundWinner(new Card(Rank.Ace, Suit.Clubs), new Card(Rank.Jack, Suit.Diamonds)),
            Is.EqualTo(Player.FirstPlayer));
        That(RoundWinner(new Card(Rank.Jack, Suit.Diamonds), new Card(Rank.Nine, Suit.Hearts)),
            Is.EqualTo(Player.FirstPlayer));
        That(RoundWinner(new Card(Rank.Queen, Suit.Clubs), new Card(Rank.Ace, Suit.Clubs)),
            Is.EqualTo(Player.SecondPlayer));
        That(RoundWinner(new Card(Rank.Six, Suit.Diamonds), new Card(Rank.Seven, Suit.Hearts)),
            Is.EqualTo(Player.SecondPlayer));
        That(RoundWinner(new Card(Rank.Six, Suit.Hearts), new Card(Rank.Ace, Suit.Spades)),
            Is.EqualTo(Player.SecondPlayer));
        That(RoundWinner(new Card(Rank.Eight, Suit.Clubs), new Card(Rank.Eight, Suit.Hearts)),
            Is.EqualTo(null));
        That(RoundWinner(new Card(Rank.Jack, Suit.Spades), new Card(Rank.Jack, Suit.Spades)),
            Is.EqualTo(null));
    }

    [Test]
    public void FullDeckTest()
    {
        var deck = FullDeck();
        That(deck, Has.Count.EqualTo(DeckSize));
        That(deck.Contains(new Card(Rank.Ace, Suit.Clubs)), Is.EqualTo(true));
        That(deck.Contains(new Card(Rank.Six, Suit.Diamonds)), Is.EqualTo(true));
        That(deck.Contains(new Card(Rank.King, Suit.Hearts)), Is.EqualTo(true));
        foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                That(deck.Contains(new Card(rank, suit)), Is.EqualTo(true));
            }
        }
    }

    [Test]
    public void RoundTest()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Game2CardsTest()
    {
        var six = TODO<Card>();
        var ace = TODO<Card>();
        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { TODO<Player>(), new List<Card> {six} },
            { TODO<Player>(), new List<Card> {ace} }
        };
        var gameWinner = Game(hands);
        That(gameWinner, Is.EqualTo(TODO<Player>()));
    }
    
    private static T TODO<T>()
    {
        throw new NotImplementedException();
    }
}