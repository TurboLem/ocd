using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OCD.Models;
using System.Reflection.Emit;

namespace OCD.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CampaignRequest> CampaignRequests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<BusinessUnit> BusinessUnits { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<CommunicationType> CommunicationTypes { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<PublicationOwner> PublicationOwners { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<BrokerCode> BrokerCodes { get; set; }
        public DbSet<SourceBreak> SourceBreaks { get; set; }
        public DbSet<ConnexDiallerCampaign> ConnexDiallerCampaigns { get; set; }
        public DbSet<ConnexDiallerSource> ConnexDiallerSources { get; set; }
        public DbSet<PackType> PackTypes { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CampaignRequest>(entity =>
            {
                

                entity.HasOne(d => d.PackType)
                    .WithMany()
                    .HasForeignKey(d => d.PackTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany()
                    .HasForeignKey(d => d.BusinessUnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Channel)
                    .WithMany()
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Source)
                    .WithMany()
                    .HasForeignKey(d => d.SourceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Media)
                    .WithMany()
                    .HasForeignKey(d => d.MediaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.PublicationOwner)
                    .WithMany()
                    .HasForeignKey(d => d.PublicationOwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Publication)
                    .WithMany()
                    .HasForeignKey(d => d.PublicationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Slot)
                    .WithMany()
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ConnexDiallerCampaign)
                .WithMany()
                    .HasForeignKey(d => d.ConnexDiallerCampaignId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ConnexDiallerSource)
                .WithMany()
                    .HasForeignKey(d => d.ConnexDiallerSourceId)
                    .OnDelete(DeleteBehavior.Restrict);



                entity.HasOne(d => d.SourceBreak)
                .WithMany()
                    .HasForeignKey(d => d.SourceBreakId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CommunicationType)
                .WithMany()
                    .HasForeignKey(d => d.CommunicationTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Campaign)
                .WithMany()
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Requester)
                .WithMany()
                    .HasForeignKey(d => d.RequesterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Actioner)
                .WithMany()
                    .HasForeignKey(d => d.ActionedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CampaignRequestMain)
                      .WithMany(p => p.SubCampaignRequests)
                      .HasForeignKey(d => d.CampaignRequestMainId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ConnexDiallerCampaign>()
                .HasMany(c => c.ConnexDiallerSources)
                .WithOne(s => s.ConnexDiallerCampaign)
                .HasForeignKey(s => s.ConnexDiallerCampaignId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.CampaignRequest)
                .WithMany(cr => cr.Comments)
                .HasForeignKey(c => c.CampaignRequestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
