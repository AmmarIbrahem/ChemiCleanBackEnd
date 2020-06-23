using ChemiClean.Models;
using ChemiClean.Repositories;
using NUnit.Framework;
using System;
using System.Security.Cryptography;

namespace ChemiCleanTest
{
    public class Tests
    {
        IRepository mock;

        [SetUp]
        public void Setup()
        {
            mock = new MockRepository();
        }


        [Test]
        public void GetAllDataSheets_Return_List_Size_2()
        {
            var result = mock.GetAllDataSheets();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void getDataSheet_Return_Value(int dataSheetId)
        {
            var result = mock.getDataSheet(dataSheetId);
            Assert.AreEqual(true, result!=null);
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void getDataSheet_Return_NULL(int dataSheetId)
        {
            var result = mock.getDataSheet(dataSheetId);
            Assert.AreEqual(null, result);
        }

        [Test]
        [TestCase(1, "https://www.imaginary.Notfound.pdf", UrlState.Invalid)]
        [TestCase(2, "http://www.flamol.dk/Flamol%20B.pdf", UrlState.Valid)]
        public void UpdateURLStatus_Return_True(int dataSheetId, string url, UrlState status)
        {
            var result = mock.UpdateURLStatus(dataSheetId, url, status);
            Assert.AreEqual(true, result);
        }
        
        [Test]
        [TestCase(3, "https://www.iminar.Notfound.pdf", UrlState.Invalid)]
        [TestCase(4, "http://www.flamol.d/Flamol%20B.pdf", UrlState.Valid)]
        public void UpdateURLStatus_Return_False(int dataSheetId, string url, UrlState status)
        {
            var result = mock.UpdateURLStatus(dataSheetId, url, status);
            Assert.AreEqual(false, result);
        }

        [Test]
        [TestCase(1, @"C:\Downloads", new byte[] { 80 })]
        [TestCase(2, @"C:\Downloads", new byte[] { 80 })]
        public void StoreFile_Return_True(int dataSheetId, string localURL, byte[] hashValue)
        {
            var result = mock.StoreFile(dataSheetId, localURL, hashValue);
            Assert.AreEqual(true, result);
        }

        [Test]
        [TestCase(3, @"C:\Downloads", new byte[] { 80 })]
        public void StoreFile_Return_False(int dataSheetId, string localURL, byte[] hashValue)
        {
            var result = mock.StoreFile(dataSheetId, localURL, hashValue);
            Assert.AreEqual(false, result);
        }

        [Test]
        [TestCase(1)]
        public void getHashValue_Return_Value(int dataSheetId)
        {

            var result = mock.getHashValue(dataSheetId);
            Assert.AreEqual(MD5.Create().ComputeHash(new byte[] { 80 }), result);
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        public void getHashValue_Return_Null(int dataSheetId)
        {

            var result = mock.getHashValue(dataSheetId);
            Assert.IsNull(result);
        }
        //-------------------------Not Mandatory--------------------------------------------

        [Test]
        public void GetAllSuppliers_Return_List_Size_2()
        {
            var result = mock.GetAllSuppliers();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void GetSupplierProducts_Return_List_Size_1(int supplierId)
        {
            var result = mock.GetSupplierProducts(supplierId);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [TestCase(0)]
        [TestCase(3)]
        public void GetSupplierProducts_Return_List_Size_0(int supplierId)
        {
            var result = mock.GetSupplierProducts(supplierId);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        [TestCase(1)]
        public void GetProductDataSheet_Return_URL(int productId)
        {
            var result = mock.GetProductDataSheet(productId);
            Assert.AreEqual("https://www.imaginary.Notfound.pdf", result);
        }

        [Test]
        [TestCase(3)]
        public void GetProductDataSheet_Return_NULL(int productId)
        {
            var result = mock.GetProductDataSheet(productId);
            Assert.AreEqual(null, result);
        }
    }
}