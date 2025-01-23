# Elasticsearch .NET Core Projesi

Bu proje, .NET Core kullanarak Elasticsearch üzerinde temel işlemleri gerçekleştirmek için örnek bir uygulamadır. 
Proje, Elasticsearch veritabanı üzerinde index oluşturma, döküman ekleme, güncelleme, silme ve arama işlemlerini içerir.


## Kullanılan Teknolojiler ve Paketler

- .NET Core
- Elastic.Clients.Elasticsearch
- Elastic.Transport
- Microsoft.Extensions.Options
- Bogus

## Temel Özellikler

### 1. Elasticsearch Temel İşlemler
- Index oluşturma
- Döküman ekleme (tekli ve toplu)
- Döküman güncelleme
- Döküman silme
- Döküman getirme

### 2. Arama Özellikleri
- Tek alan üzerinde arama
- Çoklu alan üzerinde arama
- Özelleştirilmiş filtreler:
  - Fiyat aralığı filtreleme
  - Metin bazlı arama (Match Query)
  - Tam eşleşme araması (Term Query)
  - Sıralama ve puanlama (RankFeature)
  - Filterler çoğaltılabilir.
	

### 3. Sayfalama
- Tüm sorgu sonuçlarında sayfalama desteği
- Sayfa başına sonuç sayısı ayarlama

## Mimari Yapı

- Service katmanında Elasticsearch işlemleri
- Controller katmanında API endpoint'leri
- Dependency Injection ile servis yönetimi
- Settings yönetimi için Options Pattern kullanımı
- Basit Result Pattern kullanımı (SearchResult)(Tüm endpointlere göre özelleştirilebilir)

## Konfigürasyon

Elasticsearch bağlantı ayarları:
- URL
- Varsayılan index
- Kullanıcı adı/şifre (opsiyonel)

## Örnek Kullanım Senaryoları

1. Ürün kataloğu yönetimi
2. Metin bazlı arama
3. Fiyat bazlı filtreleme
4. Çoklu alan araması
5. Toplu veri işlemleri

### FakeData üretme
Projede örnek veri oluşturmak için Bogus kütüphanesi kullanılmıştır. 
Bu datalar üzerinde search işlemleri gerçekleştirilebilir.

