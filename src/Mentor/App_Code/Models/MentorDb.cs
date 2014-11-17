using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Mentor
{
    public class MentorDb : DbContext
    {
        public MentorDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MentorDb, MentorDbMigrationConfiguration>());
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ValidateOnSaveEnabled = true;
        }

        //REF: http://msdn.microsoft.com/en-us/data/jj819164.aspx
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().Configure(c => c.ToTable(c.ClrType.Name));
            //modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Properties<string>().Configure(c => c.IsUnicode(false));
        }

        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencyCode> AgencyCodes { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<User> Users { get; set; }

        public T Save<T>(T entity, bool isNew = false) where T : class
        {
            if (isNew)
                return Add(entity);

            Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public T Add<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Added;
            return entity;
        }

        public void Delete<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public override int SaveChanges()
        {
            //Remove orphaned AgencyCodes
            var orphanedAgencyCodes = ChangeTracker.Entries().Where(e => (e.State == EntityState.Modified || e.State == EntityState.Added) && e.Entity is AgencyCode && e.Reference("Agency").CurrentValue == null);
            foreach (var orphanedAgencyCode in orphanedAgencyCodes)
            {
                AgencyCodes.Remove((AgencyCode)orphanedAgencyCode.Entity);
            }

            return base.SaveChanges();
        }
    };

    internal sealed class MentorDbMigrationConfiguration : DbMigrationsConfiguration<MentorDb>
    {
        public MentorDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MentorDb context)
        {
            context.Codes.AddOrUpdate(x => x.Id,
                new []
                {
                    new Code { Id=1, Type="Agency.ProgramType", Value="OneOnOne", Label="One-on-one mentoring (one mentor with one youth)", Seq=1 },
                    new Code { Id=2, Type="Agency.ProgramType", Value="Group", Label="Group mentoring (one mentor with more than one youth)", Seq=2 },
                    new Code { Id=3, Type="Agency.ProgramType", Value="Team", Label="Team mentoring (multiple mentors teamed with one youth)", Seq=3 },
                    new Code { Id=4, Type="Agency.ProgramType", Value="MultipleGroups", Label="Multiple groups mentoring (more than one mentor with more than one youth)", Seq=4 },
                    new Code { Id=5, Type="Agency.ProgramType", Value="E-mentoring", Label="E-mentoring (primarily online)", Seq=5 },
                    new Code { Id=6, Type="Agency.MeetingLocation", Value="Community", Label="In the community", Seq=1 },
                    new Code { Id=7, Type="Agency.MeetingLocation", Value="School", Label="At a school", Seq=2 },
                    new Code { Id=8, Type="Agency.MeetingLocation", Value="Organization", Label="At a community-based organization", Seq=3 },
                    new Code { Id=9, Type="Agency.MeetingLocation", Value="FaithBased", Label="At a faith-based location", Seq=4 },
                    new Code { Id=10, Type="Agency.MeetingLocation", Value="Business", Label="At a business", Seq=5 },
                    new Code { Id=11, Type="Agency.MeetingLocation", Value="JuvenileJustice", Label="In a juvenile justice facility", Seq=6 },
                    new Code { Id=12, Type="Agency.MeetingLocation", Value="Online", Label="Online", Seq=7 },
                    new Code { Id=13, Type="Agency.MeetingLocation", Value="Other", Label="Other (please describe)", Seq=8 },
                    new Code { Id=14, Type="Agency.MetroSection", Value="CenterCity", Label="Center City", Seq=1 },
                    new Code { Id=15, Type="Agency.MetroSection", Value="Northeast", Label="Northeast", Seq=2 },
                    new Code { Id=16, Type="Agency.MetroSection", Value="Northwest", Label="Northwest", Seq=3 },
                    new Code { Id=17, Type="Agency.MetroSection", Value="Southeast", Label="Southeast", Seq=4 },
                    new Code { Id=18, Type="Agency.MetroSection", Value="Southwest", Label="Southwest", Seq=5 },
                    new Code { Id=19, Type="Agency.MentorAge", Value="Adults", Label="Adults", Seq=1 },
                    new Code { Id=20, Type="Agency.MentorAge", Value="Youth", Label="Youth under 18", Seq=2 },
                    new Code { Id=21, Type="Agency.MenteeAge", Value="Elementary", Label="Elementary school (approx. 5-12 years old)", Seq=1 },
                    new Code { Id=22, Type="Agency.MenteeAge", Value="MiddleSchool", Label="Middle school (approx. 12-15 years old)", Seq=2 },
                    new Code { Id=23, Type="Agency.MenteeAge", Value="HighSchool", Label="High school (approx. 15-18 years old)", Seq=3 },
                    new Code { Id=24, Type="Agency.MenteeGender", Value="Male", Label="Male", Seq=1 },
                    new Code { Id=25, Type="Agency.MenteeGender", Value="Female", Label="Female", Seq=2 },
                    new Code { Id=26, Type="Agency.ReferralMethod", Value="Self", Label="Self-referral", Seq=1 },
                    new Code { Id=27, Type="Agency.ReferralMethod", Value="Parent", Label="Parent/guardian", Seq=2 },
                    new Code { Id=28, Type="Agency.ReferralMethod", Value="SocialWorker", Label="Social Worker", Seq=3 },
                    new Code { Id=29, Type="Agency.ReferralMethod", Value="Teacher", Label="Teacher", Seq=4 },
                    new Code { Id=30, Type="Agency.ReferralMethod", Value="OtherProfessional", Label="Other professional", Seq=5 },
                    new Code { Id=31, Type="Agency.ReferralMethod", Value="CourtOrdered", Label="Court-ordered", Seq=6 },
                    new Code { Id=32, Type="Agency.ReferralMethod", Value="Other", Label="Other (please describe)", Seq=7 },
                    new Code { Id=33, Type="Agency.ExpectedCommitment", Value="None", Label="No expected length (experience varies for each participant)", Seq=1 },
                    new Code { Id=34, Type="Agency.ExpectedCommitment", Value="<3Months", Label="Less than 3 months", Seq=2 },
                    new Code { Id=35, Type="Agency.ExpectedCommitment", Value="3-6Months", Label="3 - 6 Months", Seq=3 },
                    new Code { Id=36, Type="Agency.ExpectedCommitment", Value="7-9Months", Label="7 - 9 Months", Seq=4 },
                    new Code { Id=37, Type="Agency.ExpectedCommitment", Value="10-12Months", Label="10 - 12 Months", Seq=5 },
                    new Code { Id=38, Type="Agency.ExpectedCommitment", Value="1Year", Label="1 Year", Seq=6 },
                    new Code { Id=39, Type="Agency.ExpectedCommitment", Value="1-2Years", Label="Between 1 and 2 years", Seq=7 },
                    new Code { Id=40, Type="Agency.ExpectedCommitment", Value=">2Years", Label="Over 2 years", Seq=8 },
                    new Code { Id=41, Type="Agency.MentoringFrequency", Value="Weekly", Label="Weekly", Seq=1 },
                    new Code { Id=42, Type="Agency.MentoringFrequency", Value="Bi-weekly", Label="Bi-weekly", Seq=2 },
                    new Code { Id=43, Type="Agency.MentoringFrequency", Value="Monthly", Label="Monthly", Seq=3 },
                    new Code { Id=44, Type="Agency.SessionLength", Value="30Minutes", Label="30 minutes", Seq=1 },
                    new Code { Id=45, Type="Agency.SessionLength", Value="1Hour", Label="1 hour", Seq=2 },
                    new Code { Id=46, Type="Agency.SessionLength", Value="2Hours", Label="2 hours", Seq=3 },
                    new Code { Id=47, Type="Agency.SessionLength", Value="3Hours", Label="3 hours or more", Seq=4 },
                    new Code { Id=48, Type="Agency.ProgramExistence", Value="<1Year", Label="Less than 1 year", Seq=1 },
                    new Code { Id=49, Type="Agency.ProgramExistence", Value="1-2Years", Label="Between 1 and 2 years", Seq=2 },
                    new Code { Id=50, Type="Agency.ProgramExistence", Value="3-5Years", Label="Between 3 and 5 years", Seq=3 },
                    new Code { Id=51, Type="Agency.ProgramExistence", Value="6-10Years", Label="Between 6 and 10 years", Seq=4 },
                    new Code { Id=52, Type="Agency.ProgramExistence", Value="11-15Years", Label="Between 11 and 15 years", Seq=5 },
                    new Code { Id=53, Type="Agency.ProgramExistence", Value=">15Years", Label="More than 15 years", Seq=6 },
                    new Code { Id=54, Type="Agency.MenteeIntake", Value="ApplicationForm", Label="Youth application form", Seq=1 },
                    new Code { Id=55, Type="Agency.MenteeIntake", Value="PermissionForm", Label="Parent/guardian permission form", Seq=2 },
                    new Code { Id=56, Type="Agency.MenteeIntake", Value="Interview", Label="Youth interview", Seq=3 },
                    new Code { Id=57, Type="Agency.MenteeIntake", Value="HomeVisit", Label="Home visit with mentee and parent/guardian", Seq=4 },
                    new Code { Id=58, Type="Agency.MenteeIntake", Value="MenteeOrientation", Label="Mentee orientation training", Seq=5 },
                    new Code { Id=59, Type="Agency.MenteeIntake", Value="ParentOrientation", Label="Parent/guardian orientation training", Seq=6 },
                    new Code { Id=60, Type="Agency.MenteeIntake", Value="Other", Label="Other (please describe)", Seq=7 },
                    new Code { Id=61, Type="Agency.MentorScreening", Value="ApplicationForm", Label="Volunteer application form", Seq=1 },
                    new Code { Id=62, Type="Agency.MentorScreening", Value="ReferenceChecks", Label="Personal/professional reference checks", Seq=2 },
                    new Code { Id=63, Type="Agency.MentorScreening", Value="Interview", Label="Interview", Seq=3 },
                    new Code { Id=64, Type="Agency.MentorScreening", Value="HomeVisit", Label="Home visit", Seq=4 },
                    new Code { Id=65, Type="Agency.MentorScreening", Value="LocalBackground", Label="Local criminal background check", Seq=5 },
                    new Code { Id=66, Type="Agency.MentorScreening", Value="StateBackground", Label="State criminal background check", Seq=6 },
                    new Code { Id=67, Type="Agency.MentorScreening", Value="FederalBackground", Label="Federal criminal background check", Seq=7 },
                    new Code { Id=68, Type="Agency.MentorScreening", Value="DrivingRecord", Label="Driving record check", Seq=8 },
                    new Code { Id=69, Type="Agency.MentorScreening", Value="SexOffender", Label="NC sex Offender Registry", Seq=9 },
                    new Code { Id=70, Type="Agency.MentorScreening", Value="Other", Label="Other (please describe)", Seq=10 },
                    new Code { Id=71, Type="Agency.MentorTraining", Value="AgencyLed", Label="Agency-led", Seq=1 },
                    new Code { Id=72, Type="Agency.MentorTraining", Value="Outsourced", Label="Outsourced", Seq=2 },
                    new Code { Id=73, Type="Agency.MentorTraining", Value="National", Label="National", Seq=3 },
                    new Code { Id=74, Type="Agency.MentorTraining", Value="MMA", Label="Mayor's Mentoring Alliance Mentor 101 sessions", Seq=4 },
                    new Code { Id=75, Type="Agency.MentorTraining", Value="CharMeckSchools", Label="Charlotte-Mecklenburg Schools-sponsored", Seq=5 },
                    new Code { Id=76, Type="Agency.MentorTraining", Value="Other", Label="Other (please describe)", Seq=6 },
                });

            context.Users.AddOrUpdate(x => x.Email,
                new[]
                {
                    new User {Email = "paul@tagovi.com", Password = "pw", Active = true},
                });
        }
    };
}
