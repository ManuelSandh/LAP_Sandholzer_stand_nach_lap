﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CardGame.DAL.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ClonestoneFSEntities : DbContext
    {
        public ClonestoneFSEntities()
            : base("name=ClonestoneFSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblcard> tblcard { get; set; }
        public virtual DbSet<tblclass> tblclass { get; set; }
        public virtual DbSet<tblcollection> tblcollection { get; set; }
        public virtual DbSet<tbldeck> tbldeck { get; set; }
        public virtual DbSet<tbldeckcard> tbldeckcard { get; set; }
        public virtual DbSet<tblorder> tblorder { get; set; }
        public virtual DbSet<tblpack> tblpack { get; set; }
        public virtual DbSet<tblperson> tblperson { get; set; }
        public virtual DbSet<tbltype> tbltype { get; set; }
    }
}