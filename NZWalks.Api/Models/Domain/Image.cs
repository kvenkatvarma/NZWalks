﻿using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.Api.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        [NotMapped]//Here we are telling this property data will not be passed to database
        public IFormFile File { get; set; }
        public string FileName { get; set; }

        public string? FileDescription { get; set; }

        public string FileExtension { get; set; }

        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}
