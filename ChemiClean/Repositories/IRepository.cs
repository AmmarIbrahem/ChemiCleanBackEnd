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
        public List<DataSheetViewModel> GetAllDataSheets();
        public bool UpdateURLStatus(int dataSheetId, string url, UrlState status);
        public bool StoreFile(int dataSheetId, string localURL, byte[] hashValue);
        public byte[] getHashValue(int dataSheetId);
        public DataSheets getDataSheet(int dataSheetId);


        //-------------------------Not Mandatory--------------------------------------------

        public List<Suppliers> GetAllSuppliers();
        public List<Products> GetSupplierProducts(int supplierId);
        public string GetProductDataSheet(int productId);


    }
}
