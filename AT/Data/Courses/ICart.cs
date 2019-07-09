using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Courses
{
    public interface ICart
    {
        int Subtotal { get;  }
        int VAT { get; }
        int Total { get; }

        string ClientSecret { get; set; }
        IEnumerable<CartLine> CartLines { get; }
        void AddItem(Course course, int quantity);
        void RemoveLine(int productId);
      
        void Clear();
    }
}
