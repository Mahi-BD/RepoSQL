# RepoSQL - Professional SQL Script Generator

![image](https://github.com/user-attachments/assets/07f9afd5-c2aa-4bd2-8141-2d3699919829)


A powerful, modern desktop application for generating SQL scripts and creating database backups with a professional UI/UX experience.

## 🚀 Features

### 📋 Script Generation
- **Table Creation Scripts** - Generate complete CREATE TABLE statements with proper data types, constraints, and identity columns
- **Data Export** - Export table data as INSERT statements (configurable row limits)
- **Views & Procedures** - Export views, stored procedures, and user-defined functions
- **Indexes & Triggers** - Generate index creation and trigger scripts
- **Permissions** - Export security and permission scripts

### 💾 Database Backup
- **Enhanced Backup Manager** - Intelligent backup with automatic directory detection
- **Multiple Backup Methods** - Standard method and SQLCMD fallback for permission issues
- **Compression Support** - Automatic ZIP compression with progress tracking
- **SQL Server Version Detection** - Adapts features based on SQL Server version and edition

### ⚙️ Advanced Configuration
- **INI-based Settings** - Persistent configuration with auto-save functionality
- **DROP Statement Generation** - Optional IF EXISTS DROP statements before CREATE
- **File Overwrite Control** - Configurable file overwrite behavior
- **Connection Profiles** - Save and load database connection profiles

### 🎨 Modern UI/UX
- **Professional Interface** - Clean, modern design with intuitive navigation
- **Real-time Progress** - Visual progress indicators for all operations
- **Smart Status Updates** - Color-coded button states and detailed status messages
- **Table Search & Filter** - Quick table search with pattern matching
- **Bulk Selection Tools** - Select all, select none, and invert selection

## 🛠️ Technical Specifications

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![VB.NET](https://img.shields.io/badge/VB.NET-Windows_Forms-blue?style=flat)
![SQL Server](https://img.shields.io/badge/SQL_Server-2012+-CC2927?style=flat&logo=microsoft-sql-server)
![Windows](https://img.shields.io/badge/Windows-10/11-0078D4?style=flat&logo=windows)

- **Framework**: .NET 8.0 Windows Forms
- **Language**: Visual Basic .NET
- **Database**: SQL Server 2012 and later
- **Authentication**: Windows Authentication & SQL Server Authentication
- **IDE**: Visual Studio 2022

## 📦 Dependencies

```xml
<PackageReference Include="Microsoft.Data.SqlClient" Version="6.0.2" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.5" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.5" />
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="System.Text.Json" Version="9.0.5" />
```

## 🚀 Getting Started

### Prerequisites
- Windows 10/11
- .NET 8.0 Runtime
- SQL Server 2012 or later
- Visual Studio 2022 (for development)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/RepoSQL.git
   ```
2. Open `RepoSQL.sln` in Visual Studio 2022
3. Build the solution (F6)
4. Run the application (F5)

### Quick Start
1. **Connect to Database**
   - Enter server name and database name
   - Choose authentication method (Windows or SQL Server)
   - Click "🔌 Connect" to test connection

2. **Load Tables**
   - Click "📋 Load Tables" to retrieve database objects
   - Use search box to filter tables
   - Select tables using checkboxes or bulk selection tools

3. **Configure Options**
   - Choose script types (Tables, Views, Procedures, etc.)
   - Enable "Include Table Data" for INSERT statements
   - Access advanced settings via Tools → Settings

4. **Generate Scripts**
   - Click "📁 Select Output" to choose destination folder
   - Click "🚀 Generate SQL" to create script files
   - Use "💾 Backup DB" for database backup with compression

## 📁 Generated Files

The application creates organized SQL files with standardized naming:

```
Output_Directory/
├── 01_TableCreate.sql      # Table creation scripts
├── 02_TableInsert.sql      # Table data (if enabled)
├── 03_Views.sql            # View definitions
├── 04_StoredProcedures.sql # Stored procedure scripts
├── 05_Functions.sql        # User-defined functions
├── 06_Indexes.sql          # Index creation scripts
├── 07_Triggers.sql         # Trigger scripts
└── 08_Permissions.sql      # Permission and security scripts
```

## ⚙️ Configuration

### Settings (Tools → Settings)
- **Generate DROP Statements**: Add IF EXISTS DROP statements before CREATE
- **Overwrite Existing Files**: Control file overwrite behavior without prompts

### INI Configuration (`sconfig.ini`)
Settings are automatically saved to `sconfig.ini` in the application directory:

```ini
[ServerConfig]
Server=YourServer
Database=YourDatabase
IntegratedSecurity=True

[ScriptOptions]
Tables=True
Views=False
StoredProcedures=False
IncludeData=False

[GenerationSettings]
GenerateDropStatements=False
OverwriteExistingFiles=True
```

## 🔧 Advanced Features

### Database Backup Manager
- **Smart Directory Detection**: Automatically finds SQL Server accessible directories
- **Permission Handling**: Handles SQL Server permission issues gracefully
- **Progress Tracking**: Real-time backup progress with percentage completion
- **Compression**: Automatic ZIP compression with size optimization
- **Fallback Methods**: SQLCMD alternative for problematic environments

### Enhanced Error Handling
- **SQL Error Translation**: User-friendly error messages with solutions
- **Connection Validation**: Comprehensive connection testing
- **File System Checks**: Automatic directory creation and permission validation

## 🎯 Use Cases

- **Database Migration**: Export complete database structure and data
- **Version Control**: Generate scripts for source control integration
- **Database Documentation**: Create comprehensive database documentation
- **Backup & Recovery**: Regular database backups with compression
- **Development**: Quick script generation for development environments
- **Schema Comparison**: Export schemas for comparison tools

## 📊 Performance

- **Async Operations**: Non-blocking UI during database operations
- **Memory Efficient**: Streaming data processing for large tables
- **Timeout Management**: Configurable timeouts for large operations
- **Progress Feedback**: Real-time progress updates for all operations

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/RepoSQL/issues)
- **Documentation**: Check the wiki for detailed documentation
- **Feature Requests**: Use GitHub Issues with enhancement label

## 🎉 Acknowledgments

- Built with love using Visual Basic .NET and Windows Forms
- Inspired by the need for a modern, user-friendly SQL script generator
- Thanks to the SQL Server development community for best practices

---

![Made with VB.NET](https://img.shields.io/badge/Made%20with-VB.NET-blue?style=flat&logo=dot-net)
![Platform](https://img.shields.io/badge/Platform-Windows-blue?style=flat&logo=windows)
![License](https://img.shields.io/badge/License-MIT-green?style=flat)

**RepoSQL v2.0** - Professional SQL Script Generator for Modern Databases
