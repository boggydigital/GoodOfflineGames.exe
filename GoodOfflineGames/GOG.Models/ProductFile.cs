﻿using System.Runtime.Serialization;

namespace GOG.Models
{
    [DataContract]
    public class ProductFile
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
        [DataMember(Name = "operatingSystem")]
        public string OperatingSystem { get; set; }
        [DataMember(Name = "language")]
        public string Language { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "size")]
        public string Size { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }
        [DataMember(Name = "folder")]
        public string Folder { get; set; }
        [DataMember(Name = "file")]
        public string File { get; set; }
        [DataMember(Name = "extra")]
        public bool Extra { get; set; }
        [DataMember(Name = "validated")]
        public bool Validated { get; set; }
    }
}