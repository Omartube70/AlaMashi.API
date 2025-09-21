<div align="center"><img src="https://raw.githubusercontent.com/Omartube70/AlaMashi.API/master/AlaMashi.API/.assets/logo.png" alt="AlaMashi API Logo" width="250" /><h1 align="center">🔱 AlaMashi.API 🔱</h1><p align="center">واجهة خلفية احترافية لتطبيق سوبر ماركت حديث، مبنية على الهندسة النظيفة ومستضافة بالكامل على Azure.</p><!-- Shields --><p align="center"><img src="https://img.shields.io/badge/.NET-8.0-blueviolet?style=for-the-badge&logo=.net" alt=".NET 8.0"><img src="https://www.google.com/search?q=https://img.shields.io/badge/Architecture-Clean-orange%3Fstyle%3Dfor-the-badge%26logo%3Dc-sharp" alt="Clean Architecture"><img src="https://www.google.com/search?q=https://img.shields.io/badge/Deployment-Azure-0078D4%3Fstyle%3Dfor-the-badge%26logo%3Dmicrosoftazure" alt="Deployed to Azure"><img src="https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge" alt="License MIT"></p></div>🚀 API مباشر وتوثيقاستكشف الـ API بشكل تفاعلي من خلال واجهة Swagger على الرابط المباشر.الرابط الأساسي للـ API (Azure):https://alamashiapi.azurewebsites.netالتوثيق التفاعلي (Swagger UI):https://alamashiapi.azurewebsites.net/swaggerالتوثيق الكامل للـ API (Markdown):📄 ملف توثيق الـ API✨ الميزات الرئيسيةالأيقونةالميزةالوصف🏛️بنية نظيفة (Clean Architecture)فصل تام بين منطق العمل، قواعد التطبيق، والبنية التحتية لسهولة التطوير.🔐نظام مصادقة آمن (JWT)استخدام AccessToken و RefreshToken لتجربة مستخدم آمنة وسلسة.🛡️صلاحيات وأدوار (Authorization)نظام صلاحيات قائم على الأدوار لحماية نقاط النهاية (Endpoints).🛍️إدارة المنتجات والفئاتعمليات CRUD كاملة للمنتجات والفئات مع دعم لرفع الصور.🔑إدارة المستخدمين وكلمات المرورتسجيل، تسجيل دخول، تغيير كلمة المرور، وآلية "نسيت كلمة المرور" عبر البريد الإلكتروني.📤رفع ومعالجة الصوررفع الصور إلى Azure Blob Storage مع إمكانية تغيير حجمها على السيرفر.📧خدمة بريد إلكتروني احترافيةاستخدام Azure Communication Services لإرسال إيميلات موثوقة (مثل OTP).✔️التحقق من صحة المدخلات (Validation)استخدام FluentValidation لضمان صحة البيانات القادمة من العميل.🔀نمط CQRS و MediatRفصل أوامر الكتابة عن استعلامات القراءة لزيادة التنظيم والأداء.🏛️ هيكل المشروع (Clean Architecture)تم تصميم المشروع لضمان استقلالية طبقة منطق العمل عن التفاصيل التقنية، مما يجعل الكود نظيفًا وقابلاً للاختبار.<div dir="ltr">graph TD;
    A[WebAPI] --> B(Application);
    C[Infrastructure] --> B;
    B --> D{Domain};

    style A fill:#0078D4,stroke:#333,stroke-width:2px,color:#fff
    style B fill:#5C2D91,stroke:#333,stroke-width:2px,color:#fff
    style C fill:#00A4EF,stroke:#333,stroke-width:2px,color:#fff
    style D fill:#F25022,stroke:#333,stroke-width:2px,color:#fff

    subgraph " "
        direction LR
        A
    end
    subgraph " "
        direction LR
        B
        C
    end
    subgraph " "
        direction LR
        D
    end
</div>Domain (القلب): يحتوي على الـ Entities وقواعد العمل الأساسية. لا يعتمد على أي طبقة أخرى.Application (العقل): يحتوي على منطق التطبيق. ينسق بين الواجهة والبنية التحتية عبر الـ Commands, Queries, Handlers, و Interfaces.Infrastructure (الأيدي): يحتوي على التفاصيل التقنية والتنفيذ الفعلي للـ Interfaces، مثل الاتصال بقاعدة البيانات وخدمات Azure.WebAPI (الواجهة): نقطة الدخول للتطبيق. مسؤولة عن استقبال طلبات HTTP وتوجيهها لطبقة الـ Application.🛠️ التقنيات المستخدمةFramework: ASP.NET Core 8.0Architecture: Clean Architecture, CQRS PatternData Access: Entity Framework Core 8Database: Azure SQL DatabaseAuthentication: JWT Bearer AuthenticationValidation: FluentValidationImage Storage: Azure Blob StorageEmail Service: Azure Communication ServicesImage Processing: SixLabors.ImageSharpAPI Documentation: Swagger (Swashbuckle)🏁 البدء والتشغيل المحلي<details><summary><strong>1. استنساخ المستودع</strong></summary>git clone [https://github.com/Omartube70/AlaMashi.API.git](https://github.com/Omartube70/AlaMashi.API.git)
cd AlaMashi.API
</details><details><summary><strong>2. تكوين قاعدة البيانات</strong></summary>قم بإنشاء قاعدة بيانات SQL Server (محلية أو على Azure).افتح ملف appsettings.Development.json داخل مشروع الـ API وقم بتحديث ConnectionStrings ليتناسب مع إعداداتك.افتح نافذة Package Manager Console في Visual Studio.تأكد من أن المشروع الافتراضي هو Infrastructure أو Persistence.قم بتنفيذ الأمر التالي لتطبيق الـ Migrations وإنشاء الجداول:Update-Database
</details><details><summary><strong>3. تكوين الأسرار (Secrets) محليًا</strong></summary>للتشغيل المحلي، قم بتخزين المفاتيح الحساسة باستخدام .NET Secret Manager.افتح نافذة Terminal في مسار مشروع الـ API وقم بتنفيذ الأوامر التالية بعد استبدال القيم:dotnet user-secrets set "AzureBlobStorage:ConnectionString" "YOUR_BLOB_CONNECTION_STRING"
dotnet user-secrets set "AzureEmailSettings:ConnectionString" "YOUR_EMAIL_CONNECTION_STRING"
</details><details><summary><strong>4. تشغيل المشروع</strong></summary>افتح الحل (.sln) في Visual Studio 2022.اضغط F5 أو زر التشغيل الأخضر.سيتم فتح صفحة Swagger UI تلقائيًا في متصفحك.</details>☁️ ملاحظات النشر على Azureعند النشر على بيئة العمل (Production) في Azure، يتم تخزين كل الإعدادات الحساسة (Connection Strings والمفاتيح الأخرى) في App Service Configuration كمتغيرات بيئة، مما يضمن عدم وجود أي أسرار في الكود المصدري.🤝 المساهمةنرحب بمساهماتك! إذا كان لديك أي اقتراحات أو كنت ترغب في تحسين الكود، فلا تتردد في فتح pull request.📜 ترخيصهذا المشروع مرخص بموجب ترخيص MIT.
