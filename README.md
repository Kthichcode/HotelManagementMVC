# Hotel Management System

Hệ thống quản lý khách sạn được xây dựng bằng ASP.NET Core MVC 8.0, áp dụng kiến trúc đa lớp (Multi-layer Architecture) với các tính năng quản lý phòng, đặt phòng, thanh toán và đánh giá.

## 📋 Mục lục

- [Tổng quan](#tổng-quan)
- [Kiến trúc dự án](#kiến-trúc-dự-án)
- [Tính năng](#tính-năng)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Cài đặt và chạy dự án](#cài-đặt-và-chạy-dự-án)
- [Cấu hình](#cấu-hình)
- [Tài khoản mặc định](#tài-khoản-mặc-định)
- [Cấu trúc thư mục](#cấu-trúc-thư-mục)
- [Database Schema](#database-schema)

## 🎯 Tổng quan

Hệ thống quản lý khách sạn cung cấp đầy đủ các chức năng cần thiết cho việc quản lý hoạt động của một khách sạn, bao gồm:

- Quản lý phòng và loại phòng
- Đặt phòng trực tuyến
- Thanh toán qua VNPay
- Quản lý ví điện tử
- Đánh giá và nhận xét
- Dashboard thống kê
- Phân quyền người dùng

## 🏗️ Kiến trúc dự án

Dự án được tổ chức theo kiến trúc đa lớp (Layered Architecture) với các lớp sau:

```
HotelManagementMVC/
├── BusinessObjects/          # Lớp Domain Models
│   ├── Entities/            # Các entity models
│   └── Enums/               # Các enum định nghĩa
├── DataAccessObjects/       # Lớp Data Access (DbContext)
├── Repositories/            # Lớp Repository Pattern
│   └── Interfaces/          # Repository interfaces
├── Services/                # Lớp Business Logic
│   ├── Interfaces/          # Service interfaces
│   └── DTOs/                # Data Transfer Objects
└── HotelManagementMVC/      # Lớp Presentation (MVC)
    ├── Controllers/         # Controllers
    ├── Views/               # Razor Views
    ├── Models/              # ViewModels
    └── Data/                # DbSeeder
```

### Luồng xử lý

```
Controller → Service → Repository → DbContext → Database
```

## ✨ Tính năng

### 1. Quản lý người dùng và phân quyền
- Đăng ký, đăng nhập, đăng xuất
- Phân quyền theo vai trò: **Admin**, **Manager**, **Staff**, **Customer**
- Quản lý profile người dùng

### 2. Quản lý phòng
- **Quản lý loại phòng (Room Types)**
  - Thêm, sửa, xóa loại phòng
  - Đặt giá theo đêm
  - Mô tả chi tiết
  
- **Quản lý phòng (Rooms)**
  - CRUD phòng
  - Upload nhiều ảnh cho mỗi phòng
  - Quản lý trạng thái phòng (Available, Maintenance)
  - Tìm kiếm và lọc phòng

### 3. Đặt phòng (Booking)
- Tìm kiếm phòng theo ngày check-in/check-out
- Đặt phòng trực tuyến
- Quản lý trạng thái đặt phòng:
  - `Pending` - Chờ xác nhận
  - `Confirmed` - Đã xác nhận
  - `CheckedIn` - Đã check-in
  - `Completed` - Hoàn thành
  - `Cancelled` - Đã hủy
- Xem lịch sử đặt phòng

### 4. Thanh toán (Payment)
- Thanh toán bằng tiền mặt
- Tích hợp **VNPay** payment gateway
- Quản lý trạng thái thanh toán:
  - `Pending` - Chờ thanh toán
  - `Paid` - Đã thanh toán
  - `Failed` - Thanh toán thất bại
- Lịch sử giao dịch

### 5. Ví điện tử (Wallet)
- Quản lý số dư ví
- Nạp tiền vào ví
- Thanh toán bằng ví

### 6. Đánh giá (Review)
- Khách hàng có thể đánh giá sau khi hoàn thành đặt phòng
- Đánh giá từ 1-5 sao
- Nhận xét chi tiết

### 7. Dashboard
- Thống kê tổng quan:
  - Tổng số phòng, phòng trống, phòng bảo trì
  - Số đặt phòng hôm nay, tháng này
  - Doanh thu hôm nay, tháng này
  - Top loại phòng được đặt nhiều nhất
- Chỉ dành cho **Admin** và **Manager**

### 8. Quản lý đặt phòng (Booking Management)
- Xem danh sách đặt phòng với bộ lọc:
  - Lọc theo ngày
  - Lọc theo trạng thái
  - Tìm kiếm theo số điện thoại
- Cập nhật trạng thái đặt phòng
- Xem chi tiết đặt phòng

## 🛠️ Công nghệ sử dụng

- **.NET 8.0** - Framework chính
- **ASP.NET Core MVC** - Web framework
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Database
- **ASP.NET Core Identity** - Authentication & Authorization
- **VNPay SDK** - Payment gateway integration
- **Bootstrap** - UI framework (có thể có trong wwwroot/lib)

## 💻 Yêu cầu hệ thống

- **.NET 8.0 SDK** hoặc cao hơn
- **SQL Server** (LocalDB hoặc SQL Server Express/Full)
- **Visual Studio 2022** hoặc **Visual Studio Code** với C# extension
- **Git** (tùy chọn)

## 🚀 Cài đặt và chạy dự án

### Bước 1: Clone repository

```bash
git clone <repository-url>
cd HotelManagementMVC
```

### Bước 2: Cấu hình Connection String

Mở file `HotelManagementMVC/HotelManagementMVC/appsettings.json` và cập nhật connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=HotelManagementDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

**Lưu ý:** Thay `YourPassword` bằng mật khẩu SQL Server của bạn.

### Bước 3: Cấu hình VNPay (Tùy chọn)

Nếu muốn sử dụng tính năng thanh toán VNPay, cập nhật trong `appsettings.json`:

```json
{
  "VnPay": {
    "TmnCode": "YOUR_TMN_CODE",
    "HashSecret": "YOUR_HASH_SECRET",
    "BaseUrl": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
    "ReturnUrl": "https://localhost:7073/Bookings/PaymentCallback"
  }
}
```

### Bước 4: Restore packages và build

```bash
dotnet restore
dotnet build
```

### Bước 5: Tạo database và chạy migrations

```bash
# Di chuyển đến thư mục chứa DbContext
cd HotelManagementMVC/Repositories

# Tạo migration (nếu chưa có)
dotnet ef migrations add InitialCreate --startup-project ../HotelManagementMVC

# Cập nhật database
dotnet ef database update --startup-project ../HotelManagementMVC
```

**Hoặc** nếu DbContext nằm trong DataAccessObjects:

```bash
cd HotelManagementMVC/DataAccessObjects
dotnet ef migrations add InitialCreate --startup-project ../HotelManagementMVC
dotnet ef database update --startup-project ../HotelManagementMVC
```

### Bước 6: Chạy ứng dụng

```bash
cd HotelManagementMVC/HotelManagementMVC
dotnet run
```

Hoặc nhấn **F5** trong Visual Studio.

Ứng dụng sẽ chạy tại: `https://localhost:7073` hoặc `http://localhost:5000`

## ⚙️ Cấu hình

### Connection String

Cấu hình trong `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HotelManagementDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
}
```

### VNPay Configuration

Cấu hình VNPay trong `appsettings.json`:

- `TmnCode`: Mã terminal của bạn
- `HashSecret`: Secret key để hash dữ liệu
- `BaseUrl`: URL của VNPay gateway (sandbox hoặc production)
- `ReturnUrl`: URL callback sau khi thanh toán

## 👤 Tài khoản mặc định

Hệ thống tự động tạo các tài khoản mặc định khi khởi động lần đầu:

| Vai trò | Username | Password | Mô tả |
|---------|----------|----------|-------|
| **Admin** | `Admin` | `Admin@123` | Quản trị viên hệ thống |
| **Manager** | `Manager` | `Manager@123` | Quản lý khách sạn |
| **Staff** | `Staff` | `Staff@123` | Nhân viên |

**Lưu ý:** Đổi mật khẩu ngay sau lần đăng nhập đầu tiên trong môi trường production!

## 📁 Cấu trúc thư mục

```
HotelManagementMVC/
├── BusinessObjects/              # Domain Layer
│   ├── Entities/                 # Entity models
│   │   ├── ApplicationUser.cs
│   │   ├── Booking.cs
│   │   ├── BookingRoom.cs
│   │   ├── Payment.cs
│   │   ├── Review.cs
│   │   ├── Room.cs
│   │   ├── RoomImage.cs
│   │   ├── RoomType.cs
│   │   └── Wallet.cs
│   └── Enums/                    # Enumerations
│       ├── BookingStatus.cs
│       ├── PaymentStatus.cs
│       └── RoomStatus.cs
│
├── DataAccessObjects/            # Data Access Layer
│   ├── AppDbContext.cs
│   └── Migrations/               # EF Core migrations
│
├── Repositories/                 # Repository Layer
│   ├── Interfaces/               # Repository interfaces
│   ├── BookingRepository.cs
│   ├── PaymentRepository.cs
│   ├── ReviewRepository.cs
│   ├── RoomImageRepository.cs
│   ├── RoomRepository.cs
│   ├── RoomTypeRepository.cs
│   ├── UserRepository.cs
│   └── WalletRepository.cs
│
├── Services/                     # Business Logic Layer
│   ├── Interfaces/               # Service interfaces
│   ├── DTOs/                     # Data Transfer Objects
│   ├── AccountService.cs
│   ├── BookingService.cs
│   ├── DashboardService.cs
│   ├── ReviewService.cs
│   ├── RoomService.cs
│   ├── RoomTypeService.cs
│   ├── VnPayService.cs
│   └── WalletService.cs
│
└── HotelManagementMVC/           # Presentation Layer
    ├── Controllers/              # MVC Controllers
    │   ├── AccountController.cs
    │   ├── BookingManagementController.cs
    │   ├── BookingsController.cs
    │   ├── DashboardController.cs
    │   ├── HomeController.cs
    │   ├── ReviewsController.cs
    │   ├── RoomsController.cs
    │   ├── RoomsManagementController.cs
    │   ├── RoomTypesController.cs
    │   ├── UsersController.cs
    │   └── WalletsController.cs
    │
    ├── Views/                    # Razor Views
    │   ├── Account/
    │   ├── BookingManagement/
    │   ├── Bookings/
    │   ├── Dashboard/
    │   ├── Home/
    │   ├── Reviews/
    │   ├── Rooms/
    │   ├── RoomsManagement/
    │   ├── RoomTypes/
    │   ├── Shared/
    │   ├── Users/
    │   └── Wallets/
    │
    ├── Models/                   # ViewModels
    ├── Data/                     # DbSeeder
    ├── ValidationAttributes/     # Custom validation
    ├── wwwroot/                  # Static files
    │   ├── css/
    │   ├── js/
    │   ├── lib/                  # Third-party libraries
    │   └── uploads/              # Uploaded images
    │
    ├── Program.cs                # Application entry point
    └── appsettings.json          # Configuration
```

## 🗄️ Database Schema

### Các bảng chính:

- **AspNetUsers** - Người dùng (mở rộng từ Identity)
- **AspNetRoles** - Vai trò
- **AspNetUserRoles** - Phân quyền người dùng
- **RoomTypes** - Loại phòng
- **Rooms** - Phòng
- **RoomImages** - Ảnh phòng
- **Bookings** - Đặt phòng
- **BookingRooms** - Chi tiết phòng trong đặt phòng
- **Payments** - Thanh toán
- **Wallets** - Ví điện tử
- **Reviews** - Đánh giá

### Quan hệ:

- `Room` → `RoomType` (Many-to-One)
- `Room` → `RoomImage[]` (One-to-Many)
- `Booking` → `ApplicationUser` (Many-to-One)
- `Booking` → `BookingRoom[]` → `Room[]` (Many-to-Many)
- `Booking` → `Payment[]` (One-to-Many)
- `Booking` → `Review` (One-to-One)
- `Wallet` → `ApplicationUser` (Many-to-One)

## 🔐 Phân quyền

### Admin
- Toàn quyền quản lý hệ thống
- Quản lý người dùng
- Xem dashboard
- Quản lý phòng và loại phòng
- Quản lý đặt phòng

### Manager
- Xem dashboard
- Quản lý phòng và loại phòng
- Quản lý đặt phòng
- Xem báo cáo

### Staff
- Quản lý đặt phòng (cập nhật trạng thái)
- Xem thông tin phòng

### Customer
- Tìm kiếm và xem phòng
- Đặt phòng
- Thanh toán
- Quản lý ví
- Đánh giá

## 📝 Ghi chú

- Database sẽ được tự động seed với các tài khoản mặc định khi ứng dụng khởi động lần đầu
- Ảnh phòng được lưu trong thư mục `wwwroot/uploads/rooms/`
- VNPay đang sử dụng môi trường sandbox, cần cấu hình lại cho production
- Đảm bảo SQL Server đang chạy trước khi start ứng dụng

## 🤝 Đóng góp

Mọi đóng góp đều được chào đón! Vui lòng tạo issue hoặc pull request.

## 📄 License

Dự án này được phát triển cho mục đích học tập và nghiên cứu.

---

**Phát triển bởi:** PRN222 Team 2
**Framework:** ASP.NET Core MVC 8.0  
**Database:** SQL Server
