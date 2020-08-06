using Newtonsoft.Json;
using Pineapple.Common;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static Pineapple.Common.Preconditions;

namespace DeckOfCards
{
    public class Deck
    {
        static Deck()
        {
            const string StandardDeck = "Standard";

            var standardSuits = new[] { Suit.Clubs, Suit.Hearts, Suit.Spades, Suit.Diamonds };

            Standard = new Deck(StandardDeck)
                .Add(FaceOrNumber.Ace, standardSuits)
                .Add(FaceOrNumber.Two, standardSuits)
                .Add(FaceOrNumber.Three, standardSuits)
                .Add(FaceOrNumber.Four, standardSuits)
                .Add(FaceOrNumber.Five, standardSuits)
                .Add(FaceOrNumber.Six, standardSuits)
                .Add(FaceOrNumber.Seven, standardSuits)
                .Add(FaceOrNumber.Eight, standardSuits)
                .Add(FaceOrNumber.Nine, standardSuits)
                .Add(FaceOrNumber.Ten, standardSuits)
                .Add(FaceOrNumber.Jack, standardSuits)
                .Add(FaceOrNumber.Queen, standardSuits)
                .Add(FaceOrNumber.King, standardSuits);
        }

        public static Deck Standard { get; }

        private IImmutableList<Card> _cards;

        public Deck()
        {
            _cards = ImmutableList<Card>.Empty;

            DeckId = ShortGuid.NewGuid();
            Enabled = true;
            CreatedBy = CreatedBy.DeckOfCards;
        }

        protected Deck(Deck other, IImmutableList<Card> cards)
        {
            CheckIsNotNull(nameof(other), other);
            CheckIsNotNull(nameof(other), cards);

            DeckId = other.DeckId;
            Name = other.Name;
            Description = other.Description;
            Enabled = other.Enabled;
            CreatedBy = other.CreatedBy;

            _cards = cards;
        }

        public Deck(string name) : this()
        {
            CheckIsNotNullOrWhitespace(nameof(name), name);
            Name = name;
        }

        public string DeckId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<Card> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                var b = ImmutableList.CreateBuilder<Card>();
                b.AddRange(value);
                _cards = b.ToImmutableList();
            }
        }

        [JsonIgnore]
        public IEnumerable<Suit> Suits
        {
            get
            {
                return _cards.Select(x => x.Suit)
                             .Where(x => x != Suit.Black &&
                                         x != Suit.Red)
                             .Distinct()
                             .ToList();
            }
        }

        public CreatedBy CreatedBy { get; set; }

        public bool Enabled { get; set; }

        public Deck Add(FaceOrNumber faceOrNumber, params Suit[] suits)
        {
            var b = ImmutableList.CreateBuilder<Card>();
            b.AddRange(Cards);

            foreach (var s in suits)
            {
                var c = new Card { FaceOrNumber = faceOrNumber, Suit = s };
                b.Add(c);
            }

            return new Deck(this, b.ToImmutable());
        }

        public Deck Add(params Card[] cards)
        {
            CheckIsNotNull(nameof(cards), cards);
            CheckIsNotLessThanOrEqualTo(nameof(cards), cards.Length, 0);

            var b = ImmutableList.CreateBuilder<Card>();
            b.AddRange(Cards);

            foreach (var card in cards)
            {
                var c = new Card { FaceOrNumber = card.FaceOrNumber, Suit = card.Suit, Image = card.Image };
                b.Add(c);
            }

            return new Deck(this, b.ToImmutable());
        }

        public Deck Remove(FaceOrNumber faceOrNumber, params Suit[] suits)
        {
            var b = ImmutableList.CreateBuilder<Card>();
            b.AddRange(Cards);

            foreach (var s in suits)
            {
                var removeThese = b.Where(x => x.FaceOrNumber == faceOrNumber &&
                                          x.Suit == s).ToList();

                foreach (var c in removeThese)
                {
                    b.Remove(c);
                }
            }

            return new Deck(this, b.ToImmutable());
        }

        public Deck Remove(FaceOrNumber faceOrNumber)
        {
            var b = ImmutableList.CreateBuilder<Card>();
            b.AddRange(Cards);

            var removeThese = b.Where(x => x.FaceOrNumber == faceOrNumber).ToList();

            foreach (var c in removeThese)
            {
                b.Remove(c);
            }

            return new Deck(this, b.ToImmutable());
        }

        public Deck Remove(params Card[] cards)
        {
            var b = ImmutableList.CreateBuilder<Card>();
            b.AddRange(Cards);

            foreach (var card in cards)
            {
                var remoteThese = b.Where(x => x.FaceOrNumber == card.FaceOrNumber &&
                                                  x.Suit == card.Suit &&
                                                  x.Image == card.Image).ToList();

                foreach (var c in remoteThese)
                {
                    b.Remove(c);
                }
            }

            return new Deck(this, b.ToImmutable());
        }

        public Deck Clear()
        {
            return new Deck(this, ImmutableList<Card>.Empty);
        }
    }
}
