-- Sample data for testing replenishment functionality
-- Run this in pgAdmin or psql after the schema has been created

-- Insert sample products first (if they don't exist)
INSERT INTO Product (ProductID, Name, Description, MonthlyCost, StockThreshold, CategoryID, Status)
VALUES
    (1, 'Laptop Dell XPS 13', 'High-performance laptop for business use', 89.99, 10, 1, 'AVAILABLE'),
    (2, 'Wireless Mouse Logitech', 'Ergonomic wireless mouse', 12.99, 50, 2, 'AVAILABLE'),
    (3, 'Monitor Samsung 24inch', '24-inch LED monitor with USB-C', 45.99, 15, 1, 'AVAILABLE'),
    (4, 'Webcam Logitech HD', 'HD webcam for video conferencing', 25.99, 25, 2, 'AVAILABLE'),
    (5, 'Keyboard Mechanical RGB', 'RGB mechanical keyboard', 35.99, 20, 2, 'AVAILABLE')
ON CONFLICT (ProductID) DO UPDATE SET
    Name = EXCLUDED.Name,
    Description = EXCLUDED.Description,
    MonthlyCost = EXCLUDED.MonthlyCost,
    StockThreshold = EXCLUDED.StockThreshold;

-- Insert sample replenishment requests
INSERT INTO ReplenishmentRequest (RequestId, RequestedBy, Status, CreatedAt, CompletedAt, CompletedBy, Remarks)
VALUES
    (1, 'STAFF001', 'DRAFT', NOW() - INTERVAL '2 hours', NULL, NULL, 'Initial stock replenishment for Q1'),
    (2, 'STAFF002', 'SUBMITTED', NOW() - INTERVAL '1 day', NULL, NULL, 'Urgent replacement for damaged items'),
    (3, 'STAFF001', 'COMPLETED', NOW() - INTERVAL '3 days', NOW() - INTERVAL '1 day', 'STAFF003', 'Monthly routine replenishment'),
    (4, 'STAFF003', 'CANCELLED', NOW() - INTERVAL '5 days', NULL, NULL, 'Cancelled due to budget constraints'),
    (5, 'STAFF002', 'SUBMITTED', NOW() - INTERVAL '6 hours', NULL, NULL, 'High demand for laptops this month')
ON CONFLICT (RequestId) DO UPDATE SET
    RequestedBy = EXCLUDED.RequestedBy,
    Status = EXCLUDED.Status,
    CreatedAt = EXCLUDED.CreatedAt,
    CompletedAt = EXCLUDED.CompletedAt,
    CompletedBy = EXCLUDED.CompletedBy,
    Remarks = EXCLUDED.Remarks;

-- Insert sample line items for the replenishment requests
INSERT INTO LineItem (RequestId, ProductId, QuantityRequest, ReasonCode, Remarks)
VALUES
    -- Request 1 (DRAFT) - 3 items
    (1, 1, 5, 'LOWSTOCK', 'Stock below threshold'),
    (1, 2, 20, 'DEMANDSPIKE', 'Increased demand from new hires'),
    (1, 3, 8, 'LOWSTOCK', 'Monitor inventory running low'),

    -- Request 2 (SUBMITTED) - 2 items
    (2, 4, 10, 'REPLACEMENT', 'Damaged webcams need replacement'),
    (2, 5, 15, 'NEWITEM', 'New mechanical keyboards for developers'),

    -- Request 3 (COMPLETED) - 4 items
    (3, 1, 3, 'LOWSTOCK', 'Routine monthly replenishment'),
    (3, 2, 25, 'DEMANDSPIKE', 'High demand in December'),
    (3, 3, 6, 'LOWSTOCK', 'Monitor stock replenishment'),
    (3, 4, 12, 'LOWSTOCK', 'Webcam inventory low'),

    -- Request 4 (CANCELLED) - 1 item
    (4, 1, 10, 'NEWITEM', 'Cancelled order for new laptops'),

    -- Request 5 (SUBMITTED) - 2 items
    (5, 1, 8, 'DEMANDSPIKE', 'Urgent laptop demand from marketing team'),
    (5, 3, 12, 'LOWSTOCK', 'Monitor shortage reported')
ON CONFLICT DO NOTHING;

-- Set the sequence values to avoid ID conflicts
SELECT setval('replenishmentrequest_requestid_seq', (SELECT MAX(RequestId) FROM ReplenishmentRequest));
SELECT setval('lineitem_lineitemid_seq', (SELECT MAX(LineItemId) FROM LineItem));