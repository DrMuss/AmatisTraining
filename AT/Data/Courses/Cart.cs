
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Courses
{
    public class Cart : ICart
    {
        //used to store the paymentintent as soon as possible.
        //before we make the charge we can then retrieve it and
        //update the amount if necessary
        public string ClientSecret { get; set; }
        private List<CartLine> _linesCol = new List<CartLine>();

       

        public void AddItem(Course course, int quantity)
        {
            CartLine line = _linesCol.Where(p => p.Product.Id == course.Id)
                            .FirstOrDefault();

            if (line == null)
            {
                _linesCol.Add(new CartLine() { Product = course, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(int productId)
        {
            _linesCol.RemoveAll(p => p.Product.Id == productId);

        }

      


        public int Subtotal
        {
            get {
                return _linesCol.Sum(p => p.Product.Price * p.Quantity);
            }
            
            
        }

        public int VAT
        {
            get
            {
                return (int)Math.Ceiling(Subtotal * 0.20);
            }
        }

        public int Total
        {
            get
            {
                return Subtotal + VAT;
            }
        }

        public IEnumerable<CartLine> CartLines
        {
            get
            {
                return _linesCol;
            }
           
        }

        public void Clear()
        {
            _linesCol.Clear();
        }


      
    }
}
