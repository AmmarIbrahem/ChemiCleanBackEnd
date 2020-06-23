using ChemiClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemiClean.Repositories
{
    public class MockRepository : IRepository
    {
        List<DataSheets> dataSheets;
        List<Suppliers> suppliers;
        List<Products> products;

        public MockRepository()
        {
            suppliers = new List<Suppliers>() {
                new Suppliers() { SupplierId=1, SupplierName="supplier 1"},
                new Suppliers() { SupplierId=2, SupplierName="supplier 2"},
            };

            products= new List<Products>() {
                new Products() { ProductId=1, ProductName="product 1"},
                new Products() { ProductId=2, ProductName="supplier 2"},
            };
            dataSheets = new List<DataSheets>()
            {
                new DataSheets(){DataSheetId=1, ProductId=1, SupplierId=1, DataSheetUrl="https://www.imaginary.Notfound.pdf" },
                new DataSheets(){DataSheetId=2, ProductId=2, SupplierId=2, DataSheetUrl="http://www.flamol.dk/Flamol%20B.pdf" },
            };
        }


        public List<DataSheets> GetAllDataSheets()
        {
            return dataSheets.ToList();
        }

        public DataSheets getDataSheet(int dataSheetId)
        {
            return dataSheets.Where(ds => ds.DataSheetId == dataSheetId).FirstOrDefault();
        }

        public bool UpdateURLStatus(int dataSheetId, string url, bool status)
        {
            var sheet = dataSheets.Select(d => d).Where(ds => ds.DataSheetId == dataSheetId).FirstOrDefault();
            if (sheet == null)
                return false;
            foreach (var dataSheet in dataSheets)
            {
                if (dataSheet.DataSheetId == dataSheetId)
                    dataSheet.IsValid = status;
            }
            return true;
        }

        public bool StoreFile(int dataSheetId, string localURL, byte[] hashValue)
        {
            var sheet = dataSheets.Select(d => d).Where(ds => ds.DataSheetId == dataSheetId).FirstOrDefault();
            if (sheet == null)
                return false;
            foreach (var dataSheet in dataSheets)
            {
                if(dataSheet.DataSheetId == dataSheetId)
                {
                    dataSheet.LocalURL = localURL;
                    dataSheet.HashValue = hashValue;
                }
            }
            return true;
        }

        public bool IsHashExist(byte[] hashValue)
        {
            var result = dataSheets.Where(ds => ds.HashValue == hashValue).ToList();
            if (result.Count > 0)
                return true;
            return false;
        }

       




        public List<Suppliers> GetAllSuppliers()
        {
            return suppliers.ToList();
        }
        public List<Products> GetSupplierProducts(int supplierId)
        {
            var result =
                from ds in dataSheets
                where ds.SupplierId == supplierId
                select ds.ProductId;
            return products.Where(p => result.ToList().Contains(p.ProductId)).ToList();
        }
        public string GetProductDataSheet(int productId)
        {
            return dataSheets
                .Where(ds => ds.ProductId == productId)
                .Select(s => s.DataSheetUrl)
                .Distinct()
                .FirstOrDefault();
        }
    }
}
