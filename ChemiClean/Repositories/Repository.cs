using ChemiClean.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ChemiClean.Repositories
{
    public class Repository : IRepository
    {
        ChemiCleanDBContext context;
        static List<Products> products;
        public Repository(ChemiCleanDBContext _context)
        {
            context = _context;
        }

        public List<DataSheets> GetAllDataSheets()
        {
            return context.DataSheets.ToList();
        }

        public DataSheets getDataSheet(int dataSheetId)
        {
           return context.DataSheets.Where(ds => ds.DataSheetId == dataSheetId).FirstOrDefault();
        }

        public bool UpdateURLStatus(int dataSheetId, string url, bool status)
        {
            var sheet = context.DataSheets.Select(d => d).Where(ds => ds.DataSheetId == dataSheetId).FirstOrDefault();
            if (sheet == null)
                return false;
            sheet.IsValid = status;
            context.Attach(sheet);
            if(sheet.DataSheetUrl != url)
            {
                sheet.DataSheetUrl = url;
                context.Entry(sheet).Property(p => p.DataSheetUrl).IsModified = true;
            }

            context.Entry(sheet).Property(p => p.IsValid).IsModified = true;
            context.SaveChanges();
            return true;
        }

        public bool StoreFile(int dataSheetId, string localURL, byte[] hashValue)
        {
            var sheet = context.DataSheets.Select(d => d).Where(ds => ds.DataSheetId == dataSheetId).FirstOrDefault();
            if (sheet == null)
                return false;
            sheet.LocalURL = localURL;
            sheet.HashValue = hashValue;
            context.Attach(sheet);
                context.Entry(sheet).Property(p => p.LocalURL).IsModified = true;
                context.Entry(sheet).Property(p => p.HashValue).IsModified = true;
            context.SaveChanges();
            return true;
        }
        public bool IsHashExist(byte[] hashValue)
        {
            var result = context.DataSheets.Where(ds => ds.HashValue == hashValue).ToList();
            if (result.Count > 0)
                return true;
            return false;
        }
     

        

        public string GetProductDataSheet(int productId)
        {
            return context.DataSheets
                .Where(ds => ds.ProductId == productId)
                .Select(s => s.DataSheetUrl)
                .Distinct()
                .FirstOrDefault();
        }        
        public List<Suppliers> GetAllSuppliers()
        {
            products = context.Products.ToList();
            return context.Suppliers.ToList(); 
        }
        public List<Products> GetSupplierProducts(int supplierId)
        {
            var result =
                from ds in context.DataSheets
                where ds.SupplierId == supplierId
                select ds.ProductId;
            return products.Where(p => result.ToList().Contains(p.ProductId)).ToList();
        }
    }

}
