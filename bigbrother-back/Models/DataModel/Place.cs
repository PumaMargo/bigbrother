﻿using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace bigbrother_back.Models.DataModel
{
    public class Place
    {
        #region Properties

        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<Tag> Tags { get; set; } = new List<Tag>();

        #endregion
    }
}
