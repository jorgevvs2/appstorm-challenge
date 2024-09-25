SELECT c.CustomerName
FROM Orders o
JOIN Customers c ON o.CustomerID = c.CustomerID
GROUP BY c.CustomerName
HAVING COUNT(o.OrderID) > 5;
