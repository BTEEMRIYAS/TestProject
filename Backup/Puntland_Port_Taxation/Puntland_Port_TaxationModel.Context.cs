﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Puntland_Port_Taxation
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class Puntland_Port_Taxation_DBEntities : DbContext
    {
        public Puntland_Port_Taxation_DBEntities()
            : base("name=Puntland_Port_Taxation_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Financial_Period> Financial_Period { get; set; }
        public DbSet<Financial_Quarter> Financial_Quarter { get; set; }
        public DbSet<Financial_Year> Financial_Year { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Function_Role_Map> Function_Role_Map { get; set; }
        public DbSet<Geography> Geographies { get; set; }
        public DbSet<Goods_Category> Goods_Category { get; set; }
        public DbSet<Goods_Subcategory> Goods_Subcategory { get; set; }
        public DbSet<Goods_Type> Goods_Type { get; set; }
        public DbSet<Importer> Importers { get; set; }
        public DbSet<Importer_Type> Importer_Type { get; set; }
        public DbSet<Importing_Status> Importing_Status { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<Quarter> Quarters { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Ship_Arrival> Ship_Arrival { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Levi_Type> Levi_Type { get; set; }
        public DbSet<Goods_Heirarchy> Goods_Heirarchy { get; set; }
        public DbSet<Calculated_Levi> Calculated_Levi { get; set; }
        public DbSet<EU_Check> EU_Check { get; set; }
        public DbSet<Exchange_Rate> Exchange_Rate { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<Goods_Tariff> Goods_Tariff { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<Levi> Levis { get; set; }
        public DbSet<Levi_Entry> Levi_Entry { get; set; }
        public DbSet<Reject_Reason> Reject_Reason { get; set; }
        public DbSet<Tally_Sheet> Tally_Sheet { get; set; }
        public DbSet<Tally_Sheet_Details> Tally_Sheet_Details { get; set; }
        public DbSet<Unit_Of_Measure> Unit_Of_Measure { get; set; }
        public DbSet<Way_Bill> Way_Bill { get; set; }
        public DbSet<Way_Bill_Details> Way_Bill_Details { get; set; }
    
        public virtual int Calculate_Tax(Nullable<int> way_bill_id)
        {
            var way_bill_idParameter = way_bill_id.HasValue ?
                new ObjectParameter("way_bill_id", way_bill_id) :
                new ObjectParameter("way_bill_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Calculate_Tax", way_bill_idParameter);
        }
    
        public virtual ObjectResult<Display_Levi_Entries_View_Result> Display_Levi_Entries_View(Nullable<int> levi_entry_id)
        {
            var levi_entry_idParameter = levi_entry_id.HasValue ?
                new ObjectParameter("levi_entry_id", levi_entry_id) :
                new ObjectParameter("levi_entry_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Display_Levi_Entries_View_Result>("Display_Levi_Entries_View", levi_entry_idParameter);
        }
    
        public virtual ObjectResult<Display_Payment_Result> Display_Payment(Nullable<int> way_bill_id)
        {
            var way_bill_idParameter = way_bill_id.HasValue ?
                new ObjectParameter("way_bill_id", way_bill_id) :
                new ObjectParameter("way_bill_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Display_Payment_Result>("Display_Payment", way_bill_idParameter);
        }
    
        public virtual ObjectResult<Display_Release_Result> Display_Release(Nullable<int> way_bill_id)
        {
            var way_bill_idParameter = way_bill_id.HasValue ?
                new ObjectParameter("way_bill_id", way_bill_id) :
                new ObjectParameter("way_bill_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Display_Release_Result>("Display_Release", way_bill_idParameter);
        }
    
        public virtual ObjectResult<Display_Tax_Details_Result> Display_Tax_Details(Nullable<int> way_bill_id)
        {
            var way_bill_idParameter = way_bill_id.HasValue ?
                new ObjectParameter("way_bill_id", way_bill_id) :
                new ObjectParameter("way_bill_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Display_Tax_Details_Result>("Display_Tax_Details", way_bill_idParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> Get_Grand_Total(Nullable<int> way_bill_id)
        {
            var way_bill_idParameter = way_bill_id.HasValue ?
                new ObjectParameter("way_bill_id", way_bill_id) :
                new ObjectParameter("way_bill_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("Get_Grand_Total", way_bill_idParameter);
        }
    
        public virtual int Reject_Calculated_Tax(Nullable<int> way_bill_id, Nullable<int> importing_Status, string reason, string reject_From)
        {
            var way_bill_idParameter = way_bill_id.HasValue ?
                new ObjectParameter("way_bill_id", way_bill_id) :
                new ObjectParameter("way_bill_id", typeof(int));
    
            var importing_StatusParameter = importing_Status.HasValue ?
                new ObjectParameter("Importing_Status", importing_Status) :
                new ObjectParameter("Importing_Status", typeof(int));
    
            var reasonParameter = reason != null ?
                new ObjectParameter("Reason", reason) :
                new ObjectParameter("Reason", typeof(string));
    
            var reject_FromParameter = reject_From != null ?
                new ObjectParameter("Reject_From", reject_From) :
                new ObjectParameter("Reject_From", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Reject_Calculated_Tax", way_bill_idParameter, importing_StatusParameter, reasonParameter, reject_FromParameter);
        }
    
        public virtual int Update_Levi()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Update_Levi");
        }
    
        public virtual int Update_Levi_Entry()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Update_Levi_Entry");
        }
    
        public virtual int Update_Tally_Sheet_Details(Nullable<int> tally_sheet_id)
        {
            var tally_sheet_idParameter = tally_sheet_id.HasValue ?
                new ObjectParameter("tally_sheet_id", tally_sheet_id) :
                new ObjectParameter("tally_sheet_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Update_Tally_Sheet_Details", tally_sheet_idParameter);
        }
    
        public virtual int Update_Way_Bill_table(Nullable<int> goods_id, Nullable<int> currency_id, Nullable<decimal> unit_price, Nullable<int> way_bill_id)
        {
            var goods_idParameter = goods_id.HasValue ?
                new ObjectParameter("goods_id", goods_id) :
                new ObjectParameter("goods_id", typeof(int));
    
            var currency_idParameter = currency_id.HasValue ?
                new ObjectParameter("currency_id", currency_id) :
                new ObjectParameter("currency_id", typeof(int));
    
            var unit_priceParameter = unit_price.HasValue ?
                new ObjectParameter("unit_price", unit_price) :
                new ObjectParameter("unit_price", typeof(decimal));
    
            var way_bill_idParameter = way_bill_id.HasValue ?
                new ObjectParameter("way_bill_id", way_bill_id) :
                new ObjectParameter("way_bill_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Update_Way_Bill_table", goods_idParameter, currency_idParameter, unit_priceParameter, way_bill_idParameter);
        }
    }
}
