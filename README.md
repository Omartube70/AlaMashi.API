# 🔱 AlaMashi.API

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://github.com/Omartube70/AlaMashi.API/blob/master/LICENSE)
[![Deployed on Render](https://img.shields.io/badge/Deployment-Render-00979D?style=for-the-badge&logo=render)](https://alamashi-api.onrender.com)

مرحبًا بك في مشروع **AlaMashi.API**! هذا المشروع عبارة عن واجهة برمجة تطبيقات (API) خلفية (Backend) قوية وآمنة، مصممة بلغة C# باستخدام إطار عمل ASP.NET Core 8.0. تم بناء المشروع على أساس هندسة معمارية نظيفة متعددة الطبقات (3-Tier) لضمان فصل الاهتمامات، سهولة الصيانة، وقابلية التوسع.

<div align="center">
  <img src="data:image/png;base64,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" alt="AlaMashi API Logo" width="300" />
</div>

---

## 🚀 API مباشر وتوثيق

يمكنك اختبار الـ API مباشرة من خلال الرابط التالي، والاطلاع على التوثيق الكامل لفهم جميع نقاط النهاية (Endpoints).

-   **الرابط الأساسي للـ API:**
    [`https://alamashi-api.onrender.com`](https://alamashi-api.onrender.com)
    
-   **التوثيق الكامل للـ API:**
    [**API Documentation Link**](https://github.com/Omartube70/AlaMashi.API/blob/master/API_Documentation.md)

---

## ✨ الميزات الرئيسية

-   **🔐 نظام مصادقة آمن (JWT):**
    -   استخدام `AccessToken` قصير الصلاحية للوصول الآمن.
    -   تطبيق آلية `RefreshToken` لتجربة مستخدم سلسة دون الحاجة لتسجيل الدخول المتكرر.
    -   تشفير كلمات المرور باستخدام خوارزمية **BCrypt.Net** القوية.

-   **🛡️ صلاحيات وأدوار (Authorization):**
    -   نظام صلاحيات قائم على الأدوار (مثل `Admin` و `User`).
    -   حماية الـ Endpoints لضمان أن كل مستخدم يصل فقط إلى البيانات المسموح له بها.

-   **⚙️ إدارة كاملة للمستخدمين (CRUD):**
    -   إنشاء، قراءة، تحديث، وحذف المستخدمين.
    -   دعم التحديث الجزئي للبيانات باستخدام `JsonPatch`.

-   **🔑 إدارة كلمة المرور:**
    -   آلية "نسيت كلمة المرور" مع إرسال رابط آمن عبر البريد الإلكتروني.
    -   إمكانية إعادة تعيين كلمة المرور باستخدام توكن مؤقت.

-   **🏛️ بنية تحتية قوية:**
    -   تصميم احترافي بثلاث طبقات (API, BLL, DAL).
    -   معالج أخطاء مركزي (`Middleware`) لتوحيد شكل الاستجابات في حالة الفشل.
    -   استجابات API موحدة (`{status, data}`) لتسهيل التعامل معها في الواجهة الأمامية (Frontend).

-   **🐳 جاهز للنشر (Dockerized):**
    -   إعدادات متكاملة للـ **Docker**، مما يجعله جاهزًا للنشر الفوري على أي منصة تدعم الحاويات مثل **Render** أو **AWS**.

## 🛠️ التقنيات والمكتبات المستخدمة

-   **ASP.NET Core 8.0:** إطار العمل الرئيسي لبناء الـ API.
-   **ADO.NET:** للتعامل المباشر والفعال مع قاعدة البيانات.
-   **SQL Server:** قاعدة البيانات العلائقية لتخزين البيانات.
-   **JWT Bearer Authentication:** لتأمين الـ API.
-   **BCrypt.Net:** لتشفير كلمات المرور.
-   **JsonPatch:** لدعم عمليات التحديث الجزئي (HTTP PATCH).
-   **Docker:** لتهيئة بيئة التشغيل وتغليف التطبيق.

## 🏛️ هيكل المشروع

-   **`AlaMashi.API` (طبقة العرض):** مسؤولة عن استقبال طلبات HTTP، التحقق من صحة المدخلات (DTOs)، وتنسيق استجابات JSON.
-   **`AlaMashi.BLL` (طبقة منطق العمل):** تحتوي على القواعد والعمليات المنطقية للتطبيق. تقوم بتنسيق العمليات بين طبقة العرض وطبقة البيانات.
-   **`AlaMashi.DAL` (طبقة الوصول للبيانات):** مسؤولة عن كل عمليات الاتصال بقاعدة البيانات وتنفيذ استعلامات SQL بشكل آمن.

## 🏁 البدء والتشغيل المحلي

1.  **استنساخ المستودع:**
    ```bash
    git clone [https://github.com/Omartube70/AlaMashi.API.git](https://github.com/Omartube70/AlaMashi.API.git)
    cd AlaMashi.API
    ```

2.  **تكوين قاعدة البيانات:**
    -   قم بإنشاء قاعدة بيانات SQL Server.
    -   **مهم:** ستحتاج إلى تنفيذ السكربت الخاص بإنشاء الجداول. (يمكنك إنشاؤه من قاعدة البيانات الخاصة بك).
    -   افتح ملف `appsettings.json` وقم بتحديث `ConnectionStrings` ليتناسب مع إعداداتك.

3.  **تشغيل المشروع:**
    -   افتح الحل (`.sln`) في Visual Studio 2022.
    -   اضغط `F5` أو زر التشغيل الأخضر.
    -   سيتم فتح صفحة Swagger UI تلقائيًا في متصفحك.

## 📚 توثيق API

-   **التوثيق التفاعلي (محليًا):** يمكنك الوصول إلى وثائق Swagger UI التفاعلية على هذا المسار بعد تشغيل التطبيق:
    `https://localhost:xxxx/swagger` (استبدل xxxx بالبورت الخاص بك).
-   **التوثيق الثابت (على GitHub):**
    للاطلاع السريع على جميع الـ Endpoints، راجع [ملف توثيق الـ API](https://github.com/Omartube70/AlaMashi.API/blob/main/API_Documentation.md).

## المساهمة
نرحب بمساهماتك! إذا كان لديك أي اقتراحات أو كنت ترغب في تحسين الكود، فلا تتردد في فتح `pull request`.

## ترخيص
هذا المشروع مرخص بموجب [ترخيص MIT](https://github.com/Omartube70/AlaMashi.API/blob/master/LICENSE).
