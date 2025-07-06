# TaskTracker API

## Proje Yapısı

Proje Clean Architecture prensiplerine uygun olarak katmanlı mimari ile geliştirilmiştir:

- **TaskTracker.API**: Web API katmanı, controller'lar ve middleware'ler
- **TaskTracker.Application**: İş mantığı katmanı, CQRS pattern ile MediatR kullanımı
- **TaskTracker.Domain**: Domain entities ve repository interfaces
- **TaskTracker.Infrastructure**: Veritabanı işlemleri ve external servisler
- **TaskTracker.Core**: Ortak kullanılan utilities ve base sınıflar

## Kullanılan Teknolojiler

### Backend Framework
- **.NET 8.0**: Modern .NET platformu
- **ASP.NET Core**: Web API framework

### Veritabanı
- **Entity Framework Core 8.0**: ORM framework
- **In-Memory Database**: Test amaçlı basit bir uygulama

### Authentication & Authorization
- **JWT (JSON Web Tokens)**: Stateless authentication
- **JWT Bearer Authentication**: Token tabanlı güvenlik
- Login ve Register basic seviyede tutuldu

### Validation & Business Logic
- **FluentValidation**: Input validation
- **MediatR**: MediatR kullanıldı

### Logging & Monitoring
- **Serilog**: Structured logging
- **Middleware**: Exception handling ve rate limiting

### API Documentation
- **Swagger/OpenAPI**: API dokümantasyonu
- **XML Documentation**: Code documentation

### Development Tools
- **CORS**: Cross-origin resource sharing
- **Rate Limiting**: API rate limiting middleware
- **Exception Middleware**: Global exception handling

## Özellikler

### Kullanıcı Yönetimi
- Kullanıcı kaydı ve girişi
- JWT token tabanlı authentication
- Şifre hash'leme ve salt'lama
- Kullanıcı profil bilgileri

### Görev Yönetimi
- Görev oluşturma ve düzenleme
- Görev durumu takibi (tamamlandı/beklemede)
- Görev başlığı güncelleme
- Kullanıcıya özel görev listesi
- Görev silme işlemleri

### Güvenlik
- JWT token authentication
- Şifre güvenliği (hash + salt)
- Rate limiting
- CORS policy
- Global exception handling

## API Endpoints

### Authentication
- `POST /api/auth/register` - Kullanıcı kaydı
- `POST /api/auth/login` - Kullanıcı girişi

### Tasks
- `GET /api/tasks` - Kullanıcının görevlerini listele
- `POST /api/tasks` - Yeni görev oluştur
- `PUT /api/tasks/{id}/title` - Görev başlığını güncelle
- `PUT /api/tasks/{id}/status` - Görev durumunu güncelle
- `DELETE /api/tasks/{id}` - Görev sil

## Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8.0 SDK

### Environment Variables
```bash
JWT_SECRET_KEY=your-secret-key-here
ASPNETCORE_ENVIRONMENT=Development
- Örnek olması açısından 2 adet env şeklinde tasarlandı.
```

## Seed Data

Uygulama başlatıldığında otomatik olarak aşağıdaki test verileri oluşturulur:

### Varsayılan Kullanıcı
- **Username**: admin
- **Email**: admin@tasktracker.com
- **Password**: Admin123!
- **Ad**: Admin
- **Soyad**: User

### Varsayılan Görevler
1. **Görev 1**: "Proje planlaması yap" (Beklemede)
2. **Görev 2**: "API dokümantasyonu hazırla" (Tamamlandı)

Bu veriler sadece veritabanı boş olduğunda oluşturulur. Mevcut veriler varsa seed işlemi atlanır.

## Mimari Özellikler

### Clean Architecture
- Separation of concerns
- Dependency inversion
- Testable code structure

### CQRS Pattern
- Command ve Query separation
- MediatR ile handler pattern
- Validation pipeline

### Repository Pattern
- Generic repository implementation
- Unit of Work pattern
- Async/await support

### Middleware Pipeline
- Exception handling
- Rate limiting
- CORS policy
- Authentication

## Geliştirme Notları

- Proje .NET 8.0 ile geliştirilmiştir
- Nullable reference types aktif
- XML documentation destekli
- Structured logging kullanılmaktadır
- In-memory database ile hızlı geliştirme
- Production için SQLite veya SQL Server kullanılabilir

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir. 