USE [master]
GO
/****** Object:  Database [mytestdb]    Script Date: 3/29/2022 11:40:58 PM ******/
CREATE DATABASE [mytestdb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'mytestdb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\mytestdb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'mytestdb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\mytestdb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [mytestdb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [mytestdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [mytestdb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [mytestdb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [mytestdb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [mytestdb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [mytestdb] SET ARITHABORT OFF 
GO
ALTER DATABASE [mytestdb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [mytestdb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [mytestdb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [mytestdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [mytestdb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [mytestdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [mytestdb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [mytestdb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [mytestdb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [mytestdb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [mytestdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [mytestdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [mytestdb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [mytestdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [mytestdb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [mytestdb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [mytestdb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [mytestdb] SET RECOVERY FULL 
GO
ALTER DATABASE [mytestdb] SET  MULTI_USER 
GO
ALTER DATABASE [mytestdb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [mytestdb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [mytestdb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [mytestdb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [mytestdb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [mytestdb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'mytestdb', N'ON'
GO
ALTER DATABASE [mytestdb] SET QUERY_STORE = OFF
GO
USE [mytestdb]
GO
/****** Object:  Table [dbo].[machine_component]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[machine_component](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[machineName] [nvarchar](100) NOT NULL,
	[other_machine] [int] NOT NULL,
	[worksite] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_assignement_team_pos]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_assignement_team_pos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[username] [nvarchar](25) NOT NULL,
	[productionline] [int] NOT NULL,
	[po] [nvarchar](50) NOT NULL,
	[shift] [nvarchar](50) NOT NULL,
	[worksite] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_downtimeReason]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_downtimeReason](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[reason] [nvarchar](50) NOT NULL,
	[downtimeType] [nvarchar](50) NOT NULL,
	[worksite] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_formats]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_formats](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[format] [nvarchar](15) NOT NULL,
	[shape] [nvarchar](40) NOT NULL,
	[mat1] [nvarchar](15) NOT NULL,
	[mat2] [nvarchar](15) NOT NULL,
	[mat3] [nvarchar](15) NOT NULL,
	[design_rate] [int] NOT NULL,
	[productionlineName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_machines]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_machines](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[operation] [nvarchar](50) NOT NULL,
	[fabricant] [nvarchar](100) NOT NULL,
	[modele] [nvarchar](100) NOT NULL,
	[productionline_name] [nvarchar](100) NOT NULL,
	[denomination_ordre] [nvarchar](75) NOT NULL,
	[ordre] [int] NOT NULL,
	[rejection] [int] NOT NULL,
	[worksite] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_planned_events]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_planned_events](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[OLE] [nvarchar](50) NOT NULL,
	[productionline] [nvarchar](50) NOT NULL,
	[reason] [nvarchar](100) NOT NULL,
	[duration] [int] NOT NULL,
	[comment] [text] NULL,
	[kind] [int] NOT NULL,
	[shift] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_pos]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_pos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[number] [nvarchar](50) NOT NULL,
	[GMIDCode] [nvarchar](50) NOT NULL,
	[productionline_name] [nvarchar](50) NOT NULL,
	[state] [int] NOT NULL,
	[totalOperatingTime] [int] NOT NULL,
	[totalNetOperatingTime] [int] NOT NULL,
	[Availability] [real] NOT NULL,
	[Performance] [real] NOT NULL,
	[Quality] [real] NOT NULL,
	[OLE] [real] NOT NULL,
	[qtyProduced] [int] NOT NULL,
	[workingDuration] [int] NOT NULL,
	[shift] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_productionline]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_productionline](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[productionline_name] [nvarchar](50) NOT NULL,
	[worksite_name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_products]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product] [nvarchar](150) NOT NULL,
	[GMID] [nvarchar](50) NOT NULL,
	[BULK] [nvarchar](50) NOT NULL,
	[family] [nvarchar](150) NOT NULL,
	[GIFAP] [nvarchar](50) NOT NULL,
	[description] [nvarchar](150) NOT NULL,
	[formulationType] [nvarchar](150) NOT NULL,
	[size] [nvarchar](20) NOT NULL,
	[idealRate] [real] NOT NULL,
	[bottlesPerCase] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_rejection_counters]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_rejection_counters](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[po] [nvarchar](50) NULL,
	[fillerCounter] [int] NOT NULL,
	[caperCounter] [int] NOT NULL,
	[labelerCounter] [int] NOT NULL,
	[weightBoxCounter] [int] NOT NULL,
	[qualityControlCounter] [int] NOT NULL,
	[fillerRejection] [int] NOT NULL,
	[caperRejection] [int] NOT NULL,
	[labelerRejection] [int] NOT NULL,
	[weightBoxRejection] [int] NOT NULL,
	[qualityControlRejection] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_speed_losses]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_speed_losses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[OLE] [nvarchar](50) NOT NULL,
	[productionline] [nvarchar](50) NOT NULL,
	[duration] [int] NOT NULL,
	[reason] [nvarchar](150) NOT NULL,
	[comment] [text] NULL,
	[shift] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_unplanned_event_changing_clients]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_unplanned_event_changing_clients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[OLE] [nvarchar](50) NOT NULL,
	[productionline] [nvarchar](50) NOT NULL,
	[predicted_duration] [int] NOT NULL,
	[total_duration] [int] NOT NULL,
	[lot_number] [nvarchar](50) NOT NULL,
	[comment] [text] NULL,
	[type] [nvarchar](255) NOT NULL,
	[kind] [int] NOT NULL,
	[shift] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_unplanned_event_changing_formats]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_unplanned_event_changing_formats](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[OLE] [nvarchar](50) NOT NULL,
	[productionline] [nvarchar](50) NOT NULL,
	[predicted_duration] [int] NOT NULL,
	[total_duration] [int] NOT NULL,
	[comment] [text] NULL,
	[type] [nvarchar](255) NOT NULL,
	[kind] [int] NOT NULL,
	[shift] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_unplanned_event_cips]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_unplanned_event_cips](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[OLE] [nvarchar](50) NOT NULL,
	[previous_bulk] [nvarchar](255) NOT NULL,
	[predicted_duration] [int] NOT NULL,
	[total_duration] [int] NOT NULL,
	[comment] [text] NULL,
	[productionline] [nvarchar](50) NOT NULL,
	[type] [nvarchar](255) NOT NULL,
	[kind] [int] NOT NULL,
	[shift] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ole_unplanned_event_unplanned_downtimes]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ole_unplanned_event_unplanned_downtimes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NOT NULL,
	[OLE] [nvarchar](50) NOT NULL,
	[productionline] [nvarchar](255) NOT NULL,
	[implicated_machine] [nvarchar](255) NOT NULL,
	[component] [nvarchar](255) NOT NULL,
	[total_duration] [int] NOT NULL,
	[comment] [text] NULL,
	[type] [nvarchar](255) NOT NULL,
	[kind] [int] NOT NULL,
	[shift] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[teamInfo]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[teamInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workingDebut] [int] NOT NULL,
	[workingEnd] [int] NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[worksite_name] [nvarchar](50) NOT NULL,
	[state] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[login] [nvarchar](25) NOT NULL,
	[password] [nvarchar](25) NOT NULL,
	[worksite_name] [nvarchar](100) NOT NULL,
	[lastname] [nvarchar](50) NOT NULL,
	[firstname] [nvarchar](50) NOT NULL,
	[status] [int] NOT NULL,
	[productionline] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[worksite]    Script Date: 3/29/2022 11:40:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[worksite](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ole_assignement_team_pos] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_assignement_team_pos] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_machines] ADD  DEFAULT ((0)) FOR [rejection]
GO
ALTER TABLE [dbo].[ole_planned_events] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_planned_events] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_planned_events] ADD  DEFAULT ('') FOR [comment]
GO
ALTER TABLE [dbo].[ole_planned_events] ADD  DEFAULT ((0)) FOR [kind]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((1)) FOR [state]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((0)) FOR [totalOperatingTime]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((0)) FOR [totalNetOperatingTime]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((0.00)) FOR [Availability]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((0.00)) FOR [Performance]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((0.00)) FOR [Quality]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((0.00)) FOR [OLE]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((0)) FOR [qtyProduced]
GO
ALTER TABLE [dbo].[ole_pos] ADD  DEFAULT ((0)) FOR [workingDuration]
GO
ALTER TABLE [dbo].[ole_rejection_counters] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_rejection_counters] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_rejection_counters] ADD  DEFAULT (NULL) FOR [po]
GO
ALTER TABLE [dbo].[ole_speed_losses] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_speed_losses] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_speed_losses] ADD  DEFAULT (NULL) FOR [comment]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_clients] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_clients] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_clients] ADD  DEFAULT ('/') FOR [comment]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_clients] ADD  DEFAULT ('Changement de lot') FOR [type]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_clients] ADD  DEFAULT ((1)) FOR [kind]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_formats] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_formats] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_formats] ADD  DEFAULT ('/') FOR [comment]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_formats] ADD  DEFAULT ('Changement de format') FOR [type]
GO
ALTER TABLE [dbo].[ole_unplanned_event_changing_formats] ADD  DEFAULT ((1)) FOR [kind]
GO
ALTER TABLE [dbo].[ole_unplanned_event_cips] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_unplanned_event_cips] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_unplanned_event_cips] ADD  DEFAULT ('/') FOR [comment]
GO
ALTER TABLE [dbo].[ole_unplanned_event_cips] ADD  DEFAULT ('CIP') FOR [type]
GO
ALTER TABLE [dbo].[ole_unplanned_event_cips] ADD  DEFAULT ((1)) FOR [kind]
GO
ALTER TABLE [dbo].[ole_unplanned_event_unplanned_downtimes] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ole_unplanned_event_unplanned_downtimes] ADD  DEFAULT ('1900-01-01 00:00:00') FOR [updated_at]
GO
ALTER TABLE [dbo].[ole_unplanned_event_unplanned_downtimes] ADD  DEFAULT ('/') FOR [comment]
GO
ALTER TABLE [dbo].[ole_unplanned_event_unplanned_downtimes] ADD  DEFAULT ('unplannedDowntime') FOR [type]
GO
ALTER TABLE [dbo].[ole_unplanned_event_unplanned_downtimes] ADD  DEFAULT ((1)) FOR [kind]
GO
ALTER TABLE [dbo].[teamInfo] ADD  DEFAULT ((0)) FOR [state]
GO
USE [master]
GO
ALTER DATABASE [mytestdb] SET  READ_WRITE 
GO
