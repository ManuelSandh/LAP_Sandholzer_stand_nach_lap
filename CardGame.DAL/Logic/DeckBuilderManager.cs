using CardGame.DAL.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CardGame.DAL.Logic
{
    public class DeckBuilderManager
    {
        public static List<Deck> GetDecks(string username)
        {
            List<Deck> userDecks = null;

            try
            {
                using (var context = new CardGame_v2Entities())
                {
                    User currentUser = context.AllUsers.FirstOrDefault(x => x.Mail == username);

                    if (currentUser != null)
                        userDecks = currentUser.AllDecks.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            return userDecks;
        }

        public static List<Card> GetCardsForDeck(int idDeck)
        {
            List<Card> deckCards = null;

            try
            {
                using (var context = new CardGame_v2Entities())
                {
                    Deck deck = context.AllDecks.FirstOrDefault(x => x.ID == idDeck);

                    if (deck != null)
                    {
                        User currentUser = deck.User;
                        /// ermittle alle userCardCollections für diesen User
                        List<UserCardCollection> userCardCollections = currentUser.AllUserCardCollections.ToList();

                        foreach (var ucc in userCardCollections)
                        {
                            /// Schau nach ob diese karte auch bereits in diesem Deck ist
                            DeckCard deckCard = context.AllDeckCards.FirstOrDefault(x => x.ID_Card == ucc.ID_Card && x.ID_Deck == idDeck);

                            /// wenn diese karte bereits im gewünschten Deck verwendet wird
                            if (deckCard != null)
                                ucc.NumberOfCards -= deckCard.NumberOfCards; // dann verringere die Anzahl
                        }

                        /// gib nur die Karten zurück die man noch verwenden kann
                        deckCards = userCardCollections.Where(x => x.NumberOfCards > 0).Select(x => x.Card).ToList();

                        /// lade nachträglich alle zugeordneten CardType Objekte
                        /// alternativ: bereits beim Zugriff via context auf Datenbank
                        /// mit Include alle notwendigen Elemente raus laden
                        deckCards.ForEach(x => context.Entry(x).Reference("CardType").Query().Load());
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            return deckCards;
        }

        public static bool AddCardToDeck(int idDeck, int idCard)
        {
            bool success = false;

            try
            {
                using (var context = new CardGame_v2Entities())
                {
                    Deck deck = context.AllDecks.FirstOrDefault(x => x.ID == idDeck);

                    if (deck != null)
                    {
                        DeckCard deckCard = deck.AllDeckCards.FirstOrDefault(x => x.ID_Card == idCard);

                        if (deckCard != null)
                        {
                            deckCard.NumberOfCards++;
                        }
                        else
                        {
                            deckCard = new DeckCard()
                            {
                                ID_Card = idCard,
                                ID_Deck = idDeck,
                                NumberOfCards = 1
                            };
                            context.AllDeckCards.Add(deckCard);
                        }

                        context.SaveChanges();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
            return success;
        }

        public static List<Card> GetCardsInDeck(int idDeck)
        {
            List<Card> cardsInDeck = null;

            try
            {
                using (var context = new CardGame_v2Entities())
                {
                    Deck deck = context.AllDecks
                        .Include(x => x.AllDeckCards)
                        .Include(x => x.AllDeckCards.Select(y => y.Card))
                        .Include(x => x.AllDeckCards.Select(y => y.Card.CardType))
                        .FirstOrDefault(x => x.ID == idDeck);

                    if (deck != null)
                    {
                        cardsInDeck = deck.AllDeckCards.Select(x => x.Card).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            return cardsInDeck;
        }

        public static bool RemoveCardFromDeck(int idDeck, int idCard)
        {
            bool success = false;

            try
            {
                using (var context = new CardGame_v2Entities())
                {
                    Deck deck = context.AllDecks.FirstOrDefault(x => x.ID == idDeck);

                    if (deck != null)
                    {
                        DeckCard deckCard = deck.AllDeckCards.FirstOrDefault(x => x.ID_Card == idCard);

                        if (deckCard != null)
                        {
                            deckCard.NumberOfCards--;

                            if (deckCard.NumberOfCards == 0)
                            {
                                deck.AllDeckCards.Remove(deckCard);
                                deckCard.Card.AllDeckCards.Remove(deckCard); 
                                context.AllDeckCards.Remove(deckCard);
                            }
                        }

                        context.SaveChanges();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
            return success;
        }

        public static Deck GetDeck(int idDeck)
        {
            Deck deck = null;

            try
            {
                using (var context = new CardGame_v2Entities())
                {
                    deck = context.AllDecks.FirstOrDefault(x => x.ID == idDeck);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            return deck;
        }
    }
}
