﻿using System.Configuration;
using System.Data.Entity;
using System.Reflection;

namespace Wardship.Models
{
    public class DataContext : DbContext
    {
        public static string GetRDSConnectionString()
        {
            var appConfig = ConfigurationManager.AppSettings;

            string dbname = appConfig["DB_NAME"];
            if (!string.IsNullOrEmpty(dbname))
            {
                string username = appConfig["RDS_USERNAME"];
                string password = appConfig["RDS_PASSWORD"];
                string hostname = appConfig["RDS_HOSTNAME"];
                string port = appConfig["RDS_PORT"];
                var settings = ConfigurationManager.ConnectionStrings[1];
                var fi = typeof(ConfigurationElement).GetField(
                              "_bReadOnly",
                              BindingFlags.Instance | BindingFlags.NonPublic);
                fi.SetValue(settings, false);
                settings.ConnectionString = "Server=" + hostname + ";port=" + port + ";database=" + dbname + ";user id=" + username + ";password=" + password;
            }
            return appConfig["DataContextName"];
        }

        public DataContext()
            : base(GetRDSConnectionString())
        { }

        //SSG Standard Models
        /// <summary>
        /// Frequently Asked Questions
        /// </summary>
        public DbSet<FAQ> FAQs { get; set; }
        /// <summary>
        /// Single event record
        /// </summary>
        public DbSet<AuditEvent> AuditEvents { get; set; }
        /// <summary>
        /// Field level changes collection from an AuditEvent
        /// </summary>
        public DbSet<AuditEventDataRow> AuditEventRows { get; set; }
        /// <summary>
        /// Description field for Audit Event
        /// </summary>
        public DbSet<AuditEventDescription> AuditDescriptions { get; set; }
        /// <summary>
        /// Controls access to the system
        /// </summary>
        public DbSet<Role> Roles { get; set; }
        /// <summary>
        /// List of defined users
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// List of Active Directory Groups
        /// </summary>
        public DbSet<ADGroup> ADGroups { get; set; }
        /// <summary>
        /// List of alerts to display to users
        /// </summary>
        public DbSet<Alert> Alerts { get; set; }

        /// <summary>
        /// List of Templates in XML format
        /// </summary>
        public DbSet<WordTemplate> WordTemplate { get; set; }

        //ToDo Replace temp objects
        public DbSet<DataUpload> DataUploads { get; set; }
         public DbSet<WardshipRecord> WardshipRecord { get; set; }
        // Application Specifc Models
        public DbSet<CaseType> CaseType { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<DistrictJudge> DistrictJudges { get; set; }
        public DbSet<Lapsed> Lapsed { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<CWO> CWO { get; set; }
        public DbSet<CAFCASS> CAFCASS { get; set; }
        public DbSet<Record> Records { get; set; }
      

        /// <summary>
        /// Saves changes made to the database context - can be overridden to provide audit function
        /// </summary>
        

        public int SaveChanges(int userID)
        {
            //TODO implement new audit on save
            return base.SaveChanges();
        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
    internal static class myDataContextHelper
    {
        public static DataContext CurrentContext
        {
            get
            {
                if (ServiceLayer.UnitOfWorkHelper.CurrentDataStore["myDBContext"] == null)
                {
                    ServiceLayer.UnitOfWorkHelper.CurrentDataStore["myDBContext"] = new DataContext();
                }
                return (DataContext)ServiceLayer.UnitOfWorkHelper.CurrentDataStore["myDBContext"];
            }
        }
    }

}