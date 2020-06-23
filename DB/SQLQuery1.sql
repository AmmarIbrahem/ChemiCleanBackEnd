--Migrating data to new Schema
INSERT into Products (ProductName)
SELECT  DISTINCT Productname  FROM tblProduct 


INSERT into Suppliers (SupplierName) 
SELECT  DISTINCT SupplierName  FROM tblProduct

INSERT into Users (UserName, Password) 
SELECT  DISTINCT UserName, Password FROM tblProduct


INSERT INTO DataSheets (SupplierID, ProductID, DataSheetURL, UserID)

SELECT  FROM (

(select ProductID from Products where ProductName in (select ProductName from tblProduct)) as P

SupplierID = (
select SupplierID from Suppliers where SupplierName in (select SupplierName from tblProduct)
) and
UserID = (
select UserID from Users where UserName  in (select UserName from tblProduct)
) 


INSERT INTO DataSheets (SupplierID, ProductID, DataSheetURL, UserID)

select distinct supplierID, productID, url, UserID from tblProduct
join suppliers on tblProduct.suppliername = suppliers.suppliername
join products on tblProduct.productname = products.productname
left outer join users on tblProduct.username = users.username

--Create trigger for update date
create trigger dataSheetsTrgon dbo.DataSheets AFTER INSERT, UPDATE ASBEGINDECLARE @ID  INT
SET @ID = (SELECT DataSheetId FROM INSERTED )
UPDATE dbo.DataSheets 
SET dbo.DataSheets.UpdatedAt = GETDATE()
WHERE  dbo.DataSheets.DataSheetID = @ID
END

