﻿using System.Runtime.Serialization;

namespace NAvocado
{
    [DataContract]
    public class Couple
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "currentUser")]
        public User CurrentUser { get; set; }

        [DataMember(Name = "otherUser")]
        public User OtherUser { get; set; }

        [DataMember(Name = "subscription")]
        public object Subscription { get; set; }

        [DataMember(Name = "googleCalenderInfo")]
        public object GoogleCalendarInfo { get; set; }

        [DataMember(Name = "hasPearsProfile")]
        public bool HasPearsProfile { get; set; }

        [DataMember(Name = "sharesToPears")]
        public bool SharesToPears { get; set; }
    }
}