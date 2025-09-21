Proyek Microservice Online Marketplace

Proyek ini adalah implementasi backend untuk sebuah online marketplace menggunakan arsitektur microservice, dibangun sebagai bagian dari Microservice Development Challenge.

📖 Deskripsi Singkat
Sistem ini terdiri dari tiga layanan utama yang independen:
•	UserService: Mengelola registrasi, login (menggunakan JWT), dan manajemen peran pengguna (Admin, Staff).
•	ProductService: Mengelola data produk (CRUD - Create, Read, Update, Delete).
•	CartService: Mengelola keranjang belanja pengguna dan proses checkout.

🛠️ Arsitektur & Teknologi
Berikut adalah tumpukan teknologi utama yang digunakan dalam proyek ini:
•	Framework: C# .NET 8 (ASP.NET Core Web API)
•	Arsitektur: Microservice
•	Akses Data: Entity Framework Core
•	Database: PostgreSQL
•	Containerization: Docker & Docker Compose
•	Autentikasi: JSON Web Tokens (JWT)
•	Dokumentasi API: Swagger (OpenAPI)

⚙️ Prasyarat
Sebelum memulai, pastikan Anda telah menginstal perangkat lunak berikut di mesin Anda:
•	Git
•	.NET SDK (versi 8.0 atau lebih baru)
•	Docker Desktop

📂 Struktur Proyek
OnlineMarketplaceMicroservices/
│── UserService/          # Layanan pengguna
│── ProductService/       # Layanan produk
│── CartService/          # Layanan keranjang belanja
│── docker-compose.yml    # File Docker Compose
│── .env.example          # Contoh environment variables
│── README.md             # Dokumentasi proyek

🔑 Konfigurasi Environment
Buat file .env di root proyek berdasarkan .env.example:
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres123
Catatan:
•   Database yang akan dibuat: users_db, products_db, carts_db (sudah disiapkan di docker-compose.yml).
•   Username & password diambil dari file .env.

🚀 Instalasi & Menjalankan Proyek
Untuk menginstal dan menjalankan seluruh layanan, ikuti langkah-langkah sederhana berikut:
1. Clone Repositori Buka terminal Anda dan clone repositori ini ke direktori lokal Anda:
Bash
git clone https://github.com/Tekkheng/OnlineMarketplaceMicroservices.git
2. Masuk ke Direktori Proyek
Bash
cd OnlineMarketplaceMicroservices
3. Jalankan dengan Docker Compose Jalankan perintah berikut dari direktori root proyek (folder yang berisi file docker-compose.yml). Perintah ini akan membangun image untuk setiap layanan dan menjalankan semua container secara bersamaan.

docker-compose up --build

Tunggu beberapa saat hingga semua layanan selesai dibangun dan berjalan. Anda akan melihat log dari setiap layanan muncul di terminal.

🕹️ Cara Menggunakan & Menguji API
Setelah semua layanan berjalan, Anda dapat mengakses dan menguji setiap API melalui antarmuka Swagger yang telah disediakan.
Buka browser Anda dan akses URL berikut:
•	UserService: http://localhost:8001/swagger
•	ProductService: http://localhost:8002/swagger
•	CartService: http://localhost:8003/swagger

Alur Pengujian yang Direkomendasikan:
1.	Buka Swagger UserService.
2.	Gunakan endpoint POST /api/auth/register untuk membuat pengguna baru dengan peran "Admin".
3.	Gunakan POST /api/auth/login untuk mendapatkan JWT Token.
4.	Salin token tersebut.
5.	Buka Swagger ProductService, klik tombol "Authorize", dan masukkan token dengan format 
Bearer [TOKEN_ANDA].
6.	Sekarang bisa mencoba endpoint yang memerlukan otorisasi, seperti POST /api/products untuk menambahkan produk.
7.  Tambahkan produk ke keranjang melalui CartService (POST /api/cart/items).

🗄️ Database & Migrasi

Masing-masing service memiliki database terpisah:
•   users_db → untuk UserService
•   products_db → untuk ProductService
•   carts_db → untuk CartService

Jika ingin menjalankan migrasi secara manual:
cd UserService
dotnet ef database update

cd ../ProductService
dotnet ef database update

cd ../CartService
dotnet ef database update

🛠️ Troubleshooting
•   Port 5432 sudah digunakan → matikan service Postgres lain atau ubah port di docker-compose.yml.
•   Error login → pastikan sudah register user baru dan gunakan JWT yang valid.
•   Database kosong → jalankan dotnet ef database update di masing-masing service.

