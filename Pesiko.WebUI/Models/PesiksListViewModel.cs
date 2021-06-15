using Pesiko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pesiko.WebUI.Models
{
    public class PesiksListViewModel
    {
        public IEnumerable<Pesik> Pesiks { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}