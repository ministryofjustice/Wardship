using System.Configuration;
using System.Data.Entity;
using System.Reflection;
using System;
using Npgsql;

namespace Wardship.Models
{
    public class DataContext : DbContext
    {
        private const string DbNameVariable = "DB_NAME";
        private const string UsernameVariable = "RDS_USERNAME";
        private const string PasswordVariable = "RDS_PASSWORD";
        private const string HostnameVariable = "RDS_HOSTNAME";
        private const string PortVariable = "RDS_PORT";
        private const string DataContextNameSetting = "DataContextName";
        private const string ReadOnlyFlagField = "_bReadOnly";
        public static string GetRDSConnectionString()
        {
            var appConfig = ConfigurationManager.AppSettings;
            string dbname = Environment.GetEnvironmentVariable(DbNameVariable);
            if (!string.IsNullOrEmpty(dbname))
            {
                var settings = ConfigurationManager.ConnectionStrings[1];
                var fi = typeof(ConfigurationElement).GetField(
                              ReadOnlyFlagField,
                              BindingFlags.Instance | BindingFlags.NonPublic);
                fi.SetValue(settings, false);
                settings.ConnectionString = BuildRDSConnectionString(dbname);
            }
            return appConfig[DataContextNameSetting];
        }

        /// <summary>
        /// Builds the RDS connection string from the environment variables supplied by the task definition.
        /// TLS is requested explicitly because RDS for PostgreSQL 15 and later default rds.force_ssl to 1.
        /// </summary>
        private static string BuildRDSConnectionString(string dbname)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = Environment.GetEnvironmentVariable(HostnameVariable),
                Database = dbname,
                Username = Environment.GetEnvironmentVariable(UsernameVariable),
                Password = Environment.GetEnvironmentVariable(PasswordVariable),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            int port;
            if (int.TryParse(Environment.GetEnvironmentVariable(PortVariable), out port))
            {
                builder.Port = port;
            }

            return builder.ConnectionString;
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
        /// List of Templates in DOTX format
        /// </summary>
        public DbSet<WordTemplate> WordTemplate { get; set; }

        //ToDo Replace temp objects
        public DbSet<DataUpload> DataUploads { get; set; }
        public DbSet<WardshipRecord> WardshipRecord { get; set; }
        // Application Specifc Models
        public DbSet<CaseType> CaseTypes { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Wardship.Models.Type> Types { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<DistrictJudge> DistrictJudges { get; set; }
        public DbSet<Lapsed> Lapsed { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<CWO> CWOs { get; set; }
        public DbSet<CAFCASSInvolved> CAFCASSInvolveds { get; set; }
        public DbSet<CAFCASS> CAFCASSes { get; set; }
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