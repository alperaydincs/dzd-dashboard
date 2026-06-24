# Parametrik Hale Getirilebilecek Alanlar — Çok Kiracılı (Multi-Tenant) & Çift Dil Yol Haritası

Amaç: Uygulamayı yalnızca DZD'ye özel değil, farklı şirketlerin de kullanabileceği,
yapılandırılabilir (parametrik) ve çift dilli (TR/EN) bir ürüne dönüştürmek.

Aşağıdaki bulgular kod tabanının taranmasıyla çıkarıldı. Her madde için: **Şu an**,
**Sorun**, **Öneri** ve tahmini **Efor** (S/M/L) verildi.

---

## 0. Temel: Multi-Tenancy (Kiracı/Şirket İzolasyonu) — L
**Şu an:** Tüm veriler tek şirket varsayımıyla; `Company` entity'si var ama bir
"tenant" kavramı yok. Tüm sorgular global. Auth tek Azure AD tenant'ına bağlı.
**Sorun:** Farklı şirketlerin verisi izole değil; ayarlar (adımlar, roller, lookuplar)
global.
**Öneri:** Bir `Tenant` (Organization) kavramı ekle. İki model var:
- **A) Tek veritabanı + TenantId kolonu** (satır bazlı izolasyon, global query filter). Daha ucuz, en yaygın KOBİ SaaS modeli. **Önerilen.**
- B) Kiracı başına ayrı veritabanı (daha izole, daha pahalı operasyon).
Tüm tenant-bağımlı entity'lere `TenantId` + EF `HasQueryFilter`. Checklist şablonları,
roller, lookup'lar, tema bunun üstüne oturur. **Bu, diğer her şeyin önkoşulu.**

## 1. Marka / Tema (Branding) — M
**Şu an:** Sabit logo dosyaları (`dzd_logo.svg`, `dzd_logo_small.svg`), `AppTheme`
içinde sabit renk paleti, sabit "DZD Dashboard" başlığı (~18 sabit referans).
**Öneri:** Kiracı başına: logo (yükleme), birincil/ikincil renk, uygulama adı,
favicon. `TenantBranding` tablosu + tema runtime'da kiracıdan üretilsin.

## 2. Çift Dil / Yerelleştirme (i18n TR/EN) — L
**Şu an:** Karışık dil — eski ekranlar İngilizce ("Active", "Employees", "On Leave"),
yeni onboarding/offboarding ekranları Türkçe (~85 sabit TR string). Tüm metinler
koda gömülü.
**Öneri:**
- .NET `IStringLocalizer` + `.resx` kaynak dosyaları (tr, en) veya JSON tabanlı sözlük.
- Kullanıcı/kiracı dil tercihi (profil + tarayıcı `Accept-Language` fallback).
- Tarih/sayı/para birimi biçimleri `CultureInfo` ile.
- **Not:** Checklist adım başlıkları artık DB'de (şablon) — bunlar için dil-bazlı
  başlık alanı (TitleTr/TitleEn) veya çeviri tablosu gerekir.
- Önce bir "string envanteri" çıkarıp tek tek kaynaklara taşımak gerekir (mekanik ama geniş).

