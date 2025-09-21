🔱 AlaMashi.API

<div align="center">
<img src="https://raw.githubusercontent.com/Omartube70/AlaMashi.API/master/AlaMashi.API/.assets/logo.png" alt="AlaMashi API Logo" width="300" />
</div>

مرحبًا بك في مشروع AlaMashi.API! هذا المشروع هو الواجهة الخلفية (Backend) لتطبيق سوبر ماركت حديث، تم بناؤه باستخدام ASP.NET Core 8.0 وتطبيق مبادئ الهندسة النظيفة (Clean Architecture) لضمان أقصى درجات القابلية للصيانة، الاختبار، والتوسع.

🚀 API مباشر وتوثيق
يمكنك استكشاف وتوثيق الـ API بشكل تفاعلي من خلال واجهة Swagger على الرابط المباشر.

الرابط الأساسي للـ API (Azure):
https://alamashiapi.azurewebsites.net

التوثيق التفاعلي (Swagger UI):
https://alamashiapi.azurewebsites.net/swagger

التوثيق الكامل للـ API (Markdown):
API Documentation File

✨ الميزات الرئيسية
🏛️ بنية نظيفة (Clean Architecture): فصل تام بين منطق العمل (Domain)، قواعد التطبيق (Application)، والبنية التحتية (Infrastructure) لسهولة التطوير.

🔐 نظام مصادقة آمن (JWT): استخدام AccessToken و RefreshToken لتجربة مستخدم آمنة وسلسة.

🛡️ صلاحيات وأدوار (Authorization): نظام صلاحيات قائم على الأدوار لحماية نقاط النهاية (Endpoints).

🛍️ إدارة كاملة للمنتجات والفئات: عمليات CRUD كاملة للمنتجات والفئات مع دعم لرفع الصور.

🔑 إدارة متكاملة للمستخدمين وكلمات المرور: تسجيل، تسجيل دخول، تغيير كلمة المرور، وآلية "نسيت كلمة المرور" عبر البريد الإلكتروني.

📤 رفع ومعالجة الصور: رفع الصور إلى Azure Blob Storage مع إمكانية تغيير حجمها على السيرفر.

📧 خدمة إرسال بريد إلكتروني: استخدام Azure Communication Services لإرسال إيميلات موثوقة (مثل OTP لإعادة تعيين كلمة المرور).

✔️ التحقق من صحة المدخلات (Validation): استخدام FluentValidation لضمان صحة البيانات القادمة من العميل.

🔀 نمط CQRS و MediatR: فصل أوامر الكتابة (Commands) عن استعلامات القراءة (Queries) لزيادة التنظيم والأداء.

🛠️ التقنيات المستخدمة
Framework: ASP.NET Core 8.0

Architecture: Clean Architecture, CQRS Pattern

Data Access: Entity Framework Core 8

Database: Azure SQL Database

Authentication: JWT Bearer Authentication

Validation: FluentValidation

Image Storage: Azure Blob Storage

Email Service: Azure Communication Services

Image Processing: SixLabors.ImageSharp

API Documentation: Swagger (Swashbuckle)

🏛️ هيكل المشروع
Domain: يحتوي على الـ Entities وقواعد العمل الأساسية (Business Logic) التي لا تعتمد على أي شيء آخر.

Application: يحتوي على منطق التطبيق. ينسق بين الواجهة والبنية التحتية باستخدام الـ Commands, Queries, Handlers, DTOs, و Interfaces.

Infrastructure: يحتوي على التفاصيل التقنية والتنفيذ الفعلي للـ Interfaces المعرفة في طبقة الـ Application، مثل الاتصال بقاعدة البيانات، خدمات رفع الملفات، وخدمات إرسال الإيميل.

WebAPI (أو AlaMashi.API): نقطة الدخول للتطبيق. مسؤولة عن استقبال طلبات HTTP وتوجيهها إلى طبقة الـ Application عبر MediatR.

🏁 البدء والتشغيل المحلي
استنساخ المستودع:

git clone [https://github.com/Omartube70/AlaMashi.API.git](https://github.com/Omartube70/AlaMashi.API.git)
cd AlaMashi.API

تكوين قاعدة البيانات:

قم بإنشاء قاعدة بيانات SQL Server (محلية أو على Azure).

افتح ملف appsettings.Development.json داخل مشروع الـ API وقم بتحديث ConnectionStrings ليتناسب مع إعداداتك.

افتح نافذة Package Manager Console في Visual Studio.

تأكد من أن المشروع الافتراضي هو Infrastructure أو Persistence.

قم بتنفيذ الأمر التالي لتطبيق الـ Migrations وإنشاء الجداول:

Update-Database

تكوين الأسرار (Secrets):

للتشغيل المحلي، قم بتخزين المفاتيح الحساسة (مثل Connection String لخدمات Azure) باستخدام .NET Secret Manager.

افتح نافذة Terminal في مسار مشروع الـ API وقم بتنفيذ الأوامر التالية بعد استبدال القيم:

dotnet user-secrets set "AzureBlobStorage:ConnectionString" "YOUR_BLOB_CONNECTION_STRING"
dotnet user-secrets set "AzureEmailSettings:ConnectionString" "YOUR_EMAIL_CONNECTION_STRING"

تشغيل المشروع:

افتح الحل (.sln) في Visual Studio 2022.

اضغط F5 أو زر التشغيل الأخضر.

سيتم فتح صفحة Swagger UI تلقائيًا في متصفحك.

☁️ ملاحظات النشر على Azure
عند النشر على بيئة العمل (Production) في Azure، يتم تخزين كل الإعدادات الحساسة (Connection Strings والمفاتيح الأخرى) في App Service Configuration كمتغيرات بيئة (Environment Variables)، مما يضمن عدم وجود أي أسرار في الكود المصدري.

المساهمة
نرحب بمساهماتك! إذا كان لديك أي اقتراحات أو كنت ترغب في تحسين الكود، فلا تتردد في فتح pull request.

ترخيص
هذا المشروع مرخص بموجب ترخيص MIT.
