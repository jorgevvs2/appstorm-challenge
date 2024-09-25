SELECT ProductID, SUM(TotalAmount) AS TotalSales
FROM Sales
GROUP BY ProductID;
