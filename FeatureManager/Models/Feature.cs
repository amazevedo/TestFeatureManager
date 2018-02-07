using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeatureManager.Models
{
    public class Feature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeatureId { get; set; }

        public string FeatureName { get; set; }

        [InverseProperty("Feature")]
        public virtual ICollection<FeatureVersion> Versions { get; set; }
    }

    public class FeatureVersion
    {
        [Key]
        [Column(Order = 1)]
        public int VersionMajor { get; set; }
        [Key]
        [Column(Order = 2)]
        public int VersionMinor { get; set; }
        [Key]
        [Column(Order = 3)]
        public int VersionBuild { get; set; }
        [Key]
        [Column(Order = 4)]
        public int VersionRevision { get; set; }

        [NotMapped]
        public Version Version
        {
            get
            {
                return new Version(this.VersionMajor, this.VersionMinor, this.VersionBuild, this.VersionRevision);
            }

            set
            {
                this.VersionMajor = value.Major;
                this.VersionMinor = value.Minor;
                this.VersionBuild = value.Build;
                this.VersionRevision = value.Revision;
            }
        }
        [Key]
        [Column(Order = 5)]
        public int FeatureID { get; set; }

        [ForeignKey("FeatureID")]
        public Feature Feature { get; set; }

        public ICollection<System> IsEnabledIn { get; set; }
        public ICollection<System> IsDisabledIn { get; set; }
    }

}