# NoviCode

**NoviCode** is a .NET 9 Web API application for managing wallets and real-time currency exchange rates, using a background service to fetch rates from the European Central Bank (ECB).

## 📸 API Usage Examples (Screenshots)

### Create Wallet

![Create Wallet](./b96018bc-920b-40aa-b255-4d760e291e4d.png)

### Retrieve Wallet in USD

![Retrieve Wallet in USD](./7c597df1-2fd3-4670-9648-306a8e3db9f4.png)

### Add Funds Strategy

![Add Funds Strategy](./9231e769-a1de-42d6-b4c5-8837fc8aed25.png)

### Subtract Funds Strategy

![Subtract Funds Strategy](./1ca2e9bf-cf00-4d2e-9871-d6971fef391c.png)

### Force Subtract Funds Strategy

![Force Subtract Funds Strategy](./131b3e3b-91f7-40e1-9e32-10fab97c1f9d.png)

### Check Balance After Adjustments

![Check Balance After Adjustments](./810dcf80-3bd2-4b7b-a379-78d69d489c21.png)

## 🚀 Tech Stack

- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core** (with SQL Server)
- **Hosted Background Services**
- **OpenAPI support** via Scalar.AspNetCore

## 📁 Project Structure

```text
NoviCode/
├── Controllers/           # Wallet API endpoints
├── Services/              # Business logic and background worker
├── DTO/                   # Data transfer objects
├── Entities/              # EF Core entities (Wallet, CurrencyRate)
├── Configurations/        # Fluent API configurations
├── NoviCodeDbContext/     # EF DbContext setup
├── Migrations/            # EF Core migrations
├── SQLBuilder/            # Merge SQL generator
├── appsettings.json       # Configuration
└── Program.cs             # Application entry point
```

## ⚙️ Features

- 💼 **Wallet Management** – Create, retrieve, and adjust wallet balances
- 💱 **Currency Conversion** – Converts wallet balances into requested currencies
- 🔁 **Background Rate Sync** – Automatically fetches and updates currency rates from the ECB
- 📄 **OpenAPI/Swagger UI** – Explore and test endpoints interactively

## 🛠️ Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/)
- SQL Server (or Azure SQL)

### Setup
```bash
git clone https://github.com/TheodoulosChristou/NoviCode.git
cd NoviCode/NoviCode
dotnet restore
dotnet ef database update
dotnet run
```

App will be available at: `https://localhost:7111`
Swagger docs: `https://localhost:7111/docs`

## 📬 API Endpoints

### Wallets

- `POST /api/wallets` – Create a wallet
- `GET /api/wallets/{walletId}?currency=EUR` – Retrieve wallet balance with optional conversion
- `POST /api/wallets/{walletId}/adjustbalance?amount=100&currency=USD&strategy=AddFundsStrategy` – Adjust wallet balance

## 🧠 Background Service

The `ECBCurrencyBackgroundService` fetches and updates `CurrencyRate` entities from ECB.

## 📌 Configuration

Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=NoviCodeDb;Trusted_Connection=True;"
  }
}
```

## 📄 License
This project is licensed under the MIT License.

## 🙋‍♂️ Author
**Theodoulos Christou**  
🔗 [GitHub Profile](https://github.com/TheodoulosChristou)
