using System;
using System.Collections.Generic;
using System.Text;

namespace DT.Samples.Agora.Cross.Models
{
    public enum MenuItemType
    {
        Call,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
