﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamAlumniNETBackend.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public int UserId { get; set; }
        public ICollection<Rsvp>? Rsvps { get; set; }
        public ICollection<Topic>? Topics { get; set; }
        public ICollection<Group>? Groups { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
