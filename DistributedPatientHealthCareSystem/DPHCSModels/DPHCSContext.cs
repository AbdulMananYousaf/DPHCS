using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DistributedPatientHealthCareSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DistributedPatientHealthCareSystem.Hubs;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class DPHCSContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<UserConnection> UserConnection { get; set; }
        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<DoctorSpecializationList> DoctorSpecializationList { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<LabTestOrder> LabTestOrder { get; set; }
        public virtual DbSet<LabTestResult> LabTestResult { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientAllergie> PatientAllergie { get; set; }
        public virtual DbSet<PatientHealthRecord> PatientHealthRecord { get; set; }
        public virtual DbSet<PatientPrescription> PatientPrescription { get; set; }
        public virtual DbSet<PatientVital> PatientVital { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<TestBloodGroup> TestBloodGroup { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //            optionsBuilder.UseSqlServer(@"Server=NOMAN;Database=DPHCS;Trusted_Connection=True;");
        //        }

        public DPHCSContext(DbContextOptions<DPHCSContext> options)
            : base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserConnection>(entity =>
            {
                entity.Property(e => e.ConnectionID).ValueGeneratedNever().IsRequired()
                    .HasMaxLength(50);
                    
                entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
                

            });
                modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.AppointmentStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Discription).HasMaxLength(50);

                entity.Property(e => e.HeldDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PatientLastVisitDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SetedDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VisitReson)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DoctorEmployee)
                    .WithMany(p => p.AppointmentDoctorEmployee)
                    .HasForeignKey(d => d.DoctorEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Appointment_DoctorEmployeeId");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Appointments_Patients");

                entity.HasOne(d => d.ReceptionistEmployeet)
                    .WithMany(p => p.AppointmentReceptionistEmployeet)
                    .HasForeignKey(d => d.ReceptionistEmployeetId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Appointment_ReceptionistEmployee");
            });

        

            modelBuilder.Entity<DoctorSpecializationList>(entity =>
            {
                entity.HasKey(e => e.Value)
                    .HasName("PK__DoctorSp__07D9BBC336470DEF");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).ValueGeneratedNever();

                entity.Property(e => e.JoinDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Spectialization)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithOne(p => p.Employee)
                    .HasForeignKey<Employee>(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Employee_Person");
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Api)
                    .IsRequired()
                    .HasColumnName("API")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.WebSite)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LabTestOrder>(entity =>
            {
                entity.Property(e => e.Discription).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TestTableName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PatientHealthRecord)
                    .WithMany(p => p.LabTestOrder)
                    .HasForeignKey(d => d.PatientHealthRecordId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_HealthReportOrders_HealthRecords");
            });

            modelBuilder.Entity<LabTestResult>(entity =>
            {
                entity.HasKey(e => e.TestResultId)
                    .HasName("PK_LabTestResult");

                entity.Property(e => e.TestTableName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.LabTestOrder)
                    .WithMany(p => p.LabTestResult)
                    .HasForeignKey(d => d.LabTestOrderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_LabTestResult_LabTestOrder");

                entity.HasOne(d => d.PatientHealthRecord)
                    .WithMany(p => p.LabTestResult)
                    .HasForeignKey(d => d.PatientHealthRecordId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_LabTestResult_[PatientHealthRecord");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.PatientId).ValueGeneratedNever();

                entity.Property(e => e.RegDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PatientNavigation)
                    .WithOne(p => p.Patient)
                    .HasForeignKey<Patient>(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Patients_Person");
            });

            modelBuilder.Entity<PatientAllergie>(entity =>
            {
                entity.HasKey(e => e.AllergyId)
                    .HasName("PK_Allergies");

                entity.Property(e => e.Discription).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PatientHealthRecord)
                    .WithMany(p => p.PatientAllergie)
                    .HasForeignKey(d => d.PatientHealthRecordId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Allergies_PatientHealthRecords");
            });

            modelBuilder.Entity<PatientHealthRecord>(entity =>
            {
                entity.HasKey(e => e.HealthRecordId)
                    .HasName("PK_HealthRecords");

                entity.HasOne(d => d.DoctorEmployee)
                    .WithMany(p => p.PatientHealthRecord)
                    .HasForeignKey(d => d.DoctorEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PatientHealthRecord_Employee");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.PatientHealthRecord)
                    .HasForeignKey(d => d.HospitalId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_HealthRecords_Hospitals");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientHealthRecord)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_HealthRecords_PatientDetail");
            });

            modelBuilder.Entity<PatientPrescription>(entity =>
            {
                entity.HasKey(e => e.PrescriptionId)
                    .HasName("PK_Prescriptions");

                entity.Property(e => e.MedicineName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UsageDirections)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PatientHealthRecord)
                    .WithMany(p => p.PatientPrescription)
                    .HasForeignKey(d => d.PatientHealthRecordId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Prescriptions_PatientHealthRecord");
            });

            modelBuilder.Entity<PatientVital>(entity =>
            {
                entity.HasKey(e => e.VitalId)
                    .HasName("PK_Vitals");

                entity.Property(e => e.VitalId).ValueGeneratedNever();

                entity.Property(e => e.Detail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PatientHealthRecord)
                    .WithMany(p => p.PatientVital)
                    .HasForeignKey(d => d.PatientHealthRecordId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Vitals_PatientHealthRecord");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.PersonId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cnic)
                    .HasColumnName("CNIC")
                    .HasMaxLength(50);

                entity.Property(e => e.DoB)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PersonNavigation)
                    .WithOne(p => p.Person)
                    .HasForeignKey<Person>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Person_UserAccount");
            });

            modelBuilder.Entity<TestBloodGroup>(entity =>
            {
                entity.HasKey(e => e.BloodTestTestId)
                    .HasName("PK_BloodGroupTests");

                entity.Property(e => e.GeneratedDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Result)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.HealthRecord)
                    .WithMany(p => p.TestBloodGroup)
                    .HasForeignKey(d => d.HealthRecordId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_BloodGroupTests_HealthRecords");

                entity.HasOne(d => d.LabEmployee)
                    .WithMany(p => p.TestBloodGroup)
                    .HasForeignKey(d => d.LabEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TestBloodGroup_LabEmplyeeId");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}