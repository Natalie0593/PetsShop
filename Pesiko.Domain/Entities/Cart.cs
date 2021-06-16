using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesiko.Domain.Entities
{
   public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Pesik pesik, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Pesik.PesikId == pesik.PesikId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Pesik = pesik,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Pesik pesik)
        {
            lineCollection.RemoveAll(l => l.Pesik.PesikId == pesik.PesikId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Pesik.Price * e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Pesik Pesik { get; set; }
        public int Quantity { get; set; }
    }
}
