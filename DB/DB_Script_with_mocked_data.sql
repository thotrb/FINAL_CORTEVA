CREATE TABLE [machine_component] (
    [id] int identity(1,1) primary key,
    [name] nvarchar(50) not null,
    [machineName] nvarchar(100) not null,
    [other_machine] int not null,
    [worksite] nvarchar(100) null,
    [productionLine] nvarchar(100) null,
);

INSERT INTO [machine_component] VALUES
('downstreamSaturation', 'filler', 1, 'Cernay', 'F52'),
('dosingTurret', 'filler', 0, 'Cernay', 'F52'),
('bowlStopper', 'filler', 0, 'Cernay', 'F52'),
('screwingTurret', 'filler', 0, 'Cernay', 'F52'),
('missingBottle', 'filler', 1, 'Cernay', 'F52');

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

INSERT INTO [users] VALUES
('mpascal', '1234', 'Cernay', 'Minault', 'Pascal', 1, 'F52'),
('slaurent', '1234', 'Cernay', 'Sebire', 'Laurent', 1, 'F52'),
('pbenjamin', '1234', 'Cernay', 'Pitoizet', 'Benjamin', 1, 'F52'),
('jhyacinthe', '1234', 'Cernay', 'Jeandel', 'Hyacinthe', 1, 'F52'),
('mguillaume', '123', 'Cernay', 'Menetre', 'Guillaume', 1, 'F52'),
('test', 'test', 'Cernay', 'Test', 'Test', 1, 'F52'),
('test_op', 'test', 'Cernay', 'Test', 'Test', 0, 'F52');


CREATE TABLE [worksite] (
  [id] int identity(1,1) primary key,
  [name] nvarchar(50) not null
);

INSERT INTO [worksite] VALUES
('Cernay'), ('Medan');

CREATE TABLE [teamInfo] (
  [id] int identity(1,1) primary key,
  [workingDebut] int not null,
  [workingEnd] int not null,
  [type] nvarchar(50) not null,
  [worksite_name] nvarchar(50) not null,
  [state] int not null default 0
);

INSERT INTO [teamInfo] VALUES
('10', '18', 'A', 'Cernay', 0),
('18', '02', 'B', 'Cernay', 0),
('8', '13', 'First', 'Medan', 0),
('13', '18', 'Second', 'Medan', 0),
('18', '00', 'Third', 'Medan', 0);

CREATE TABLE [ole_productionline] (
  [id] int identity(1,1) primary key,
  [productionline_name] nvarchar(50) not null,
  [worksite_name] nvarchar(100) not null,
);

INSERT INTO [ole_productionline] (productionline_name, worksite_name) VALUES
('F52', 'Cernay'),
('F53', 'Cernay'),
('Medan1', 'Medan'),
('Medan2', 'Medan');

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

INSERT INTO [ole_machines] (name, operation, fabricant, modele, productionline_name, denomination_ordre, ordre, rejection, worksite) VALUES
('depalletizer', 'Manual', 'NA', 'NA', 'F52', 'M1', 1, 0, 'Cernay'),
('filler', 'Automated', 'SERAC', 'R1V1', 'F52', 'Filler/Caper', 2, 1, 'Cernay'),
('labeler', 'Automated', 'PE Labeler', 'XXX', 'F52', 'M2', 3, 1, 'Cernay'),
('casePacker', 'Manual', 'NA', 'NA', 'F52', 'M4', 5, 0, 'Cernay'),
('palletizer', 'Manual', 'NA', 'NA', 'F52', 'M8', 9, 0, 'Cernay'),
('boxWeigher', 'Automated', 'METTLER TOLEDO', 'MT-20', 'F52', 'M7', 8, 1, 'Cernay'),
('caseSealer', 'Automated', 'LANTECH', 'CS-300', 'F52', 'M6', 7, 0, 'Cernay'),
('caseFormer', 'Semi-Auto', 'LANTECH', 'C-300', 'F52', 'M5', 6, 0, 'Cernay'),
('bottleApplicator', 'Automated', 'SERAC', 'RX20', 'F52', 'M3', 4, 0, 'Cernay');

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

INSERT INTO [ole_formats] (format, shape, mat1, mat2, mat3, design_rate, productionlineName) VALUES
('1L', 'round', 'HDPE', 'FHDPE', 'PE/PA', 25, 'F52'),
('2L', 'square', 'HDPE', 'PET', 'None', 20, 'F52'),
('3L', 'square', 'HDPE', 'PET', 'None', 16, 'F52'),
('5L', 'square', 'HPDE', 'PET', 'None', 15, 'F52'),
('10L', 'square', 'HDPE', 'PET', 'None', 8, 'F52');

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

INSERT INTO [ole_pos] (created_at, updated_at, number, GMIDCode, productionline_name, state, totalOperatingTime, totalNetOperatingTime, Availability, Performance, Quality, OLE, qtyProduced, workingDuration, shift) VALUES
('2021-11-15 16:24:12', '1900-01-01 00:00:00', 'titi', '87507', 'F52', 0, 0, 0, 72.00, 64.00, 98.00, 78.00, 200, 160, 'A'),
('2021-11-15 16:24:17', '1900-01-01 00:00:00', 'toto', '87507', 'F52', 0, 0, 0, 1.00, 0.67, 1.00, 0.67, 100, 20, 'A'),
('2021-10-10 15:12:56', '2021-09-16 06:37:25', 't1', '335871', 'F53', 0, 0, 0, 0.00, 0.00, 0.00, 0.00, 0, 0, 'A'),
('2021-10-10 15:12:57', '2021-09-16 06:37:25', 't2', '335871', 'F53', 0, 0, 0, 0.00, 0.00, 0.00, 0.00, 0, 0, 'A'),
('2021-11-15 16:24:23', '2021-09-20 11:02:38', 'lala', '335871', 'F52', 1, 0, 0, 0.00, 0.00, 0.00, 0.00, 50, 50, 'A');

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

