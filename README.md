# 🔱 AlaMashi.API

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://github.com/Omartube70/AlaMashi.API/blob/master/LICENSE)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen?style=for-the-badge)](https://github.com/Omartube70/AlaMashi.API)

مرحبًا بك في مشروع **AlaMashi.API**! واجهة برمجة تطبيقات (API) خلفية قوية وآمنة مصممة لتطبيق توصيل متكامل. تم بناء المشروع على أساس **Clean Architecture** مع أعلى معايير الأمان والأداء.

<div align="center">
  <img src="https://raw.githubusercontent.com/Omartube70/AlaMashi.API/master/AlaMashi.API/.assets/logo.png" alt="AlaMashi API Logo" width="250" />
  
  ### تطبيق توصيل ذكي وآمن 🚀
  
  [التوثيق الكامل](#-توثيق-api--endpoints) • [البدء السريع](#-البدء-والتشغيل-المحلي) • [المساهمة](#-المساهمة)
</div>

---

## 📋 جدول المحتويات

- [الميزات الرئيسية](#✨-الميزات-الرئيسية)
- [التقنيات المستخدمة](#🛠️-التقنيات-والمكتبات-المستخدمة)
- [هيكل المشروع](#🏛️-هيكل-المشروع-clean-architecture)
- [البدء السريع](#🚀-البدء-والتشغيل-المحلي)
- [توثيق الـ API](#📚-توثيق-api--endpoints)
- [أمثلة عملية](#💡-أمثلة-عملية)
- [المساهمة](#🤝-المساهمة)

---

## ✨ الميزات الرئيسية

### 🔐 نظام مصادقة آمن (JWT)
- **Access Token** قصير الصلاحية (60 دقيقة)
- **Refresh Token** طويل الصلاحية (60 يوم)
- تشفير كلمات المرور بـ **BCrypt.Net**
- دعم تسجيل الدخول وتسجيل جديد وإعادة تعيين كلمات المرور

### 🛡️ نظام صلاحيات متقدم (Authorization)
- نظام أدوار قائم على الأدوار (**Admin** و **User**)
- حماية شاملة للـ Endpoints
- تحكم دقيق في الوصول إلى البيانات

### ⚙️ إدارة شاملة للموارد
- **المستخدمين:** CRUD كامل + تحديثات جزئية (PATCH)
- **المنتجات:** إدارة شاملة مع دعم الصور
- **الفئات:** نظام فئات هرمي (parent/child)
- **العروض:** إدارة العروض والخصومات
- **العناوين:** إدارة عناوين التوصيل
- **الطلبات:** نظام طلبات متكامل مع تتبع الحالة
- **الدفعات:** إدارة سجلات الدفع

### 📊 لوحة تحكم متقدمة
- إحصائيات شاملة للمبيعات
- تقارير يومية وشهرية
- قائمة المنتجات الأكثر مبيعاً
- تحليل الإيرادات

### 🔧 بنية تحتية قوية
- معالج أخطاء مركزي **Middleware**
- استجابات موحدة لجميع الـ Endpoints
- التحقق من صحة المدخلات بـ **FluentValidation**
- تسجيل شامل للعمليات **Logging**

### ☁️ جاهز للنشر
- دعم **Azure App Service**
- دعم **Docker Containers**
- قواعد بيانات **SQL Server** و **Azure SQL**
- استقرار وأمان عالي

---

## 🛠️ التقنيات والمكتبات المستخدمة

| التقنية | الإصدار | الوصف |
|---------|--------|-------|
| **ASP.NET Core** | 8.0 | إطار العمل الرئيسي |
| **Entity Framework Core** | 9.0 | ORM لإدارة قواعد البيانات |
| **SQL Server** | 2019+ | قاعدة البيانات العلائقية |
| **JWT Bearer** | - | المصادقة الآمنة |
| **BCrypt.Net** | 4.0.3 | تشفير كلمات المرور |
| **FluentValidation** | 12.0 | التحقق من المدخلات |
| **AutoMapper** | 13.0 | تحويل البيانات |
| **MediatR** | 13.0 | نمط CQRS |
| **MailKit** | 4.13 | خدمة البريد الإلكتروني |

---

## 🏛️ هيكل المشروع (Clean Architecture)

```
AlaMashi.API/
├── AlaMashi.API/              # طبقة العرض (Presentation Layer)
│   ├── Controllers/           # معالجات الطلبات
│   ├── Program.cs            # إعدادات التطبيق
│   └── ErrorHandlingMiddleware.cs
│
├── Application/               # طبقة التطبيق (Application Layer)
│   ├── Commands/             # أوامر الكتابة
│   ├── Queries/              # استعلامات القراءة
│   ├── Dtos/                 # نماذج نقل البيانات
│   ├── Interfaces/           # العقود والواجهات
│   └── Exceptions/           # الأخطاء المخصصة
│
├── Infrastructure/            # طبقة البنية التحتية
│   ├── Data/                 # قاعدة البيانات والـ Context
│   ├── Repositories/         # تطبيق الـ Repositories
│   ├── Services/             # الخدمات الخارجية
│   ├── Security/             # الأمان والمصادقة
│   └── Migrations/           # هجرات قاعدة البيانات
│
└── Domain/                    # الطبقة الأساسية (Domain Layer)
    ├── Entities/             # نماذج البيانات
    └── Common/               # الثوابت والـ Enums
```

### شرح الطبقات:

| الطبقة | المسؤولية | الاعتماديات |
|-------|---------|-----------|
| **Domain** | نماذج البيانات والمنطق الأساسي | بدون اعتماديات |
| **Application** | منطق العمل والـ Business Logic | Domain فقط |
| **Infrastructure** | تطبيق قواعد البيانات والخدمات | Application + Domain |
| **Presentation** | معالجة الطلبات والاستجابات | جميع الطبقات |

---

## 🚀 البدء والتشغيل المحلي

### المتطلبات الأساسية

- **.NET 8.0 SDK** أو أحدث
- **SQL Server** 2019 أو أحدث
- **Visual Studio 2022** أو VS Code
- **Git** لاستنساخ المستودع

### خطوات التثبيت

#### 1️⃣ استنساخ المستودع
```bash
git clone https://github.com/Omartube70/AlaMashi.API.git
cd AlaMashi.API
```

#### 2️⃣ تكوين قاعدة البيانات
افتح ملف `appsettings.json` وحدّث Connection String:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AlaMashi;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
  }
}
```

#### 3️⃣ تطبيق الـ Migrations
```powershell
# في Package Manager Console
Update-Database
```

#### 4️⃣ تشغيل التطبيق
```bash
dotnet run
```

التطبيق سيعمل على:
- **HTTPS:** https://localhost:7164
- **HTTP:** http://localhost:5242
- **Swagger UI:** https://localhost:7164/swagger

---

## 📚 توثيق API - Endpoints

### 🔐 المصادقة والمستخدمين

#### التسجيل - Register
```http
POST /api/users/register
Content-Type: application/json

{
  "userName": "أحمد محمد",
  "email": "ahmed@example.com",
  "phone": "01012345678",
  "password": "SecurePass123!"
}
```
**الاستجابة:** `201 Created`

#### تسجيل الدخول - Login
```http
POST /api/users/login
Content-Type: application/json

{
  "email": "ahmed@example.com",
  "password": "SecurePass123!"
}
```
**الاستجابة:** `200 OK` مع tokens

#### تحديث التوكن - Refresh Token
```http
POST /api/users/refresh
Content-Type: application/json

{
  "refreshToken": "base64encodedtoken..."
}
```
**الاستجابة:** `200 OK` مع access token جديد

#### جلب المستخدمين
```http
GET /api/users/all
Authorization: Bearer {token}
```
**الصلاحيات:** Admin فقط

#### جلب مستخدم محدد
```http
GET /api/users/{userId}
Authorization: Bearer {token}
```

#### تحديث المستخدم
```http
PATCH /api/users/{userId}
Authorization: Bearer {token}
Content-Type: application/json-patch+json

[
  {
    "op": "replace",
    "path": "/userName",
    "value": "اسم جديد"
  }
]
```

---

### 🏆 الفئات - Categories

#### إنشاء فئة
```http
POST /api/categories/Create
Authorization: Bearer {token}
Content-Type: application/json

{
  "categoryName": "الأطعمة",
  "iconName": "food-icon",
  "parentId": null
}
```
**الصلاحيات:** Admin فقط

#### جلب الفئات (قائمة مسطحة)
```http
GET /api/categories/flat
Authorization: Bearer {token}
```

#### جلب الفئات (هيكل شجري)
```http
GET /api/categories/tree
Authorization: Bearer {token}
```

#### جلب فئة محددة
```http
GET /api/categories/{categoryId}
Authorization: Bearer {token}
```

#### تحديث فئة
```http
PUT /api/categories/{categoryId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "newCategoryName": "الأطعمة والمشروبات"
}
```

#### حذف فئة
```http
DELETE /api/categories/{categoryId}
Authorization: Bearer {token}
```

---

### 🛍️ المنتجات - Products

#### إنشاء منتج
```http
POST /api/products/Create
Authorization: Bearer {token}
Content-Type: multipart/form-data

{
  "productName": "تمر برني",
  "barcode": "1234567890123",
  "productDescription": "تمر من أفضل الأنواع",
  "price": 50.00,
  "quantityInStock": 100,
  "categoryID": 1,
  "productImageFile": <binary>
}
```
**الصلاحيات:** Admin فقط

#### جلب المنتجات
```http
GET /api/products/all?pageNumber=1&pageSize=20
Authorization: Bearer {token}
```

#### جلب منتج محدد
```http
GET /api/products/{productId}
Authorization: Bearer {token}
```

#### جلب منتجات فئة
```http
GET /api/products/category/{categoryId}
Authorization: Bearer {token}
```

#### تحديث منتج
```http
PATCH /api/products/{productId}
Authorization: Bearer {token}
Content-Type: application/json-patch+json

[
  {
    "op": "replace",
    "path": "/price",
    "value": 55.00
  }
]
```

#### حذف منتج
```http
DELETE /api/products/{productId}
Authorization: Bearer {token}
```

---

### 🎁 العروض - Offers

#### إنشاء عرض
```http
POST /api/offers/Create
Authorization: Bearer {token}
Content-Type: application/json

{
  "offer": {
    "title": "خصم 30% على جميع المنتجات",
    "description": "عرض محدود الوقت",
    "discountPercentage": 30.00,
    "startDate": "2024-11-01T00:00:00Z",
    "endDate": "2024-11-30T23:59:59Z"
  }
}
```

#### جلب العروض النشطة
```http
GET /api/offers/active
Authorization: Bearer {token}
```

#### جلب جميع العروض
```http
GET /api/offers/All
Authorization: Bearer {token}
```

#### جلب عرض محدد
```http
GET /api/offers/{offerId}
Authorization: Bearer {token}
```

---

### 📍 العناوين - Addresses

#### إنشاء عنوان
```http
POST /api/addresses/Create
Authorization: Bearer {token}
Content-Type: application/json

{
  "street": "شارع الملك فيصل",
  "city": "الرياض",
  "addressDetails": "بجانب المول",
  "addressType": "home"
}
```

#### جلب عناوين المستخدم
```http
GET /api/addresses/all/ByUser
Authorization: Bearer {token}
```

#### جلب عنوان محدد
```http
GET /api/addresses/{addressId}
Authorization: Bearer {token}
```

#### تحديث عنوان
```http
PATCH /api/addresses/{addressId}
Authorization: Bearer {token}
```

#### حذف عنوان
```http
DELETE /api/addresses/{addressId}
Authorization: Bearer {token}
```

---

### 📦 الطلبات - Orders

#### إنشاء طلب
```http
POST /api/orders/create
Authorization: Bearer {token}
Content-Type: application/json

{
  "addressId": 1,
  "deliveryDate": "2024-11-05T00:00:00Z",
  "deliveryTimeSlot": "10:00 AM - 12:00 PM",
  "items": [
    {
      "productId": 1,
      "quantity": 2
    }
  ]
}
```

#### جلب الطلبات
```http
GET /api/orders/my-orders
Authorization: Bearer {token}
```

#### جلب جميع الطلبات (Admin)
```http
GET /api/orders/all?status=Pending
Authorization: Bearer {token}
```

#### جلب طلب محدد
```http
GET /api/orders/{orderId}
Authorization: Bearer {token}
```

#### تحديث تفاصيل التوصيل
```http
PATCH /api/orders/{orderId}/delivery-details
Authorization: Bearer {token}
Content-Type: application/json

{
  "newDeliveryDate": "2024-11-10T00:00:00Z",
  "newDeliveryTimeSlot": "06:00 PM - 08:00 PM",
  "newAddressId": 2
}
```

#### إلغاء الطلب
```http
PATCH /api/orders/{orderId}/cancel
Authorization: Bearer {token}
```

#### تحديث حالة الطلب (Admin)
```http
PATCH /api/orders/{orderId}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "newStatus": "InPreparation"
}
```

---

### 💳 الدفعات - Payments

#### جلب جميع الدفعات (Admin)
```http
GET /api/orders/payments/all
Authorization: Bearer {token}
```

#### جلب دفعات طلب
```http
GET /api/orders/{orderId}/payments
Authorization: Bearer {token}
```

---

### 📊 لوحة التحكم - Dashboard

#### جلب الملخص (Admin)
```http
GET /api/Dashboard/summary
Authorization: Bearer {token}
```

---

## 💡 أمثلة عملية

### مثال 1: عملية تسجيل وتسجيل دخول

```bash
# التسجيل
curl -X POST "https://localhost:7164/api/users/register" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "أحمد",
    "email": "ahmed@example.com",
    "phone": "01012345678",
    "password": "SecurePass123!"
  }'

# تسجيل الدخول
curl -X POST "https://localhost:7164/api/users/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ahmed@example.com",
    "password": "SecurePass123!"
  }'
```

### مثال 2: إنشاء طلب

```bash
TOKEN="your_jwt_token"

curl -X POST "https://localhost:7164/api/orders/create" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "addressId": 1,
    "deliveryDate": "2024-11-05T00:00:00Z",
    "items": [{"productId": 1, "quantity": 2}]
  }'
```

---

## 🔒 معايير الأمان

✅ **JWT Authentication** - توكنات آمنة
✅ **Role-Based Authorization** - تحكم دقيق في الوصول
✅ **Password Hashing** - تشفير آمن بـ BCrypt
✅ **HTTPS Only** - اتصالات مشفرة
✅ **Input Validation** - التحقق من جميع المدخلات
✅ **Error Handling** - عدم كشف معلومات حساسة

---

## 📊 رموز الأخطاء الشائعة

| الكود | المعنى | الحل |
|------|-------|-----|
| **200** | OK | الطلب نجح ✅ |
| **201** | Created | تم الإنشاء بنجاح ✅ |
| **400** | Bad Request | تحقق من البيانات المرسلة |
| **401** | Unauthorized | توكن مفقود أو غير صالح |
| **403** | Forbidden | لا توجد صلاحيات كافية |
| **404** | Not Found | المورد غير موجود |
| **409** | Conflict | تضارب (مثل بريد مسجل) |
| **500** | Server Error | خطأ في الخادم |

---

## 🤝 المساهمة

نرحب بمساهماتك! إذا كان لديك أي اقتراحات أو وجدت أخطاء:

1. **Fork** المستودع
2. **Clone** النسخة الخاصة بك
3. **Create** فرع جديد (`git checkout -b feature/AmazingFeature`)
4. **Commit** التغييرات (`git commit -m 'Add AmazingFeature'`)
5. **Push** إلى الفرع (`git push origin feature/AmazingFeature`)
6. **Open** Pull Request

---

## 📄 الترخيص

هذا المشروع مرخص بموجب [ترخيص MIT](LICENSE) - انظر ملف `LICENSE` للتفاصيل.

---

## 📞 التواصل والدعم

- **GitHub:** [AlaMashi.API](https://github.com/Omartube70/AlaMashi.API)
- **API الحي:** https://alamashi.runasp.net
- **Swagger UI:** https://alamashi.runasp.net/swagger

---

## 👨‍💻 المطورون

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/Omartube70">
        <img src="https://avatars.githubusercontent.com/u/omartube70?v=4" width="100px;" alt="Omar"/>
        <br />
        <sub><b>عمر محمود</b></sub>
      </a>
      <br />
      <a href="https://github.com/Omartube70" title="Lead Developer">Lead Developer</a>
    </td>
  </tr>
</table>

---

<div align="center">

### ⭐ إذا أعجبك المشروع، لا تنسَ إعطاء Star ⭐

**آخر تحديث:** 29 أكتوبر 2024 | **الإصدار:** 1.0.0 | **الحالة:** ✅ Production Ready

</div>
