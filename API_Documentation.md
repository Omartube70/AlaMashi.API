# 🔱 AlaMashi.API Documentation

مرحبًا بك في توثيق واجهة برمجة التطبيقات (API) لمشروع **AlaMashi**. يوفر هذا المستند شرحًا تفصيليًا لجميع نقاط النهاية (Endpoints) المتاحة، بما في ذلك الطلبات والاستجابات المتوقعة.

-   **Base URL:** `https://YOUR_AZURE_API_LINK.azurewebsites.net/api`

---

## 🔐 Authentication Endpoints

هذه النقاط مسؤولة عن عمليات المصادقة وإدارة حسابات المستخدمين.

### 1. Register a New User

-   **Endpoint:** `POST /auth/register`
-   **Description:** إنشاء مستخدم جديد في النظام. بشكل افتراضي، يتم منحه دور "User".
-   **Requires Authentication:** No.
-   **Request Body:**
    ```json
    {
      "firstName": "Omar",
      "lastName": "Tube",
      "email": "user@example.com",
      "password": "Password123!"
    }
    ```
-   **Success Response (200 OK):**
    ```json
    {
      "isAuthenticated": true,
      "message": "User registered successfully.",
      "email": "user@example.com",
      "roles": ["User"],
      "token": "ey...",
      "refreshToken": "ey...",
      "refreshTokenExpiration": "2025-09-28T12:00:00Z"
    }
    ```
-   **Error Response (400 Bad Request):**
    ```json
    {
      "succeeded": false,
      "message": "Email is already registered!"
    }
    ```

### 2. Login

-   **Endpoint:** `POST /auth/login`
-   **Description:** تسجيل دخول مستخدم موجود وإرجاع `AccessToken` و `RefreshToken`.
-   **Requires Authentication:** No.
-   **Request Body:**
    ```json
    {
      "email": "user@example.com",
      "password": "Password123!"
    }
    ```
-   **Success Response (200 OK):** (نفس استجابة التسجيل)
-   **Error Response (400 Bad Request):**
    ```json
    {
      "succeeded": false,
      "message": "Invalid credentials."
    }
    ```

### 3. Generate a New Access Token

-   **Endpoint:** `POST /auth/refresh-token`
-   **Description:** إنشاء `AccessToken` جديد باستخدام `RefreshToken` صالح.
-   **Requires Authentication:** No. (يتم إرسال التوكن القديم في الـ Header)
-   **Request Body:**
    ```json
    {
      "accessToken": "ey...",
      "refreshToken": "ey..."
    }
    ```
-   **Success Response (200 OK):** (نفس استجابة التسجيل)

### 4. Revoke a Refresh Token

-   **Endpoint:** `POST /auth/revoke-token`
-   **Description:** إلغاء صلاحية `RefreshToken` لمستخدم معين لمنعه من إنشاء `AccessTokens` جديدة.
-   **Requires Authentication:** Yes (Bearer Token).
-   **Request Body:**
    ```json
    {
      "refreshToken": "ey..."
    }
    ```
-   **Success Response (200 OK):**
    ```json
    {
      "succeeded": true,
      "message": "Token revoked successfully."
    }
    ```

---

## 👤 Users Endpoints

هذه النقاط مسؤولة عن إدارة بيانات المستخدمين.

### 1. Get All Users

-   **Endpoint:** `GET /users`
-   **Description:** الحصول على قائمة بجميع المستخدمين المسجلين في النظام.
-   **Requires Authentication:** Yes (Role: `Admin`).
-   **Success Response (200 OK):**
    ```json
    [
      {
        "id": "guid-goes-here",
        "firstName": "Admin",
        "lastName": "User",
        "email": "admin@example.com",
        "roles": ["Admin"]
      },
      {
        "id": "guid-goes-here-2",
        "firstName": "Regular",
        "lastName": "User",
        "email": "user@example.com",
        "roles": ["User"]
      }
    ]
    ```

### 2. Get User by ID

-   **Endpoint:** `GET /users/{id}`
-   **Description:** الحصول على بيانات مستخدم معين باستخدام الـ ID الخاص به.
-   **Requires Authentication:** Yes (Role: `Admin` or the user themself).
-   **Success Response (200 OK):** (نفس شكل العنصر الواحد في Get All Users)
-   **Error Response (404 Not Found):**
    ```json
    {
      "succeeded": false,
      "message": "User not found."
    }
    ```

### 3. Partially Update User Data

-   **Endpoint:** `PATCH /users/{id}`
-   **Description:** تحديث جزء من بيانات المستخدم (مثل الاسم الأول) باستخدام `JSON Patch`.
-   **Requires Authentication:** Yes (Role: `Admin` or the user themself).
-   **Request Body (Content-Type: `application/json-patch+json`):**
    ```json
    [
      {
        "op": "replace",
        "path": "/firstName",
        "value": "NewFirstName"
      }
    ]
    ```
-   **Success Response (204 No Content):** لا يوجد محتوى في الاستجابة عند النجاح.

### 4. Delete a User

-   **Endpoint:** `DELETE /users/{id}`
-   **Description:** حذف مستخدم من النظام.
-   **Requires Authentication:** Yes (Role: `Admin`).
-   **Success Response (204 No Content):** لا يوجد محتوى في الاستجابة عند النجاح.
