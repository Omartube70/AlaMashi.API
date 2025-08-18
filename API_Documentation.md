# 📄 توثيق API مشروع "على ماشي" (AlaMashi)

مرحبًا بك في توثيق الواجهة البرمجية (API) لمشروع "على ماشي". هذا الدليل يوفر كل المعلومات اللازمة للتفاعل مع الـ API بنجاح.

---

## 🧭 جدول المحتويات
1.  [**مقدمة**](#مقدمة)
    -   [الرابط الأساسي (Base URL)](#الرابط-الأساسي-base-url)
    -   [المصادقة (Authentication)](#المصادقة-authentication)
2.  [**المصادقة (Authentication Endpoints)**](#1-المصادقة-authentication-endpoints)
    -   [تسجيل الدخول](#11-تسجيل-الدخول)
    -   [تحديث التوكن](#12-تحديث-التوكن)
    -   [تسجيل الخروج](#13-تسجيل-الخروج-إلغاء-التوكن)
3.  [**إدارة المستخدمين (User Management)**](#2-إدارة-المستخدمين-user-management)
    -   [إنشاء مستخدم جديد](#21-إنشاء-مستخدم-جديد)
    -   [جلب مستخدم بواسطة ID](#22-جلب-مستخدم-بواسطة-id)
    -   [جلب كل المستخدمين](#23-جلب-كل-المستخدمين)
    -   [حذف مستخدم](#24-حذف-مستخدم)
4.  [**إدارة كلمة المرور (Password Management)**](#3-إدارة-كلمة-المرور-password-management)
    -   [طلب إعادة تعيين كلمة المرور](#31-طلب-إعادة-تعيين-كلمة-المرور)
    -   [إعادة تعيين كلمة المرور](#32-إعادة-تعيين-كلمة-المرور)

---

## مقدمة

### الرابط الأساسي (Base URL)
جميع الروابط المذكورة في هذا التوثيق تبدأ من هذا الرابط الأساسي:
`https://alamashi-api.onrender.com`

### المصادقة (Authentication)
المصادقة تتم باستخدام **JWT Bearer Tokens**. عند تسجيل الدخول، ستحصل على `accessToken` والذي يجب إرساله مع كل الطلبات للـ Endpoints المحمية في هيدر الطلب.

**مثال على الهيدر:**
```
Authorization: Bearer <your_accessToken>
```

---

## 1. المصادقة (Authentication Endpoints)

### 1.1 تسجيل الدخول
يقوم بتسجيل دخول المستخدم وإرجاع `accessToken`, `refreshToken`, وبيانات المستخدم.

-   **Method:** `POST`
-   **Endpoint:** `/api/Users/login`

-   **جسم الطلب (Request Body):**
    ```json
    {
        "email": "user@example.com",
        "password": "YourPassword123"
    }
    ```

-   **الاستجابة (Success `200 OK`):**
    ```json
    {
        "status": "success",
        "data": {
            "accessToken": "ey...",
            "refreshToken": "AbCd...",
            "user": {
                "userID": 1,
                "userName": "Omar",
                "email": "user@example.com",
                "phone": "0123456789",
                "permissions": "User"
            }
        }
    }
    ```

### 1.2 تحديث التوكن
يستخدم الـ `refreshToken` للحصول على `accessToken` و `refreshToken` جديدين.

-   **Method:** `POST`
-   **Endpoint:** `/api/Users/refresh`

-   **جسم الطلب (Request Body):**
    ```json
    {
        "refreshToken": "Your_Long_RefreshToken_String_Here"
    }
    ```

-   **الاستجابة (Success `200 OK`):**
    ```json
    {
        "status": "success",
        "data": {
            "accessToken": "ey_New...",
            "refreshToken": "XyZ_New..."
        }
    }
    ```

### 1.3 تسجيل الخروج (إلغاء التوكن)
يقوم بإلغاء صلاحية الـ `refreshToken` الحالي. يتطلب `accessToken` صالحًا.

-   **Method:** `POST`
-   **Endpoint:** `/api/Users/revoke`
-   **الصلاحية:** `Bearer Token` مطلوب.

-   **الاستجابة (Success `200 OK`):**
    ```json
    {
        "status": "success",
        "data": "Token revoked successfully."
    }
    ```

---

## 2. إدارة المستخدمين (User Management)

### 2.1 إنشاء مستخدم جديد
يقوم بتسجيل مستخدم جديد في النظام.

-   **Method:** `POST`
-   **Endpoint:** `/api/Users/Create`

-   **جسم الطلب (Request Body):**
    ```json
    {
        "userName": "New User",
        "email": "new.user@example.com",
        "phone": "01122334455",
        "password": "Password123!",
        "permissions": "User"
    }
    ```

-   **الاستجابة (Success `200 OK`):**
    ```json
    {
        "status": "success",
        "data": {
            "userID": 10,
            "userName": "New User",
            "email": "new.user@example.com",
            "phone": "01122334455",
            "permissions": "User"
        }
    }
    ```

### 2.2 جلب مستخدم بواسطة ID
يجلب بيانات مستخدم محدد. (المستخدم العادي يمكنه جلب بياناته فقط، الأدمن يمكنه جلب أي مستخدم).

-   **Method:** `GET`
-   **Endpoint:** `/api/Users/{UserID}`
-   **الصلاحية:** `Bearer Token` مطلوب.

-   **الاستجابة (Success `200 OK`):**
    ```json
    {
        "status": "success",
        "data": {
            "userID": 1,
            "userName": "Omar",
            "email": "user@example.com",
            "phone": "0123456789",
            "permissions": "User"
        }
    }
    ```

### 2.3 جلب كل المستخدمين
يجلب قائمة بكل المستخدمين في النظام. (للأدمن فقط).

-   **Method:** `GET`
-   **Endpoint:** `/api/Users/All`
-   **الصلاحية:** `Bearer Token` مطلوب (صلاحية `Admin` فقط).

-   **الاستجابة (Success `200 OK`):**
    ```json
    {
        "status": "success",
        "data": [
            {
                "userID": 1,
                "userName": "Admin User",
                "email": "admin@example.com",
                "phone": "...",
                "permissions": "Admin"
            },
            {
                "userID": 2,
                "userName": "Regular User",
                "email": "user@example.com",
                "phone": "...",
                "permissions": "User"
            }
        ]
    }
    ```

### 2.4 حذف مستخدم
يقوم بحذف مستخدم من النظام. (المستخدم يمكنه حذف حسابه، والأدمن يمكنه حذف أي حساب).

-   **Method:** `DELETE`
-   **Endpoint:** `/api/Users/{UserID}`
-   **الصلاحية:** `Bearer Token` مطلوب.

-   **الاستجابة (Success `200 OK`):**
    ```json
    {
        "status": "success",
        "data": "User deleted successfully."
    }
    ```

---

## 3. إدارة كلمة المرور (Password Management)

### 3.1 طلب إعادة تعيين كلمة المرور
يرسل رابطًا لإعادة التعيين إلى البريد الإلكتروني للمستخدم.

-   **Method:** `POST`
-   **Endpoint:** `/api/Users/forgot-password`

-   **جسم الطلب (Request Body):**
    ```json
    {
        "email": "user.to.reset@example.com"
    }
    ```

-   **الاستجابة (Success `200 OK`):**
    > **ملاحظة:** يتم إرجاع نفس الرسالة دائمًا لمنع المهاجمين من معرفة ما إذا كان البريد الإلكتروني مسجلاً أم لا.
    ```json
    {
        "message": "If your email is registered, you will receive a password reset link."
    }
    ```

### 3.2 إعادة تعيين كلمة المرور
يقوم بتعيين كلمة مرور جديدة باستخدام التوكن المرسل عبر البريد الإلكتروني.

-   **Method:** `POST`
-   **Endpoint:** `/api/Users/reset-password`

-   **جسم الطلب (Request Body):**
    ```json
    {
        "token": "The_Reset_Token_From_Email_Link",
        "newPassword": "NewSecurePassword123!"
    }
    ```

-   **الاستجابة (Success `200 OK`):**
    ```json
    {
        "message": "Password has been reset successfully."
    }
    ```
