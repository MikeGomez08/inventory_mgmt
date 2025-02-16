# Simple Inventory Management System in C# .NET 8.0

## Overview
This is a **technical skills assessment** project for a QA specialist, built using **C# .NET 8.0**. The system provides basic inventory management functionalities such as adding, modifying, and removing products, as well as calculating the total inventory value.

## Features
- Add, remove, and modify products
- Retrieve a list of all products
- Calculate total inventory value
- Handle floating-point precision for price calculations
- Automated tests using NUnit and MSTest
- Performance testing included

## Installation
1. **Clone the repository**
   ```sh
   git clone https://github.com/formidablefrank/inventory_mgmt.git
   cd inventory_mgmt
   ```
2. **Install dependencies**
   Ensure you have **.NET 8.0 SDK** installed.
   ```sh
   dotnet restore
   ```
3. **Build the project**
   ```sh
   dotnet build
   ```
4. **Run the application**
   ```sh
   dotnet run
   ```

## Running Tests
### Unit Tests
This project uses **MSTest** and **NUnit** for automated testing.
```sh
dotnet test
```

### Performance Testing
Performance tests are implemented using NUnit to ensure efficient retrieval and calculations.
```sh
dotnet test --filter TestCategory=Performance
```

## Tools
| Area                   | Tool Used                                      |
|------------------------|----------------------------------------------|
| Test Management       | [Google Sheets](https://docs.google.com/spreadsheets/d/1Vst67mvOYJKGjpv7jdpM-lX8-CyVDwirIpfVKvP3MCk/edit?gid=1868055553#gid=1868055553) |
| Automation Testing    | **NUnit** with **C#** |
| Performance Testing   | **JMeter / NUnit Test 3** |
| Database Testing      | **SQL Server Management Studio** *(Not yet implemented)* |

## Documentation
- **Test Plan:** [Google Docs](https://docs.google.com/document/d/11pTuBtNRu-U5WS_FhjRVcxMaxq8FB6tqC_J9XCZEoLs/edit?tab=t.0)
- **Test Cases:** [Google Sheets](https://docs.google.com/spreadsheets/d/1Vst67mvOYJKGjpv7jdpM-lX8-CyVDwirIpfVKvP3MCk/edit?gid=1868055553#gid=1868055553)

## CI/CD Status
[![.NET](https://github.com/formidablefrank/inventory_mgmt/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/formidablefrank/inventory_mgmt/actions/workflows/dotnet.yml)

## License
This project is licensed under the **MIT License**.

## Contributing
Feel free to submit **pull requests** and **issues** for improvements or bug fixes.

