# FrontOffice - AppEnd Public Landing Page

این اپلیکیشن Front Office برای AppEnd است که یک صفحه عمومی و لندینگ پیج زیبا برای نمایش به کاربران عمومی ارائه می‌دهد.

## ویژگی‌ها

- ✅ صفحه Hero با معرفی AppEnd
- ✅ بخش Features با نمایش ویژگی‌های کلیدی
- ✅ بخش Products برای نمایش محصولات از دیتابیس
- ✅ بخش About Us
- ✅ بخش Testimonials
- ✅ فرم Contact Us برای ارسال پیام
- ✅ Footer با لینک‌های مفید
- ✅ طراحی Responsive و مدرن
- ✅ پشتیبانی از RTL/LTR
- ✅ دکمه Login برای ورود به Back Office

## راه‌اندازی

### 1. اجرای اسکریپت SQL

برای ایجاد جداول و داده‌های تست، اسکریپت SQL زیر را در دیتابیس AppEnd اجرا کنید:

```sql
-- فایل: workspace/server/CreateFrontOfficeTestData.sql
```

این اسکریپت جداول زیر را ایجاد می‌کند:
- `Products`: برای نمایش محصولات
- `Services`: برای نمایش خدمات
- `ContactMessages`: برای ذخیره پیام‌های تماس

### 2. دسترسی به FrontOffice

پس از راه‌اندازی، می‌توانید به آدرس زیر دسترسی داشته باشید:

```
http://localhost:5000/FrontOffice/
```

یا

```
http://your-domain/FrontOffice/
```

### 3. APIهای عمومی

FrontOffice از APIهای عمومی زیر استفاده می‌کند که در `appsettings.json` به عنوان `PublicMethods` تعریف شده‌اند:

- `DefaultRepo.PublicMethods.GetProductsList`: دریافت لیست محصولات
- `DefaultRepo.PublicMethods.GetServices`: دریافت لیست خدمات
- `DefaultRepo.PublicMethods.SendContactMessage`: ارسال پیام تماس

این متدها در فایل `workspace/server/DefaultRepo.PublicMethods.cs` تعریف شده‌اند.

## ساختار فایل‌ها

```
FrontOffice/
├── index.html          # صفحه اصلی HTML
├── app.json            # تنظیمات اپلیکیشن
├── assets/
│   ├── custom.css      # استایل‌های سفارشی
│   └── custom.js        # اسکریپت‌های سفارشی
└── components/
    ├── Home.vue         # صفحه اصلی (شامل همه بخش‌ها)
    ├── Hero.vue         # بخش Hero
    ├── Features.vue     # بخش Features
    ├── ProductsSection.vue  # بخش Products
    ├── AboutUs.vue      # بخش About Us
    ├── Testimonials.vue # بخش Testimonials
    ├── ContactForm.vue  # فرم تماس
    └── Footer.vue       # Footer
```

## سفارشی‌سازی

### تغییر رنگ‌ها

در فایل `assets/custom.css` می‌توانید متغیرهای CSS را تغییر دهید:

```css
:root {
    --fo-primary: #3674ff;
    --fo-gradient-start: #667eea;
    --fo-gradient-end: #764ba2;
}
```

### تغییر محتوا

- محتوای Features در `components/Features.vue`
- محتوای Testimonials در `components/Testimonials.vue`
- محتوای About Us در `components/AboutUs.vue`

### تغییر ترجمه‌ها

ترجمه‌ها در فایل `app.json` در بخش `translation` تعریف شده‌اند.

## نکات مهم

1. **دیتابیس**: اگر جداول Products و Services وجود نداشته باشند، APIها داده‌های نمونه را برمی‌گردانند.

2. **احراز هویت**: FrontOffice کاملاً عمومی است و نیاز به احراز هویت ندارد.

3. **Layout**: از Layout `FO.vue` استفاده می‌کند که در `a.Layouts/` قرار دارد.

4. **RPC**: از `rpcAEP` از `append-client.js` برای فراخوانی APIها استفاده می‌کند.

## عیب‌یابی

### محصولات نمایش داده نمی‌شوند

1. بررسی کنید که جدول Products در دیتابیس وجود دارد
2. اسکریپت SQL را اجرا کنید
3. Console مرورگر را برای خطاها بررسی کنید

### فرم تماس کار نمی‌کند

1. بررسی کنید که `DefaultRepo.PublicMethods.SendContactMessage` در `PublicMethods` در `appsettings.json` وجود دارد
2. بررسی کنید که `DynaCode.Refresh()` اجرا شده است
3. Console مرورگر را برای خطاها بررسی کنید

### استایل‌ها اعمال نمی‌شوند

1. بررسی کنید که فایل `assets/custom.css` لود شده است
2. Cache مرورگر را پاک کنید
3. بررسی کنید که فایل CSS در مسیر درست قرار دارد

## پشتیبانی

برای سوالات و مشکلات، به مستندات اصلی AppEnd مراجعه کنید:
- `مستندات-معماری-AppEnd.md`
- Wiki: https://github.com/mirshahreza/AppEnd/wiki


