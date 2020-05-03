﻿drop Table SP_NyeEkger--
CREATE TABLE SP_NyeEkger ( 
id_måling  INT  IDENTITY (1, 1) PRIMARY KEY, 
id_medarbejder nvarchar(10) NOT NULL,
borger_cprnr NVARCHAR(MAX) NULL,
start_tidspunkt TEXT NOT NULL,
raa_data VARBINARY (MAX) NOT NULL,
antal_maalepunkter int Not NULL,
samplerate_hz  DECIMAL(12, 4) NULL,
)

--Select COUNT(id_måling) as AntalMålinger From SP_NyeEkger

Select id_måling From SP_NyeEkger