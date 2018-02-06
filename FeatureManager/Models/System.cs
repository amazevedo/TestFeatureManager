using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeatureManager.Models
{
    public class System
    {
        [Key]
        public Guid SystemIdentifier { get; set; }
        public string SystemName { get; set; }

    }
    
}