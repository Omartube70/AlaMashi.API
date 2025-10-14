# 🔱 AlaMashi.API



[![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://github.com/Omartube70/AlaMashi.API/blob/master/LICENSE)


مرحبًا بك في مشروع **AlaMashi.API**! هذا المشروع عبارة عن واجهة برمجة تطبيقات (API) خلفية (Backend) قوية وآمنة، مصممة بلغة C# باستخدام إطار عمل ASP.NET Core 8.0. تم بناء المشروع على أساس **هندسة Clean Architecture** لضمان أقصى درجات فصل الاهتمامات (Separation of Concerns)، سهولة الصيانة، وقابلية التوسع والاختبار.



<div align="center">
  <img src="https://raw.githubusercontent.com/Omartube70/AlaMashi.API/master/AlaMashi.API/.assets/logo.png" alt="AlaMashi API Logo" width="300" />
</div>



---



## 🚀 API مباشر وتوثيق



يمكنك اختبار الـ API مباشرة من خلال الرابط التالي، والاطلاع على التوثيق الكامل لفهم جميع نقاط النهاية (Endpoints).



-   **الرابط الأساسي للـ API:**
`[https://alamashiapi.azurewebsites.net/swagger/index.html]`
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

-   **🏛️ بنية تحتية قوية (Clean Architecture):**
    -   تصميم احترافي يفصل بين منطق العمل، التطبيق، وقواعد البيانات.
    -   معالج أخطاء مركزي (`Middleware`) لتوحيد شكل الاستجابات في حالة الفشل.
    -   استجابات API موحدة (`ApiResponse`) لتسهيل التعامل معها في الواجهة الأمامية (Frontend).

-   **☁️ جاهز للنشر على السحابة:**
    -   تم إعداده للنشر بسهولة على منصات مثل **Azure App Service**.



## 🛠️ التقنيات والمكتبات المستخدمة



-   **ASP.NET Core 8.0:** إطار العمل الرئيسي لبناء الـ API.
-   **Entity Framework Core 8.0:** للتعامل مع قاعدة البيانات باستخدام (ORM).
-   **SQL Server / Azure SQL Database:** قاعدة البيانات العلائقية لتخزين البيانات.
-   **JWT Bearer Authentication:** لتأمين الـ API.
-   **BCrypt.Net:** لتشفير كلمات المرور.
-   **JsonPatch:** لدعم عمليات التحديث الجزئي (HTTP PATCH).
-   **AutoMapper:** لتبسيط عملية تحويل البيانات بين الطبقات (DTOs & Entities).
-   **FluentValidation:** للتحقق من صحة المدخلات بشكل منظم.



## 🏛️ هيكل المشروع (Clean Architecture)



-   **`Core` (الطبقة الأساسية):** تحتوي على الـ Entities (نماذج البيانات) والواجهات (Interfaces) الأساسية للمشروع. هذه الطبقة ليس لها أي اعتماديات خارجية.
-   **`Infrastructure` (طبقة البنية التحتية):** مسؤولة عن تطبيق الواجهات الموجودة في طبقة Core، مثل التعامل مع قاعدة البيانات باستخدام EF Core والخدمات الخارجية.
-   **`Application` (طبقة التطبيق):** تحتوي على منطق العمل (Business Logic)، أوامر (Commands)، استعلامات (Queries)، و DTOs. تقوم بتنسيق العمليات بين الواجهة وقاعدة البيانات.
-   **`API` (طبقة العرض):** مسؤولة عن استقبال طلبات HTTP، تمريرها إلى طبقة Application، وتنسيق استجابات JSON النهائية.



## 🏁 البدء والتشغيل المحلي



1.  **استنساخ المستودع:**
    ```bash
    git clone [https://github.com/Omartube70/AlaMashi.API.git](https://github.com/Omartube70/AlaMashi.API.git)
    cd AlaMashi.API
    ```

2.  **تكوين قاعدة البيانات:**
    -   قم بإنشاء قاعدة بيانات فارغة على SQL Server المحلي أو على Azure.
    -   في مشروع `AlaMashi.API`، افتح ملف `appsettings.Development.json`.
    -   قم بتحديث `ConnectionStrings` ليتناسب مع إعدادات قاعدة البيانات الخاصة بك.

3.  **تطبيق تهجير قاعدة البيانات (Migrations):**
    -   افتح نافذة `Package Manager Console` في Visual Studio.
    -   تأكد من اختيار مشروع `AlaMashi.Infrastructure` كـ "Default project".
    -   نفّذ الأمر التالي لإنشاء الجداول في قاعدة البيانات:
    ```powershell
    Update-Database
    ```

4.  **تشغيل المشروع:**
    -   افتح الحل (`.sln`) في Visual Studio 2022 أو أحدث.
    -   اضغط `F5` أو زر التشغيل الأخضر.
    -   سيتم فتح صفحة Swagger UI تلقائيًا في متصفحك.



## 📚 توثيق API



-   **التوثيق التفاعلي (محليًا):** يمكنك الوصول إلى وثائق Swagger UI التفاعلية على هذا المسار بعد تشغيل التطبيق:
    `https://localhost:xxxx/swagger` (استبدل xxxx بالبورت الخاص بك).

-   **التوثيق الثابت (على GitHub):**
    للاطلاع السريع على جميع الـ Endpoints، راجع [ملف توثيق الـ API](https://github.com/Omartube70/AlaMashi.API/blob/master/API_Documentation.md).



## المساهمة

نرحب بمساهماتك! إذا كان لديك أي اقتراحات أو كنت ترغب في تحسين الكود، فلا تتردد في فتح `pull request`.



## ترخيص

هذا المشروع مرخص بموجب [ترخيص MIT](https://github.com/Omartube70/AlaMashi.API/blob/master/LICENSE).
