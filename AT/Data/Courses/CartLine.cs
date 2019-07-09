
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Courses
{
    public class CartLine
    {
        public int CartLineId { get; set; }
        public Course Product { get; set; }
        public int Quantity { get; set; }

        public IList<SelectListItem> Quantities
        {
            get
            {
                IList<SelectListItem> list = new List<SelectListItem>();
                for(int i=1; i<6;i++)
                {
                    SelectListItem li = new SelectListItem() { Value = i.ToString(), Text = i.ToString() };
                    list.Add(li);
                }

                return list;
                    
            }
        }

    }
}
