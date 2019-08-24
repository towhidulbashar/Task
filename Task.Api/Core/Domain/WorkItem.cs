﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Api.Core.Domain
{
    public class WorkItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }
    }
}
