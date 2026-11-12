using System.Configuration;
using System.Data.Entity;
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

        /// <summary>
        /// The connection string is resolved once per process: the environment does not change after the container starts.
        /// </summary>
        private static readonly Lazy<string> ConnectionString = new Lazy<string>(ResolveConnectionString);

        /// <summary>
        /// Uses the RDS environment variables when DB_NAME is set (AWS), otherwise the Web.config connection string (local development).
        /// </summary>
        private static string ResolveConnectionString()
        {
            string dbname = Environment.GetEnvironmentVariable(DbNameVariable);
            if (!string.IsNullOrEmpty(dbname))
            {
                return BuildRDSConnectionString(dbname);
            }

            string name = ConfigurationManager.AppSettings[DataContextNameSetting];
            var settings = string.IsNullOrEmpty(name) ? null : ConfigurationManager.ConnectionStrings[name];
            if (settings == null)
            {
                throw new ConfigurationErrorsException(
                    $"{DbNameVariable} is not set and no connection string named '{name}' (appSetting {DataContextNameSetting}) was found in Web.config.");
            }
            return settings.ConnectionString;
        }

        /// <summary>
        /// Builds the RDS connection string from the environment variables supplied by the task definition.
        /// TLS is requested explicitly because RDS for PostgreSQL 15 and later default rds.force_ssl to 1.
        /// </summary>
        private static string BuildRDSConnectionString(string dbname)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = GetRequiredVariable(HostnameVariable),
                Database = dbname,
                Username = GetRequiredVariable(UsernameVariable),
                Password = GetRequiredVariable(PasswordVariable),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            string portValue = Environment.GetEnvironmentVariable(PortVariable);
            if (!string.IsNullOrWhiteSpace(portValue))
            {
                int port;
                if (!int.TryParse(portValue.Trim(), out port) || port < 1 || port > 65535)
                {
                    throw new ConfigurationErrorsException($"Environment variable {PortVariable} is not a valid port number: '{portValue}'.");
                }
                builder.Port = port;
            }

            return builder.ConnectionString;
        }

        private static string GetRequiredVariable(string variable)
        {
            string value = Environment.GetEnvironmentVariable(variable);
            if (string.IsNullOrEmpty(value))
            {
                throw new ConfigurationErrorsException($"Environment variable {variable} is required when {DbNameVariable} is set.");
            }
            return value;
        }

        /// <summary>
        /// Passes EF a ready-made Npgsql connection rather than a connection string name, so Web.config never needs modifying at runtime.
        /// </summary>
        public DataContext()
           : base(new NpgsqlConnection(ConnectionString.Value), contextOwnsConnection: true)
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