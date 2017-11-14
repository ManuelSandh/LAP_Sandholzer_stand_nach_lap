﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CardGame.DAL.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class CardGame_v2Entities : DbContext
    {
        public CardGame_v2Entities()
            : base("name=CardGame_v2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Card> AllCards { get; set; }
        public virtual DbSet<CardPack> AllCardPacks { get; set; }
        public virtual DbSet<CardType> AllCardTypes { get; set; }
        public virtual DbSet<Deck> AllDecks { get; set; }
        public virtual DbSet<DeckCard> AllDeckCards { get; set; }
        public virtual DbSet<User> AllUsers { get; set; }
        public virtual DbSet<UserCardCollection> AllUserCardCollections { get; set; }
        public virtual DbSet<UserRole> AllUserRoles { get; set; }
        public virtual DbSet<DiamantenPack> AllDiamantenPacks { get; set; }
        public virtual DbSet<UserRanking> UserRanking { get; set; }
        public virtual DbSet<VirtualPurchase> AllVirtualPurchases { get; set; }
    
        public virtual ObjectResult<Nullable<int>> ptopdreiPacks()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ptopdreiPacks");
        }
    }
}