## 3. Referans/Lookup Verileri — M
**Şu an:** Sabit kod constants: `Currencies` (TRY/USD/EUR), `PaymentPeriods`,
`BenefitTypes`, `DeductionTypes`, `AdditionalPaymentTypes`, `DependentTypes`.
Ayrıca DB lookup entity'leri: `DzdStatus`, `WorkType`, `IssueStatus/Type/Priority`,
`Resolution`, `JiraStatus` vb. (DZD'nin iş akışına özgü).
**Öneri:** Para birimleri, periyotlar, yan hak tipleri, kesinti tipleri → kiracı
yönetilebilir lookup tablolarına. "DZD"ye özgü statü/iş tipleri kiracı bazlı
seed + Settings'ten düzenlenebilir olmalı.

## 4. İş Kuralları / Politikalar — M
**Şu an:** Koda gömülü sabitler:
- `MaxBenefitDependents = 5`, `MaxFileSizeBytes = 20MB`, `AllowedMimeTypes`.
- Onboarding/offboarding gate kuralları (artık adımlar parametrik, ama gate mantığı
  kodda: payment-done önkoşulu, zorunlu adım atlanamaz).
- ÖSS/BES → Payment yansıma davranışı `BenefitKind`'e bağlı.
**Öneri:** Bağımlı limiti, dosya boyutu/izinli tipler kiracı ayarı olsun.
Gate davranışı şablon seviyesinde "bu adım şu adımlar bitmeden açılamaz" şeklinde
parametrik kurallara taşınabilir (ileri faz).

## 5. Roller & Yetkilendirme — M
**Şu an:** `Roles` sabit: yalnız `Admin`, `HR`. Azure AD app-rollerinden okunuyor.
**Öneri:** Çok kiracıda roller/izinler kiracı bazlı tanımlanabilir olmalı
(ör. "Manager", "Payroll", "Read-only"). En azından rol→izin haritası
yapılandırılabilir; ileri fazda tam RBAC.

## 6. Kimlik / Auth — L
**Şu an:** Tek Azure AD tenant (`TenantId` appsettings'te sabit). Giriş = kurumsal
Azure AD. Onboarding kullanıcı eşleştirme e-posta ile.
**Öneri:** Kiracı başına kimlik sağlayıcı yapılandırması (kendi Azure AD / Google /
e-posta+şifre). Multi-tenant OIDC. Bu, B2B SaaS için en kritik ve en zor parçalardan.

## 7. Belge Yönetimi / Saklama — S/M
**Şu an:** Dosyalar DB'de (`StoredFile`, byte[]). İzinli tipler ve boyut sabit.
**Öneri:** Kiracı bazlı izinli belge tipleri/limitler; ölçek için blob storage
(Azure Blob/S3) seçeneği. Zorunlu belge listesi (onboarding) — şu an serbest;
kiracı bazlı "zorunlu belge tipleri" tanımı eklenebilir.

## 8. Bildirimler / E-posta — M
**Şu an:** Uygulama içi bildirim merkezi var; e-posta/SMS entegrasyonu yok.
Onboarding daveti, düzeltme isteği gibi olaylar dış bildirim üretmiyor.
**Öneri:** Kiracı bazlı e-posta sağlayıcı/şablonları; aday daveti, düzeltme isteği,
süreç tamamlandı gibi olaylarda bildirim. Şablonlar da yerelleştirilebilir.

## 9. Organizasyon Yapısı Varsayımları — M
**Şu an:** Departman/Takım/Pozisyon/Kariyer yolu modeli DZD'nin yapısına göre
(grade, head/lead coefficient, project bonus vb. DZD'ye özgü kavramlar).
**Öneri:** Bazı kavramlar (grade katsayıları, proje bonus) opsiyonel/kiracı bazlı
modüllere alınmalı; her şirket bunları kullanmıyor.

## 10. Para Birimi & Bölgesel Biçim — S
**Şu an:** Para birimi listesi sabit; biçimlendirme `N2` + sabit kültür.
**Öneri:** Kiracı varsayılan para birimi/kültürü; çoklu para birimi gösterimi
zaten kısmen var (Payment summary). Bölgesel tarih/sayı biçimleri i18n ile birlikte.

---

## Önerilen Fazlama
1. **Temel multi-tenancy** (TenantId + query filter + tenant çözümleme). Her şeyin önkoşulu.
2. **i18n altyapısı** (IStringLocalizer + resx, dil tercihi) — sonra metinleri taşı.
3. **Kiracı yönetilebilir lookup/ayarlar** (para birimi, yan hak tipleri, politikalar, roller).
4. **Marka/tema** (logo, renk, ad).
5. **Çok kiracılı kimlik** (kiracı bazlı IdP).
6. **Bildirim/e-posta** + ileri RBAC + belge politikaları.

> En yüksek değer/risk: #1 (multi-tenancy) ve #2 (i18n). Diğerleri bunların üstüne
> kademeli eklenebilir. Checklist adımlarını zaten parametrik yaptık (Settings) —
> bu, #3'ün ilk parçası ve doğru yönde bir örnek.
