using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
    public interface ISoftDelete
    {
        public DateTime? DeletedAt { get; set; }
    }
}
