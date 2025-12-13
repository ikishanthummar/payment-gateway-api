# Payment Gateway API

## Overview

This project implements a **secure payment gateway backend** using **ASP.NET Core, EF Core, and SQL Server**.  
It demonstrates a **complete payment lifecycle** including payment initiation, webhook handling with **HMAC SHA256 verification**, idempotent processing, atomic database updates, and transaction listing for UI consumption.

The solution follows **real-world payment gateway behavior** and focuses on **security, consistency, and correctness**.

* * *

## Tech Stack

*   **Backend:** ASP.NET Core Web API
    
*   **Database:** SQL Server + Entity Framework Core
    
*   **Security:** HMAC SHA256 webhook signature verification
    
*   **Architecture:** Middleware + Service-based design
    

* * *

## Features Implemented

### 1\. Payment Initiation

*   Create a new transaction via REST API
    
*   Generates unique `OrderNumber` and `TransactionNumber`
    
*   Initial status set to **Pending**
    

**Endpoint**

`POST /api/payments/initiate`

* * *

### 2\. Transaction Listing

*   Fetch all transactions for UI/portal
    
*   Returns only required fields
    

**Endpoint**

`GET /api/transactions`

**Fields Returned**

*   Id
    
*   OrderId
    
*   ProviderReference
    
*   Amount
    
*   Status
    
*   UpdatedOn
    

* * *

### 3\. Secure Webhook Handling (HMAC SHA256)

*   Custom middleware validates webhook signature
    
*   Raw request body preserved
    
*   Invalid or tampered requests rejected with **401 Unauthorized**
    

**Security Highlights**

*   HMAC SHA256
    
*   Timing-safe comparison
    
*   Middleware-level enforcement (before controller)
    

* * *

### 4\. Payment Callback Processing

*   Processes webhook callbacks securely
    
*   Logs webhook payloads
    
*   Prevents duplicate processing (idempotency)
    
*   Updates transaction status atomically
    

**Endpoint**

`POST /api/payments/callback`

* * *

### 5\. Enum-Based Payment Status

Payment status is handled using enums internally for safety:

`public enum PaymentStatus {     Pending = 0,     Success = 1,     Failed  = 2 }`

*   Case-insensitive enum parsing enabled
    
*   Invalid or misspelled values rejected automatically
    
*   Database stores string values for backward compatibility
    

* * *

## Database Design

### Tables Used

*   **Transactions** – core payment data
    
*   **PaymentProviders** – gateway abstraction
    
*   **WebhookLogs** – webhook audit & idempotency
    

### Key Design Decisions

*   Atomic updates using EF Core transactions
    
*   Final payment states cannot be overwritten
    
*   Duplicate webhook callbacks safely ignored
    

* * *

## Testing Guide

### 1\. Initiate Payment

`POST /api/payments/initiate`

### 2\. List Transactions

`GET /api/transactions`

### 3\. Webhook Callback (Success)

`{   "transactionNumber": "TXN-XXXX",   "status": "Success" }`

### 4\. Webhook Callback (Failed)

`{   "transactionNumber": "TXN-XXXX",   "status": "Failed" }`

### 5\. Duplicate Webhook

*   Same payload sent again
    
*   Safely ignored (idempotent behavior)
    

### 6\. Invalid Signature / Wrong Secret

*   Returns **401 Unauthorized**
    
*   No DB update occurs
    

* * *

## Security Behavior Summary

| Scenario | Result |
| --- | --- |
| Valid signature | ✅ Processed |
| Invalid signature | ❌ 401 Unauthorized |
| Duplicate webhook | ✅ Ignored |
| Invalid enum value | ❌ 400 Bad Request |
| Status already final | ✅ Not overwritten |

* * *

## Key Engineering Highlights

*   Middleware-based webhook security
    
*   Enum-driven state management
    
*   Idempotent webhook processing
    
*   Atomic database transactions
    
*   Clean separation of concerns
