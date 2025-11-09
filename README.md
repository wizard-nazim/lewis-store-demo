# lewis-store-demo


| Section               | done?             | Extra needed                                |
| --------------------- | ----------------  | ------------------------------------------- |
| Auth                  | ✅               | validation, token expiry, duplicates        |
| Products              | ✅               | update/delete, validations, stock rules     |
| Orders                | ✅               | stock decrement, validation, error handling |
| Payments              | ✅                | validation, failure handling, security      |
| Admin                 | ✅                | validation, strict admin enforcement        |
| DTOs / Business Logic | Partial            | enforce rules, validations                  |
| Security / Logging    | ❌                | JWT, permissions, exception handling        |
| Edge Cases            | ❌                | negative tests, concurrency, invalid input  |

---

# Auth

> Register, Login, JWT token generation ✅
> 
> Still needed: Input validation (email format, password strength), duplicate email handling, token expiry/refresh ❌

# Products

> Have: List products, Create product, Get product by ID ✅
> 
> Still needed: Update/Delete endpoints if required, input validation (price ≥ 0, stock ≥ 0), handle inactive products ❌

# Orders
> Have: Checkout, Get order by ID ✅
> 
> Still needed: Stock decrement logic, validation (items exist, quantity available), error handling for empty/invalid carts ❌

# Payments
> Have: Create session, Record payment, Simulate webhook ✅
> 
> Still needed: Validate order exists, handle payment failures/retries, match amount with order total, enforce auth ❌

# Admin
> Have: Top up balance, List users ✅
> 
> Still needed: Strict admin auth, validation (amount ≥ 0) ❌

# DTOs / Business Logic
> Have: Basic DTOs ✅
> 
> Still needed: Enforce business rules, full validation, error handling ❌

# Security / Logging / Edge Cases
>Have: Basic JWT auth ✅
>
>Still needed: JWT expiry handling, invalid token handling, permission enforcement, exception logging, concurrency handling ❌