INSERT INTO [ole_planned_events] (created_at, updated_at, OLE, productionline, reason, duration, comment, kind, shift) VALUES
('2021-11-04 13:23:47', '2021-09-02 10:38:32', 'titi', 'F52', 'meeting', 15, 'lala', 0, 'A'),
('2021-11-04 13:23:45', '2021-09-04 14:23:15', 'titi', 'F53', 'break', 1, 'lunch', 0, 'A'),
('2021-11-04 13:23:43', '2021-09-06 13:40:47', 'titi', 'F52', 'break', 30, 'Pause déjeuner', 0, 'A'),
('2021-11-04 13:23:40', '2021-09-06 13:41:21', 'titi', 'F52', 'meeting', 45, 'lala', 0, 'A');

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

INSERT INTO [ole_unplanned_event_cips] (created_at, updated_at, OLE, previous_bulk, predicted_duration, total_duration, comment, productionline, type, kind, shift, finished) VALUES
('2021-11-17 14:58:47', '2021-11-17 14:58:47', 'titi', 'PreviousBULKTEST', 5, 30, 'J''ai été lent.', 'F52', 'CIP', 1, 'A', 1);


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

INSERT INTO [ole_downtimeReason] (reason, downtimeType, worksite) VALUES
('CIP', 'unplannedDowntime', 'Cernay', 'F52'),
('formatChanging', 'unplannedDowntime', 'Cernay', 'F52'),
('packNumberChanging', 'unplannedDowntime', 'Cernay', 'F52'),
('unplannedDowntime', 'unplannedDowntime', 'Cernay', 'F52'),
('break', 'plannedDowntime', 'Cernay', 'F52'),
('lunch', 'plannedDowntime', 'Cernay', 'F52'),
('emergency', 'plannedDowntime', 'Cernay', 'F52'),
('meeting', 'plannedDowntime', 'Cernay', 'F52'),
('noProductionPlanned', 'plannedDowntime', 'Cernay', 'F52'),
('maintenance', 'plannedDowntime', 'Cernay', 'F52'),
('projectImplementation', 'plannedDowntime', 'Cernay', 'F52');

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
  [idealRate] float(2) not null,
  [bottlesPerCase] int not null
);

INSERT INTO [ole_products] VALUES
('SUCCESSNATURALY BTLPE1X1GL USA', '87507', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '4', 20.00, 4),
('SUCCESSNATURALY BTLHPE4X1QT EN', '101223', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('CONSERVESC BTLHPE4X4X1QT USA', '124042', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('SUCCESS BTLHPE4X4X1L CAN', '138087', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('GF120NFBAIT BTLHPE4X1GL USA GF1111', '225830', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '4', 20.00, 4),
('GF120FFBAIT BTLHPE4X3.78L CAN GF1111', '275681', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '4', 20.00, 4),
('RADIANTSC BTLHPE4X1GL USA', '295528', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '4', 20.00, 4),
('RADIANTSC BTLHPE4X4X1QT USA', '295529', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('CONSERVESC BTLHPE4X4X1QT PRI', '335871', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Liquid', '1', 40.00, 12),
('GF120NFNATURALYTEFFB BTLFPE4X4L PAK', '336953', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '4', 20.00, 4),
('ENTRUST BTLHPE12X1L CAN', '11039846', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('NATURALUREFLYBAIT BTLHPE2X10L AUS', '11045177', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '10', 10.00, 2),
('INTREPIDEDGE BTLHPE4X1GL USA', '11045655', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '4', 20.00, 4),
('SUCCESSNATURALYTE BTLHPE12X1QT USA', '97018444', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('ENTRUSTSC BTLHPE12X1QT USA', '97018807', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('RADIANTSC BTLHPE12X1QT USA', '97018841', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('RADIANTSC BTLHPE12X1QT PRI', '97019730', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('ENTRUSTSC BTLHPE12X1QT PRI', '97019823', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('MOZKILL120SC BTLHPE4X5L NGA', '99056786', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '5', 17.00, 4),
('ENTRUSTSC BTLHPE4X4X1QT PRI', '99059298', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('RADIANTSC BTLHPE4X4X1QT PRI', '99059340', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '1', 40.00, 12),
('EXALT60SC BTLPET24x0.25L PHL', '11111', 'BT100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '0.25', 100.00, 24),
('EXALT BTLPET24x0.2 5L THA', '11112', 'EX100', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Solid', '0.25', 100.00, 24),
('TENANO360SC BTLPET50x0.1L IDN', '11113', 'BULKTENANO', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Sparkling', '0.1', 115.00, 50),
('ENDURE60SC BTLPET20X0.5L MYS', '11114', 'ENDURE', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Sparkling', '0.5', 80.00, 20),
('TENANO360SC BTLPET24x0.2L IDN', '22222', 'BULKTENANO', 'Herbicide', 'WG', 'Water Dispersible Granule', 'Sparkling', '0.2', 100.00, 24);

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