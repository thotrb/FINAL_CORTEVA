CREATE TABLE [machine_component] (
    [id] int identity(1,1) primary key,
    [name] nvarchar(50) not null,
    [machineName] nvarchar(100) not null,
    [other_machine] int not null,
    [worksite] nvarchar(100) null,
    [productionLine] nvarchar(100) null
);

CREATE TABLE [users] (
  [id] int identity(1,1) primary key,
  [login] nvarchar(25) not null,
  [password] nvarchar(25) not null,
  [worksite_name] nvarchar(100) not null,
  [lastname] nvarchar(50) not null,
  [firstname] nvarchar(50) not null,
  [status] int not null,
  [productionline] nvarchar(100) null
);

CREATE TABLE [worksite] (
  [id] int identity(1,1) primary key,
  [name] nvarchar(50) not null
);

CREATE TABLE [teamInfo] (
  [id] int identity(1,1) primary key,
  [workingDebut] int not null,
  [workingEnd] int not null,
  [type] nvarchar(50) not null,
  [worksite_name] nvarchar(50) not null,
  [state] int not null default 0
);

CREATE TABLE [ole_productionline] (
  [id] int identity(1,1) primary key,
  [productionline_name] nvarchar(50) not null,
  [worksite_name] nvarchar(100) not null,
);

CREATE TABLE [ole_machines] (
  [id] int identity(1,1) primary key,
  [name] nvarchar(50) not null,
  [operation] nvarchar(50) not null,
  [fabricant] nvarchar(100) not null,
  [modele] nvarchar(100) not null,
  [productionline_name] nvarchar(100) not null,
  [denomination_ordre] nvarchar(75) not null,
  [ordre] int not null,
  [rejection] int not null default 0,
  [worksite] nvarchar(100) null
);

CREATE TABLE [ole_formats] (
  [id] int identity(1,1) primary key,
  [format] nvarchar(15) not null,
  [shape] nvarchar(40) not null,
  [mat1] nvarchar(15) not null,
  [mat2] nvarchar(15) not null,
  [mat3] nvarchar(15) not null,
  [design_rate] int not null,
  [productionlineName] nvarchar(100) not null
);

CREATE TABLE [ole_speed_losses] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [OLE] nvarchar(50) not null,
  [productionline] nvarchar(50) not null,
  [duration] int not null,
  [reason] nvarchar(150) not null,
  [comment] text default null,
  [shift] nvarchar(100) null
);

CREATE TABLE [ole_assignement_team_pos] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [username] nvarchar(25) not null,
  [productionline] int not null,
  [po] nvarchar(50) not null,
  [shift] nvarchar(50) not null,
  [worksite] int not null
);

CREATE TABLE [ole_pos] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [number] nvarchar(50) not null,
  [GMIDCode] nvarchar(50) not null,
  [productionline_name] nvarchar(50) not null,
  [state] int not null default 1,
  [totalOperatingTime] int not null default 0,
  [totalNetOperatingTime] int not null default 0,
  [Availability] float(2) not null default 0.00,
  [Performance] float(2) not null default 0.00,
  [Quality] float(2) not null default 0.00,
  [OLE] float(2) not null default 0.00,
  [qtyProduced] int not null default 0,
  [workingDuration] int not null default 0,
  [shift] nvarchar(100) null
);

CREATE TABLE [ole_planned_events] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [OLE] nvarchar(50) not null,
  [productionline] nvarchar(50) not null,
  [reason] nvarchar(100) not null,
  [duration] int not null,
  [comment] text default '''',
  [kind] int not null default 0,
  [shift] nvarchar(100) null
);

CREATE TABLE [ole_unplanned_event_changing_clients] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [OLE] nvarchar(50) not null,
  [productionline] nvarchar(50) not null,
  [predicted_duration] int not null,
  [total_duration] int not null,
  [lot_number] nvarchar(50) not null,
  [comment] text default '/',
  [type] nvarchar(255) not null default 'packNumberChanging',
  [kind] int not null default 1,
  [shift] nvarchar(100) null
);

CREATE TABLE [ole_unplanned_event_changing_formats] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [OLE] nvarchar(50) not null,
  [productionline] nvarchar(50) not null,
  [predicted_duration] int not null,
  [total_duration] int not null,
  [comment] text default '/',
  [type] nvarchar(255) not null default 'formatChanging',
  [kind] int not null default 1,
  [shift] nvarchar(100) null
);

CREATE TABLE [ole_unplanned_event_cips] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [OLE] nvarchar(50) not null,
  [previous_bulk] nvarchar(255) not null,
  [predicted_duration] int not null,
  [total_duration] int not null,
  [comment] text default '/',
  [productionline] nvarchar(50) not null,
  [type] nvarchar(255) not null default 'CIP',
  [kind] int not null default 1,
  [shift] nvarchar(100) null,
  [finished] bit not null default 0
);

CREATE TABLE [ole_unplanned_event_unplanned_downtimes] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [OLE] nvarchar(50) not null,
  [productionline] nvarchar(255) not null,
  [implicated_machine] nvarchar(255) not null,
  [component] nvarchar(255) not null,
  [total_duration] int not null,
  [comment] text DEFAULT '/',
  [type] nvarchar(255) not null default 'unplannedDowntime',
  [kind] int not null default 1,
  [shift] nvarchar(100) null
);

CREATE TABLE [ole_downtimeReason] (
  [id] int identity(1,1) primary key,
  [reason] nvarchar(50) not null,
  [downtimeType] nvarchar(50) not null,
  [worksite] nvarchar(100) null,
  [production_line] nvarchar(100) null
);

CREATE TABLE [ole_products] (
  [id] int identity(1,1) primary key,
  [product] nvarchar(150) not null,
  [GMID] nvarchar(50) not null,
  [bulk] nvarchar(50) not null,
  [family] nvarchar(150) not null,
  [GIFAP] nvarchar(50) not null,
  [description] nvarchar(150) not null,
  [formulationType] nvarchar(150) not null,
  [size] nvarchar(20) not null,
  [idealRate] float(2) not null default 0.00,
  [bottlesPerCase] int not null
);

CREATE TABLE [ole_rejection_counters] (
  [id] int identity(1,1) primary key,
  [created_at] datetime not null default CURRENT_TIMESTAMP,
  [updated_at] datetime not null default '1900-01-01 00:00:00',
  [po] nvarchar(50) default null,
  [shift] nvarchar(50) default null,
  [fillerCounter] int not null,
  [caperCounter] int not null,
  [labelerCounter] int not null,
  [weightBoxCounter] int not null,
  [qualityControlCounter] int not null,
  [fillerRejection] int not null,
  [caperRejection] int not null,
  [labelerRejection] int not null,
  [weightBoxRejection] int not null,
  [qualityControlRejection] int not null
);