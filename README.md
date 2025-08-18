# 🔱 AlaMashi.API

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://github.com/Omartube70/AlaMashi.API/blob/master/LICENSE)
[![Deployed on Render](https://img.shields.io/badge/Deployment-Render-00979D?style=for-the-badge&logo=render)](https://alamashi-api.onrender.com)

مرحبًا بك في مشروع **AlaMashi.API**! هذا المشروع عبارة عن واجهة برمجة تطبيقات (API) خلفية (Backend) قوية وآمنة، مصممة بلغة C# باستخدام إطار عمل ASP.NET Core 8.0. تم بناء المشروع على أساس هندسة معمارية نظيفة متعددة الطبقات (3-Tier) لضمان فصل الاهتمامات، سهولة الصيانة، وقابلية التوسع.

<div align="center">
  <img src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2023/11/Header_image_for_NET_8_Announce_Blog_Post-scaled.jpg" alt=".NET 8.0" width="600"/>
</div>

---

## 🚀 API مباشر وتوثيق

يمكنك اختبار الـ API مباشرة من خلال الرابط التالي، والاطلاع على التوثيق الكامل لفهم جميع نقاط النهاية (Endpoints).

-   **الرابط الأساسي للـ API:**
    [`https://alamashi-api.onrender.com`](https://alamashi-api.onrender.com)

-   **التوثيق الكامل للـ API:**
    [**API Documentation Link**](https://github.com/Omartube70/AlaMashi.API/blob/main/API_Documentation.md)

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
