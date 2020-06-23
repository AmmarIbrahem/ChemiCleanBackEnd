using ChemiClean.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ChemiClean.Repositories
{
    public interface IRepository
    {
        public List<DataSheets> GetAllDataSheets();
        public bool UpdateURLStatus(int dataSheetId, string url, bool status);
        public bool StoreFile(int dataSheetId, string localURL, byte[] hashValue);
        public bool IsHashExist(byte[] hashValue);
        public DataSheets getDataSheet(int dataSheetId);

        public List<Suppliers> GetAllSuppliers();
        public List<Products> GetSupplierProducts(int supplierId);
        public string GetProductDataSheet(int productId);


    }
}
